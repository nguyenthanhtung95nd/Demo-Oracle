using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using DataAccess;
using Domain;
using Util;

namespace UserInterface_Devexpress
{
    public partial class frmNhanVienInfo : DevExpress.XtraEditors.XtraForm
    {
        public frmNhanVienInfo()
        {
            InitializeComponent();
        }

        public bool IsInsert = false;
        public EventHandler _lamMoi;
        public string ID = string.Empty;
        private decimal _orgId;

        public decimal OrgId
        {
            get { return _orgId; }
            set { _orgId = value; }
        }

        private void frmNhanVienInfo_Load(object sender, EventArgs e)
        {
            LoadComboboxHonNhan();
            LoadComboboxLoaiNhanVien();
            LoadViTriNhanVien();
            if (!IsInsert)
            {
                BinhdingData();
            }
           
        }
        #region Xử lý Combobox
        public void LoadComboboxLoaiNhanVien()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(1, "FullTime");
            comboSource.Add(2, "PartTime");
            cmbLoaiNV.Properties.DataSource = new BindingSource(comboSource, null);
            cmbLoaiNV.Properties.DisplayMember = "Value";
            cmbLoaiNV.Properties.ValueMember = "Key";
            cmbLoaiNV.Properties.NullText = "--Select Type--";
        }

