using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using HiStaff.Util;
using HiStaff.Domain;
using HiStaff.Dal;
using System.IO;

namespace HiStaff.Auto
{
    public partial class frmMain : Telerik.WinControls.UI.RadForm
    {
        private bool _manualGetData = false;
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            BindSetting();
            notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
            notifyIcon1.BalloonTipText = "Histaff - Professinal HRM Solution";
            notifyIcon1.ShowBalloonTip(5000);
        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
        private void BindSetting()
        {
            try
            {
                string pathFile = Application.StartupPath + "\\setting.ini";
                if (!System.IO.File.Exists(pathFile))
                {
                    return;
                }
                else
                {
                    IniFile iniFile = new IniFile(pathFile);
                    txtOrclUser.Text = iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLUSER);
                    txtOrclPass.Text = iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLPASSWORD);
                    txtOrclServer.Text = iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLSERVER);
                    txtOrclPort.Text = iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLPORT);
                    txtOrclService.Text = iniFile.IniReadValue(ORCL.NAME, ORCL.ORCLSERVICE);

                    txtSqlUser.Text = iniFile.IniReadValue(SQL.NAME, SQL.SQLUSER);
                    txtSqlPass.Text = iniFile.IniReadValue(SQL.NAME, SQL.SQLPASSWORD);
                    txtSqlServer.Text = iniFile.IniReadValue(SQL.NAME, SQL.SQLSERVER);
                    txtSqlDatabase.Text = iniFile.IniReadValue(SQL.NAME, SQL.SQLDATABASE);

                    DateTime time;

                    if (DateTime.TryParseExact(iniFile.IniReadValue(INOUT.NAME, INOUT.STARTHOUR), COMMON.FORMATHOUR, null, System.Globalization.DateTimeStyles.None, out time))
                    {
                        txtIOTime1.Value = time;
                    }
                    else
                    {
                        txtIOTime1.Value = null;
                    }
                    
                    if (DateTime.TryParseExact(iniFile.IniReadValue(INOUT.NAME, INOUT.ENDHOUR), COMMON.FORMATHOUR, null, System.Globalization.DateTimeStyles.None, out time))
                    {
                        txtIOTime2.Value = time;
                    }
                    else
                    {
                        txtIOTime2.Value = null;
                    }

