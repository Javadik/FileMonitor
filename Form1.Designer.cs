namespace FileMonitor
{
    partial class FileMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbCarPlayItogReplace = new System.Windows.Forms.Label();
            this.сbCarPlayItogReplace = new System.Windows.Forms.ComboBox();
            this.lbCarplayItog = new System.Windows.Forms.Label();
            this.cbCurplayItog = new System.Windows.Forms.ComboBox();
            this.lblfCopy = new System.Windows.Forms.Label();
            this.cblfCopy = new System.Windows.Forms.ComboBox();
            this.lbCarplay = new System.Windows.Forms.Label();
            this.cbCurplay = new System.Windows.Forms.ComboBox();
            this.lblpCopy = new System.Windows.Forms.Label();
            this.cblpCopy = new System.Windows.Forms.ComboBox();
            this.lbLogger = new System.Windows.Forms.Label();
            this.cbLogger = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.сbDays = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(75, 39);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(197, 39);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 92);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbCarPlayItogReplace);
            this.splitContainer1.Panel1.Controls.Add(this.сbCarPlayItogReplace);
            this.splitContainer1.Panel1.Controls.Add(this.lbCarplayItog);
            this.splitContainer1.Panel1.Controls.Add(this.cbCurplayItog);
            this.splitContainer1.Panel1.Controls.Add(this.lblfCopy);
            this.splitContainer1.Panel1.Controls.Add(this.cblfCopy);
            this.splitContainer1.Panel1.Controls.Add(this.lbCarplay);
            this.splitContainer1.Panel1.Controls.Add(this.cbCurplay);
            this.splitContainer1.Panel1.Controls.Add(this.lblpCopy);
            this.splitContainer1.Panel1.Controls.Add(this.cblpCopy);
            this.splitContainer1.Panel1.Controls.Add(this.lbLogger);
            this.splitContainer1.Panel1.Controls.Add(this.cbLogger);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(958, 359);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 3;
            // 
            // lbCarPlayItogReplace
            // 
            this.lbCarPlayItogReplace.AutoSize = true;
            this.lbCarPlayItogReplace.Location = new System.Drawing.Point(17, 95);
            this.lbCarPlayItogReplace.Name = "lbCarPlayItogReplace";
            this.lbCarPlayItogReplace.Size = new System.Drawing.Size(203, 13);
            this.lbCarPlayItogReplace.TabIndex = 11;
            this.lbCarPlayItogReplace.Text = "Путь для замены cur_playing итоговый";
            // 
            // сbCarPlayItogReplace
            // 
            this.сbCarPlayItogReplace.FormattingEnabled = true;
            this.сbCarPlayItogReplace.Location = new System.Drawing.Point(12, 115);
            this.сbCarPlayItogReplace.Name = "сbCarPlayItogReplace";
            this.сbCarPlayItogReplace.Size = new System.Drawing.Size(393, 21);
            this.сbCarPlayItogReplace.TabIndex = 10;
            this.сbCarPlayItogReplace.Validated += new System.EventHandler(this.сbCarPlayItogReplace_Validated);
            // 
            // lbCarplayItog
            // 
            this.lbCarplayItog.AutoSize = true;
            this.lbCarplayItog.Location = new System.Drawing.Point(463, 95);
            this.lbCarplayItog.Name = "lbCarplayItog";
            this.lbCarplayItog.Size = new System.Drawing.Size(352, 13);
            this.lbCarplayItog.TabIndex = 9;
            this.lbCarplayItog.Text = "cur_playing итоговый    Файл будет перезаписан при первом старте";
            // 
            // cbCurplayItog
            // 
            this.cbCurplayItog.FormattingEnabled = true;
            this.cbCurplayItog.Location = new System.Drawing.Point(458, 115);
            this.cbCurplayItog.Name = "cbCurplayItog";
            this.cbCurplayItog.Size = new System.Drawing.Size(393, 21);
            this.cbCurplayItog.TabIndex = 8;
            this.cbCurplayItog.Validated += new System.EventHandler(this.cbCurplayItog_Validated);
            // 
            // lblfCopy
            // 
            this.lblfCopy.AutoSize = true;
            this.lblfCopy.Location = new System.Drawing.Point(463, 48);
            this.lblfCopy.Name = "lblfCopy";
            this.lblfCopy.Size = new System.Drawing.Size(75, 13);
            this.lblfCopy.TabIndex = 7;
            this.lblfCopy.Text = "копировать в";
            // 
            // cblfCopy
            // 
            this.cblfCopy.FormattingEnabled = true;
            this.cblfCopy.Location = new System.Drawing.Point(458, 67);
            this.cblfCopy.Name = "cblfCopy";
            this.cblfCopy.Size = new System.Drawing.Size(393, 21);
            this.cblfCopy.TabIndex = 6;
            this.cblfCopy.Validated += new System.EventHandler(this.cblfCopy_Validated);
            // 
            // lbCarplay
            // 
            this.lbCarplay.AutoSize = true;
            this.lbCarplay.Location = new System.Drawing.Point(17, 48);
            this.lbCarplay.Name = "lbCarplay";
            this.lbCarplay.Size = new System.Drawing.Size(61, 13);
            this.lbCarplay.TabIndex = 5;
            this.lbCarplay.Text = "cur_playing";
            // 
            // cbCurplay
            // 
            this.cbCurplay.FormattingEnabled = true;
            this.cbCurplay.Location = new System.Drawing.Point(12, 67);
            this.cbCurplay.Name = "cbCurplay";
            this.cbCurplay.Size = new System.Drawing.Size(393, 21);
            this.cbCurplay.TabIndex = 4;
            this.cbCurplay.Validated += new System.EventHandler(this.cbCurplay_Validated);
            // 
            // lblpCopy
            // 
            this.lblpCopy.AutoSize = true;
            this.lblpCopy.Location = new System.Drawing.Point(463, 2);
            this.lblpCopy.Name = "lblpCopy";
            this.lblpCopy.Size = new System.Drawing.Size(75, 13);
            this.lblpCopy.TabIndex = 3;
            this.lblpCopy.Text = "копировать в";
            // 
            // cblpCopy
            // 
            this.cblpCopy.FormattingEnabled = true;
            this.cblpCopy.Location = new System.Drawing.Point(458, 22);
            this.cblpCopy.Name = "cblpCopy";
            this.cblpCopy.Size = new System.Drawing.Size(393, 21);
            this.cblpCopy.TabIndex = 2;
            this.cblpCopy.Validated += new System.EventHandler(this.cblpCopy_Validated);
            // 
            // lbLogger
            // 
            this.lbLogger.AutoSize = true;
            this.lbLogger.Location = new System.Drawing.Point(17, 3);
            this.lbLogger.Name = "lbLogger";
            this.lbLogger.Size = new System.Drawing.Size(80, 13);
            this.lbLogger.TabIndex = 1;
            this.lbLogger.Text = "папка логгера";
            // 
            // cbLogger
            // 
            this.cbLogger.FormattingEnabled = true;
            this.cbLogger.Location = new System.Drawing.Point(12, 22);
            this.cbLogger.Name = "cbLogger";
            this.cbLogger.Size = new System.Drawing.Size(393, 21);
            this.cbLogger.TabIndex = 0;
            this.cbLogger.Validated += new System.EventHandler(this.cbLogger_Validated);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(958, 210);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(337, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(455, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Файлы в директориях назначения хранятся : ";
            // 
            // сbDays
            // 
            this.сbDays.Location = new System.Drawing.Point(458, 58);
            this.сbDays.Name = "сbDays";
            this.сbDays.Size = new System.Drawing.Size(55, 20);
            this.сbDays.TabIndex = 6;
            this.сbDays.Text = "90";
            this.сbDays.Validated += new System.EventHandler(this.сbDays_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(519, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "дней";
            // 
            // FileMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.сbDays);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Name = "FileMonitor";
            this.Text = "FileMonitor 1.31";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileMonitor_FormClosing);
            this.Load += new System.EventHandler(this.FileMonitor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lbLogger;
        private System.Windows.Forms.ComboBox cbLogger;
        private System.Windows.Forms.Label lblpCopy;
        private System.Windows.Forms.ComboBox cblpCopy;
        private System.Windows.Forms.Label lblfCopy;
        private System.Windows.Forms.ComboBox cblfCopy;
        private System.Windows.Forms.Label lbCarplay;
        private System.Windows.Forms.ComboBox cbCurplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox сbDays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbCarplayItog;
        private System.Windows.Forms.ComboBox cbCurplayItog;
        private System.Windows.Forms.Label lbCarPlayItogReplace;
        private System.Windows.Forms.ComboBox сbCarPlayItogReplace;
    }
}

