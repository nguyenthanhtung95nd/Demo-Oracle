using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataAccess;
using Domain;
using Telerik.WinControls;

namespace UserInterface
{
    public partial class frmViTri : Telerik.WinControls.UI.RadForm
    {
        public frmViTri()
        {
            InitializeComponent();
            
        }


        private void frmDM_Load(object sender, EventArgs e)
        {
            txtId.Visible = true;
            radGridView1.DataSource = BindingData();
        }

        DataTable BindingData()
        {
            DataTable dt;
            DataSet lstData;
            lstData = OracleHelper.ExcuteSelectDataSet(PKG_ORG.NAME, PKG_ORG.GET_TITLE);
            return dt = lstData.Tables[0];
        }

    }
}
