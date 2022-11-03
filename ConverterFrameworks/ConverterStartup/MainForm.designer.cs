using ConverterTools.Logging;

namespace ConverterStartup
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.richTextBoxLog = new ConverterTools.Logging.MessageListBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbAsmInfo = new System.Windows.Forms.GroupBox();
            this.gvAsmInfo = new System.Windows.Forms.DataGridView();
            this.cParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cParamVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripStart = new System.Windows.Forms.ToolStripButton();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbAsmInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAsmInfo)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(598, 209);
            this.richTextBoxLog.TabIndex = 12;
            this.richTextBoxLog.Text = "";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 414);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(628, 22);
            this.statusStrip.TabIndex = 26;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // backgroundWorker
            // 
            //this.backgroundWorker.WorkerReportsProgress = true;
            //this.backgroundWorker.WorkerSupportsCancellation = true;
            //this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            //this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            //this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.richTextBoxLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(604, 228);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Протокол конвертирования";
            // 
            // gbAsmInfo
            // 
            this.gbAsmInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAsmInfo.Controls.Add(this.gvAsmInfo);
            this.gbAsmInfo.Location = new System.Drawing.Point(12, 27);
            this.gbAsmInfo.Name = "gbAsmInfo";
            this.gbAsmInfo.Size = new System.Drawing.Size(604, 150);
            this.gbAsmInfo.TabIndex = 31;
            this.gbAsmInfo.TabStop = false;
            this.gbAsmInfo.Text = "Информация о сборке";
            // 
            // gvAsmInfo
            // 
            this.gvAsmInfo.AllowUserToAddRows = false;
            this.gvAsmInfo.AllowUserToDeleteRows = false;
            this.gvAsmInfo.AllowUserToResizeRows = false;
            this.gvAsmInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvAsmInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAsmInfo.ColumnHeadersVisible = false;
            this.gvAsmInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cParamName,
            this.cParamVal});
            this.gvAsmInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAsmInfo.Location = new System.Drawing.Point(3, 16);
            this.gvAsmInfo.Name = "gvAsmInfo";
            this.gvAsmInfo.ReadOnly = true;
            this.gvAsmInfo.RowHeadersVisible = false;
            this.gvAsmInfo.Size = new System.Drawing.Size(598, 131);
            this.gvAsmInfo.TabIndex = 0;
            // 
            // cParamName
            // 
            this.cParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cParamName.DataPropertyName = "ParamName";
            this.cParamName.Frozen = true;
            this.cParamName.HeaderText = "";
            this.cParamName.Name = "cParamName";
            this.cParamName.ReadOnly = true;
            // 
            // cParamVal
            // 
            this.cParamVal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cParamVal.DataPropertyName = "ParamVal";
            this.cParamVal.HeaderText = "";
            this.cParamVal.Name = "cParamVal";
            this.cParamVal.ReadOnly = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStripSettings,
            this.toolStripStart});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(628, 25);
            this.toolStrip1.TabIndex = 32;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpen.Image = global::ConverterStartup.Properties.Resources.folder_open;
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpen.Text = "toolStripOpen";
            this.toolStripOpen.ToolTipText = "Открыть";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // toolStripSettings
            // 
            this.toolStripSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSettings.Image = global::ConverterStartup.Properties.Resources.exec;
            this.toolStripSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSettings.Name = "toolStripSettings";
            this.toolStripSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripSettings.Text = "toolStripSettings";
            this.toolStripSettings.ToolTipText = "Параметры";
            this.toolStripSettings.Click += new System.EventHandler(this.toolStripSettings_Click);
            // 
            // toolStripStart
            // 
            this.toolStripStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStart.Image = global::ConverterStartup.Properties.Resources.bt_play;
            this.toolStripStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStart.Name = "toolStripStart";
            this.toolStripStart.Size = new System.Drawing.Size(23, 22);
            this.toolStripStart.Text = "toolStripStart";
            this.toolStripStart.ToolTipText = "Запуск";
            this.toolStripStart.Click += new System.EventHandler(this.toolStripStart_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 436);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gbAsmInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добро пожаловать";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbAsmInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAsmInfo)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MessageListBox richTextBoxLog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbAsmInfo;
        private System.Windows.Forms.DataGridView gvAsmInfo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripButton toolStripSettings;
        private System.Windows.Forms.ToolStripButton toolStripStart;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParamVal;
    }
}

