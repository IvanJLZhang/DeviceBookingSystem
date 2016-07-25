namespace DataBackupTool
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.gb_srcFilePath = new System.Windows.Forms.GroupBox();
            this.btn_Pause = new System.Windows.Forms.Button();
            this.btn_StartToBackup = new System.Windows.Forms.Button();
            this.btn_srcPathRemove = new System.Windows.Forms.Button();
            this.btn_srcPathAdd = new System.Windows.Forms.Button();
            this.lv_srcPath = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gb_dbBackup = new System.Windows.Forms.GroupBox();
            this.btn_PauseDB = new System.Windows.Forms.Button();
            this.btn_StartToBackupDB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_connectionStr = new System.Windows.Forms.TextBox();
            this.gb_TargetPath = new System.Windows.Forms.GroupBox();
            this.btn_TargetPathRemove = new System.Windows.Forms.Button();
            this.btn_TargetPathAdd = new System.Windows.Forms.Button();
            this.lv_TargetPath = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pg_operaProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tssl_progressText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslb_displayProcessInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_Backup = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer_MailService = new System.Windows.Forms.Timer(this.components);
            this.timer_DailyEvent = new System.Windows.Forms.Timer(this.components);
            this.gb_srcFilePath.SuspendLayout();
            this.gb_dbBackup.SuspendLayout();
            this.gb_TargetPath.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_srcFilePath
            // 
            this.gb_srcFilePath.Controls.Add(this.btn_Pause);
            this.gb_srcFilePath.Controls.Add(this.btn_StartToBackup);
            this.gb_srcFilePath.Controls.Add(this.btn_srcPathRemove);
            this.gb_srcFilePath.Controls.Add(this.btn_srcPathAdd);
            this.gb_srcFilePath.Controls.Add(this.lv_srcPath);
            this.gb_srcFilePath.Location = new System.Drawing.Point(12, 12);
            this.gb_srcFilePath.Name = "gb_srcFilePath";
            this.gb_srcFilePath.Size = new System.Drawing.Size(499, 190);
            this.gb_srcFilePath.TabIndex = 0;
            this.gb_srcFilePath.TabStop = false;
            this.gb_srcFilePath.Text = "File Data backup";
            // 
            // btn_Pause
            // 
            this.btn_Pause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Pause.Enabled = false;
            this.btn_Pause.Location = new System.Drawing.Point(415, 109);
            this.btn_Pause.Name = "btn_Pause";
            this.btn_Pause.Size = new System.Drawing.Size(65, 65);
            this.btn_Pause.TabIndex = 4;
            this.btn_Pause.Text = "PAUSE";
            this.btn_Pause.UseVisualStyleBackColor = true;
            this.btn_Pause.Click += new System.EventHandler(this.btn_Pause_Click);
            // 
            // btn_StartToBackup
            // 
            this.btn_StartToBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_StartToBackup.Location = new System.Drawing.Point(328, 109);
            this.btn_StartToBackup.Name = "btn_StartToBackup";
            this.btn_StartToBackup.Size = new System.Drawing.Size(65, 65);
            this.btn_StartToBackup.TabIndex = 3;
            this.btn_StartToBackup.Text = "GO";
            this.btn_StartToBackup.UseVisualStyleBackColor = true;
            this.btn_StartToBackup.Click += new System.EventHandler(this.StartToBackup_Click);
            // 
            // btn_srcPathRemove
            // 
            this.btn_srcPathRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_srcPathRemove.Enabled = false;
            this.btn_srcPathRemove.Location = new System.Drawing.Point(415, 28);
            this.btn_srcPathRemove.Name = "btn_srcPathRemove";
            this.btn_srcPathRemove.Size = new System.Drawing.Size(65, 65);
            this.btn_srcPathRemove.TabIndex = 2;
            this.btn_srcPathRemove.Text = "REMOVE";
            this.btn_srcPathRemove.UseVisualStyleBackColor = true;
            this.btn_srcPathRemove.Click += new System.EventHandler(this.btn_srcPathRemove_Click);
            // 
            // btn_srcPathAdd
            // 
            this.btn_srcPathAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_srcPathAdd.Location = new System.Drawing.Point(328, 28);
            this.btn_srcPathAdd.Name = "btn_srcPathAdd";
            this.btn_srcPathAdd.Size = new System.Drawing.Size(65, 65);
            this.btn_srcPathAdd.TabIndex = 1;
            this.btn_srcPathAdd.Text = "ADD";
            this.btn_srcPathAdd.UseVisualStyleBackColor = true;
            this.btn_srcPathAdd.Click += new System.EventHandler(this.btn_srcPathAdd_Click);
            // 
            // lv_srcPath
            // 
            this.lv_srcPath.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lv_srcPath.FullRowSelect = true;
            this.lv_srcPath.GridLines = true;
            this.lv_srcPath.Location = new System.Drawing.Point(19, 28);
            this.lv_srcPath.MultiSelect = false;
            this.lv_srcPath.Name = "lv_srcPath";
            this.lv_srcPath.ShowItemToolTips = true;
            this.lv_srcPath.Size = new System.Drawing.Size(291, 146);
            this.lv_srcPath.TabIndex = 0;
            this.lv_srcPath.UseCompatibleStateImageBehavior = false;
            this.lv_srcPath.View = System.Windows.Forms.View.Details;
            this.lv_srcPath.SelectedIndexChanged += new System.EventHandler(this.lv_srcPath_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Source Path List";
            this.columnHeader1.Width = 277;
            // 
            // gb_dbBackup
            // 
            this.gb_dbBackup.Controls.Add(this.btn_PauseDB);
            this.gb_dbBackup.Controls.Add(this.btn_StartToBackupDB);
            this.gb_dbBackup.Controls.Add(this.label1);
            this.gb_dbBackup.Controls.Add(this.tb_connectionStr);
            this.gb_dbBackup.Location = new System.Drawing.Point(12, 208);
            this.gb_dbBackup.Name = "gb_dbBackup";
            this.gb_dbBackup.Size = new System.Drawing.Size(499, 141);
            this.gb_dbBackup.TabIndex = 1;
            this.gb_dbBackup.TabStop = false;
            this.gb_dbBackup.Text = "Database backup";
            // 
            // btn_PauseDB
            // 
            this.btn_PauseDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_PauseDB.Enabled = false;
            this.btn_PauseDB.Location = new System.Drawing.Point(415, 65);
            this.btn_PauseDB.Name = "btn_PauseDB";
            this.btn_PauseDB.Size = new System.Drawing.Size(65, 65);
            this.btn_PauseDB.TabIndex = 6;
            this.btn_PauseDB.Text = "PAUSE";
            this.btn_PauseDB.UseVisualStyleBackColor = true;
            this.btn_PauseDB.Click += new System.EventHandler(this.btn_PauseDB_Click);
            // 
            // btn_StartToBackupDB
            // 
            this.btn_StartToBackupDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_StartToBackupDB.Enabled = false;
            this.btn_StartToBackupDB.Location = new System.Drawing.Point(328, 65);
            this.btn_StartToBackupDB.Name = "btn_StartToBackupDB";
            this.btn_StartToBackupDB.Size = new System.Drawing.Size(65, 65);
            this.btn_StartToBackupDB.TabIndex = 5;
            this.btn_StartToBackupDB.Text = "GO";
            this.btn_StartToBackupDB.UseVisualStyleBackColor = true;
            this.btn_StartToBackupDB.Click += new System.EventHandler(this.btn_StartToBackupDB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection String:";
            // 
            // tb_connectionStr
            // 
            this.tb_connectionStr.Location = new System.Drawing.Point(109, 32);
            this.tb_connectionStr.Name = "tb_connectionStr";
            this.tb_connectionStr.Size = new System.Drawing.Size(371, 20);
            this.tb_connectionStr.TabIndex = 0;
            // 
            // gb_TargetPath
            // 
            this.gb_TargetPath.Controls.Add(this.btn_TargetPathRemove);
            this.gb_TargetPath.Controls.Add(this.btn_TargetPathAdd);
            this.gb_TargetPath.Controls.Add(this.lv_TargetPath);
            this.gb_TargetPath.Location = new System.Drawing.Point(12, 355);
            this.gb_TargetPath.Name = "gb_TargetPath";
            this.gb_TargetPath.Size = new System.Drawing.Size(499, 178);
            this.gb_TargetPath.TabIndex = 2;
            this.gb_TargetPath.TabStop = false;
            this.gb_TargetPath.Text = "Target Setting";
            // 
            // btn_TargetPathRemove
            // 
            this.btn_TargetPathRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_TargetPathRemove.Enabled = false;
            this.btn_TargetPathRemove.Location = new System.Drawing.Point(415, 58);
            this.btn_TargetPathRemove.Name = "btn_TargetPathRemove";
            this.btn_TargetPathRemove.Size = new System.Drawing.Size(65, 65);
            this.btn_TargetPathRemove.TabIndex = 8;
            this.btn_TargetPathRemove.Text = "REMOVE";
            this.btn_TargetPathRemove.UseVisualStyleBackColor = true;
            this.btn_TargetPathRemove.Click += new System.EventHandler(this.btn_TargetPathRemove_Click);
            // 
            // btn_TargetPathAdd
            // 
            this.btn_TargetPathAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_TargetPathAdd.Location = new System.Drawing.Point(328, 58);
            this.btn_TargetPathAdd.Name = "btn_TargetPathAdd";
            this.btn_TargetPathAdd.Size = new System.Drawing.Size(65, 65);
            this.btn_TargetPathAdd.TabIndex = 7;
            this.btn_TargetPathAdd.Text = "ADD";
            this.btn_TargetPathAdd.UseVisualStyleBackColor = true;
            this.btn_TargetPathAdd.Click += new System.EventHandler(this.btn_TargetPathAdd_Click);
            // 
            // lv_TargetPath
            // 
            this.lv_TargetPath.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lv_TargetPath.FullRowSelect = true;
            this.lv_TargetPath.GridLines = true;
            this.lv_TargetPath.Location = new System.Drawing.Point(19, 29);
            this.lv_TargetPath.MultiSelect = false;
            this.lv_TargetPath.Name = "lv_TargetPath";
            this.lv_TargetPath.ShowItemToolTips = true;
            this.lv_TargetPath.Size = new System.Drawing.Size(291, 121);
            this.lv_TargetPath.TabIndex = 6;
            this.lv_TargetPath.UseCompatibleStateImageBehavior = false;
            this.lv_TargetPath.View = System.Windows.Forms.View.Details;
            this.lv_TargetPath.SelectedIndexChanged += new System.EventHandler(this.lv_TargetPath_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Target Path List";
            this.columnHeader2.Width = 277;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pg_operaProgress,
            this.tssl_progressText,
            this.tsslb_displayProcessInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 536);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(523, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pg_operaProgress
            // 
            this.pg_operaProgress.Maximum = 0;
            this.pg_operaProgress.Name = "pg_operaProgress";
            this.pg_operaProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // tssl_progressText
            // 
            this.tssl_progressText.Name = "tssl_progressText";
            this.tssl_progressText.Size = new System.Drawing.Size(27, 17);
            this.tssl_progressText.Text = "0/0";
            // 
            // tsslb_displayProcessInfo
            // 
            this.tsslb_displayProcessInfo.Name = "tsslb_displayProcessInfo";
            this.tsslb_displayProcessInfo.Size = new System.Drawing.Size(20, 17);
            this.tsslb_displayProcessInfo.Text = "Hi";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Data Backup";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.hideWindowToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 70);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.showWindowToolStripMenuItem.Text = "&Show Window";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.showWindowToolStripMenuItem_Click);
            // 
            // hideWindowToolStripMenuItem
            // 
            this.hideWindowToolStripMenuItem.Name = "hideWindowToolStripMenuItem";
            this.hideWindowToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.hideWindowToolStripMenuItem.Text = "&Hide Window";
            this.hideWindowToolStripMenuItem.Click += new System.EventHandler(this.hideWindowToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // timer_Backup
            // 
            this.timer_Backup.Interval = 10000;
            this.timer_Backup.Tick += new System.EventHandler(this.timer_Backup_Tick);
            // 
            // timer_MailService
            // 
            this.timer_MailService.Interval = 1000;
            this.timer_MailService.Tick += new System.EventHandler(this.timer_MailService_Tick);
            // 
            // timer_DailyEvent
            // 
            this.timer_DailyEvent.Interval = 1000;
            this.timer_DailyEvent.Tick += new System.EventHandler(this.timer_DailyEvent_Tick);
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 558);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gb_TargetPath);
            this.Controls.Add(this.gb_dbBackup);
            this.Controls.Add(this.gb_srcFilePath);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Data Backup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.SizeChanged += new System.EventHandler(this.MainWindow_SizeChanged);
            this.gb_srcFilePath.ResumeLayout(false);
            this.gb_dbBackup.ResumeLayout(false);
            this.gb_dbBackup.PerformLayout();
            this.gb_TargetPath.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_srcFilePath;
        private System.Windows.Forms.GroupBox gb_dbBackup;
        private System.Windows.Forms.ListView lv_srcPath;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btn_srcPathRemove;
        private System.Windows.Forms.Button btn_srcPathAdd;
        private System.Windows.Forms.Button btn_Pause;
        private System.Windows.Forms.Button btn_StartToBackup;
        private System.Windows.Forms.GroupBox gb_TargetPath;
        private System.Windows.Forms.Button btn_TargetPathRemove;
        private System.Windows.Forms.Button btn_TargetPathAdd;
        private System.Windows.Forms.ListView lv_TargetPath;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_connectionStr;
        private System.Windows.Forms.Button btn_PauseDB;
        private System.Windows.Forms.Button btn_StartToBackupDB;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar pg_operaProgress;
        private System.Windows.Forms.ToolStripStatusLabel tssl_progressText;
        private System.Windows.Forms.ToolStripStatusLabel tsslb_displayProcessInfo;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Timer timer_Backup;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer timer_MailService;
        private System.Windows.Forms.Timer timer_DailyEvent;
    }
}

