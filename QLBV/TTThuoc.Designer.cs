namespace QLBV
{
    partial class TTThuoc
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
            this.components = new System.ComponentModel.Container();
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.guna2AnimateWindow2 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.BNTXemThongTin = new Guna.UI2.WinForms.Guna2Button();
            this.BTNPhieuKham = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // BNTXemThongTin
            // 
            this.BNTXemThongTin.AutoRoundedCorners = true;
            this.BNTXemThongTin.BorderRadius = 21;
            this.BNTXemThongTin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BNTXemThongTin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BNTXemThongTin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BNTXemThongTin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BNTXemThongTin.FillColor = System.Drawing.Color.White;
            this.BNTXemThongTin.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BNTXemThongTin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.BNTXemThongTin.Location = new System.Drawing.Point(363, 138);
            this.BNTXemThongTin.Name = "BNTXemThongTin";
            this.BNTXemThongTin.Size = new System.Drawing.Size(197, 44);
            this.BNTXemThongTin.TabIndex = 14;
            this.BNTXemThongTin.Text = "Xuất Hóa Đơn";
            // 
            // BTNPhieuKham
            // 
            this.BTNPhieuKham.AutoRoundedCorners = true;
            this.BTNPhieuKham.BorderRadius = 21;
            this.BTNPhieuKham.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BTNPhieuKham.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BTNPhieuKham.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BTNPhieuKham.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BTNPhieuKham.FillColor = System.Drawing.Color.White;
            this.BTNPhieuKham.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTNPhieuKham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.BTNPhieuKham.Location = new System.Drawing.Point(191, 30);
            this.BTNPhieuKham.Name = "BTNPhieuKham";
            this.BTNPhieuKham.Size = new System.Drawing.Size(189, 44);
            this.BTNPhieuKham.TabIndex = 13;
            this.BTNPhieuKham.Text = "Xem Thuốc";
            this.BTNPhieuKham.Click += new System.EventHandler(this.BTNPhieuKham_Click);
            // 
            // guna2Button1
            // 
            this.guna2Button1.AutoRoundedCorners = true;
            this.guna2Button1.BorderRadius = 21;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.White;
            this.guna2Button1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.guna2Button1.Location = new System.Drawing.Point(56, 138);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(189, 44);
            this.guna2Button1.TabIndex = 15;
            this.guna2Button1.Text = "Phát Thuốc";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // TTThuoc
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(172)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(572, 219);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.BNTXemThongTin);
            this.Controls.Add(this.BTNPhieuKham);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "TTThuoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TTThuoc";
            this.Load += new System.EventHandler(this.TTThuoc_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow2;
        private Guna.UI2.WinForms.Guna2Button BNTXemThongTin;
        private Guna.UI2.WinForms.Guna2Button BTNPhieuKham;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}