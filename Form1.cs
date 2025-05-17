using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMonitor
{
    public partial class FileMonitor : Form
    {
        string monitorDir = "E:\\testFiles";//@"\\192.168.1.72\1hdplay\files";
        string xmlFile = "C:\\Users\\VadimR\\GoogleD_VCh\\Prog\\FileMonitor\\cur_playing.xml";  // "C:\\Users\\Vadim\\source\\repos\\FileMonitor\\cur_playing.xml";
        string audioPath = "E:\\testFiles3\\";
        Thread loggerThread;
        Logger logger;
        List<string> curList;
        List<string> audioList = new List<string>();
        public FileMonitor()
        {
            InitializeComponent();

            logger = new Logger(monitorDir);
            loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
            


        }

        private void btStart_Click(object sender, EventArgs e)
        {
            logger.Start();
            splitContainer1.Panel1.Enabled = false;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            logger.Stop();
            splitContainer1.Panel1.Enabled = true;
            Thread.Sleep(1000);
        }

        public List<string> GetFilesinXML(String fl)
        {
            string pattern = @"<FILE_NAME>(.*?)<\/FILE_NAME>";
            string input;
            List<string> list = new List<string>();
            var fs = new FileStream(fl, FileMode.Open, FileAccess.Read,
                                      FileShare.ReadWrite | FileShare.Delete);
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
                            else { richTextBox1.AppendText($"такого файла не существует: {file}"); }
                        }
                    }
                }
            }
            return count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            checkAudio();


        }

        private void FileMonitor_Load(object sender, EventArgs e)
        {
            logger.Stop();
        }
    }
}
