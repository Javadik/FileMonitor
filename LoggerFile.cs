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
        private string commonFile;
        private int   maxComFile = 25;//mnax quantaty time record XMLs into commonFile, then overrecord
        private int qComFile = 0;// counter time record XMLs into commonFile


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

        public LoggerFile(string w_Dir, string audioPath, string fcommonFile, int timer_Period, int time_DoCopy) : this(w_Dir)
        { //w_Dir - xmlFile         audioPath -CopyTo path
            PathCopy = audioPath;
            logfile = this.pathLocal + "fileslog.txt";
            timerPeriod = timer_Period;
            timeDoCopy = time_DoCopy;
            commonFile = fcommonFile;
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
        public HashSet<string> GetFilesinXML(String fl)
        {
            string pattern = @"<FILE_NAME>(.*?)<\/FILE_NAME>";
            string input;
            HashSet<string> list = new HashSet<string>();
            if (!IsFileReady(fl))
                return list; // Файл занят, пропускаем
            var fs = new FileStream(fl, FileMode.Open, FileAccess.Read,
                                      FileShare.Read);
            

            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                input = reader.ReadToEnd();
            }

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

        public override void SomeProc(string fileEvent, string fnewXML)
        {
            if (fileEvent == "изменен" )
            {
                //  watcher.EnableRaisingEvents = false; // Временно отключаем
                HashSet<string>  lst = GetFilesinXML(fnewXML);
                TotalList.UnionWith(lst);

                string ftimestamp = File.GetLastWriteTime(fnewXML).ToString("yyyyMMdd_HHmmss");
                //string originalName = Path.GetFileNameWithoutExtension(fnewXML);
                //string newFileName = $"{originalName}_{ftimestamp}.xml";
                //string destinationPath = Path.Combine(PathCopy, newFileName);


                PrependLinesToFileEditorSafe(commonFile, fnewXML, ftimestamp);
                //File.Copy(fnewXML, destinationPath, true);
                DateTime arrivalTime = DateTime.UtcNow;
                //File.SetLastWriteTimeUtc(destinationPath, arrivalTime);
                richList.Add($"{DateTime.Now.ToString(fmtData)} {commonFile}  - перезаписан");
            }
        }

        public void PrependLinesToFileEditorSafe(string filePath, string newXML, string timestamp)
        {//filePath -commomon file of XML  newXML-file with new xml  timestamp -time creating
            string tempFile = Path.GetTempFileName();
            string linesToAdd;
            if (!IsFileReady(newXML))
                return; // Файл занят, пропускаем
            var fs = new FileStream(newXML, FileMode.Open, FileAccess.Read,
                                      FileShare.Read);
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                linesToAdd = "<!--" + timestamp + "-->\n" + reader.ReadToEnd();
            }

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
                richList.Add($"{DateTime.Now.ToString(fmtData)} {commonFile}  - ошибка при перезаписи");
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