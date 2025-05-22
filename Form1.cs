using System;
using System.Collections.Generic;


//using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileMonitor
{
    public partial class FileMonitor : Form
    {
        //Thread loggerThread;
        LoggerFile loggerFile;
        LoggerPath loggerPath;
        private System.Windows.Forms.Timer timer1;

        private string lfCarPlay;//loggerFile CarPlay file "E:\\testFilesX\\cur_playing.xml"
        private string lfCopyPath;//loggerFile  CopyPath   "E:\\testFiles3\\"

        private string lpLoggerPath;//loggerPath monitored  path "E:\\testFilesX\\cur_playing.xml"
        private string lpCopyPath;//loggerPath  CopyPath   "E:\\testFiles3\\"

        TimeSpan maxAge = TimeSpan.FromDays(90);
        int AgeMult = 60; //через какое кол-во обращений к таймеру действует  очистка файлов
        int cntAgeMult;

        public FileMonitor()
        {
            InitializeComponent();

            // Инициализация таймеров (изначально остановлены)
            

            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 10000; // 10 сек
            timer1.Tick += (s, e) => CheckList();

            cblfCopy.Text = "E:\\copyFiles\\"; //"E:\\testFiles3\\";
            cbCurplay.Text = "C:\\Users\\Vadim\\source\\repos\\FileMonitor\\cur_playing_w.xml";// "E:\\testFilesX\\cur_playing.xml";

            cblpCopy.Text = "E:\\copyLogger\\";
            cbLogger.Text = "E:\\testLogger\\";
            
        }

        private void CheckList()
        {
            List<string> list = new List<string>();
            if (loggerFile?.richList.Count > 0)
            {
                richTextBox1.AppendText(string.Join("\n", loggerFile?.richList) + "\n");
                loggerFile?.richList.Clear();
            }

            if (loggerPath?.richList.Count > 0)
            {
                richTextBox1.AppendText(string.Join("\n", loggerPath?.richList) + "\n");
                loggerPath?.richList.Clear();
            }
            cntAgeMult++;
            if (cntAgeMult >= AgeMult)
            { //очистка файлов
                clearOldFiles();

            }
        }

        private void clearOldFiles()
        { //очистка файлов
            OldFile.DeleteFilesOlderThan(lpCopyPath, maxAge);
            OldFile.DeleteFilesOlderThan(lfCopyPath, maxAge);
            richTextBox1.AppendText(string.Join("\n", OldFile.richListOld) + "\n");
            OldFile.richListOld.Clear();
            cntAgeMult = 0;

        }

        private void btStart_Click(object sender, EventArgs e)
        {
            //----for Xml file
            lfCopyPath = cblfCopy.Text;
            lfCarPlay = cbCurplay.Text;
            bool pathsOk = true;
            if (!LoggerFile.FileCheck(lfCarPlay)) //loggerFile != null && 
            {
                richTextBox1.AppendText($"проверьте '{lbCarplay.Text}' : '{cbCurplay.Text}' \n");
                pathsOk =false;
            }
            if (!LoggerFile.PathCheck(ref lfCopyPath))
            {
                richTextBox1.AppendText($"проверьте '{lblfCopy.Text}' : '{cblfCopy.Text}' \n");
                pathsOk = false;
            }
            

            //----for path 
            lpCopyPath = cblpCopy.Text;
            lpLoggerPath = cbLogger.Text;
            if (!LoggerPath.PathCheck(ref lpLoggerPath))
            {
                richTextBox1.AppendText($"проверьте '{lbLogger.Text}' : '{cbLogger.Text}' \n");
                pathsOk = false;
            }
            if (!LoggerPath.PathCheck(ref lpCopyPath))
            {
                richTextBox1.AppendText($"проверьте '{lblpCopy.Text}' : '{cblpCopy.Text}' \n");
                pathsOk = false;
            }

            if (!pathsOk)
                return;
            cblfCopy.Text = lfCopyPath; //  тк могут измениться из-за ref
            loggerFile = new LoggerFile(lfCarPlay, lfCopyPath, 10000, 2);
            loggerFile.Start();

            cblpCopy.Text = lpCopyPath;  //  тк могут измениться из-за ref
            cbLogger.Text = lpLoggerPath;   //  тк могут измениться из-за ref
            loggerPath = new LoggerPath(lpLoggerPath, lpCopyPath, 20000, 1);
            loggerPath.Start();
            string maxA = tbDays.Text;
            maxAge = OldFile.GetMaxAge(ref maxA);
            tbDays.Text = maxA;
            clearOldFiles();

            splitContainer1.Panel1.Enabled = false;
            timer1.Start();
            btStart.Enabled = false;
            btStop.Enabled = true;
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            
            loggerFile?.Stop();
            loggerPath?.Start();
            splitContainer1.Panel1.Enabled = true;
            timer1.Stop();
            btStart.Enabled = true;
            btStop.Enabled = false;
        }

        

        
        private void button1_Click(object sender, EventArgs e)
        {
           // loggerFile.SomeProc("изменен", "E:\\testFilesX\\cur_playing.xml");


        }

        private void FileMonitor_Load(object sender, EventArgs e)
        {
           // logger.Stop();
        }

        private void cbLogger_Validated(object sender, EventArgs e)
        {
            lpLoggerPath = cbLogger.Text;
        }

        private void cblpCopy_Validated(object sender, EventArgs e)
        {
            lpCopyPath = cblpCopy.Text;
        }

        private void cbCurplay_Validated(object sender, EventArgs e)
        {
            lfCarPlay = cbCurplay.Text;
        }
        private void cblfCopy_Validated(object sender, EventArgs e)
        {
            lfCopyPath = cblfCopy.Text;
        }

        private void tbDays_Validated(object sender, EventArgs e)
        {
            string maxA = tbDays.Text;
            maxAge = OldFile.GetMaxAge(ref maxA);
            tbDays.Text = maxA;
        }

        private void tbDays_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
