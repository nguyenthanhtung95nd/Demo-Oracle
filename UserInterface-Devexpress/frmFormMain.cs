using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace UserInterface_Devexpress
{
    public partial class frmFormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private string _tenDangNhap = string.Empty;

        public frmFormMain()
        {
            InitializeComponent();
        }

        public frmFormMain(string tenDangNhap)
        {
            InitializeComponent();
            this._tenDangNhap = tenDangNhap;
        }
        #region CheckFrom Active

        Form CheckForm(Type fType)
        {
            foreach (var f in this.MdiChildren)
            {
                if (f.GetType() == fType)
                {
                    return f;
                }
            }
            return null;
        }
        #endregion

        private void frmFormMain_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
            toolStripStatusLabelTK.Text = "Nhân viên : " + _tenDangNhap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabelTime.Text = "Hôm nay là ngày  " + DateTime.Now.ToString("dd/MM/yyyy") + "  -  Bây giờ là  " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void frmFormMain_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult warning = XtraMessageBox.Show("Bạn thật sự muốn thoát khỏi chương trình?", "Lưu ý!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (warning == DialogResult.Yes)
                Application.Exit();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = CheckForm(typeof(frmNhanVien));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmNhanVien fr = new frmNhanVien();
                fr.MdiParent = this;
                fr.Show();
            }
        }
    }
}