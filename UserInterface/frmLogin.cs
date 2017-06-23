using System.Data;
using DataAccess;
using Domain;
using System;
using Oracle.DataAccess.Client;
using System.Windows.Forms;
using Telerik.WinControls;

namespace UserInterface
{
    public partial class frmLogin : Telerik.WinControls.UI.RadForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
        }

        public static OracleConnection con;

        public static string connectionString = string.Format(@"Data Source=(DESCRIPTION=
                (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)
                (PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                (SERVICE_NAME=ORCL)));
                User Id=QLNS;Password=123");

        public static void Conn()
        {
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (OracleException e)
            {
                MessageBox.Show(e.ToString());
                return;
            }
        }

        public static void Disconn()
        {
            con = new OracleConnection(connectionString);
            try
            {
                con.Close();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Account objSearch = new Account();
            objSearch.UserName = txtUserName.Text;
            objSearch.Password = txtPassword.Text;
            try
            {
                if (CheckValid())
                {
                    if (DBConnection.Instance.TestConnection() == true)
                    {
                        Conn();

                        DataTable dt = new DataTable();
                        OracleCommand cmd = new OracleCommand("LOAD_ACCOUNT", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = txtUserName.Text;
                        cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = txtPassword.Text;
                        cmd.Parameters.Add("P_CUR", OracleDbType.RefCursor, ParameterDirection.InputOutput);
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(dt);
                        Disconn();
                        if (dt.Rows.Count > 0)
                        {
                            this.Hide();
                            frmGioiThieu frmGioiThieu = new frmGioiThieu();
                            frmGioiThieu.ShowDialog();
                          
                        }

                        else
                        {
                            MessageBox.Show("Đăng nhập không thành công.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kết nối không thành công. Vui lòng kiểm tra lại các tham số kết nối", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnLogin_Click(sender,e);
            }
        }
        #region CheckValid 

        public bool CheckValid()
        {
            if (this.txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập Tên đăng nhập!", "Thông Báo");
                this.txtUserName.Focus();
                return false;
            }
            if (this.txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập Mật khẩu đăng nhập!", "Thông Báo");
                this.txtPassword.Focus();
                return false;
            }
            return true;
        }
        #endregion
    }
}