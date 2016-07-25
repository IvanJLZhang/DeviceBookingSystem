using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataBackupTool
{
    public partial class MainWindow : Form
    {


        #region property
        TimeSpan triggerTime = new TimeSpan(09, 30, 0);// 定为每日9点30分执行
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            SettingConfig.m_configFilePathName = Application.ExecutablePath + ".config";
            ShowConfigData();

            // Send remindMailServer
            this.timer_MailService.Start();

            // Daily timer internal
            TimeSpan ts = GetTimeInternal();
            this.timer_DailyEvent.Interval = (Int32)ts.TotalMilliseconds;
            this.timer_DailyEvent.Start();
            // 程序开始即执行一次
            //timer_DailyEvent_Tick(null, null);
        }

        private void ShowConfigData()
        {
            if (m_config.value_SourcePathList.CompareTo(String.Empty) != 0)
            {
                string[] srcPathList = m_config.value_SourcePathList.Split('#');
                for (int index = 0; index != srcPathList.Length; index++)
                {
                    if (Directory.Exists(srcPathList[index]))
                        this.lv_srcPath.Items.Add(srcPathList[index]);
                }
            }

            this.tb_connectionStr.Text = m_config.value_ConnectionString;

            if (m_config.value_TargetPathList.CompareTo(String.Empty) != 0)
            {
                string[] targetPathList = m_config.value_TargetPathList.Split('#');
                for (int index = 0; index != targetPathList.Length; index++)
                {
                    if (Directory.Exists(targetPathList[index]))
                        this.lv_TargetPath.Items.Add(targetPathList[index]);
                }
            }
        }
        private SettingConfig m_config = new SettingConfig();
        #region control methods
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                // 取消关闭操作， 改为隐藏窗体
            }
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();// 给窗体焦点
        }

        private void hideWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quit the tool? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                this.notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();// 释放资源
                Application.Exit();// 关闭应用程序
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.notifyIcon1.Visible = true;
            }
        }

        private void btn_srcPathAdd_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem item = new ListViewItem(this.folderBrowserDialog1.SelectedPath);
                if (!this.lv_srcPath.Items.Contains(item))
                {
                    this.lv_srcPath.Items.Add(this.folderBrowserDialog1.SelectedPath);
                    m_config.value_SourcePathList = m_config.value_SourcePathList + "#" + this.folderBrowserDialog1.SelectedPath;
                }
            }
        }
        private void btn_srcPathRemove_Click(object sender, EventArgs e)
        {
            if (this.lv_srcPath.SelectedItems.Count > 0)
            {
                string selectStr = this.lv_srcPath.SelectedItems[0].Text;
                this.lv_srcPath.Items.Remove(this.lv_srcPath.SelectedItems[0]);
                string srcPathList = m_config.value_SourcePathList;
                if (selectStr.CompareTo(String.Empty) != 0)
                {
                    srcPathList = srcPathList.Replace("#" + selectStr, "");
                    m_config.value_SourcePathList = srcPathList;
                }
            }
        }

        private void btn_TargetPathAdd_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem item = new ListViewItem(this.folderBrowserDialog1.SelectedPath);
                if (!this.lv_TargetPath.Items.Contains(item))
                {
                    this.lv_TargetPath.Items.Add(this.folderBrowserDialog1.SelectedPath);
                    m_config.value_TargetPathList = m_config.value_TargetPathList + "#" + this.folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void btn_TargetPathRemove_Click(object sender, EventArgs e)
        {
            if (this.lv_TargetPath.SelectedItems.Count > 0)
            {
                string selectStr = this.lv_TargetPath.SelectedItems[0].Text;
                this.lv_TargetPath.Items.Remove(this.lv_TargetPath.SelectedItems[0]);
                string srcPathList = m_config.value_TargetPathList;
                if (selectStr.CompareTo(String.Empty) != 0)
                {
                    srcPathList = srcPathList.Replace("#" + selectStr, "");
                    m_config.value_TargetPathList = srcPathList;
                }
            }
        }

        private void StartToBackup_Click(object sender, EventArgs e)
        {
            this.btn_Pause.Enabled = true;
            this.btn_StartToBackup.Enabled = false;

            this.timer_Backup.Interval = 100;
            this.timer_Backup.Start();
        }
        private void btn_Pause_Click(object sender, EventArgs e)
        {
            this.timer_Backup.Stop();
            this.btn_StartToBackup.Enabled = true;
            this.btn_Pause.Enabled = false;
        }

        private void timer_MailService_Tick(object sender, EventArgs e)
        {
            timer_MailService.Stop();
            MailService mailService = new MailService();
            if (!mailService.SendMailAuto())
            {
                if (mailService.errMsg.CompareTo("no data") != 0)
                {// Write errLog

                }
            }

            timer_MailService.Start();
        }

        private void timer_Backup_Tick(object sender, EventArgs e)
        {
            this.timer_Backup.Stop();
            ArrayList srcPathList = new ArrayList();
            ArrayList dstPathList = new ArrayList();
            foreach (ListViewItem o in this.lv_srcPath.Items)
                srcPathList.Add(o.Text);
            foreach (ListViewItem o in this.lv_TargetPath.Items)
                dstPathList.Add(o.Text);

            if (srcPathList.Count <= 0 || dstPathList.Count <= 0)
            {
                this.timer_Backup.Interval = 10000;
                this.timer_Backup.Start();
                this.btn_StartToBackup.Enabled = true;
                this.btn_Pause.Enabled = false;
                return;
            }

            ManualResetEvent threadEvent = new ManualResetEvent(false);
            Thread BackupThread = new Thread(() =>
                {
                    for (int index = 0; index != srcPathList.Count; index++)
                    {
                        string srcPath = srcPathList[index].ToString();
                        for (int indey = 0; indey != dstPathList.Count; indey++)
                        {
                            string dstPath = dstPathList[indey].ToString();
                            CopyFolder(srcPath, dstPath);
                        }
                    }
                    threadEvent.Set();
                });
            //FileDataBackup();
            BackupThread.Start();
            threadEvent.WaitOne();
            this.timer_Backup.Interval = 10000;
            this.timer_Backup.Start();
        }
        #endregion

        private void FileDataBackup()
        {
            ArrayList srcPathList = new ArrayList();
            ArrayList dstPathList = new ArrayList();
            foreach (ListViewItem o in this.lv_srcPath.Items)
                srcPathList.Add(o.Text);
            foreach (ListViewItem o in this.lv_TargetPath.Items)
                dstPathList.Add(o.Text);
            if (srcPathList.Count <= 0 || dstPathList.Count <= 0)
                return;

            for (int index = 0; index != srcPathList.Count; index++)
            {
                string srcPath = srcPathList[index].ToString();
                for (int indey = 0; indey != dstPathList.Count; indey++)
                {
                    string dstPath = dstPathList[indey].ToString();
                    CopyFolder(srcPath, dstPath);
                }
            }

        }

        private void CopyFolder(string srcPath, string dstPath)
        {
            DirectoryInfo srcPathDir = new DirectoryInfo(srcPath);
            DirectoryInfo dstPathDir = new DirectoryInfo(dstPath);
            if (!srcPathDir.Exists || !dstPathDir.Exists)
                return;
            //DirectoryInfo srcParentDir = srcPathDir.Parent;
            if (!Directory.Exists(dstPath + @"\" + srcPathDir.Name))
            {
                dstPathDir = dstPathDir.CreateSubdirectory(srcPathDir.Name);
            }
            else
            {
                dstPathDir = new DirectoryInfo(dstPath + @"\" + srcPathDir.Name);
            }
            FileInfo[] srcFileArr = srcPathDir.GetFiles();
            //FileInfo[] dstFileArr = dstPathDir.GetFiles();

            // 正向拷贝文件
            foreach (FileInfo srcFile in srcFileArr)
            {
                FileInfo dstFile = new FileInfo(dstPathDir.FullName + @"\" + srcFile.Name);
                if (dstFile.Exists)
                {
                    CopyFile(srcFile, dstFile);
                }
                else
                {
                    srcFile.CopyTo(dstFile.FullName);
                    DebugLog("Copy File--From: " + srcFile.FullName + "\tTO: " + dstFile.FullName);
                }
            }
            // 反向删除文件
            FileInfo[] dstFileArr = dstPathDir.GetFiles();
            foreach (FileInfo dstFile in dstFileArr)
            {
                FileInfo srcFile = new FileInfo(srcPathDir.FullName + @"\" + dstFile.Name);
                if (!srcFile.Exists)
                {
                    dstFile.Delete();
                    DebugLog("Delete File--From: " + srcFile.FullName + "\tTO: " + dstFile.FullName);
                }

            }
        }
        ArrayList SrcFileList = new ArrayList();
        private void GetAllFilesUnderPath(DirectoryInfo dirInfo)
        {
            FileInfo[] fileArr = dirInfo.GetFiles();
            SrcFileList.AddRange(fileArr);

            DirectoryInfo[] dirArr = dirInfo.GetDirectories();
            foreach (DirectoryInfo dir in dirArr)
                GetAllFilesUnderPath(dir);

        }
        private void CopyFile(FileInfo srcFile, FileInfo dstFile)
        {
            if (!(srcFile.Name.CompareTo(dstFile.Name) == 0 && srcFile.LastWriteTime.CompareTo(dstFile.LastWriteTime) == 0))
            {
                File.Copy(srcFile.FullName, dstFile.FullName, true);
                DebugLog("Copy File--From: " + srcFile.FullName + "\tTO: " + dstFile.FullName);
            }
        }

        /// <summary>
        /// 获取MD5值32位
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public string GetMD5Str(FileStream strSrc)
        {
            MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Bytes = md5.ComputeHash(strSrc);

            StringBuilder sbMd5Str = new StringBuilder();
            for (int index = 0; index != md5Bytes.Length; index++)
            {
                sbMd5Str.Append(md5Bytes[index]);
            }
            return sbMd5Str.ToString();
        }

        #region 控件的线程表示方法

        private delegate void ActiveWindowInvoke();
        private void ActiveWindow()
        {
            ActiveWindowInvoke display = new ActiveWindowInvoke(ActiveWindowFunc);
            this.BeginInvoke(display);
        }
        private void ActiveWindowFunc()
        {
            this.Activate();
        }
        //System.IntPtr intPartEx;
        private void MessageBoxEx(string text, string caption = "")
        {
            //IntPtr intPtr = GetForegroundWindow();
            //WindowWrapper parentFrm = new WindowWrapper(intPartEx);
            MessageBox.Show(text, caption);
            //Thread.CurrentThread.Abort();
        }


        // 显示运行日志
        private delegate void DisplayInfoInvoke(string text);

        private void NoScanDir(string path)
        {
            File.AppendAllText(System.Environment.CurrentDirectory + "\\NoScanDir.txt", path + "\r\n");
        }

        private void DebugLog(string text)
        {
            File.AppendAllText(System.Environment.CurrentDirectory + "\\debug.txt", text + "\r\n");
        }
        //private void DisplayInfo(string text)
        //{
        //    DisplayInfoInvoke display = new DisplayInfoInvoke(DisplayInfoFunc);
        //    this.BeginInvoke(display, new object[] { text });
        //}
        //private void DisplayInfoFunc(string text)
        //{
        //    //this.Activate();
        //    //intPartEx = this.Handle;
        //    lv_DisplayInfo.Items.Insert(0, DateTime.Now.ToLongTimeString() + ": " + text);
        //    lv_DisplayInfo.Items[0].Selected = true;
        //    File.AppendAllText(System.Environment.CurrentDirectory + "\\debug.txt", DateTime.Now.ToLongTimeString() + ": " + text + "\r\n");
        //}
        // 显示操作信息
        private void DisplayProcessInfo(string text)
        {
            DisplayInfoInvoke display = new DisplayInfoInvoke(DisplayProcessFunc);
            this.BeginInvoke(display, new object[] { text });
        }
        private void DisplayProcessFunc(string text)
        {
            //this.Activate();
            this.tsslb_displayProcessInfo.Text = text;
            File.AppendAllText(System.Environment.CurrentDirectory + "\\debug.txt", "----" + text + "\r\n");
        }


        // 设置进度条信息
        /// <summary>
        /// 初始化进度条
        /// </summary>
        private void InitProgressBar()
        {
            InitProgressBarInvoke display = new InitProgressBarInvoke(InitProgressBarFunc);
            this.BeginInvoke(display);
        }
        private delegate void InitProgressBarInvoke();
        private void InitProgressBarFunc()
        {
            //this.Activate();
            pg_operaProgress.Value = 0;
            pg_operaProgress.Maximum = 0;
            tssl_progressText.Text = "0/0";
        }
        /// <summary>
        /// 更新当前值
        /// </summary>
        /// <param name="value"></param>
        private void NewProgressValue(int value)
        {
            NewProgressValueInvoke display = new NewProgressValueInvoke(NewProgressValueFunc);
            this.BeginInvoke(display, new object[] { value });
        }
        private delegate void NewProgressValueInvoke(int value);
        private void NewProgressValueFunc(int value)
        {
            //pg_operaProgress.Value = value;
            //this.Activate();
            if (pg_operaProgress.Value > pg_operaProgress.Maximum)
            {
                pg_operaProgress.Maximum = 0;
                pg_operaProgress.Value = 0;
                tssl_progressText.Text = "0/0";
            }
            else
            {
                //pg_operaProgress.Step = value;
                pg_operaProgress.Value = value;
                tssl_progressText.Text = pg_operaProgress.Value + "/" + pg_operaProgress.Maximum;
            }
        }
        /// <summary>
        /// 设计最大值
        /// </summary>
        /// <param name="value"></param>
        private void SetMaxNumOfPg(int value)
        {
            SetMaxNumOfPgInvoke setValue = new SetMaxNumOfPgInvoke(SetMaxNumOfPgFunc);
            this.BeginInvoke(setValue, new object[] { value });
        }
        private delegate void SetMaxNumOfPgInvoke(int value);
        private void SetMaxNumOfPgFunc(int value)
        {
            //this.Activate();
            pg_operaProgress.Maximum = value;
            string sTemp = tssl_progressText.Text;
            tssl_progressText.Text = sTemp.Substring(0, sTemp.LastIndexOf('/') + 1) + pg_operaProgress.Maximum;
        }


        // 设置listView的Item
        private delegate void AddLvItemInvoke(ListView lv, string item);
        private void AddLvItem(ListView lv, string item)
        {
            AddLvItemInvoke addItem = new AddLvItemInvoke(AddLvItemFunc);
            this.BeginInvoke(addItem, new object[] { lv, item });
        }
        private void AddLvItemFunc(ListView lv, string item)
        {
            //this.Activate();
            lv.Items.Add(item);
        }
        // 设置控件状态
        private delegate void SetCtrlEnabledInvoke(Control ctrl, bool state);
        private void SetCtrlEnabledFunc(Control ctrl, bool state)
        {
            //this.Activate();
            var form = (Form)ctrl;
            form.Enabled = state;
            //form.TopMost = !state;
            //this.lv_DisplayInfo.Enabled = true;
        }
        private void SetCtrlEnabled(Control ctrl, bool state)
        {
            SetCtrlEnabledInvoke setState = new SetCtrlEnabledInvoke(SetCtrlEnabledFunc);
            this.BeginInvoke(setState, new object[] { ctrl, state });
        }
        #endregion

        private void btn_StartToBackupDB_Click(object sender, EventArgs e)
        {
            this.btn_StartToBackupDB.Enabled = false;
            this.btn_PauseDB.Enabled = true;
        }

        private void btn_PauseDB_Click(object sender, EventArgs e)
        {
            this.btn_StartToBackupDB.Enabled = true;
            this.btn_PauseDB.Enabled = false;
        }

        private void lv_srcPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_srcPath.SelectedItems.Count <= 0)
                this.btn_srcPathRemove.Enabled = false;
            else
                this.btn_srcPathRemove.Enabled = true;
        }

        private void lv_TargetPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lv_TargetPath.SelectedItems.Count <= 0)
                this.btn_TargetPathRemove.Enabled = false;
            else
                this.btn_TargetPathRemove.Enabled = true;
        }
        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        /// <summary>
        /// Every day event(设定为9:30)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_DailyEvent_Tick(object sender, EventArgs e)
        {
            this.timer_DailyEvent.Stop();

            RecordMsgHandler recordhandler = new RecordMsgHandler();
            recordhandler.SendCheckMail();
            recordhandler.SendWarningMail();

            Thread.Sleep(1000 * 60);// 睡眠1分钟以后重新开启定时器
            this.timer_DailyEvent.Interval = (Int32)GetTimeInternal().TotalMilliseconds;
            this.timer_DailyEvent.Start();
        }

        TimeSpan GetTimeInternal(){
            TimeSpan time_internal = TimeSpan.Zero;
            TimeSpan timeNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            if (timeNow < triggerTime)
            {
                time_internal = triggerTime - timeNow;
            }
            else
            {
                time_internal = new TimeSpan(24, 0, 0) - timeNow + triggerTime;
            }
            return time_internal;
        }
    }
}
