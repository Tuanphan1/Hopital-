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
using Guna.UI2.WinForms;
using Oracle.ManagedDataAccess.Client;

namespace QLBV
{
    
    public partial class PhieuKham : Form
    {
        public PhieuKham()
        {
            InitializeComponent();

        }

        private void PhieuKham_Load(object sender, EventArgs e)
        {
            txtMaPKB.Text = GetNextMaPhieuKham();
            txtMaPKB.ReadOnly = true;
            
            LoadTinhComboBox();
            LoadNamSinhData();
           // LoadPhongKhamData();
          
            txtMaBN.ReadOnly = true;
            txtHoTenBN.ReadOnly = true;
            txtDanToc.ReadOnly = true;
            txtNgheNghiep.ReadOnly = true;
            guna2ComboBoxNamSinh.Enabled = false;
            guna2SelectedTinh.Enabled = false;
            LoadKhoaData();
            LoadPhongKhamData();
          //  LoadNhanVienComboBox();
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }
        private string GetNextMaPhieuKham()
        {
            string nextID = "PK01"; // Default if no records found
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaPhieuKham FROM PhieuKham ORDER BY MaPhieuKham DESC FETCH FIRST 1 ROW ONLY";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());

                try
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string lastID = result.ToString();
                        int numericID = int.Parse(lastID.Replace("PK", "")) + 1;
                        nextID = "PK" + numericID.ToString("D2");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while getting the next PhieuKham ID: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
            return nextID;
        }



        //
        private bool CheckIfPhieuKhamExists(string maBN)
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT COUNT(*) FROM PhieuKham WHERE MaBN = :MaBN";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add(new OracleParameter("MaBN", maBN));
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }



