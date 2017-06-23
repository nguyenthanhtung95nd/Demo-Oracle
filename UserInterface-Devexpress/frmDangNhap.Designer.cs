namespace UserInterface_Devexpress
{
    partial class frmDangNhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDangNhap));
            this.lblcMatKhau = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txteMatKhau = new DevExpress.XtraEditors.TextEdit();
            this.txteTenDangNhap = new DevExpress.XtraEditors.TextEdit();
            this.lblThongTinCoBan = new DevExpress.XtraEditors.LabelControl();
            this.btnThoat = new DevExpress.XtraEditors.SimpleButton();
            this.btnDangNhap = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txteMatKhau.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txteTenDangNhap.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblcMatKhau
            // 
            this.lblcMatKhau.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblcMatKhau.Location = new System.Drawing.Point(5, 67);
            this.lblcMatKhau.Name = "lblcMatKhau";
            this.lblcMatKhau.Size = new System.Drawing.Size(54, 14);
            this.lblcMatKhau.TabIndex = 15;
            this.lblcMatKhau.Text = "Mật khẩu:";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Location = new System.Drawing.Point(5, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(88, 14);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "Tên đăng nhập:";
            // 
            // txteMatKhau
            // 
            this.txteMatKhau.EditValue = "";
            this.txteMatKhau.Location = new System.Drawing.Point(99, 65);
            this.txteMatKhau.Name = "txteMatKhau";
            this.txteMatKhau.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txteMatKhau.Properties.Appearance.Options.UseFont = true;
            this.txteMatKhau.Properties.PasswordChar = '*';
            this.txteMatKhau.Size = new System.Drawing.Size(219, 20);
            this.txteMatKhau.TabIndex = 11;
            this.txteMatKhau.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txteMatKhau_KeyPress);
            // 
            // txteTenDangNhap
            // 
            this.txteTenDangNhap.EditValue = "";
            this.txteTenDangNhap.Location = new System.Drawing.Point(99, 39);
            this.txteTenDangNhap.Name = "txteTenDangNhap";
            this.txteTenDangNhap.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txteTenDangNhap.Properties.Appearance.Options.UseFont = true;
            this.txteTenDangNhap.Size = new System.Drawing.Size(219, 20);
            this.txteTenDangNhap.TabIndex = 10;
            // 
            // lblThongTinCoBan
            // 
            this.lblThongTinCoBan.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblThongTinCoBan.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblThongTinCoBan.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.lblThongTinCoBan.Location = new System.Drawing.Point(78, 12);
            this.lblThongTinCoBan.Name = "lblThongTinCoBan";
            this.lblThongTinCoBan.Size = new System.Drawing.Size(170, 21);
            this.lblThongTinCoBan.TabIndex = 14;
            this.lblThongTinCoBan.Text = "Thông tin đăng nhập";
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(216, 89);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(80, 23);
            this.btnThoat.TabIndex = 13;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnDangNhap.Appearance.Options.UseForeColor = true;
            this.btnDangNhap.Location = new System.Drawing.Point(130, 89);
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.Size = new System.Drawing.Size(80, 23);
            this.btnDangNhap.TabIndex = 12;
            this.btnDangNhap.Text = "Đăng nhập";
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
            // 
            // frmDangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 126);
            this.Controls.Add(this.lblcMatKhau);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txteMatKhau);
            this.Controls.Add(this.txteTenDangNhap);
            this.Controls.Add(this.lblThongTinCoBan);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnDangNhap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDangNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng Nhập";
            ((System.ComponentModel.ISupportInitialize)(this.txteMatKhau.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txteTenDangNhap.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblcMatKhau;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txteMatKhau;
        private DevExpress.XtraEditors.TextEdit txteTenDangNhap;
        private DevExpress.XtraEditors.LabelControl lblThongTinCoBan;
        private DevExpress.XtraEditors.SimpleButton btnThoat;
        private DevExpress.XtraEditors.SimpleButton btnDangNhap;
    }
}