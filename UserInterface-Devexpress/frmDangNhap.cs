using DataAccess;
using DevExpress.XtraEditors;
using Domain;
using System;
using System.Windows.Forms;

namespace UserInterface_Devexpress
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult warning = (XtraMessageBox.Show("Bạn thật sự muốn thoát chương trình?", "Cảnh báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
            if (warning == DialogResult.Yes)
                Application.Exit();
        }

        private void txteMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnDangNhap_Click(sender, e);
        }

        /// <summary>
        /// event click button btnDangNhap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            Account objSearch = new Account();
            objSearch.UserName = txteTenDangNhap.Text;
            objSearch.Password = txteMatKhau.Text;
            try
            {
                if (CheckValid())
                {
                    if (DBConnection.Instance.TestConnection() == true)
                    {
                        var lstAccounts = OracleHelper.ExcuteSelectMultiObject<Account>(PKG_ACCOUNT.NAME, PKG_ACCOUNT.PROCEDURE, objSearch);
                        if (lstAccounts.Count > 0)
                        {
                           this.Hide();
                            frmFormMain frm = new frmFormMain(txteTenDangNhap.Text);
                            frm.Show();
                        }
                        else
                        {
                            XtraMessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Error", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Kết nối không thành cô ng. Kiểm tra lại các tham số kết nối!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }             
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Check validation for user login
        /// </summary>
        /// <returns></returns>
        public bool CheckValid()
        {
            if (this.txteTenDangNhap.Text.Trim() == "")
            {
                XtraMessageBox.Show("Chưa nhập Tên đăng nhập!", "Thông Báo");
                this.txteTenDangNhap.Focus();
                return false;
            }
            if (this.txteMatKhau.Text.Trim() == "")
            {
                XtraMessageBox.Show("Chưa nhập Mật khẩu đăng nhập!", "Thông Báo");
                this.txteMatKhau.Focus();
                return false;
            }
            return true;
        }
    }
}