        //



        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (TTBenhNhanForm form = new TTBenhNhanForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PatientInfo info = form.SelectedPatientInfo;
                    txtMaBN.Text = info.MaBN;
                    txtHoTenBN.Text = info.HoTenBN;
                    SetComboBoxSelectedText(guna2ComboBoxNamSinh, info.NamSinh);
                    txtDanToc.Text = info.DanToc;
                    txtDiaChi.Text = info.DiaChi;
                    txtNgheNghiep.Text = info.NgheNghiep;
                    SetComboBoxSelectedText(guna2SelectedTinh, info.TenTinh);
                }
            }
        }
        private void SetComboBoxSelectedText(Guna.UI2.WinForms.Guna2ComboBox comboBox, string text)
        {
            comboBox.SelectedIndex = comboBox.FindStringExact(text);
            if (comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Item not found in ComboBox");
            }
        }


        private void LoadNamSinhData()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 100; year--)
            {
                guna2ComboBoxNamSinh.Items.Add(year.ToString());
            }
            guna2ComboBoxNamSinh.SelectedIndex = 0;

        }

        //  private void LoadPhongKhamData()
        //  {
        //      using (dbConect db = new dbConect())
        //  {
        //        db.OpenConnection();
        //        string query = "SELECT MaKhoa, TenKhoa FROM Khoa"; // Make sure these column names are correct
        //      using (SqlCommand cmd = new SqlCommand(query, db.GetConnection()))
        //      {
        //         using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            DataTable dt = new DataTable();
        //            dt.Load(reader);
        //           guna2ComboBox1Khoa.DisplayMember = "TenKhoa";
        //          guna2ComboBox1Khoa.ValueMember = "MaKhoa";
        //             guna2ComboBox1Khoa.DataSource = dt;
        //   }
        //     }
        //     db.CloseConnection();
        //  }
        //  }


        //





        //  private void LoadNhanVienComboBox()
        //  {
        //     using (dbConect db = new dbConect())
        //  {
        //      db.OpenConnection();
        //      string query = "SELECT MaNhanVien, HoTenNhanVien FROM NhanVien";
        //     using (SqlCommand cmd = new SqlCommand(query, db.GetConnection()))
        //    {
        //       using (SqlDataReader reader = cmd.ExecuteReader())
        //       {
        //           DataTable dt = new DataTable();
        //        dt.Load(reader);
        //         guna2ComboBoxSelectNhanVien.DataSource = dt;
        //        guna2ComboBoxSelectNhanVien.DisplayMember = "HoTenNhanVien";
        //        guna2ComboBoxSelectNhanVien.ValueMember = "MaNhanVien"; 
        //     }
        //   }
        //      db.CloseConnection();
        //   }
        //   }
        private void LoadTinhComboBox()
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaTinh, TenTinh FROM Tinh";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        guna2SelectedTinh.DataSource = dt;
                        guna2SelectedTinh.DisplayMember = "TenTinh";
                        guna2SelectedTinh.ValueMember = "MaTinh";
                    }
                }
                db.CloseConnection();
            }
        }

        private void LoadTenPhongKhamData()
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT PhongKhamID, TenPhongKham FROM PhongKham";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        guna2ComboBox1TenPhong.DataSource = dt;
                        guna2ComboBox1TenPhong.DisplayMember = "TenPhongKham";
                        guna2ComboBox1TenPhong.ValueMember = "PhongKhamID";
                    }
                }
                db.CloseConnection();
            }
        }




        private void btnSave_Click(object sender, EventArgs e)
        {
            // Perform data validation as needed
            if (string.IsNullOrEmpty(txtMaPKB.Text) || string.IsNullOrEmpty(txtMaBN.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return; // Stop processing if validation fails
            }
            if (datePickerNgayKham.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("The examination date cannot be in the future. Please enter a valid date.");
                return; // Stop processing if the date is in the future
            }
            // Prepare a SQL command to insert the data into the database
            using (var db = new dbConnect()) // Adjusted to proper dbConnect class
            {
                try
                {
                    db.OpenConnection();
                    var query = @"
INSERT INTO PhieuKham (MaPhieuKham, NgayKham, TrieuChung, DeNghiKham, KetLuan, HuongDieuTri, GhiChu, MaPhongKham, MaBN, MaNhanVien)
VALUES (:MaPKB, :NgayKham, :TrieuChung, :DeNghiKham, :KetLuan, :HuongDieuTri, :GhiChu, :MaPhongKham, :MaBN, :MaNhanVien)";

                    using (var cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        // Add parameters to the SQL command
                        cmd.Parameters.Add("MaPKB", OracleDbType.Varchar2).Value = txtMaPKB.Text;
                        cmd.Parameters.Add("NgayKham", OracleDbType.Date).Value = datePickerNgayKham.Value;
                        cmd.Parameters.Add("TrieuChung", OracleDbType.Varchar2).Value = txtTrieuChung.Text;
                        cmd.Parameters.Add("DeNghiKham", OracleDbType.Varchar2).Value = txtDeNghiKham.Text;
                        cmd.Parameters.Add("KetLuan", OracleDbType.Varchar2).Value = txtKetLuan.Text;
                        cmd.Parameters.Add("HuongDieuTri", OracleDbType.Varchar2).Value = txtHDT.Text;
                        cmd.Parameters.Add("GhiChu", OracleDbType.Varchar2).Value = txtGhiChu.Text;
                        cmd.Parameters.Add("MaPhongKham", OracleDbType.Varchar2).Value = guna2ComboBox1TenPhong.SelectedValue;
                        cmd.Parameters.Add("MaBN", OracleDbType.Varchar2).Value = txtMaBN.Text;
                        cmd.Parameters.Add("MaNhanVien", OracleDbType.Varchar2).Value = guna2ComboBoxSelectNhanVien.SelectedValue;

                        // Execute the command
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Data saved successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No data was saved to the database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaPKB.Text = GetNextMaPhieuKham(); // Get the next PKB number after insertion
            datePickerNgayKham.Value = DateTime.Now; // Reset to current date or another default value
            txtTrieuChung.Clear();
            txtDeNghiKham.Clear();
            txtKetLuan.Clear();
            txtHDT.Clear();
            txtGhiChu.Clear();
            guna2ComboBox1TenPhong.SelectedIndex = -1;
            txtMaBN.Clear();
            guna2ComboBoxSelectNhanVien.SelectedIndex = -1;
            txtHoTenBN.Clear();
            guna2ComboBoxNamSinh.SelectedIndex = -1;
            comboBoxGioiTinh.SelectedIndex = -1;
            txtDanToc.Clear();
            txtDiaChi.Clear();
            txtNgheNghiep.Clear();
            guna2SelectedTinh.SelectedIndex = -1;
        }


        //


        private void LoadKhoaData()
        {
            // Unsubscribe from the event
            guna2ComboBox1Khoa.SelectedIndexChanged -= guna2ComboBox1Khoa_SelectedIndexChanged;

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
            guna2ComboBox1Khoa.DataSource = dataTableKhoa;
            guna2ComboBox1Khoa.DisplayMember = "TenKhoa";
            guna2ComboBox1Khoa.ValueMember = "MaKhoa";

            guna2ComboBox1Khoa.SelectedIndexChanged += new EventHandler(guna2ComboBox1Khoa_SelectedIndexChanged);

            guna2ComboBox1Khoa.SelectedIndex = -1;
        }



        //
        private void guna2ComboBox1Khoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1Khoa.SelectedValue != null)
            {
                string selectedKhoa = guna2ComboBox1Khoa.SelectedValue.ToString();
                LoadNhanVienByKhoa(selectedKhoa);
            }
        }
        private void LoadNhanVienByKhoa(string maKhoa)
        {
            guna2ComboBoxSelectNhanVien.SelectedIndexChanged -= guna2ComboBoxSelectNhanVien_SelectedIndexChanged;
            DataTable dataTableNhanVien = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaNhanVien, HoTenNhanVien FROM NhanVien WHERE MaKhoa = :MaKhoa";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add(new OracleParameter("MaKhoa", maKhoa));
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableNhanVien);
                }
                db.CloseConnection();
            }

            guna2ComboBoxSelectNhanVien.DataSource = dataTableNhanVien;
            guna2ComboBoxSelectNhanVien.DisplayMember = "HoTenNhanVien";
            guna2ComboBoxSelectNhanVien.ValueMember = "MaNhanVien";
            guna2ComboBoxSelectNhanVien.SelectedIndex = -1;

            // Subscribe to the event again
            guna2ComboBoxSelectNhanVien.SelectedIndexChanged += guna2ComboBoxSelectNhanVien_SelectedIndexChanged;
        }


        private void guna2ComboBoxSelectNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void LoadPhongKhamData()
        {
            // Unsubscribe from the event if necessary
            guna2ComboBox1TenPhong.SelectedIndexChanged -= guna2ComboBox1TenPhong_SelectedIndexChanged;

            DataTable dataTablePhongKham = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaPhongKham, TenPhongKham FROM PhongKham";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTablePhongKham);
                }
                db.CloseConnection();
            }

            guna2ComboBox1TenPhong.DataSource = dataTablePhongKham;
            guna2ComboBox1TenPhong.DisplayMember = "TenPhongKham";
            guna2ComboBox1TenPhong.ValueMember = "MaPhongKham";

            // Subscribe to the event again if necessary
            guna2ComboBox1TenPhong.SelectedIndexChanged += guna2ComboBox1TenPhong_SelectedIndexChanged;

            guna2ComboBox1TenPhong.SelectedIndex = -1;
        }


        private void guna2ComboBox1TenPhong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void datePickerNgayKham_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
