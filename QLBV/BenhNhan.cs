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
using Oracle.ManagedDataAccess.Client;
namespace QLBV
{
    public partial class BenhNhan : Form
    {
        private string _maBN;
        private bool _isNewPatient;
        public BenhNhan(string maBN)
        {
            InitializeComponent();
            _maBN = maBN;
            _isNewPatient = false; // This is important! It signifies that we're editing, not creating a new record.
            LoadPatientData(_maBN); // Load the patient data into the form controls.
        }
    
        private void BenhNhan_Load(object sender, EventArgs e)
        {
            LoadNamSinhData();
            LoadTinhData();
            SetNextPatientID();
            if (_isNewPatient)
            {
                SetNextPatientID();
                Update2.Visible = false;
            }
            else
            {
                // Load the data for the patient being edited.
                LoadPatientData(_maBN);
                btnSave.Visible = false;
            }
        }

        public BenhNhan()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.BenhNhan_Load);
            this.guna2ComboBoxNamSinh.SelectedIndexChanged += new System.EventHandler(this.guna2ComboBox1_SelectedIndexChanged);
            _isNewPatient = true;

        }


        private void LoadPatientData(string maBN)  // lấy thông tin bên kia
        {
            string query = "SELECT * FROM BenhNhan WHERE MaBN = :MaBN";

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add(new OracleParameter("MaBN", maBN));
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assuming you have textboxes for each of these fields
                            txtMaBN.Text = reader["MaBN"].ToString();
                            txtHoTenBN.Text = reader["HoTenBN"].ToString();
                            // For gender, you will need to convert the bit to a string representation
                            comboBoxGioiTinh.SelectedIndex = Convert.ToBoolean(reader["GioiTinh"]) ? 0 : 1; // Assuming 0 is Male, 1 is Female
                            guna2ComboBoxNamSinh.SelectedItem = reader["NamSinh"].ToString();
                            txtDanToc.Text = reader["DanToc"].ToString();
                            txtNgheNghiep.Text = reader["NgheNghiep"].ToString();
                            txtDiaChi.Text = reader["DiaChi"].ToString();
                            guna2SelectedTinh.SelectedValue = reader["MaTinh"].ToString();
                        }
                    }
                }
                db.CloseConnection();
            }
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Load năm sinh cho bệnh nhân
        private void LoadNamSinhData()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= currentYear - 100; year--)
            {
                guna2ComboBoxNamSinh.Items.Add(year);
            }
            guna2ComboBoxNamSinh.SelectedIndex = 0;
        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedYear = Convert.ToInt32(guna2ComboBoxNamSinh.SelectedItem);
        }




        //load next patient
        private string GetNextPatientID()
        {
            string lastPatientID = "";
            string nextPatientID = "";

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                // Oracle uses ROWNUM to limit the results instead of TOP
                string query = "SELECT MaBN FROM BenhNhan ORDER BY MaBN DESC FETCH FIRST 1 ROWS ONLY";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lastPatientID = result.ToString();
                    }
                }
                db.CloseConnection();
            }

            if (!string.IsNullOrEmpty(lastPatientID))
            {
                int lastNumber = int.Parse(lastPatientID.Replace("BN", ""));
                int nextNumber = lastNumber + 1;
                nextPatientID = $"BN{nextNumber:D2}";
            }
            else
            {
                nextPatientID = "BN01";
            }

            return nextPatientID;
        }


        // Call this method in your form load event or after saving a patient
        private void SetNextPatientID()
        {
            string nextID = GetNextPatientID();
            txtMaBN.Text = nextID;
            txtMaBN.ReadOnly = true; // Making the MaBN textbox read-only
        }



        // load tinh

        private void LoadTinhData()
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaTinh, TenTinh FROM Tinh";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maTinh = reader["MATINH"].ToString(); // Oracle is case-sensitive; column names should be in uppercase
                            string tenTinh = reader["TENTINH"].ToString();
                            KeyValuePair<string, string> item = new KeyValuePair<string, string>(maTinh, tenTinh);
                            guna2SelectedTinh.Items.Add(item);
                        }
                    }
                }
                db.CloseConnection();
            }

            guna2SelectedTinh.DisplayMember = "Value";
            guna2SelectedTinh.ValueMember = "Key";
            guna2SelectedTinh.SelectedIndex = -1;
        }




        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaBN.Text) ||
        string.IsNullOrWhiteSpace(txtHoTenBN.Text) ||
        comboBoxGioiTinh.SelectedIndex == -1 ||
        guna2ComboBoxNamSinh.SelectedIndex == -1 ||
        txtDanToc.Text == "" ||
        txtDiaChi.Text == "" ||
        txtNgheNghiep.Text == "" ||
        guna2SelectedTinh.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn đã nhập thiếu thông tin!");
                return;
            }

            string maBN = txtMaBN.Text; // Replace txtMaBN with the actual name of your text box for MaBN
            string hoTenBN = txtHoTenBN.Text; // Replace txtHoTenBN with the actual name of your text box for HoTenBN
            string gioiTinhStr = comboBoxGioiTinh.SelectedItem.ToString(); // Get the selected gender from the combo box
            bool gioiTinh = (gioiTinhStr == "Nam");     // check if gioi tinh Nam = 1 la true false la nu~ ( vi csdl chon bit )
            int namSinh = Convert.ToInt32(guna2ComboBoxNamSinh.SelectedItem); // You already have this part
            string danToc = txtDanToc.Text; // Replace txtDanToc with the actual name of your text box for DanToc
            string diaChi = txtDiaChi.Text; // Replace txtDiaChi with the actual name of your text box for DiaChi
            string ngheNghiep = txtNgheNghiep.Text; // Replace txtNgheNghiep with the actual name of your text box for NgheNghiep
            string maTinh = ""; // Initialize maTinh with an empty string

            if (guna2SelectedTinh.SelectedItem is KeyValuePair<string, string> selectedTinh)
            {
                maTinh = selectedTinh.Key; // This gets the MaTinh from the selected item in the combo box
            }

            // Now, construct the SQL INSERT command to add the data to the BenhNhan table
            string query = @"
        INSERT INTO BenhNhan (MaBN, HoTenBN, GioiTinh, NamSinh, DanToc, DiaChi, NgheNghiep, MaTinh) 
        VALUES (:MaBN, :HoTenBN, :GioiTinh, :NamSinh, :DanToc, :DiaChi, :NgheNghiep, :MaTinh)";

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    // Add the parameters to the SQL command
                    cmd.Parameters.Add(new OracleParameter("MaBN", maBN));
                    cmd.Parameters.Add(new OracleParameter("HoTenBN", hoTenBN));
                    // Oracle uses 1/0 for true/false, so you might need to adjust this if your column is a NUMBER type
                    cmd.Parameters.Add(new OracleParameter("GioiTinh", gioiTinh ? 1 : 0));
                    cmd.Parameters.Add(new OracleParameter("NamSinh", namSinh));
                    cmd.Parameters.Add(new OracleParameter("DanToc", danToc));
                    cmd.Parameters.Add(new OracleParameter("DiaChi", diaChi));
                    cmd.Parameters.Add(new OracleParameter("NgheNghiep", ngheNghiep));
                    cmd.Parameters.Add(new OracleParameter("MaTinh", maTinh));

                    // Execute the command
                    cmd.ExecuteNonQuery();
                }
                db.CloseConnection();
            }
            MessageBox.Show("Patient information saved successfully.");
            // Clear form fields
            txtMaBN.Text = ""; // Clear MaBN
              txtHoTenBN.Text = ""; // Clear HoTenBN
             comboBoxGioiTinh.SelectedIndex = -1; // Reset GioiTinh
             guna2ComboBoxNamSinh.SelectedIndex = 0; // Reset NamSinh to current year
              txtDanToc.Text = ""; // Clear DanToc
              txtDiaChi.Text = ""; // Clear DiaChi
              txtNgheNghiep.Text = ""; // Clear NgheNghiep
              guna2SelectedTinh.SelectedIndex = -1; // Reset Tinh
              SetNextPatientID();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtHoTenBN.Text = ""; // Clear HoTenBN
            comboBoxGioiTinh.SelectedIndex = -1; // Reset GioiTinh
            guna2ComboBoxNamSinh.SelectedIndex = 0; // Reset NamSinh to current year
            txtDanToc.Text = ""; // Clear DanToc
            txtDiaChi.Text = ""; // Clear DiaChi
            txtNgheNghiep.Text = ""; // Clear NgheNghiep
            guna2SelectedTinh.SelectedIndex = -1; // Reset Tinh
        }



        private void Update2_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
        UPDATE BenhNhan
        SET HoTenBN = :HoTenBN,
            GioiTinh = :GioiTinh,
            NamSinh = :NamSinh,
            DanToc = :DanToc,
            NgheNghiep = :NgheNghiep,
            DiaChi = :DiaChi,
            MaTinh = :MaTinh
        WHERE MaBN = :MaBN";

                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        // Use the textboxes and other controls to get the updated data
                        cmd.Parameters.Add(new OracleParameter("MaBN", txtMaBN.Text));

                        cmd.Parameters.Add(new OracleParameter("HoTenBN", txtHoTenBN.Text));
                        // Assuming your GioiTinh column is a number in Oracle (1 for male, 0 for female)
                        cmd.Parameters.Add(new OracleParameter("GioiTinh", OracleDbType.Int32)).Value = comboBoxGioiTinh.SelectedIndex == 0 ? 1 : 0;
                        cmd.Parameters.Add(new OracleParameter("NamSinh", Convert.ToInt32(guna2ComboBoxNamSinh.SelectedItem)));
                        cmd.Parameters.Add(new OracleParameter("DanToc", txtDanToc.Text));
                        cmd.Parameters.Add(new OracleParameter("NgheNghiep", txtNgheNghiep.Text));
                        cmd.Parameters.Add(new OracleParameter("DiaChi", txtDiaChi.Text));

                        var selectedTinh = guna2SelectedTinh.SelectedItem as KeyValuePair<string, string>?;
                        cmd.Parameters.Add(new OracleParameter("MaTinh", selectedTinh.HasValue ? (object)selectedTinh.Value.Key : DBNull.Value));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Patient information updated successfully.");
                            this.Close(); // Close the form to return to the TTBenhNhanForm
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the patient ID.");
                        }
                    }
                    db.CloseConnection();
                }
            }
            catch (OracleException oex)
            {
                // Handle Oracle-specific exceptions
                MessageBox.Show("Oracle database error: " + oex.Message);
            }
            catch (Exception ex)
            {
                // Handle non-Oracle exceptions
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMaBN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtDanToc_TextChanged(object sender, EventArgs e)
        {

        }
    }
}