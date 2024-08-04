using Guna.UI2.WinForms;
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
    public partial class PhieuNhapVien : Form
    {
        public PhieuNhapVien()
        {
            InitializeComponent();
        }

        private void Appoiument_Load(object sender, EventArgs e)
        {
            //LoadNhanVienComboBox();
            LoadKhoaData();
            txtMaPNV.Text = GetNextMaPhieuNhapVien();
            txtMaPNV.ReadOnly = true;
            txtMaBN.ReadOnly = true;
            txtMaPhieuKham.ReadOnly = true;
            txtHoTenNV.ReadOnly = true;
            txtTC.ReadOnly = true;
            txtKL.ReadOnly = true;
            txtGC.ReadOnly = true;
            guna2DateTime2.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //
        private string GetNextMaPhieuNhapVien()
        {
            string nextID = "PNV01"; // Default ID if no records are found
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaPhieuNhapVien FROM PhieuNhapVien ORDER BY MaPhieuNhapVien DESC FETCH FIRST ROW ONLY";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());

                try
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string lastID = result.ToString();
                        int numericID = int.Parse(lastID.Replace("PNV", "")) + 1;
                        nextID = "PNV" + numericID.ToString("D2"); // Adjust the "D2" if more digits are needed
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while getting the next PhieuNhapVien ID: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
            return nextID;
        }


        private void GenerateAndSetMaPNV()
        {
            string nextMaPNV = GetNextMaPhieuNhapVien();
            txtMaPNV.Text = nextMaPNV;
            txtMaPNV.ReadOnly = true; // Make the field read-only if needed
        }

        private bool CheckIfPhieuNhapVienExistsForPhieuKham(string maPhieuKham)
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT COUNT(*) FROM PhieuNhapVien WHERE MaPhieuKham = :MaPhieuKham";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());
                cmd.Parameters.Add(new OracleParameter("MaPhieuKham", maPhieuKham));
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }


        // Call this method when a new PhieuKham is created or when the form loads
        private void CheckAndSetMaPNV()
        {
            string maPhieuKham = txtMaPhieuKham.Text; // Get the MaPhieuKham value from the form

            // Check if a PhieuNhapVien already exists for this PhieuKham
            if (!CheckIfPhieuNhapVienExistsForPhieuKham(maPhieuKham))
            {
                // If not, generate and set a new MaPNV
                GenerateAndSetMaPNV();
            }
            else
            {
                MessageBox.Show("This PhieuKham already has a PhieuNhapVien.");
                // You might want to disable creating a new entry or retrieve and show the existing MPNV
            }
        }


        //
        //   private void LoadNhanVienComboBox()
        // {
        //    using (dbConect db = new dbConect())
        //   {
        //     db.OpenConnection();
        //      string query = "SELECT MaNhanVien, HoTenNhanVien FROM NhanVien";
        //     using (SqlCommand cmd = new SqlCommand(query, db.GetConnection()))
        //     {
        //      using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //       DataTable dt = new DataTable();
        //        dt.Load(reader);
        //       guna2ComboBoxSelectNhanVien.DataSource = dt;
        //        guna2ComboBoxSelectNhanVien.DisplayMember = "HoTenNhanVien";
        //       guna2ComboBoxSelectNhanVien.ValueMember = "MaNhanVien";
        //   }
        //    }
        //  db.CloseConnection();
        //        }
        //   }
        private void LoadKhoaData()
        {
            // Unsubscribe from the event
            guna2ComboBox1Khoa.SelectedIndexChanged -= guna2ComboBox1Khoa_SelectedIndexChanged;

            DataTable dataTableKhoa = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaKhoa, TenKhoa FROM Khoa";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dataTableKhoa);
            }

            guna2ComboBox1Khoa.DataSource = dataTableKhoa;
            guna2ComboBox1Khoa.DisplayMember = "TenKhoa";
            guna2ComboBox1Khoa.ValueMember = "MaKhoa";
            guna2ComboBox1Khoa.SelectedIndex = -1;

            // Subscribe to the event again
            guna2ComboBox1Khoa.SelectedIndexChanged += guna2ComboBox1Khoa_SelectedIndexChanged;
        }

        private void LoadPhongDieuTri(string maKhoa)
        {
            // Unsubscribe from the event
            Guna2comboPDT.SelectedIndexChanged -= Guna2comboPDT_SelectedIndexChanged;

            DataTable dataTablePhongDieuTri = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaPhongDieuTri, TenPhongDieuTri FROM PhongDieuTri WHERE MaKhoa = :MaKhoa";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());
                cmd.Parameters.Add(new OracleParameter("MaKhoa", maKhoa));
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dataTablePhongDieuTri);
            }

            Guna2comboPDT.DataSource = dataTablePhongDieuTri;
            Guna2comboPDT.DisplayMember = "TenPhongDieuTri";
            Guna2comboPDT.ValueMember = "MaPhongDieuTri";
            Guna2comboPDT.SelectedIndex = -1;

            // Subscribe to the event again
            Guna2comboPDT.SelectedIndexChanged += Guna2comboPDT_SelectedIndexChanged;
        }

        private void LoadGiuongDieuTri(string maPhongDieuTri)
        {
            DataTable dataTableGiuongDieuTri = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaGiuongDieuTri, TenGiuongDieuTri FROM GiuongDieuTri WHERE MaPhongDieuTri = :MaPhongDieuTri";
                OracleCommand cmd = new OracleCommand(query, db.GetConnection());
                cmd.Parameters.Add(new OracleParameter("MaPhongDieuTri", maPhongDieuTri));
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dataTableGiuongDieuTri);
            }

            // Clear existing items
            guna2ComboGDT.DataSource = null;
            guna2ComboGDT.Items.Clear();

            // Set new data source
            guna2ComboGDT.DataSource = dataTableGiuongDieuTri;
            guna2ComboGDT.DisplayMember = "TenGiuongDieuTri";
            guna2ComboGDT.ValueMember = "MaGiuongDieuTri";
            guna2ComboGDT.SelectedIndex = -1;
        }






        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (TTPhieuKham form = new TTPhieuKham())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PhieuKhamInfo2 info = form.SelectedPhieuKhamInfo;
                    txtMaBN.Text = info.MaBN;
                    txtHoTenBN.Text = info.HoTenBN;
                    txtMaPhieuKham.Text = info.MaPhieuKham;
                    
                    txtHoTenNV.Text = info.HoTenNhanVien;
                    txtTC.Text = info.TrieuChung;
                    txtKL.Text = info.KetLuan;
                    txtGC.Text = info.GhiChu;
                    guna2DateTime2.Value = info.NgayKham;


                }
            }
        }

        private void LoadLoaiPhongAndDonGia(string maPhongDieuTri)
        {
            DataTable dataTableLoaiPhong = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                // Adjust the query to join with the PhongDieuTri table if necessary
                string query = @"
        SELECT LP.MaLoaiPhong, LP.TenLoai, LP.DonGia
        FROM LoaiPhong LP
        INNER JOIN PhongDieuTri PDT ON LP.MaPhongDieuTri = PDT.MaPhongDieuTri
        WHERE PDT.MaPhongDieuTri = :MaPhongDieuTri";

                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add(new OracleParameter("MaPhongDieuTri", maPhongDieuTri));
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableLoaiPhong);
                }
                db.CloseConnection();
            }

            GunaSelectLoaiPhong.DataSource = dataTableLoaiPhong;
            GunaSelectLoaiPhong.DisplayMember = "TenLoai";
            GunaSelectLoaiPhong.ValueMember = "MaLoaiPhong";

            guna2ComboDonGia.DataSource = dataTableLoaiPhong;
            guna2ComboDonGia.DisplayMember = "DonGia";
            guna2ComboDonGia.ValueMember = "MaLoaiPhong";

            // Reset the selected index if you want to force the user to make a selection again
            GunaSelectLoaiPhong.SelectedIndex = -1;
            guna2ComboDonGia.SelectedIndex = -1;
        }





        private void SetComboBoxSelectedText(Guna.UI2.WinForms.Guna2ComboBox comboBox, string text)
        {
            comboBox.SelectedIndex = comboBox.FindStringExact(text);
            if (comboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Item not found in ComboBox");
            }
        }

















        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSavePKB_Click(object sender, EventArgs e)
        {
            // First, check if the MaPhieuKham has already been used for Nhap Vien
            string maPhieuKham = txtMaPhieuKham.Text;
            if (CheckIfPhieuNhapVienExistsForPhieuKham(maPhieuKham))
            {
                MessageBox.Show("This MaPhieuKham already has a PhieuNhapVien. Cannot create a new entry.");
                return; // Stop further processing
            }
            if (datePickerNgayNhapVien.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("The examination date cannot be in the future. Please enter a valid date.");
                return; // Stop processing if the date is in the future
            }
            // If not used, then proceed to save the new PhieuNhapVien
            try
            {
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    string query = @"
            INSERT INTO PhieuNhapVien (
                MaPhieuNhapVien,
                NgayNhapVien,
                TienTamUng,
                MaPhieuKham,
                MaKhoa,
                MaNhanVien,
                MaPhongDieuTri,
                MaGiuongDieuTri,
                MaLoaiPhong
            )
            VALUES (
                :MaPNV,
                :NgayNhap,
                :TienTamUng,
                :MaPhieuKham,
                :MaKhoa,
                :MaNV,
                :MaPDT,
                :MaGiuong,
                :MaLoaiPhong
            )";

                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add(new OracleParameter("MaPNV", txtMaPNV.Text));
                        cmd.Parameters.Add(new OracleParameter("NgayNhap", datePickerNgayNhapVien.Value));
                        cmd.Parameters.Add(new OracleParameter("TienTamUng", Convert.ToDecimal(txtTTU.Text))); // Assuming TienTamUng is a decimal
                        cmd.Parameters.Add(new OracleParameter("MaPhieuKham", maPhieuKham));
                        cmd.Parameters.Add(new OracleParameter("MaKhoa", ((DataRowView)guna2ComboBox1Khoa.SelectedItem)["MaKhoa"]));
                        cmd.Parameters.Add(new OracleParameter("MaNV", ((DataRowView)guna2ComboBoxSelectNhanVien.SelectedItem)["MaNhanVien"]));
                        cmd.Parameters.Add(new OracleParameter("MaPDT", ((DataRowView)Guna2comboPDT.SelectedItem)["MaPhongDieuTri"]));
                        cmd.Parameters.Add(new OracleParameter("MaGiuong", ((DataRowView)guna2ComboGDT.SelectedItem)["MaGiuongDieuTri"]));
                        cmd.Parameters.Add(new OracleParameter("MaLoaiPhong", ((DataRowView)GunaSelectLoaiPhong.SelectedItem)["MaLoaiPhong"]));

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("PhieuNhapVien has been saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving PhieuNhapVien: " + ex.Message);
            }
        }


        private void Guna2comboPDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Guna2comboPDT.SelectedValue != null)
            {
                string selectedMaPhongDieuTri = Guna2comboPDT.SelectedValue.ToString();
                LoadGiuongDieuTri(selectedMaPhongDieuTri); 

                LoadLoaiPhongAndDonGia(selectedMaPhongDieuTri);
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

            // guna2ComboBoxSelectNhanVien.SelectedIndex = -1;
        }


        private void guna2ComboBox1Khoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedKhoa = guna2ComboBox1Khoa.SelectedValue.ToString();
            LoadPhongDieuTri(selectedKhoa);

            LoadNhanVienByKhoa(selectedKhoa);
            
        }

        private void guna2ComboDonGia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ComboBoxSelectNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
