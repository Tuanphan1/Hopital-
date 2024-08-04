using Guna.UI2.WinForms.Enums;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBV
{

    public partial class NhanVienForm : Form
    {
        private string _maNV;
        private bool _isNewEmployee;
        public NhanVienForm()
        {
            InitializeComponent();
            _isNewEmployee = true;
        }
        public NhanVienForm(string maNV)
        {
            InitializeComponent();
            _maNV = maNV;
            _isNewEmployee = false;
            LoadEmployeeData(_maNV);
        }
        private void NhanVienForm_Load(object sender, EventArgs e)
        {
            LoadTinhData();
            LoadKhoaData();
            LoadChuyenNganhData();
            LoadChucVuData();
            
            if (_isNewEmployee)
            {
                SetNextNhanVienID();
                button1.Visible = false;
            }
            else
            {
                LoadEmployeeData(_maNV);
                btnSave.Visible = false;
            }
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textMaNV.Text == "" || textTenNV.Text == "" || textAddress.Text == "" ||
            cbGioiTinh.SelectedIndex == -1 || comboBoxKhoa.SelectedIndex == -1 ||
            comboChuyenNganh.SelectedIndex == -1 || comboBoxChucVu.SelectedIndex == -1 ||
            guna2SelectedTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn đã nhập thiếu thông tin!");
                return;
            }

            DateTime selectedDate = datePickerNgaySinh.Value;
            int age = DateTime.Now.Year - selectedDate.Year;
            if (selectedDate > DateTime.Now.AddYears(-age)) age--; // Correct for leap years, etc.
            if (age < 18)
            {
                MessageBox.Show("Nhân Viên không được dưới 18 tuổi!");
                return; // Stop processing the event further
            }

            try
            {
                string query = @"
        INSERT INTO NhanVien 
        (MaNhanVien, HoTenNhanVien, NgaySinh, GioiTinh, DiaChi, MaChucVu, MaKhoa, MaChuyenNganh, MaTinh) 
        VALUES 
        (:MaNhanVien, :HoTenNhanVien, :NgaySinh, :GioiTinh, :DiaChi, :MaChucVu, :MaKhoa, :MaChuyenNganh, :MaTinh)";

                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add(new OracleParameter("MaNhanVien", textMaNV.Text));
                        cmd.Parameters.Add(new OracleParameter("HoTenNhanVien", textTenNV.Text));
                        cmd.Parameters.Add(new OracleParameter("NgaySinh", OracleDbType.Varchar2)).Value = datePickerNgaySinh.Value.ToString("dd-MMM-yyyy");
                        cmd.Parameters.Add(new OracleParameter("GioiTinh", cbGioiTinh.SelectedIndex == 0 ? 1 : 0)); // Assuming GioiTinh stores 1 for Male, 0 for Female
                        cmd.Parameters.Add(new OracleParameter("DiaChi", textAddress.Text));
                        cmd.Parameters.Add(new OracleParameter("MaChucVu", comboBoxChucVu.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaKhoa", comboBoxKhoa.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaChuyenNganh", comboChuyenNganh.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaTinh", guna2SelectedTinh.SelectedValue ?? DBNull.Value));

                        cmd.ExecuteNonQuery();
                    }
                    db.CloseConnection();
                    MessageBox.Show("Thêm Nhân Viên thành công.");
                    ResetFormFields();
                    SetNextNhanVienID();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void LoadKhoaData()
        {
            DataTable dataTableKhoa = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaKhoa, TenKhoa FROM Khoa";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableKhoa);
                }
            }

            comboBoxKhoa.DataSource = dataTableKhoa;
            comboBoxKhoa.DisplayMember = "TenKhoa";
            comboBoxKhoa.ValueMember = "MaKhoa";
            comboBoxKhoa.SelectedIndex = -1;
        }

        private void LoadTinhData()
        {
            DataTable dataTableTinh = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaTinh, TenTinh FROM Tinh";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableTinh);
                }
            }

            guna2SelectedTinh.DataSource = dataTableTinh;
            guna2SelectedTinh.DisplayMember = "TenTinh";
            guna2SelectedTinh.ValueMember = "MaTinh";
            guna2SelectedTinh.SelectedIndex = -1;
        }
        private void LoadChuyenNganhData()
        {
            DataTable dataTableChuyenNganh = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaChuyenNganh, TenChuyenNganh FROM ChuyenNganh";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableChuyenNganh);
                }
            }

            comboChuyenNganh.DataSource = dataTableChuyenNganh;
            comboChuyenNganh.DisplayMember = "TenChuyenNganh";
            comboChuyenNganh.ValueMember = "MaChuyenNganh";
            comboChuyenNganh.SelectedIndex = -1;
        }


        private void LoadChucVuData()
        {
            DataTable dataTableChucVu = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaChucVu, TenChucVu FROM ChucVu";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableChucVu);
                }
            }

            comboBoxChucVu.DataSource = dataTableChucVu;
            comboBoxChucVu.DisplayMember = "TenChucVu";
            comboBoxChucVu.ValueMember = "MaChucVu";
            comboBoxChucVu.SelectedIndex = -1;
        }
        private string GetNextNhanVienID()
        {
            string nextNhanVienID = "";
            using (var db = new dbConnect()) // Assuming you have a dbConnect class for Oracle
            {
                db.OpenConnection();
                // Oracle uses ROWNUM or FETCH FIRST to limit the results, here's the modern approach with FETCH FIRST
                string query = "SELECT MaNhanVien FROM NhanVien ORDER BY MaNhanVien DESC FETCH FIRST 1 ROWS ONLY";
                using (var cmd = new OracleCommand(query, db.GetConnection()))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string lastID = result.ToString();
                        int numericID = int.Parse(lastID.Substring(2)); // Assuming IDs are like NV01, NV02, etc.
                        nextNhanVienID = "NV" + (numericID + 1).ToString("D2"); // Formats as NV03, NV04, etc., with leading zeros
                    }
                    else
                    {
                        nextNhanVienID = "NV01"; // If there's no entry in the database yet
                    }
                }
            }
            return nextNhanVienID;
        }


        private void SetNextNhanVienID()
        {
            textMaNV.Text = GetNextNhanVienID(); // Assuming you have a TextBox txtMaNhanVien
            textMaNV.ReadOnly = true; // Prevent editing
        }


        private void ResetFormFields()   //clearform
        {
            // Clear the textboxes
            textTenNV.Text = "";
            textAddress.Text = "";

            // Reset the ComboBoxes
            cbGioiTinh.SelectedIndex = -1;
            comboBoxKhoa.SelectedIndex = -1;
            comboChuyenNganh.SelectedIndex = -1;
            comboBoxChucVu.SelectedIndex = -1;
            guna2SelectedTinh.SelectedIndex = -1;

            // Reset the DateTimePicker to today's date or another default date
            datePickerNgaySinh.Value = DateTime.Now;
        }

        private void LoadEmployeeData(string maNV)
        {
            string query = @"
SELECT nv.MaNhanVien, nv.HoTenNhanVien, nv.NgaySinh, nv.GioiTinh, nv.DiaChi,
       k.TenKhoa, cn.TenChuyenNganh, cv.TenChucVu, t.TenTinh,
       nv.MaKhoa, nv.MaChuyenNganh, nv.MaChucVu, nv.MaTinh
FROM NhanVien nv
LEFT JOIN Khoa k ON nv.MaKhoa = k.MaKhoa
LEFT JOIN ChuyenNganh cn ON nv.MaChuyenNganh = cn.MaChuyenNganh
LEFT JOIN ChucVu cv ON nv.MaChucVu = cv.MaChucVu
LEFT JOIN Tinh t ON nv.MaTinh = t.MaTinh
WHERE nv.MaNhanVien = :MaNhanVien";

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add(new OracleParameter("MaNhanVien", maNV));
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textMaNV.Text = reader["MaNhanVien"].ToString();
                            textTenNV.Text = reader["HoTenNhanVien"].ToString();                       
                            datePickerNgaySinh.Value = Convert.ToDateTime(reader["NgaySinh"]);                         
                            cbGioiTinh.SelectedIndex = Convert.ToBoolean(reader["GioiTinh"]) ? 0 : 1;
                            comboBoxKhoa.SelectedValue = reader["MaKhoa"].ToString();
                            comboChuyenNganh.SelectedValue = reader["MaChuyenNganh"].ToString();
                            comboBoxChucVu.SelectedValue = reader["MaChucVu"].ToString();
                            guna2SelectedTinh.SelectedValue = reader["MaTinh"].ToString();

                            textAddress.Text = reader["DiaChi"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Employee not found.");
                        }
                    }
                }
                db.CloseConnection();
            }
        }




        private void datePickerNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetFormFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
            UPDATE NhanVien
            SET HoTenNhanVien = :HoTenNhanVien,
                GioiTinh = :GioiTinh,
                NgaySinh = :NgaySinh,
                DiaChi = :DiaChi,
                MaKhoa = :MaKhoa,
                MaChuyenNganh = :MaChuyenNganh,
                MaChucVu = :MaChucVu,
                MaTinh = :MaTinh
            WHERE MaNhanVien = :MaNhanVien";

                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        // Use the textboxes and other controls to get the updated data
                        cmd.Parameters.Add(new OracleParameter("MaNhanVien", textMaNV.Text));
                        cmd.Parameters.Add(new OracleParameter("HoTenNhanVien", textTenNV.Text));
                        // Oracle typically does not have a boolean type; assuming your table uses 1 or 0 for true/false
                        cmd.Parameters.Add(new OracleParameter("GioiTinh", cbGioiTinh.SelectedIndex == 0 ? 1 : 0));
                        // Change the parameter binding for the 'NgaySinh' field to use a string format and TO_DATE
                        cmd.Parameters.Add(new OracleParameter("NgaySinh", OracleDbType.Date) { Value = datePickerNgaySinh.Value });
                        cmd.Parameters.Add(new OracleParameter("DiaChi", textAddress.Text));
                        cmd.Parameters.Add(new OracleParameter("MaKhoa", comboBoxKhoa.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaChuyenNganh", comboChuyenNganh.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaChucVu", comboBoxChucVu.SelectedValue ?? DBNull.Value));
                        cmd.Parameters.Add(new OracleParameter("MaTinh", guna2SelectedTinh.SelectedValue ?? DBNull.Value));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Employee information updated successfully.");
                            this.Close(); // Close the form to return to the main form
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the employee ID.");
                        }
                    }
                    db.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void comboBoxKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
