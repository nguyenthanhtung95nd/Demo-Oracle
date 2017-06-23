using System.Data.OracleClient;
using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Util;

namespace UserInterface
{
    public delegate void TreeViewAfterSelectHandler(TreeNode node);

    public partial class frmDMNhanSu : Telerik.WinControls.UI.RadForm
    {
        public bool _isInserrt = false;
        public frmDMNhanSu()
        {
            InitializeComponent();
        }

        private void frmDMNhanSu_Load(object sender, EventArgs e)
        {
            LoadComboboxLoaiNhanVien();
            LoadComboboxHonNhan();
            LoadViTriNhanVien();
            TreeOrganization_Load();
        }

        private void frmDMNhanSu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public frmDMNhanSu(int idOrgLevel)
        {
            idLevel = idOrgLevel;
            InitializeComponent();
        }

        #region Xử lý Load Nhân Viên

        public void rgvEmployee_Load()
        {
            Employee_basic objSearch = new Employee_basic();
            objSearch.ORG_ID = OrgId;
            List<Employee_basic> lstOrg =
                OracleHelper.ExcuteSelectMultiObject<Employee_basic>(PKG_LOAD_DATA.NAME,
                    PKG_LOAD_DATA.GET_EMPLOYEE_BY_ORG, objSearch);
            dgvEmployee.DataSource = lstOrg;
            dgvEmployee.Columns["ORG_ID"].IsVisible = false;
        }

        private void dgvEmployee_Click(object sender, EventArgs e)
        {
            int selectedindex = dgvEmployee.Rows.IndexOf((
                GridViewDataRowInfo
                )dgvEmployee.CurrentRow);
            try
            {
                txtHoTen.Text = dgvEmployee.Rows[selectedindex].Cells[1].Value.ToString();
            }
            catch (Exception)
            {

                txtHoTen.Text = "";
            }
            try
            {
                txtDiaChi.Text = dgvEmployee.Rows[selectedindex].Cells["PER_ADDRESS"].Value.ToString();
            }
            catch (Exception)
            {

                txtDiaChi.Text = "";
            }
            try
            {
                //txtSDT.Text = dgvEmployee.Rows[selectedindex].Cells[1].Value.ToString();
                txtSDT.Text = "";
            }
            catch (Exception)
            {

                txtSDT.Text = "";
            }
            
            try
            {
                txtEmail.Text = dgvEmployee.Rows[selectedindex].Cells["EMAIL"].Value.ToString();
            }
            catch (Exception)
            {
                txtEmail.Text = "";
            }
            txtMaNV.Text = dgvEmployee.Rows[selectedindex].Cells[0].Value.ToString();
            try
            {
                txtCMND.Text = dgvEmployee.Rows[selectedindex].Cells["ID_NO"].Value.ToString();
            }
            catch (Exception)
            {

                txtCMND.Text = "";
            }
            
            try
            {
                txtNoiCap.Text = dgvEmployee.Rows[selectedindex].Cells["ID_PLACE"].Value.ToString();
            }
            catch (Exception)
            {
                txtNoiCap.Text = "";
            }

            cmbLoaiNhanVien.Text = Convert.ToString(dgvEmployee.Rows[selectedindex].Cells[5].Value.ToString());
            try
            {
                cmbHonNhan.Text = Convert.ToString(dgvEmployee.Rows[selectedindex].Cells["MARRIED"].Value.ToString());
            }
            catch (Exception)
            {

                cmbHonNhan.Text = "";
            }
            try
            {
                cmbViTri.Text = dgvEmployee.Rows[selectedindex].Cells["TITLE_NAME"].Value.ToString();
            }
            catch (Exception)
            {

                cmbViTri.Text = "";
            }
           
        }

        #endregion Xử lý Load Nhân Viên

        #region Xử lý TreeView

        public event TreeViewAfterSelectHandler TreeViewAfterSelect;

        public decimal? OrgId { get; set; }
        public string OrgName { get; set; }
        public string Username { get; set; }

        private int _idLevel = 0;

        public int idLevel
        {
            get { return _idLevel; }
            set { _idLevel = value; }
        }

        public void ucTreeCheckBox(bool checkbox)
        {
            trvOrg.CheckBoxes = checkbox;
        }

        private static string strNodes;

        public string getNodes()
        {
            strNodes = ",";
            foreach (TreeNode child in trvOrg.Nodes)
            {
                CheckboxForChildNodes(child);
            }
            return strNodes;
        }

