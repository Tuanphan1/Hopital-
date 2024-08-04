using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBV
{


    public partial class ThemThuoc : Form
    {
        private string _maThuoc;
        private bool _isNew;
        public ThemThuoc()
        {

            InitializeComponent();
            _isNew = true;
        }


        public ThemThuoc(string maThuoc) : this()
        {
            _maThuoc = maThuoc;
            _isNew = false; // since we're editing, it is not a new record
            LoadThuocData(maThuoc); //
        }

        private void SetNextMaThuoc()
        {
            if (_isNew)
            {
                txtMaThuoc.Text = GenerateNextMaThuoc();
                txtMaThuoc.ReadOnly = true;
            }
        }
        private void LoadThuocData(string maThuoc)
        {
            string query = "SELECT MaThuoc, TenThuoc, DonGia, DonViTinh FROM Thuoc WHERE MaThuoc = :MaThuoc";
            using (dbConnect db = new dbConnect()) // Ensure dbConnect is compatible with OracleConnection
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add("MaThuoc", OracleDbType.Varchar2).Value = maThuoc;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtMaThuoc.Text = reader["MaThuoc"].ToString();
                            txtTenThuoc.Text = reader["TenThuoc"].ToString();
                            txtDonGia.Text = reader["DonGia"].ToString();
                            GunaSelectDVT.SelectedItem = reader["DonViTinh"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Medication not found.");
                        }
                    }
                }
                db.CloseConnection();
            }
        }



        public ThuocMainForm ParentForm { get; set; }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaThuoc.Text) ||
                string.IsNullOrEmpty(txtTenThuoc.Text) ||
                string.IsNullOrEmpty(txtDonGia.Text) ||
                GunaSelectDVT.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn đã nhập thiếu thông tin!");
                return;
            }

            if (!decimal.TryParse(txtDonGia.Text, out decimal parsedDonGia))
            {
                MessageBox.Show("Please enter a valid number for Đơn Giá.");
                return;
            }

            if (!IsUnique(txtMaThuoc.Text, txtTenThuoc.Text))
            {
                MessageBox.Show("MaThuoc and TenThuoc must be unique. The provided values already exist.");
                return;
            }

            string mathuoc1 = txtMaThuoc.Text;
            string tenthuoc1 = txtTenThuoc.Text;
            string dongia1 = txtDonGia.Text;
            string dvt1 = GunaSelectDVT.SelectedItem?.ToString();

            string query = "INSERT INTO Thuoc (MaThuoc, TenThuoc, DonGia, DonViTinh) " +
                           "VALUES (:MaThuoc, :TenThuoc, :DonGia, :DonViTinh)";

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add("MaThuoc", OracleDbType.Varchar2).Value = mathuoc1;
                    cmd.Parameters.Add("TenThuoc", OracleDbType.Varchar2).Value = tenthuoc1;
                    cmd.Parameters.Add("DonGia", OracleDbType.Decimal).Value = dongia1;
                    cmd.Parameters.Add("DonViTinh", OracleDbType.Varchar2).Value = dvt1;

                    cmd.ExecuteNonQuery();
                }
                db.CloseConnection();
            }

            MessageBox.Show("Patient information saved successfully.");
            ClearForm();
            SetNextMaThuoc();
        }

        private bool IsUnique(string maThuoc, string tenThuoc)
        {
            string query = "SELECT COUNT(*) FROM Thuoc WHERE MaThuoc = :MaThuoc OR TenThuoc = :TenThuoc";
            int count = 0;
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add("MaThuoc", OracleDbType.Varchar2).Value = maThuoc;
                    cmd.Parameters.Add("TenThuoc", OracleDbType.Varchar2).Value = tenThuoc;
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                db.CloseConnection();
            }
            return count == 0;
        }




        private string GenerateNextMaThuoc()
        {
            string lastMaThuoc = string.Empty;
            int numericPart = 0;
            using (dbConnect db = new dbConnect())
            {
                try
                {
                    db.OpenConnection();
                    string query = "SELECT MaThuoc FROM Thuoc ORDER BY MaThuoc DESC FETCH FIRST 1 ROWS ONLY";
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lastMaThuoc = result.ToString();
                            if (Regex.IsMatch(lastMaThuoc, @"\d+"))
                            {
                                numericPart = int.Parse(Regex.Match(lastMaThuoc, @"\d+").Value);
                                numericPart++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    return null;
                }
            }

            if (numericPart > 0)
            {
                return "THUOC" + numericPart.ToString("D2");
            }
            return "THUOC01";
        }

        //  private void SetNextMaThuoc()
        // {
        //     txtMaThuoc.Text = GenerateNextMaThuoc(); // Assuming you have a TextBox txtMaNhanVien
        //     txtMaThuoc.ReadOnly = true; // Prevent editing
        // }




        private void ClearForm()
        {
            txtMaThuoc.ReadOnly = true;
            txtMaThuoc.Text = GenerateNextMaThuoc();
            txtTenThuoc.Clear();
            txtDonGia.Clear();
            GunaSelectDVT.SelectedIndex = -1;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ThemThuoc_Load(object sender, EventArgs e)
        {
            if (_isNew)
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                SetNextMaThuoc(); // Only set next MaThuoc if adding new record
            }
            else
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
                // No need to call SetNextMaThuoc here since we're editing
                btnClear.Visible = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        //








        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                string maThuoc = txtMaThuoc.Text;
                string tenThuoc = txtTenThuoc.Text;
                if (!decimal.TryParse(txtDonGia.Text, out decimal donGia))
                {
                    MessageBox.Show("Invalid number for Don Gia.");
                    return;
                }
                string donViTinh = GunaSelectDVT.SelectedItem.ToString();

                string query = "UPDATE Thuoc SET TenThuoc = :TenThuoc, DonGia = :DonGia, DonViTinh = :DonViTinh WHERE MaThuoc = :MaThuoc";

                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaThuoc", OracleDbType.Varchar2).Value = maThuoc;
                        cmd.Parameters.Add("TenThuoc", OracleDbType.Varchar2).Value = tenThuoc;
                        cmd.Parameters.Add("DonGia", OracleDbType.Decimal).Value = donGia;
                        cmd.Parameters.Add("DonViTinh", OracleDbType.Varchar2).Value = donViTinh;

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Updated successfully.");
                            this.DialogResult = DialogResult.OK; // Close the form and indicate success
                        }
                        else
                        {
                            MessageBox.Show("Update failed.");

                        }
                    }
                    db.CloseConnection();
                }
            }
            catch (OracleException ex)
            {
                // This will catch any Oracle database related error.
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // This will catch any general error.
                MessageBox.Show("An error occurred: " + ex.Message);
                Console.WriteLine(ex.ToString()); // Output the entire exception to the debug console.
            }
        }
    }
}
