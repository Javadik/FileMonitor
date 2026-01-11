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
            btStart = new Button();
            btStop = new Button();
            splitContainer1 = new SplitContainer();
            tbLive = new TextBox();
            label5 = new Label();
            label3 = new Label();
            tbDelay = new TextBox();
            label4 = new Label();
            label2 = new Label();
            tbDays = new TextBox();
            label1 = new Label();
            lbCarPlayItogReplace = new Label();
            сbCarPlayItogReplace = new ComboBox();
            lbCarplayItog = new Label();
            cbCurplayItog = new ComboBox();
            lblfCopy = new Label();
            cblfCopy = new ComboBox();
            lbCarplay = new Label();
            cbCurplay = new ComboBox();
            lblpCopy = new Label();
            cblpCopy = new ComboBox();
            lbLogger = new Label();
            cbLogger = new ComboBox();
            richTextBox1 = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // btStart
            // 
            btStart.Location = new Point(88, 16);
            btStart.Margin = new Padding(4);
            btStart.Name = "btStart";
            btStart.Size = new Size(88, 30);
            btStart.TabIndex = 0;
            btStart.Text = "Start";
            btStart.UseVisualStyleBackColor = true;
            btStart.Click += btStart_Click;
            // 
            // btStop
            // 
            btStop.Enabled = false;
            btStop.Location = new Point(230, 16);
            btStop.Margin = new Padding(4);
            btStop.Name = "btStop";
            btStop.Size = new Size(88, 30);
            btStop.TabIndex = 1;
            btStop.Text = "Stop";
            btStop.UseVisualStyleBackColor = true;
            btStop.Click += btStop_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 51);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tbLive);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(tbDelay);
            splitContainer1.Panel1.Controls.Add(label4);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(tbDays);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(lbCarPlayItogReplace);
            splitContainer1.Panel1.Controls.Add(сbCarPlayItogReplace);
            splitContainer1.Panel1.Controls.Add(lbCarplayItog);
            splitContainer1.Panel1.Controls.Add(cbCurplayItog);
            splitContainer1.Panel1.Controls.Add(lblfCopy);
            splitContainer1.Panel1.Controls.Add(cblfCopy);
            splitContainer1.Panel1.Controls.Add(lbCarplay);
            splitContainer1.Panel1.Controls.Add(cbCurplay);
            splitContainer1.Panel1.Controls.Add(lblpCopy);
            splitContainer1.Panel1.Controls.Add(cblpCopy);
            splitContainer1.Panel1.Controls.Add(lbLogger);
            splitContainer1.Panel1.Controls.Add(cbLogger);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(richTextBox1);
            splitContainer1.Size = new Size(1009, 569);
            splitContainer1.SplitterDistance = 276;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 3;
            // 
            // tbLive
            // 
            tbLive.Location = new Point(716, 60);
            tbLive.Name = "tbLive";
            tbLive.Size = new Size(277, 25);
            tbLive.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(716, 12);
            label5.Name = "label5";
            label5.Size = new Size(190, 17);
            label5.TabIndex = 18;
            label5.Text = "Фраза поиска прямого эфира:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(90, 68);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(84, 17);
            label3.TabIndex = 17;
            label3.Text = "миллисекунд";
            // 
            // tbDelay
            // 
            tbDelay.Location = new Point(20, 58);
            tbDelay.Margin = new Padding(4);
            tbDelay.Name = "tbDelay";
            tbDelay.Size = new Size(64, 25);
            tbDelay.TabIndex = 16;
            tbDelay.Text = "100";
            tbDelay.Validated += tbDelay_Validated;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 12);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(69, 17);
            label4.TabIndex = 15;
            label4.Text = "Задержка:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(607, 66);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(36, 17);
            label2.TabIndex = 14;
            label2.Text = "дней";
            // 
            // tbDays
            // 
            tbDays.Location = new Point(535, 58);
            tbDays.Margin = new Padding(4);
            tbDays.Name = "tbDays";
            tbDays.Size = new Size(64, 25);
            tbDays.TabIndex = 13;
            tbDays.Text = "90";
            tbDays.Validated += сbDays_Validated;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(535, 12);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.MaximumSize = new Size(160, 0);
            label1.Name = "label1";
            label1.Size = new Size(145, 34);
            label1.TabIndex = 12;
            label1.Text = "Файлы в директориях назначения хранятся : ";
            // 
            // lbCarPlayItogReplace
            // 
            lbCarPlayItogReplace.AutoSize = true;
            lbCarPlayItogReplace.Location = new Point(20, 214);
            lbCarPlayItogReplace.Margin = new Padding(4, 0, 4, 0);
            lbCarPlayItogReplace.Name = "lbCarPlayItogReplace";
            lbCarPlayItogReplace.Size = new Size(238, 17);
            lbCarPlayItogReplace.TabIndex = 11;
            lbCarPlayItogReplace.Text = "Путь для замены cur_playing итоговый";
            // 
            // сbCarPlayItogReplace
            // 
            сbCarPlayItogReplace.FormattingEnabled = true;
            сbCarPlayItogReplace.Location = new Point(14, 239);
            сbCarPlayItogReplace.Margin = new Padding(4);
            сbCarPlayItogReplace.Name = "сbCarPlayItogReplace";
            сbCarPlayItogReplace.Size = new Size(458, 25);
            сbCarPlayItogReplace.TabIndex = 10;
            сbCarPlayItogReplace.Validated += сbCarPlayItogReplace_Validated;
            // 
            // lbCarplayItog
            // 
            lbCarplayItog.AutoSize = true;
            lbCarplayItog.Location = new Point(540, 214);
            lbCarplayItog.Margin = new Padding(4, 0, 4, 0);
            lbCarplayItog.Name = "lbCarplayItog";
            lbCarplayItog.Size = new Size(414, 17);
            lbCarplayItog.TabIndex = 9;
            lbCarplayItog.Text = "cur_playing итоговый    Файл будет перезаписан при первом старте";
            // 
            // cbCurplayItog
            // 
            cbCurplayItog.FormattingEnabled = true;
            cbCurplayItog.Location = new Point(535, 239);
            cbCurplayItog.Margin = new Padding(4);
            cbCurplayItog.Name = "cbCurplayItog";
            cbCurplayItog.Size = new Size(458, 25);
            cbCurplayItog.TabIndex = 8;
            cbCurplayItog.Validated += cbCurplayItog_Validated;
            // 
            // lblfCopy
            // 
            lblfCopy.AutoSize = true;
            lblfCopy.Location = new Point(540, 152);
            lblfCopy.Margin = new Padding(4, 0, 4, 0);
            lblfCopy.Name = "lblfCopy";
            lblfCopy.Size = new Size(89, 17);
            lblfCopy.TabIndex = 7;
            lblfCopy.Text = "копировать в";
            // 
            // cblfCopy
            // 
            cblfCopy.FormattingEnabled = true;
            cblfCopy.Location = new Point(535, 176);
            cblfCopy.Margin = new Padding(4);
            cblfCopy.Name = "cblfCopy";
            cblfCopy.Size = new Size(458, 25);
            cblfCopy.TabIndex = 6;
            cblfCopy.Validated += cblfCopy_Validated;
            // 
            // lbCarplay
            // 
            lbCarplay.AutoSize = true;
            lbCarplay.Location = new Point(20, 152);
            lbCarplay.Margin = new Padding(4, 0, 4, 0);
            lbCarplay.Name = "lbCarplay";
            lbCarplay.Size = new Size(73, 17);
            lbCarplay.TabIndex = 5;
            lbCarplay.Text = "cur_playing";
            // 
            // cbCurplay
            // 
            cbCurplay.FormattingEnabled = true;
            cbCurplay.Location = new Point(14, 176);
            cbCurplay.Margin = new Padding(4);
            cbCurplay.Name = "cbCurplay";
            cbCurplay.Size = new Size(458, 25);
            cbCurplay.TabIndex = 4;
            cbCurplay.Validated += cbCurplay_Validated;
            // 
            // lblpCopy
            // 
            lblpCopy.AutoSize = true;
            lblpCopy.Location = new Point(540, 91);
            lblpCopy.Margin = new Padding(4, 0, 4, 0);
            lblpCopy.Name = "lblpCopy";
            lblpCopy.Size = new Size(89, 17);
            lblpCopy.TabIndex = 3;
            lblpCopy.Text = "копировать в";
            // 
            // cblpCopy
            // 
            cblpCopy.FormattingEnabled = true;
            cblpCopy.Location = new Point(535, 118);
            cblpCopy.Margin = new Padding(4);
            cblpCopy.Name = "cblpCopy";
            cblpCopy.Size = new Size(458, 25);
            cblpCopy.TabIndex = 2;
            cblpCopy.Validated += cblpCopy_Validated;
            // 
            // lbLogger
            // 
            lbLogger.AutoSize = true;
            lbLogger.Location = new Point(20, 92);
            lbLogger.Margin = new Padding(4, 0, 4, 0);
            lbLogger.Name = "lbLogger";
            lbLogger.Size = new Size(93, 17);
            lbLogger.TabIndex = 1;
            lbLogger.Text = "папка логгера";
            // 
            // cbLogger
            // 
            cbLogger.FormattingEnabled = true;
            cbLogger.Location = new Point(14, 118);
            cbLogger.Margin = new Padding(4);
            cbLogger.Name = "cbLogger";
            cbLogger.Size = new Size(458, 25);
            cbLogger.TabIndex = 0;
            cbLogger.Validated += cbLogger_Validated;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Margin = new Padding(4);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1009, 288);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // FileMonitor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1009, 620);
            Controls.Add(splitContainer1);
            Controls.Add(btStop);
            Controls.Add(btStart);
            Margin = new Padding(4);
            MinimumSize = new Size(1025, 399);
            Name = "FileMonitor";
            Text = "FileMonitor 1.40";
            FormClosing += FileMonitor_FormClosing;
            Load += FileMonitor_Load;
            Validated += FileMonitor_Validated;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lbLogger;
        private System.Windows.Forms.ComboBox cbLogger;
        private System.Windows.Forms.Label lblpCopy;
        private System.Windows.Forms.ComboBox cblpCopy;
        private System.Windows.Forms.Label lblfCopy;
        private System.Windows.Forms.ComboBox cblfCopy;
        private System.Windows.Forms.Label lbCarplay;
        private System.Windows.Forms.ComboBox cbCurplay;
        private System.Windows.Forms.Label lbCarplayItog;
        private System.Windows.Forms.ComboBox cbCurplayItog;
        private System.Windows.Forms.Label lbCarPlayItogReplace;
        private System.Windows.Forms.ComboBox сbCarPlayItogReplace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDays;
        private System.Windows.Forms.Label label1;
        private RichTextBox richTextBox1;
        private Label label5;
        private TextBox tbLive;
    }
}