                    txtPathSave.Text = iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE);
                    txtPathBackup.Text = iniFile.IniReadValue(INOUT.NAME, INOUT.PATHBACKUP);
                    chkReGet.Checked = Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.REGET));
                    chkDel.Checked = Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.DELFILE));
                    txtBackdate.Value = decimal.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.BACKDATE));

                    txtMin.Value = decimal.Parse(iniFile.IniReadValue(COMMON.NAME, COMMON.CYCLEGETDATA));
                    
                    timer1.Interval = (int)txtMin.Value * 60000;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "LoadSetting");
            }
        }
        private bool CheckBeforeSave()
        {
            if (!CheckSaveGroup(grCM.Controls))
                return false;
            if (!CheckSaveGroup(grOrcl.Controls))
                return false;
            if (!CheckSaveGroup(grSql.Controls))
                return false;
            if (!CheckSaveGroup(grIO.Controls))
                return false;
            return true;
        }
        private bool CheckSaveGroup(Control.ControlCollection container)
        {
            foreach (Control ctrl in container)
            {
                if (ctrl is RadTextBox)
                {
                    if (string.IsNullOrEmpty(((RadTextBox)ctrl).Text))
                    {
                        return false;
                    }
                }
                else if (ctrl is RadTimePicker)
                {
                    if (!((RadTimePicker)ctrl).Value.HasValue)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (CheckBeforeSave())
                {
                    string pathFile = Application.StartupPath + "\\setting.ini";
                    if (!System.IO.File.Exists(pathFile))
                    {
                        System.IO.File.Create(pathFile);
                    }
                    IniFile iniFile = new IniFile(pathFile);

                    iniFile.IniWriteValue(ORCL.NAME, ORCL.ORCLUSER, txtOrclUser.Text);
                    iniFile.IniWriteValue(ORCL.NAME, ORCL.ORCLPASSWORD, txtOrclPass.Text);
                    iniFile.IniWriteValue(ORCL.NAME, ORCL.ORCLSERVER, txtOrclServer.Text);
                    iniFile.IniWriteValue(ORCL.NAME, ORCL.ORCLPORT, txtOrclPort.Text);
                    iniFile.IniWriteValue(ORCL.NAME, ORCL.ORCLSERVICE, txtOrclService.Text);

                    iniFile.IniWriteValue(SQL.NAME, SQL.SQLUSER, txtSqlUser.Text);
                    iniFile.IniWriteValue(SQL.NAME, SQL.SQLPASSWORD, txtSqlPass.Text);
                    iniFile.IniWriteValue(SQL.NAME, SQL.SQLDATABASE, txtSqlDatabase.Text);
                    iniFile.IniWriteValue(SQL.NAME, SQL.SQLSERVER, txtSqlServer.Text);

                    iniFile.IniWriteValue(INOUT.NAME, INOUT.STARTHOUR, string.Format("{0:HH:mm:ss}", txtIOTime1.Value));
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.ENDHOUR, string.Format("{0:HH:mm:ss}", txtIOTime2.Value));
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.PATHSAVE, txtPathSave.Text);
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.PATHBACKUP, txtPathBackup.Text);
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.REGET, chkReGet.Checked.ToString());
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.DELFILE, chkDel.Checked.ToString());
                    iniFile.IniWriteValue(INOUT.NAME, INOUT.BACKDATE, txtBackdate.Value.ToString());

                    iniFile.IniWriteValue(COMMON.NAME, COMMON.CYCLEGETDATA, txtMin.Value.ToString());

                    timer1.Interval = (int)txtMin.Value * 60000;
                    alert.ContentText = COMMON.SAVESUCESS;
                    alert.Show();
                    DBConnection.New.NewConnection();
                }
                else
                {
                    alert.ContentText = COMMON.SAVENOTE;
                    alert.Show();
                }
            }
            catch (Exception ex)
            {
                alert.ContentText = COMMON.SAVEFAIL;
                alert.Show();
                Log.Instance.WriteExceptionLog(ex, "SaveSetting");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindSetting();
        }

        private void btnPathSave_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
            {
                txtPathSave.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnPathBackup_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
            {
                txtPathBackup.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
            notifyIcon1.BalloonTipText = "Histaff - Professinal HRM Solution";
            notifyIcon1.ShowBalloonTip(5000);
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                switch (e.ClickedItem.Name)
                {
                    case "setting":
                        this.Show();
                        this.WindowState = FormWindowState.Normal;
                        this.ShowInTaskbar = true;
                        break;
                    case "exit":
                        if (MessageBox.Show("Bạn muốn tắt hệ thống?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Application.Exit();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "contextMenuStrip1_ItemClicked");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string pathFile = Application.StartupPath + "\\setting.ini";
                if (!System.IO.File.Exists(pathFile))
                {
                    notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
                    notifyIcon1.BalloonTipText = "Hệ thống chưa được thiết lập thông số hệ thống...";
                    notifyIcon1.ShowBalloonTip(5000);
                    return;
                }
                ExcuteAsynchronous();
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "timer_Tick");
            }
        }
        private bool UPDATE_SWIPE_DATE(CO_SWIPE_DATA obj)
        {
            return OracleHelper.ExcuteNonQuery("PKG_ESS", "PRU_CO_SWIPE_DATA", obj);
        }
        private bool SaveDataInOut(List<CO_SWIPE_DATA> data, System.ComponentModel.BackgroundWorker bgWorker)
        {
            try
            {
                int i = 0;
                foreach (CO_SWIPE_DATA item in data)
                {
                    i++;
                    UPDATE_SWIPE_DATE(item);
                    double x = i * 100 / data.Count;
                    bgWorker.ReportProgress((int)x, "Lưu dữ liệu vào ra {0}%...");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, "Import Data IN/OUT");
                return false;
            }
        }
        private void ExcuteAsynchronous()
        {
            if (!bgwWorker.IsBusy)
            {
                bgwWorker.RunWorkerAsync();
            }
        }
        private bool AsynchronousFunctionWithBGW(System.ComponentModel.BackgroundWorker bgWorker)
        {
            try
            {
                string pathFile = Application.StartupPath + "\\setting.ini";
                IniFile iniFile = new IniFile(pathFile);

                //import động từ máy chấm công từ đường dẫn savefile ( webconfig )
                string fromHour = iniFile.IniReadValue(INOUT.NAME, INOUT.STARTHOUR);
                string toHour = iniFile.IniReadValue(INOUT.NAME, INOUT.ENDHOUR);
                DateTime FROM = DateTime.ParseExact(string.Format("{0:dd/MM/yyyy} {1}",
                    DateTime.Now, fromHour), "dd/MM/yyyy HH:mm:ss", null);
                DateTime TO = DateTime.ParseExact(string.Format("{0:dd/MM/yyyy} {1}",
                    DateTime.Now, toHour), "dd/MM/yyyy HH:mm:ss", null);

                double backdate = double.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.BACKDATE));

                if ((DateTime.Now >= FROM && DateTime.Now <= TO) || _manualGetData)
                {
                    bgWorker.ReportProgress(0, "Đọc dữ liệu vào ra {0}%...");
                    notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
                    notifyIcon1.BalloonTipText = "Đang lấy dữ liệu vào ra...";
                    notifyIcon1.ShowBalloonTip(100000);
                    string[] files = Directory.GetFiles(iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE), "*.xml");
                    string savefile;
                    string backupfile;
                    DateTime outDate;
                    List<CO_SWIPE_DATA> data = new List<CO_SWIPE_DATA>();
                    int currentFile = 0;
                    // Read data in/out from xml file
                    foreach (string file in files)
                    {
                        currentFile++;
                        if (File.Exists(file))
                        {
                            if (DateTime.TryParseExact(file.Replace(iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE), "").Substring(1, 8),
                                "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out outDate))
                            {
                                if (outDate >= DateTime.Today.AddDays((0 - backdate)))
                                {
                                    if (!Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.REGET)) ||
                                        !File.Exists(file.Replace(iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE), iniFile.IniReadValue(INOUT.NAME, INOUT.PATHBACKUP))))
                                        SXml.ReadDataInOut(file, outDate, data, bgWorker, currentFile, files.Length);
                                }
                            }
                        }
                    }
                    // Save data in/out
                    bgWorker.ReportProgress(0, "Lưu dữ liệu vào ra {0}%...");
                    SaveDataInOut(data, bgWorker);
                    // Backup xml file
                    currentFile = 0;
                    bgWorker.ReportProgress(0, "Đang backup dữ liệu...");
                    foreach (string file in files)
                    {
                        currentFile++;

                        savefile = file;
                        backupfile = file.Replace(iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE), iniFile.IniReadValue(INOUT.NAME, INOUT.PATHBACKUP));
                        //==N=======luu giu lai tat ca cac file da backup (Qua trinh doc file )
                        backupfile = backupfile + DateTime.Now.ToString ("yyyyMMddhhmmss");
                        //==N=======
                        if (DateTime.TryParseExact(file.Replace(iniFile.IniReadValue(INOUT.NAME, INOUT.PATHSAVE), "").Substring(1, 8),
                                "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out outDate))
                        {
                            if (outDate >= DateTime.Today.AddDays((0-backdate)))
                            {
                                if (File.Exists(backupfile))
                                {
                                    if (!Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.REGET)))
                                    {
                                        //luu giu lai tat ca cac file da backup (Qua trinh doc file )
                                        //File.Delete(backupfile);
                                        File.Copy(savefile, backupfile);
                                        if (Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.DELFILE)))
                                            File.Delete(savefile);
                                    }
                                }
                                else
                                {
                                    File.Copy(savefile, backupfile);
                                    if (Boolean.Parse(iniFile.IniReadValue(INOUT.NAME, INOUT.DELFILE)))
                                        File.Delete(savefile);
                                }
                            }
                        }
                        double x = (currentFile * 100) / files.Length;
                        bgWorker.ReportProgress((int)x, string.Format("Đang backup dữ liệu ({0})", file));
                    }
                    // Transfer salary data into sql naviworld
                    bgWorker.ReportProgress(0, "Đọc dữ liệu kế toán ({0})...");
                    List<SALCOSTCENTER> salData = OracleHelper.ExcuteSelectMultiObject<SALCOSTCENTER>("PKG_HCM_IPAY", "PRS_SAL_COSTCONTER");
                    bgWorker.ReportProgress(100, "Hoàn thành đọc dữ liệu ({0})...");
                    if (salData.Count > 0)
                    {
                        bgWorker.ReportProgress(0, "Chuyển dữ liệu kế toán ({0})...");
                        //if (!SqlHelper.CheckExsistStoreProc(COMMON.PROC_TVC_PRI_SAL))
                        //{
                        //    SqlHelper.ExcuteScript(File.ReadAllText(Application.StartupPath + "\\Lib\\proc.sql")); 
                        //}
                        string commandText = System.IO.File.ReadAllText(Application.StartupPath + "\\Lib\\commandText.sql");
                        for (int i = 0; i < salData.Count; i++)
                        {
                            if (SqlHelper.ExcuteCommandText(commandText, salData[i]))
                            {
                                double y = (i * 100) / salData.Count;
                                bgWorker.ReportProgress((int)y, "Chuyển dữ liệu kế toán ({0})...");
                                OracleHelper.ExcuteNonQuery("PKG_HCM_IPAY", "PRU_SAL_COSTCENTER", salData[i]);
                            }
                        }
                        
                    }
                }
                _manualGetData = false;
            }
            catch (Exception ex)
            {
                HiStaff.Util.Log.Instance.WriteExceptionLog(ex, "Auto trans data");
                bgWorker.ReportProgress(0, ex.Message);
                _manualGetData = false;
                return false;
            }
            return true;
        }
        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = AsynchronousFunctionWithBGW((BackgroundWorker)sender);
        }

        private void bgwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                processbar.Value1 = e.ProgressPercentage;
                processbar.Text = string.Format(e.UserState.ToString(), e.ProgressPercentage);
            }
            catch (Exception ex)
            {
                Log.Instance.WriteExceptionLog(ex, this.Name + "[" + this.Text + "]");
            }
        }

        private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (notifyIcon1.BalloonTipText == "Đang lấy dữ liệu vào ra...")
            {
                processbar.Value1 = 100;
                processbar.Text = string.Format("Hoàn thành. [{0:dd/MM/yyyy HH:mm:ss}]", DateTime.Now);
                notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
                notifyIcon1.BalloonTipText = "Hoàn thành việc lấy dữ liệu vào ra";
                notifyIcon1.ShowBalloonTip(5000);
            }
        }

        private void btnTestOrcl_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBConnection.New.TestConnection(txtOrclUser.Text, 
                    txtOrclPass.Text, txtOrclServer.Text, 
                    txtOrclPort.Text, txtOrclService.Text))
                {
                    MessageBox.Show("Kết nối thành công.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kết nối không thành công. Vui lòng kiểm tra lại các tham số kết nối", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            string pathFile = Application.StartupPath + "\\setting.ini";
            if (!System.IO.File.Exists(pathFile))
            {
                notifyIcon1.BalloonTipTitle = "Histaff - Professinal HRM Solution";
                notifyIcon1.BalloonTipText = "Hệ thống chưa được thiết lập thông số hệ thống...";
                notifyIcon1.ShowBalloonTip(5000);
                return;
            }
            _manualGetData = true;
            ExcuteAsynchronous();
        }

        private void btnTestSql_Click(object sender, EventArgs e)
        {
            try
            {
                if (DbSqlConnection.TestConnection(txtSqlServer.Text, txtSqlUser.Text, txtSqlPass.Text, txtSqlDatabase.Text))
                {
                    MessageBox.Show("Kết nối thành công.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kết nối không thành công. Vui lòng kiểm tra lại các tham số kết nối", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
