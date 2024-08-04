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
    public partial class Rating : Form
    {
        public Rating()
        {
            InitializeComponent();
            guna2TenDichVu.SelectedIndexChanged += guna2TenDichVu_SelectedIndexChanged_1; 
            guna2TenDichVu.SelectedIndexChanged += new EventHandler(guna2TenDichVu_SelectedIndexChanged_1);
        }

        private void textAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void ComboboxDonGia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SetNextPhieuDichVu()
        {
            // Assume GetNextPhieuDichVuId connects to the database and retrieves the next ID
            string nextMaPhieu = GetNextPhieuDichVuId();
            textMaDV.Text = nextMaPhieu;
            // If textMaDV is meant to be read-only after setting the next MaPhieu, ensure it's set here
            textMaDV.ReadOnly = true;
        }
        private string GetNextPhieuDichVuId()
        {
            string lastId = string.Empty;
            string nextId = string.Empty;
            int numericPart;

            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                // Oracle uses ROWNUM and you need an ORDER BY within a subquery
                string query = "SELECT SoPhieuDichVu FROM (SELECT SoPhieuDichVu FROM PhieuDichVu ORDER BY SoPhieuDichVu DESC) WHERE ROWNUM = 1";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lastId = result.ToString();
                        // Assuming lastId is something like "PDV01"
                        numericPart = int.Parse(lastId.Substring(3)) + 1;
                        nextId = "PDV" + numericPart.ToString("D2");
                    }
                    else
                    {
                        nextId = "PDV01";
                    }
                }
                db.CloseConnection();
            }

            return nextId;
        }


        private void PopulateTenDichVuComboBox()
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaDV, TenDV FROM DichVu";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        guna2TenDichVu.DisplayMember = "TenDV";
                        guna2TenDichVu.ValueMember = "MaDV";
                        guna2TenDichVu.DataSource = dt;
                    }
                }
                db.CloseConnection();
            }
        }


        // Event handler for when a service is selected in the Tên Dịch Vụ ComboBox


        private void UpdateDonGiaForSelectedService()
        {
           
        }

      //  private void guna2TenDichVu_SelectedIndexChanged(object sender, EventArgs e)
     //   {
     //       // Retrieve the selected service name from the ComboBox
   //         string selectedServiceName = guna2TenDichVu.SelectedItem.ToString();
     //       UpdateDonGiaForSelectedService(selectedServiceName);
    //    }

        private void Rating_Load(object sender, EventArgs e)
        {
            SetNextPhieuDichVu();
            LoadDichVuData();
           
        }

        private void LoadDichVuData()
        {
            DataTable dataTableDichVu = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaDV, TenDV FROM DichVu";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableDichVu);
                }
                db.CloseConnection();
            }
            guna2TenDichVu.DataSource = dataTableDichVu;
            guna2TenDichVu.DisplayMember = "TenDV";
            guna2TenDichVu.ValueMember = "MaDV"; // Ensure this is the column name in your data source
            guna2TenDichVu.SelectedIndex = -1; // To ensure no selection is made by default
        }


        private void guna2TenDichVu_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Clear previous selections
            comboBoxMaDV.Items.Clear();
            Guna2DonGia.Items.Clear(); // Assuming Guna2DonGia is a ComboBox; if it's a TextBox, use Text = ""

            if (guna2TenDichVu.SelectedIndex >= 0)
            {
                // Assuming 'guna2TenDichVu' is a ComboBox with 'ValueMember' set to 'MaDV'
                string selectedMaDV = guna2TenDichVu.SelectedValue.ToString();
                comboBoxMaDV.Items.Add(selectedMaDV);
                comboBoxMaDV.SelectedIndex = 0;

                // Now let's fetch the 'DonGia' based on 'MaDV'
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    string query = "SELECT DonGia FROM DichVu WHERE MaDV = :MaDV";
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaDV", OracleDbType.Varchar2).Value = selectedMaDV;
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            // Update the 'DonGia' ComboBox or TextBox
                            string donGia = result.ToString();
                            Guna2DonGia.Items.Add(donGia); // Use this if Guna2DonGia is a ComboBox
                            Guna2DonGia.SelectedIndex = 0; // Use this if Guna2DonGia is a ComboBox
                                                           // Guna2DonGia.Text = donGia; // Use this if Guna2DonGia is a TextBox
                        }
                    }
                    db.CloseConnection();
                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (TTBenhNhanForm form = new TTBenhNhanForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PatientInfo info = form.SelectedPatientInfo;
                    textTenNV.Text = info.MaBN;
                    textBox1.Text = info.HoTenBN;
                     
                    
                }
            }
        }
        // Somewhere in your form load or initialization code
        private void LoadDichVuComboBox()
        {
            DataTable dataTableDichVu = new DataTable();
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "SELECT MaDV, TenDV FROM DichVu";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dataTableDichVu);
                }
                db.CloseConnection();
            }

            guna2TenDichVu.DataSource = dataTableDichVu;
            guna2TenDichVu.DisplayMember = "TenDV";
            guna2TenDichVu.ValueMember = "MaDV";
            guna2TenDichVu.SelectedIndex = -1; // To ensure no selection is made by default
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string soPhieuDichVu = textMaDV.Text;
            DateTime ngaySuDung = datePickerNgaySinh.Value;
            string maBN = textTenNV.Text; // Ensure this is the right control for MaBN

            if (string.IsNullOrWhiteSpace(soPhieuDichVu) || guna2TenDichVu.SelectedIndex < 0 || string.IsNullOrWhiteSpace(maBN))
            {
                MessageBox.Show("Please fill in all mandatory fields.");
                return;
            }

            try
            {
                if (guna2TenDichVu.SelectedItem is DataRowView selectedDichVu)
                {
                    string maDV = selectedDichVu["MaDV"].ToString(); // Correct way to retrieve MaDV

                    using (dbConnect db = new dbConnect())
                    {
                        db.OpenConnection();
                        string query = @"
                INSERT INTO PhieuDichVu (SoPhieuDichVu, NgaySuDungDV, MaDV, MaBN)
                VALUES (:SoPhieuDichVu, :NgaySuDungDV, :MaDV, :MaBN)";

                        using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                        {
                            cmd.Parameters.Add("SoPhieuDichVu", OracleDbType.Varchar2).Value = soPhieuDichVu;
                            cmd.Parameters.Add("NgaySuDungDV", OracleDbType.Date).Value = ngaySuDung;
                            cmd.Parameters.Add("MaDV", OracleDbType.Varchar2).Value = maDV;
                            cmd.Parameters.Add("MaBN", OracleDbType.Varchar2).Value = maBN;

                            cmd.ExecuteNonQuery();
                        }
                        db.CloseConnection();
                    }
                    MessageBox.Show("Phiếu Dịch Vụ đã được lưu và đang in ra ");
                    SaveToWord(soPhieuDichVu, ngaySuDung, maDV, maBN);
                }
                else
                {
                    MessageBox.Show("Please select a valid service.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SaveToWord(string soPhieuDichVu, DateTime ngaySuDung, string maDV, string maBN)
        {
            // Create a Word application instance
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();

            // Create a new document
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Add();

            // Append text
            doc.Content.Text = $"So Phieu Dich Vu: {soPhieuDichVu}\r\n" +
                               $"Ngay Su Dung DV: {ngaySuDung.ToShortDateString()}\r\n" +
                               $"Ma DV: {maDV}\r\n" +
                               $"Ma BN: {maBN}\r\n";

            // Save the document
            string filepath = @"D:\DichVu.docx"; // Specify the file path
            doc.SaveAs2(filepath);
            doc.Close();

            // Quit the Word application
            wordApp.Quit();

            MessageBox.Show($"Phiếu Dịch Vụ Đã được in và mời thanh toán {filepath}.");
        }



        private void btnClears_Click(object sender, EventArgs e)
        {
            textTenNV.Clear();
            textBox1.Clear();
            datePickerNgaySinh.Value = DateTime.Now; // or any default value you want
            guna2TenDichVu.SelectedIndex = -1;
            Guna2DonGia.SelectedIndex = -1 ;
            comboBoxMaDV.SelectedIndex = -1;

            // Now call the method to set the next MaPhieu
            SetNextPhieuDichVu();
        }
    }
}
