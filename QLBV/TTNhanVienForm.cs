using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBV
{
    public partial class TTNhanVienForm : Form
    {
        private DataTable dt;
        public TTNhanVienForm()
        {
            InitializeComponent();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            NhanVienForm nhanVienForm = new NhanVienForm(); // Create the form instance
            nhanVienForm.Show(); // Show the form
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on a row and not the header
            if (e.RowIndex >= 0)
            {
                // Edit icon clicked
                if (dgvUser.Columns[e.ColumnIndex].Name == "Edit")
                {
                    string maNV = dgvUser.Rows[e.RowIndex].Cells["MaNhanVien"].Value.ToString();
                    EditEmployee(maNV);
                }
                // Delete icon clicked
                else if (dgvUser.Columns[e.ColumnIndex].Name == "Delete")
                {
                    string maNV = dgvUser.Rows[e.RowIndex].Cells["MaNhanVien"].Value.ToString();
                    DeleteEmployee(maNV);
                }
            }
        }
        private void EditEmployee(string maNV)
        {
            // Open the NhanVienForm in edit mode
            NhanVienForm editForm = new NhanVienForm(maNV);
            editForm.ShowDialog();

            // Refresh the DataGridView
            LoadNhanVienData();
        }

        private void DeleteEmployee(string maNV)
        {
            // Confirm deletion
            if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Employee", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = "DELETE FROM NhanVien WHERE MaNhanVien = :MaNhanVien";

                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaNhanVien", OracleDbType.Varchar2).Value = maNV;

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Employee deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while deleting the employee.");
                        }
                    }
                    db.CloseConnection();
                }

                LoadNhanVienData();
            }
        }

        private void LoadNhanVienData()
        {
            foreach (DataGridViewColumn column in dgvUser.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            string query = @"
SELECT nv.MaNhanVien, nv.HoTenNhanVien, nv.NgaySinh, nv.GioiTinh, nv.DiaChi, 
       k.TenKhoa, cn.TenChuyenNganh, cv.TenChucVu, t.TenTinh
FROM NhanVien nv
LEFT JOIN Khoa k ON nv.MaKhoa = k.MaKhoa
LEFT JOIN ChuyenNganh cn ON nv.MaChuyenNganh = cn.MaChuyenNganh
LEFT JOIN ChucVu cv ON nv.MaChucVu = cv.MaChucVu
LEFT JOIN Tinh t ON nv.MaTinh = t.MaTinh
ORDER BY nv.MaNhanVien";

            DataTable dt = new DataTable();

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                db.CloseConnection();
            }

            dgvUser.DataSource = dt;
            dgvUser.Dock = DockStyle.Fill;
        }

        private void TTNhanVienForm_Load(object sender, EventArgs e)
        {
            LoadNhanVienData();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
    

    }
