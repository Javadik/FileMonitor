using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Linq;
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

        public override void SomeProc(string fileEvent, string fnewXML)
        {
            if (fileEvent == "изменен" )
            {
                //  watcher.EnableRaisingEvents = false; // Временно отключаем
                string ftimestamp = File.GetLastWriteTime(fnewXML).ToString("yyyyMMdd_HHmmss");
                String fl;
                string input;
                
                if (!IsFileReady(fnewXML))
                    return; // Файл занят, пропускаем
                using (var fs = new FileStream(fnewXML, FileMode.Open, FileAccess.Read,
                                          FileShare.Read))
                {

                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        input = reader.ReadToEnd();
                    }
                }
                if (string.IsNullOrEmpty(input)) 
                    return;
                HashSet<string> lst;
                string output;
                //HashSet<string>  lst = GetFilesinXML(input);
                (lst,output) = ProcessXml(input);
                TotalList.UnionWith(lst);

                
                CarPlayItogAdd(carPlayItog, output, ftimestamp);
                //File.Copy(fnewXML, destinationPath, true);
                DateTime arrivalTime = DateTime.UtcNow;
                //File.SetLastWriteTimeUtc(destinationPath, arrivalTime);
                richList.Add($"{DateTime.Now.ToString(fmtData)} {carPlayItog}  - перезаписан");
            }
        }

        public void CarPlayItogAdd(string filePath, string input2, string timestamp)
        //Prepend Lines To File Editor Safe
        {//filePath -commomon file of XML  newXML-file with new xml  timestamp -time creating
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
                            // Открываем с FileShare.Read, чтобы не мешать редактору
                            using (var reader = new StreamReader(
                                new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                    writer.WriteLine(line);
                            }
                        }
                    }
                    else
                        qComFile = 0;
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