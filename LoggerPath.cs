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
    class LoggerPath: Logger
    {
        private List<string> audioList = new List<string>();
        

        public LoggerPath(string w_Dir, string Pathcp, int timer_Period, int time_DoCopy) : base(w_Dir,true)
        { //w_Dir - path-File-monitoring         Pathcp -CopyTo path
            PathCopy = Pathcp;
            logfile = this.pathLocal + "pathslog.txt";
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
        


       
    }
}