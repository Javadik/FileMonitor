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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(100, 48);
            this.btStart.Margin = new System.Windows.Forms.Padding(4);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(100, 28);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(263, 48);
            this.btStop.Margin = new System.Windows.Forms.Padding(4);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(100, 28);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 113);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
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
            this.splitContainer1.Size = new System.Drawing.Size(1277, 442);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // lblfCopy
            // 
            this.lblfCopy.AutoSize = true;
            this.lblfCopy.Location = new System.Drawing.Point(617, 85);
            this.lblfCopy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblfCopy.Name = "lblfCopy";
            this.lblfCopy.Size = new System.Drawing.Size(95, 16);
            this.lblfCopy.TabIndex = 7;
            this.lblfCopy.Text = "копировать в";
            // 
            // cblfCopy
            // 
            this.cblfCopy.FormattingEnabled = true;
            this.cblfCopy.Location = new System.Drawing.Point(611, 112);
            this.cblfCopy.Margin = new System.Windows.Forms.Padding(4);
            this.cblfCopy.Name = "cblfCopy";
            this.cblfCopy.Size = new System.Drawing.Size(523, 24);
            this.cblfCopy.TabIndex = 6;
            this.cblfCopy.Validated += new System.EventHandler(this.cblfCopy_Validated);
            // 
            // lbCarplay
            // 
            this.lbCarplay.AutoSize = true;
            this.lbCarplay.Location = new System.Drawing.Point(23, 85);
            this.lbCarplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCarplay.Name = "lbCarplay";
            this.lbCarplay.Size = new System.Drawing.Size(76, 16);
            this.lbCarplay.TabIndex = 5;
            this.lbCarplay.Text = "cur_playing";
            // 
            // cbCurplay
            // 
            this.cbCurplay.FormattingEnabled = true;
            this.cbCurplay.Location = new System.Drawing.Point(16, 112);
            this.cbCurplay.Margin = new System.Windows.Forms.Padding(4);
            this.cbCurplay.Name = "cbCurplay";
            this.cbCurplay.Size = new System.Drawing.Size(523, 24);
            this.cbCurplay.TabIndex = 4;
            this.cbCurplay.Validated += new System.EventHandler(this.cbCurplay_Validated);
            // 
            // lblpCopy
            // 
            this.lblpCopy.AutoSize = true;
            this.lblpCopy.Location = new System.Drawing.Point(617, 10);
            this.lblpCopy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblpCopy.Name = "lblpCopy";
            this.lblpCopy.Size = new System.Drawing.Size(95, 16);
            this.lblpCopy.TabIndex = 3;
            this.lblpCopy.Text = "копировать в";
            // 
            // cblpCopy
            // 
            this.cblpCopy.FormattingEnabled = true;
            this.cblpCopy.Location = new System.Drawing.Point(611, 37);
            this.cblpCopy.Margin = new System.Windows.Forms.Padding(4);
            this.cblpCopy.Name = "cblpCopy";
            this.cblpCopy.Size = new System.Drawing.Size(523, 24);
            this.cblpCopy.TabIndex = 2;
            this.cblpCopy.Validated += new System.EventHandler(this.cblpCopy_Validated);
            // 
            // lbLogger
            // 
            this.lbLogger.AutoSize = true;
            this.lbLogger.Location = new System.Drawing.Point(23, 10);
            this.lbLogger.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLogger.Name = "lbLogger";
            this.lbLogger.Size = new System.Drawing.Size(101, 16);
            this.lbLogger.TabIndex = 1;
            this.lbLogger.Text = "папка логгера";
            // 
            // cbLogger
            // 
            this.cbLogger.FormattingEnabled = true;
            this.cbLogger.Location = new System.Drawing.Point(16, 37);
            this.cbLogger.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogger.Name = "cbLogger";
            this.cbLogger.Size = new System.Drawing.Size(523, 24);
            this.cbLogger.TabIndex = 0;
            this.cbLogger.Validated += new System.EventHandler(this.cbLogger_Validated);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1277, 257);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(449, 70);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FileMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 554);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FileMonitor";
            this.Text = "FileMonitor 1.0";
            this.Load += new System.EventHandler(this.FileMonitor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}

