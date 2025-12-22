using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace FileMonitor
{
    class LoggerFile: Logger
    {
        private List<string> audioList = new List<string>();
        private string carPlayItog;
        private int   maxComFile = 25;//mnax quantaty time record XMLs into carPlayItog, then overrecord
        private int qComFile = 0;// counter time record XMLs into carPlayItog
        private string carPlayItogReplace;//path for replacing in XML
        public static bool  httpPath = false;   //if carPlayItogReplace http path or not
        public static int taskDelay = 100;



        public LoggerFile(string wDir_) : base()
        {
           /* if (!File.Exists(wDir_))  //сделал проверку в форме
            {
                throw new FileNotFoundException($"Файл {wDir_} не найден!");
            } */
            this.wDir = Path.GetDirectoryName(wDir_);
            watcher.Path = this.wDir;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = Path.GetFileName(wDir_);
        }

        public LoggerFile(string w_Dir, string audioPath, string fcarPlayItog, int timer_Period, int time_DoCopy, string fcarPlayItogReplace) : this(w_Dir)
        { //w_Dir - xmlFile         audioPath -CopyTo path
            PathCopy = audioPath;
            logfile = this.pathLocal + "fileslog.txt";
            timerPeriod = timer_Period;
            timeDoCopy = time_DoCopy;
            carPlayItog = fcarPlayItog;
            carPlayItogReplace = fcarPlayItogReplace;
        }

        public override async void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // Защита от повторных событий в короткий промежуток времени
            if ((DateTime.Now - _lastReadTime).TotalSeconds < 1)
                return;

            _lastReadTime = DateTime.Now;

            watcher.EnableRaisingEvents = false; // Отключаем watcher перед обработкой
            try
            {
                await Task.Delay(taskDelay); // Даем файлу "успокоиться" (если запись еще не завершена)
                RecordEntry("изменен", e.FullPath);
            }
            catch (Exception ex)
            {
                // Логирование ошибки (например, в файл или Debug)
                richList.Add($"Ошибка при обработке файла {e.FullPath}: {ex.Message}");
            }
            finally
            {
                watcher.EnableRaisingEvents = true; // Включаем обратно
            }
        }

        public static bool IsFileReady(string path)
        {
            try
            {
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException) {
                return false;
            }
        }
        public HashSet<string> GetFilesinXML(String input)
        {
            string pattern = @"<FILE_NAME>(.*?)<\/FILE_NAME>";
            HashSet<string> list = new HashSet<string>();

            var regex = new Regex(pattern);
            var matches = regex.Matches(input);
            // System.Console.WriteLine(match.Groups[1].Value);
            for (int i = 0; i < matches.Count; i++)
            {
                string st = matches[i].Groups[1].Value;
                list.Add(st);
            }
            return list;
        }

        /*
        private string fReplace(string inst)
        {
            string res;
            res= carPlayItogReplace + Path.GetFileName(inst);
            return res;
        } 
        */
        private string fReplace(string inst)
        {
            string fileName = Path.GetFileName(inst);
            string fullPath = Path.Combine(carPlayItogReplace, fileName);
            if(httpPath)
                return fullPath;
            return Path.GetFullPath(fullPath); // Гарантирует нормальный путь
        }



        private const string FullTagPattern = @"(<FILE_NAME>)(.*?)(<\/FILE_NAME>)";
        // Группы:        [1] открывающий тег  [2] содержимое  [3] закрывающий тег
        public (HashSet<string> FileNames, string ModifiedXml) ProcessXml(string input)
        {
            var regex = new Regex(FullTagPattern);
            var fileNames = new HashSet<string>();

            string modifiedXml = regex.Replace(input, match =>
            {
                string fileName = match.Groups[2].Value; // Группа 2 — содержимое тега
                fileNames.Add(fileName);
                string replacedValue = fReplace(fileName);
                return $"{match.Groups[1].Value}{replacedValue}{match.Groups[3].Value}";
                // Groups[1] = <FILE_NAME>
                // Groups[3] = </FILE_NAME>
            });

            return (fileNames, modifiedXml);
        }

        public override async void SomeProc(string fileEvent, string fnewXML)
        {
            if (fileEvent == "изменен" )
            {
                //  watcher.EnableRaisingEvents = false; // Временно отключаем
                //string ftimestamp = File.GetLastWriteTime(fnewXML).ToString("yyyyMMdd_HHmmss");
                var fileInfo = new FileInfo(fnewXML);
                string ftimestamp = fileInfo.LastWriteTime.ToString("yyyyMMdd_HHmmss");
                
                //if (!IsFileReady(fnewXML))
               //     return; // Файл занят, пропускаем

                int maxRetries = 3;
                int delayMs = 100;
                string input = null;
                string tempFile = null;


                /*
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        using (var fs = new FileStream(fnewXML, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                        {
                            input = reader.ReadToEnd();
                            break; // Успешно прочитали
                        }
                    }
                    catch (IOException)
                    {
                        if (i == maxRetries - 1) throw; // Последняя попытка — пробрасываем исключение
                        Thread.Sleep(delayMs); // Ждём перед повторной попыткой
                    }
                }*/

                try
                {
                    for (int i = 0; i < maxRetries; i++)
                    {
                        try
                        {
                            tempFile = Path.GetTempFileName();
                            using (var sourceStream = new FileStream(
                                fnewXML,
                                FileMode.Open,
                                FileAccess.Read,
                                FileShare.ReadWrite)) // Важно!
                            {
                                using (var tempStream = File.Create(tempFile))
                                {
                                    await sourceStream.CopyToAsync(tempStream);
                                }
                            }

                            //File.Copy(fnewXML, tempFile, overwrite: true);
                            input = File.ReadAllText(tempFile, Encoding.UTF8);
                            break;
                        }
                        catch (IOException)
                        {
                            if (tempFile != null && File.Exists(tempFile))
                                File.Delete(tempFile);
                            if (i == maxRetries - 1) throw;
                            Thread.Sleep(delayMs);
                        }
                    }

                    if (string.IsNullOrEmpty(input))
                        return;
                    (var lst, var output) = ProcessXml(input);
                    lock (obj)
                    {
                        TotalList.UnionWith(lst);
                        CurPlayItogAdd(carPlayItog, output, ftimestamp);
                        richList.Add($"{DateTime.Now.ToString(fmtData)} {carPlayItog} - перезаписан");
                    }
                }
                finally
                {
                    if (tempFile != null && File.Exists(tempFile))
                        File.Delete(tempFile);
                }
            }
        }

        public string ChangeFileName(string fullFileName, string splus)
        { //с fullpath\A.extension на fullpath\A+B.extension
            int lastDot = fullFileName.LastIndexOf('.');

            string fileNameWithoutExt;
            string extension;
            string newFileName;

            if (lastDot > 0) // расширение есть и не в начале имени (например, не ".gitignore")
            {
                fileNameWithoutExt = fullFileName.Substring(0, lastDot);
                extension = fullFileName.Substring(lastDot); // включая точку
            }
            else
            {
                // расширения нет
                fileNameWithoutExt = fullFileName;
                extension = ""; // или, например, ".txt", если нужно задать явно
            }

            // Формируем новое имя: добавляем "B" перед расширением
            newFileName = fileNameWithoutExt + splus + extension;
            return newFileName;
        }

        public void CurPlayItogAdd(string filePath, string input2, string timestamp)
        //Prepend Lines To File Editor Safe
        {//filePath -commomon file of XML  input2 strings of  new xml  timestamp -time creating
            string tempFile = Path.GetTempFileName();
            string linesToAdd;
            
            linesToAdd = "<!--" + timestamp + "-->\n" + input2;

            try
            {
                // 1. Записываем новые строки во временный файл
                using (var writer = new StreamWriter(tempFile))
                {
                    writer.WriteLine(linesToAdd);
                    if (qComFile < maxComFile) //<25 records
                    {
                        // 2. Дописываем содержимое исходного файла (если он есть)
                        if (File.Exists(filePath))
                        {
                            // Открываем с FileShare.ReadWrite, чтобы не мешать редактору
                            using (var reader = new StreamReader(
                                new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                    writer.WriteLine(line);
                            }
                        }

                    }
                }
                // здесь уже есть вся информация во временном файле tempFile 
                //далее задача обновить существующий 

                if (qComFile >= maxComFile) //<25 records
                {
                    qComFile = 0;
                    string ftstamp;
                    var fileInfo = new FileInfo(filePath);
                    try {
                        ftstamp = fileInfo.LastWriteTime.ToString("yyyyMMdd_HHmmss");
                    }
                    catch
                    {
                        ftstamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    }
                    string carPlayItogDate = ChangeFileName(filePath, "_" + ftstamp);
                    try
                    {
                        using (var source = File.Open(
                            filePath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite // ← ключевой момент
                        ))
                        using (var dest = File.Create(carPlayItogDate))
                        {

                            source.CopyTo(dest);
                            richList.Add($"{DateTime.Now.ToString(fmtData)} {carPlayItogDate}  - создан");
                        }
                    }
                    catch (IOException ex)
                    {
                        // Файл заблокирован даже на чтение — редко, но бывает
                        // Можно попробовать повторить позже
                        richList.Add($"{DateTime.Now.ToString(fmtData)} {carPlayItogDate}  - ошибка при копировании");
                    }

                    }

                // 3. Атомарная замена (работает на Windows)
                //File.Replace(tempFile, filePath, null, true);
                File.Copy(tempFile, filePath, overwrite: true);
                qComFile++;
            }
            catch
            {
                richList.Add($"{DateTime.Now.ToString(fmtData)} {carPlayItog}  - ошибка при перезаписи");
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        /*
        public int checkAudio()
        {
            int count = 0;
            curList = GetFilesinXML(xmlFile);
            if (curList.Count() != 0)
            {
                //if (!audioList.Intersect(curList).Any()) //if not all curList in audioList
                if (!audioList.Any(item => curList.Contains(item)))  //if not all curList in audioList
                {

                    foreach (var file in curList)
                    {
                        if (!audioList.Contains(file))
                        {
                            if (File.Exists(file))
                            {
                                File.Copy(file, audioPath + Path.GetFileName(file), true);
                                audioList.Add(file);
                                count++;
                            }
                            else { Form1.richTextBox1.AppendText($"такого файла не существует: {file}"); }
                        }
                    }
                }
            }
            return count;
        } */
    }
}