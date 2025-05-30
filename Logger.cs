using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Linq;
using System.Linq.Expressions;

namespace FileMonitor
{
    class Logger
    {
        public FileSystemWatcher watcher;
        object obj = new object();
#if DEBUG
        public string pathLocal = "..\\..\\";
#else
        public string pathLocal = "";
#endif
        public string logfile { get; set; } //="..\\..\\templog.txt";//"e:\\templog.txt";
                        //List<string> curList;
                        // HashSet<string> curList = new HashSet<string>();
        public HashSet<string> TotalList = new HashSet<string>();
        public string PathCopy { get; set; } // директория куда копируется
        public string wDir { get; set; }    // директория мониторинга
        private bool changed = false;
        private static System.Timers.Timer timer;
        public int timerPeriod { get; set; } = 1000;
        private int timeTimers = 0; //count
        public int timeDoCopy { get; set; } = 5; //сколько timeTimers должно произойти, чтобы отработало  Copy
        public List<string> richList { get; set; } = new List<string>();
        private DateTime _lastReadTime = DateTime.MinValue;
        public static string fmtData = "dd/MM/yyyy hh:mm:ss";


        public Logger(string wDir_,bool subDir = false):this()
        {
            this.wDir = wDir_;
            watcher.Path = wDir_; // Обновляем путь
            watcher.IncludeSubdirectories = subDir; // Включать ли  подпапки

        }
        public Logger()
        {
            watcher = new FileSystemWatcher();
            watcher.Filter = "*.*";         //следим только за файлами (не за папками)

            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
            
            timer = new System.Timers.Timer(timerPeriod) { AutoReset = true, Enabled = false };
            timer.Elapsed += (s, e) => TimerProc();
        }

        public void Start()
        {
            timer.Interval = timerPeriod;
            watcher.EnableRaisingEvents = true;
            timer.Enabled = true;
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            timer.Enabled = false;
            //enabled = false;
        }
        // переименование файлов
        public void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = pereim + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
        // изменение файлов
        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            // Если с прошлого события прошло меньше 1 секунды — игнорируем
            if ((DateTime.Now - _lastReadTime).TotalSeconds < 1)
                return;

            _lastReadTime = DateTime.Now;
            Thread.Sleep(300);
            RecordEntry(fileEvent, filePath);
            
        }
        // создание файлов
        public void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            // Если с прошлого события прошло меньше 1 секунды — игнорируем
            if ((DateTime.Now - _lastReadTime).TotalSeconds < 1)
                return;
            _lastReadTime = DateTime.Now;
            RecordEntry(fileEvent, filePath);
        }
        // удаление файлов
        public void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        const string pereim = "переименован в ";
        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logfile, true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString(fmtData), filePath, fileEvent));
                        writer.Flush();
                    }
                }
                catch { }

                if (fileEvent.Contains(pereim))
                {
                    filePath = fileEvent.Replace(pereim, "");
                    fileEvent = pereim;
                }
                SomeProc(fileEvent, filePath);
            }
        }

        public virtual void SomeProc(string fileEvent, string filePath)
        {
            changed = false;
            bool isPath = false;
            try
            {
                isPath = File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
            }
            catch
            {
                return;
            }
            if (isPath)
                return;
            switch (fileEvent)
            {
                case pereim:
                    changed = true;
                    break;
                case "изменен":
                    changed = true;
                    break;
                case "создан":
                    changed = true;
                    break;
                case "удален":
                    break;
            }
            if (changed) {
                TotalList.Add(filePath);
            }
        }

        private List<string> CopyFiles()
        {
            int count = 0;
            bool path = false;
            List<string> newFiles = new List<string>();
            string newFile;
            Type callerType = this.GetType();
            if(callerType.Name == "LoggerPath") 
                path = true;
            foreach (var file in TotalList)
            {
                if (File.Exists(file))
                {
                    if (!path)
                    {
                        newFile = PathCopy + Path.GetFileName(file);
                    }else
                        newFile = PathCopy + file.Replace(wDir, ""); 
                    try
                    {
                        string destinationDir = Path.GetDirectoryName(newFile);

                        if (!Directory.Exists(destinationDir))
                            Directory.CreateDirectory(destinationDir);

                        File.Copy(file, newFile, true);
                        DateTime arrivalTime = DateTime.UtcNow;
                        File.SetLastWriteTimeUtc(newFile, arrivalTime);

                        newFiles.Add($"{DateTime.Now.ToString(fmtData)} {newFile}");
                        count++;
                    }
                    catch (Exception ex) {
                        newFiles.Add($"{DateTime.Now.ToString(fmtData)} возникла ошибка при записи файла: {newFile}");
                    }
                }
                else { 
                    newFiles.Add($"{DateTime.Now.ToString(fmtData)} такого файла не существует: {file}"); }
            }
            TotalList.Clear();  
            return newFiles;
        }

        private void TimerProc()
        {
            timeTimers++;
            if (timeTimers >= timeDoCopy) {
                timeTimers = 0;
                richList.AddRange( CopyFiles());
            }
            
        }

        public static bool FileCheck(string filePath)
        {
            if (!File.Exists(filePath)) 
                return false;
            return true;
        }

        private static bool IsValidWindowsPathWithTrailingSlash(ref string path)
        {//определяет валидность пути, но не существование!
            if (string.IsNullOrEmpty(path)) 
                return false;
            if  (!path.EndsWith("\\"))
                path += "\\";

            try
            {
                // Проверяем, может ли путь быть преобразован в абсолютный
                string fullPath = Path.GetFullPath(path);
                if (Directory.Exists(fullPath))
                    return Path.IsPathRooted(path); // true для "C:\...", "\\Server\..."
                else 
                    return false;

            }
            catch (Exception) // PathTooLongException, ArgumentException и т. д.
            {
                return false;
            }
        }
        public static bool PathCheck(ref string filePath)
        {
            
            return IsValidWindowsPathWithTrailingSlash(ref filePath);
        }
    }
}