        public void LoadComboboxHonNhan()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(1, "Đã kết hôn");
            comboSource.Add(0, "Chưa kết hôn");
            cmbHonNhan.Properties.DataSource = new BindingSource(comboSource, null);
            cmbHonNhan.Properties.DisplayMember = "Value";
            cmbHonNhan.Properties.ValueMember = "Key";
            cmbHonNhan.Properties.NullText = "--Select Status--";
        }

        public void LoadViTriNhanVien()
        {
            DataSet ds = OracleHelper.ExcuteSelectDataSet(PKG_ORG.NAME, PKG_ORG.GET_TITLE);
            DataTable dt = ds.Tables[0];
            cmbViTri.Properties.DataSource = dt;
            cmbViTri.Properties.DisplayMember = "TITLE_NAME";
            cmbViTri.Properties.ValueMember = "TITLE_ID";
            cmbViTri.Properties.NullText = "--Chọn vị trí--";
            
        }
        #endregion

        #region Xử lý Binding Data into Textbox

        public void BinhdingData()
        {
            if (IsInsert == false)
            {
                DataTable dt = new DataTable();
                Employee_basic objSearch = new Employee_basic();
                objSearch.EMPLOYEE_ID = ID;
                List<Employee_basic> lstOrg =
               OracleHelper.ExcuteSelectMultiObject<Employee_basic>(PKG_LOAD_DATA.NAME,
                   PKG_LOAD_DATA.GET_EMPLOYEE_BY_ID, objSearch);
               dt =  ConvertHelper.ListToDataTable(lstOrg);
                txtMaNV.Text = ID;
                try
                {
                    txtHoTen.Text = dt.Rows[0]["FULL_NAME"].ToString();
                }
                catch (Exception)
                {

                    txtHoTen.Text = "";
                }
                try
                {
                    txtCMTND.Text = dt.Rows[0]["ID_NO"].ToString();
                }
                catch (Exception)
                {

                    txtCMTND.Text = "";
                }

                try
                {
                    txtDiaChi.Text = dt.Rows[0]["PER_ADDRESS"].ToString();
                }
                catch (Exception)
                {

                    txtDiaChi.Text = "";
                }
                try
                {
                    txtEmail.Text = dt.Rows[0]["EMAIL"].ToString();
                }
                catch (Exception)
                {

                    txtEmail.Text = "";
                }
                try
                {
                    txtNoiCap.Text = dt.Rows[0]["ID_PLACE"].ToString();
                }
                catch (Exception)
                {

                    txtNoiCap.Text = "";
                }
                try
                {
                    dtpNgaySinh.Text = dt.Rows[0]["BIRTH_DATE"].ToString();
                }
                catch (Exception)
                {

                    dtpNgaySinh.Text = "";
                }
                
                //if (dt.Rows[0]["EMPTYPE"].ToString() == "PartTime")
                //{
                //    cmbLoaiNV.Properties.NullText = "PartTime";
                //}
                //else
                //{
                //    cmbLoaiNV.Properties.NullText = "FullTime";
                //}
                //if (dt.Rows[0]["MARRIED"].ToString() == "ad")
                //{
                    
                //}
                //else
                //{
                //    cmbHonNhan.Text = null;
                //}

                try
                {
                    cmbViTri.EditValue = dt.Rows[0]["TITLE_NAME"].ToString();
                }
                catch (Exception)
                {

                    cmbViTri.EditValue = "";
                }
                        
            }
        }
        #endregion

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string constring = String.Format(@"Data Source=(DESCRIPTION=
                (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)
                (PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                (SERVICE_NAME=ORCL)));
                User Id=QLNS;Password=123");
            if (IsInsert)
            {
                using (OracleConnection objConnection = new OracleConnection(constring))
                {
                    objConnection.Open();
                    OracleCommand objInsertCmd = new OracleCommand();
                    objInsertCmd.Connection = objConnection;
                    objInsertCmd.CommandText = "PKG_LOAD_DATA.INSERT_EMPLOYEE";
                    objInsertCmd.CommandType = CommandType.StoredProcedure;
                    objInsertCmd.Parameters.Add("P_EMPLOYEE_ID", OracleType.VarChar).Value = txtMaNV.Text;
                    objInsertCmd.Parameters.Add("P_FULLNAME", OracleType.NVarChar).Value = txtHoTen.Text;
                    objInsertCmd.Parameters.Add("P_ORG_ID", OracleType.VarChar).Value = _orgId;
                    objInsertCmd.Parameters.Add("P_WORKSTATUS", OracleType.Number).Value = 1;                    
                    objInsertCmd.Parameters.Add("P_TITLE_ID", OracleType.VarChar).Value =
                        cmbViTri.EditValue;
                    objInsertCmd.Parameters.Add("P_EMP_TYPE", OracleType.Number).Value =
                        cmbLoaiNV.EditValue;
                    objInsertCmd.Parameters.Add("P_WORK", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_BIRTH_PROVINCE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_GENDER", OracleType.VarChar).Value = "1";
                    objInsertCmd.Parameters.Add("P_ID_NO", OracleType.Number).Value = txtCMTND.Text;
                    objInsertCmd.Parameters.Add("P_ID_PLACE", OracleType.VarChar).Value = txtNoiCap.Text;
                    objInsertCmd.Parameters.Add("P_RELIGION", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_PER_ADDRESS", OracleType.VarChar).Value = txtDiaChi.Text;
                    objInsertCmd.Parameters.Add("P_MARITAL_STATUS", OracleType.VarChar).Value =
                        cmbHonNhan.EditValue;
                    objInsertCmd.Parameters.Add("P_PHONE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_EMAIL", OracleType.VarChar).Value = txtEmail.Text;
                    try
                    {
                        objInsertCmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm mới nhân viên thành công!", "Success");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Thêm mới nhân viên thât bại!", "Fail");
                    }
                    objConnection.Close();
                }
            }
 else
            {
                using (OracleConnection objConnection = new OracleConnection(constring))
                {
                    objConnection.Open();
                    OracleCommand objInsertCmd = new OracleCommand();
                    objInsertCmd.Connection = objConnection;
                    objInsertCmd.CommandText = "PKG_LOAD_DATA.UPDATE_EMPLOYEE";
                    objInsertCmd.CommandType = CommandType.StoredProcedure;
                    objInsertCmd.Parameters.Add("P_EMPLOYEE_ID", OracleType.VarChar).Value = txtMaNV.Text;
                    objInsertCmd.Parameters.Add("P_FULLNAME", OracleType.NVarChar).Value = txtHoTen.Text;
                    objInsertCmd.Parameters.Add("P_ORG_ID", OracleType.VarChar).Value = OrgId;
                    objInsertCmd.Parameters.Add("P_WORKSTATUS", OracleType.Number).Value = 1;
                    objInsertCmd.Parameters.Add("P_TITLE_ID", OracleType.VarChar).Value =
                        cmbViTri.EditValue;
                    objInsertCmd.Parameters.Add("P_EMP_TYPE", OracleType.Number).Value =
                        cmbLoaiNV.EditValue;
                    objInsertCmd.Parameters.Add("P_WORK", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_BIRTH_PROVINCE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_GENDER", OracleType.VarChar).Value = "1";
                    objInsertCmd.Parameters.Add("P_ID_NO", OracleType.Number).Value = txtCMTND.Text;
                    objInsertCmd.Parameters.Add("P_ID_PLACE", OracleType.VarChar).Value = txtNoiCap.Text;
                    objInsertCmd.Parameters.Add("P_RELIGION", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_PER_ADDRESS", OracleType.VarChar).Value = txtDiaChi.Text;
                    objInsertCmd.Parameters.Add("P_MARITAL_STATUS", OracleType.VarChar).Value =
                        cmbHonNhan.EditValue;
                    objInsertCmd.Parameters.Add("P_PHONE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_EMAIL", OracleType.VarChar).Value = txtEmail.Text;
                    try
                    {
                        objInsertCmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật nhân viên thành công!", "Success");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Cập nhật nhân viên thât bại!", "Fail");
                    }

                    objConnection.Close();
                }

            }
            }
        }
    }