        private void CheckboxForChildNodes(TreeNode treeNode)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Checked)
                {
                    strNodes += node.Name + ",";
                }
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckboxForChildNodes(node);
                }
            }
        }

        public void TreeOrganization_Load()
        {
            try
            {
                TreeNode trChildNode;
                Organization objSearch = new Organization();
                objSearch.org_level = idLevel.ToString();
                List<Organization> lstOrg =
                    OracleHelper.ExcuteSelectMultiObject<Organization>(PKG_ORG.NAME,
                        PKG_ORG.GET_ORG, objSearch);

                trvOrg.Nodes.Clear();

                if (lstOrg != null)
                {
                    //Populate base nodes
                    var query = (from c in lstOrg
                                 where !(from o in lstOrg
                                         select o.org_id)
                                     .Contains(c.parent_id)
                                 select c).ToList();

                    foreach (var item in query)
                    {
                        trChildNode = new TreeNode(item.org_name);
                        trChildNode.Name = item.org_id.GetValueOrDefault(-1).ToString();
                        trvOrg.Nodes.Add(trChildNode);

                        PopulateTreeView(trChildNode, lstOrg);

                        trChildNode.Expand();
                    }

                    if (trvOrg.Nodes.Count > 0)
                    {
                        trvOrg.SelectedNode = trvOrg.Nodes[0];
                    }

                    trvOrg.AfterCheck += new TreeViewEventHandler(trvOrg_AfterCheck);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + ex.StackTrace);
                Log.Instance.WriteExceptionLog(ex, this.Name + "[" + this.Text + "]");
            }
        }

        protected void PopulateTreeView(TreeNode trParentNode, List<Organization> lstOrg)
        {
            TreeNode trChildNode;
            var query = (from c in lstOrg
                         where c.parent_id.Equals(Int32.Parse(trParentNode.Name))
                         select c).ToList();

            foreach (var item in query)
            {
                trChildNode = new TreeNode(item.org_name);
                trChildNode.Name = item.org_id.GetValueOrDefault(-1).ToString();
                trParentNode.Nodes.Add(trChildNode);

                PopulateTreeView(trChildNode, lstOrg);
            }
        }

        private void trvOrg_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                OrgId = Convert.ToDecimal(e.Node.Name);
                OrgName = e.Node.Text;

                e.Node.BackColor = Color.FromArgb(227, 239, 255);
            }

            if (TreeViewAfterSelect != null)
            {
                TreeViewAfterSelect(e.Node);
            }
        }

        private void trvOrg_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                OrgId = Convert.ToDecimal(e.Node.Name);
                OrgName = e.Node.Text;

                e.Node.BackColor = Color.FromArgb(227, 239, 255);
            }

            if (TreeViewAfterSelect != null)
            {
                TreeViewAfterSelect(e.Node);
            }
            rgvEmployee_Load();
        }

        private void trvOrg_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (trvOrg.SelectedNode != null)
            {
                trvOrg.SelectedNode.BackColor = Color.Empty;
            }
        }

        private void trvOrg_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        #endregion Xử lý TreeView

        #region Xử Lý Combobox

        public void LoadComboboxLoaiNhanVien()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(1, "FullTime");
            comboSource.Add(2, "PartTime");
            cmbLoaiNhanVien.DataSource = new BindingSource(comboSource, null);
            cmbLoaiNhanVien.DisplayMember = "Value";
            cmbLoaiNhanVien.ValueMember = "Key";
        }

        public void LoadComboboxHonNhan()
        {
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(1, "Đã kết hôn");
            comboSource.Add(0, "Chưa kết hôn");
            cmbHonNhan.DataSource = new BindingSource(comboSource, null);
            cmbHonNhan.DisplayMember = "Value";
            cmbHonNhan.ValueMember = "Key";
        }

        public void LoadViTriNhanVien()
        {
            DataSet ds = OracleHelper.ExcuteSelectDataSet(PKG_ORG.NAME, PKG_ORG.GET_TITLE);
            DataTable dt = ds.Tables[0];
            cmbViTri.DataSource = dt;
            cmbViTri.DisplayMember = "TITLE_NAME";
            cmbViTri.ValueMember = "TITLE_ID";
            cmbViTri.Text = "--Chọn vị trí--";
        }

        #endregion Xử Lý Combobox

        #region Xử Lý Báo Cáo

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            GridViewSpreadExport spreadExporter = new GridViewSpreadExport(this.dgvEmployee);
            SpreadExportRenderer exportRenderer = new SpreadExportRenderer();
            spreadExporter.RunExport("D:\\exportedFile.xlsx", exportRenderer);
            System.Diagnostics.Process.Start("D:\\exportedFile.xlsx");
        }

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            try
            {
                string sourcePath = System.IO.Directory.GetCurrentDirectory() + @"\Excel";
                string destinationPath = System.IO.Directory.GetCurrentDirectory() + @"\export";
                string sourceFileName = "BaoCaoNhanVien.xls";
                string destinationFileName = "BaoCaoNhanVien_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
                string destinationFile = System.IO.Path.Combine(destinationPath, destinationFileName);

                if (!System.IO.Directory.Exists(destinationPath))
                {
                    System.IO.Directory.CreateDirectory(destinationPath);
                }
                System.IO.File.Copy(sourceFile, destinationFile, true);
                System.Diagnostics.Process.Start(destinationFile);
            }
            catch
            {
                Cursor.Current = Cursors.Default;
                throw;
            }
        }

        #endregion Xử Lý Báo Cáo

        #region Xử Lý Control

        private void setFomatControl()
        {
            this.txtHoTen.Clear();
            this.txtMaNV.Clear();
            this.txtNoiCap.Clear();
            this.txtSDT.Clear();
            this.txtDiaChi.Clear();
            this.txtCMND.Clear();
            this.txtEmail.Clear();
            btnThem.Enabled = true;
            btnSua.Enabled = true;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            setFomatControl();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _isInserrt = false;
            btnThem.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedindex = dgvEmployee.Rows.IndexOf((
                GridViewDataRowInfo
                )dgvEmployee.CurrentRow);
                string id = dgvEmployee.Rows[selectedindex].Cells[0].Value.ToString();
                string empName = dgvEmployee.Rows[selectedindex].Cells[1].Value.ToString();
                Employee_basic obj = new Employee_basic();
                DialogResult warning = (MessageBox.Show("Bạn thật sự muốn xóa nhân viên : " + empName, "Cảnh báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (warning == DialogResult.Yes)
                {
                    obj.EMPLOYEE_ID = id;
                    bool temp = OracleHelper.ExcuteNonQuery(PKG_LOAD_DATA.NAME, PKG_LOAD_DATA.DELETE_EMPLOYEE, obj);
                    if (temp)
                    {
                        rgvEmployee_Load();
                        RadMessageBox.Show("Xoá nhân viên thành công!", "Success");
                    }
                    else
                    {
                        RadMessageBox.Show("Xoá nhân viên không thành công!", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string constring = String.Format(@"Data Source=(DESCRIPTION=
                (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)
                (PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                (SERVICE_NAME=ORCL)));
                User Id=QLNS;Password=123");
            if (_isInserrt)
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
                    objInsertCmd.Parameters.Add("P_ORG_ID", OracleType.VarChar).Value = OrgId;
                    objInsertCmd.Parameters.Add("P_WORKSTATUS", OracleType.Number).Value = 1;
                    objInsertCmd.Parameters.Add("P_TITLE_ID", OracleType.VarChar).Value =
                        cmbViTri.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_EMP_TYPE", OracleType.Number).Value =
                        cmbLoaiNhanVien.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_WORK", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_BIRTH_PROVINCE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_GENDER", OracleType.VarChar).Value = "1";
                    objInsertCmd.Parameters.Add("P_ID_NO", OracleType.Number).Value = txtCMND.Text;
                    objInsertCmd.Parameters.Add("P_ID_PLACE", OracleType.VarChar).Value = txtNoiCap.Text;
                    objInsertCmd.Parameters.Add("P_RELIGION", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_PER_ADDRESS", OracleType.VarChar).Value = txtDiaChi.Text;
                    objInsertCmd.Parameters.Add("P_MARITAL_STATUS", OracleType.VarChar).Value =
                        cmbHonNhan.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_PHONE", OracleType.VarChar).Value = txtSDT.Text;
                    objInsertCmd.Parameters.Add("P_EMAIL", OracleType.VarChar).Value = txtEmail.Text;
                    try
                    {
                        objInsertCmd.ExecuteNonQuery();
                        rgvEmployee_Load();
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
                        cmbViTri.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_EMP_TYPE", OracleType.Number).Value =
                        cmbLoaiNhanVien.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_WORK", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_BIRTH_PROVINCE", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_GENDER", OracleType.VarChar).Value = "1";
                    objInsertCmd.Parameters.Add("P_ID_NO", OracleType.Number).Value = txtCMND.Text;
                    objInsertCmd.Parameters.Add("P_ID_PLACE", OracleType.VarChar).Value = txtNoiCap.Text;
                    objInsertCmd.Parameters.Add("P_RELIGION", OracleType.VarChar).Value = "";
                    objInsertCmd.Parameters.Add("P_PER_ADDRESS", OracleType.VarChar).Value = txtDiaChi.Text;
                    objInsertCmd.Parameters.Add("P_MARITAL_STATUS", OracleType.VarChar).Value =
                        cmbHonNhan.SelectedValue.ToString();
                    objInsertCmd.Parameters.Add("P_PHONE", OracleType.VarChar).Value = txtSDT.Text;
                    objInsertCmd.Parameters.Add("P_EMAIL", OracleType.VarChar).Value = txtEmail.Text;
                    try
                    {
                        objInsertCmd.ExecuteNonQuery();
                        rgvEmployee_Load();
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            frmViTri frmViTri = new frmViTri();
            frmViTri.ShowDialog();
        }
        private void btnThem_Click_1(object sender, EventArgs e)
        {
            _isInserrt = true;
            btnSua.Enabled = false;
        }

        #endregion Xử Lý Control

     
    }
}