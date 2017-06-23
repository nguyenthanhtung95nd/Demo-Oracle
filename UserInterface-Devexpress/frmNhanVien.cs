using DataAccess;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Util;

namespace UserInterface_Devexpress
{
    public partial class frmNhanVien : DevExpress.XtraEditors.XtraForm
    {
        public delegate void TreeViewAfterSelectHandler(TreeNode node);

        public bool IsInsert = true;
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            TreeOrganization_Load();
        }

        #region Xử lý control chức năng

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmNhanVienInfo frm = new frmNhanVienInfo();
            frm.IsInsert = true;
            frm.OrgId = (decimal)OrgId;
            frm.ShowDialog();
        }

        #endregion Xử lý control chức năng

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
            trgOrg.CheckBoxes = checkbox;
        }

        private static string strNodes;

        public string getNodes()
        {
            strNodes = ",";
            foreach (TreeNode child in trgOrg.Nodes)
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

                trgOrg.Nodes.Clear();

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
                        trgOrg.Nodes.Add(trChildNode);

                        PopulateTreeView(trChildNode, lstOrg);

                        trChildNode.Expand();
                    }

                    if (trgOrg.Nodes.Count > 0)
                    {
                        trgOrg.SelectedNode = trgOrg.Nodes[0];
                    }

                    trgOrg.AfterCheck += new TreeViewEventHandler(trvOrg_AfterCheck);
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

        private void trvOrg_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (trgOrg.SelectedNode != null)
            {
                trgOrg.SelectedNode.BackColor = Color.Empty;
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

        private void trgOrg_AfterSelect(object sender, TreeViewEventArgs e)
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

        #endregion Xử lý TreeView

        #region Xử lý DataGirdview

        public void rgvEmployee_Load()
        {
            Employee_basic objSearch = new Employee_basic();
            objSearch.ORG_ID = OrgId;
            List<Employee_basic> lstOrg =
                OracleHelper.ExcuteSelectMultiObject<Employee_basic>(PKG_LOAD_DATA.NAME,
                    PKG_LOAD_DATA.GET_EMPLOYEE_BY_ORG, objSearch);
            dgvEmployee.DataSource = lstOrg;
        }
        private void dgvEmployee_DoubleClick(object sender, EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            frmNhanVienInfo frmNhanVienInfo = new frmNhanVienInfo();
            frmNhanVienInfo.IsInsert = false;
            frmNhanVienInfo.ID = id;
            frmNhanVienInfo.ShowDialog();
        }

        #endregion Xử lý DataGirdview

        
        /// <summary>
        /// Event Control Delete row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                string empName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                Employee_basic obj = new Employee_basic();
                DialogResult warning = (MessageBox.Show("Bạn thật sự muốn xóa nhân viên : " + empName, "Cảnh báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (warning == DialogResult.Yes)
                {
                    obj.EMPLOYEE_ID = id;
                    bool temp = OracleHelper.ExcuteNonQuery(PKG_LOAD_DATA.NAME, PKG_LOAD_DATA.DELETE_EMPLOYEE, obj);
                    if (temp)
                    {
                        rgvEmployee_Load();
                        XtraMessageBox.Show("Xoá nhân viên thành công!", "Success");
                    }
                    else
                    {
                        XtraMessageBox.Show("Xoá nhân viên không thành công!", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error!");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            if (id != null)
            {
                frmNhanVienInfo frmNhanVienInfo = new frmNhanVienInfo();
                frmNhanVienInfo.IsInsert = false;
                frmNhanVienInfo.ID = id;
                frmNhanVienInfo.ShowDialog();
            }       
        }

      
    }
}