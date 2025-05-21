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

        public LoggerFile(string w_Dir, string audioPath, int timer_Period, int time_DoCopy) : this(w_Dir)
        { //w_Dir - xmlFile         audioPath -CopyTo path
            PathCopy = audioPath;
            logfile = this.pathLocal + "fileslog.txt";
            timerPeriod = timer_Period;
            timeDoCopy = time_DoCopy;
        }

        bool IsFileReady(string path)
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

        public override void SomeProc(string fileEvent, string filePath)
        {
            if (fileEvent == "изменен" )
            {
                //  watcher.EnableRaisingEvents = false; // Временно отключаем
                HashSet<string>  lst = GetFilesinXML(filePath);
                TotalList.UnionWith(lst);

                string timestamp = File.GetLastWriteTime(filePath).ToString("yyyyMMdd_HHmmss");
                string originalName = Path.GetFileNameWithoutExtension(filePath);
                string newFileName = $"{originalName}_{timestamp}.xml";
                string destinationPath = Path.Combine(PathCopy, newFileName);

                File.Copy(filePath, destinationPath, true);
                richList.Add(destinationPath);
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