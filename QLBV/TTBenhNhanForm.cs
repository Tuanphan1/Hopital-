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
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace QLBV
{
    
    public partial class TTBenhNhanForm : Form
    {
        private DataTable dt;
        public TTBenhNhanForm()
        {
            InitializeComponent();
            dt = new DataTable();
            LoadBenhNhanData();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BenhNhan BenhNhanForm = new BenhNhan(); // Create the form instance
            BenhNhanForm.Show(); // Show the form

        }

        private void LoadBenhNhanData()

        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            string query = @"
        SELECT bn.MaBN, bn.HoTenBN, bn.NamSinh, bn.DanToc, bn.NgheNghiep, bn.DiaChi, t.TenTinh 
        FROM BenhNhan bn
        LEFT JOIN Tinh t ON bn.MaTinh = t.MaTinh
        ORDER BY bn.MaBN";

            dt.Clear();
            using (dbConnect db = new dbConnect()) // Assuming dbConnect is set up for Oracle connection
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                db.CloseConnection();
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Dock = DockStyle.Fill;
        }






        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dataGridView1.Columns.Count)
            {
                var column = dataGridView1.Columns[e.ColumnIndex];

                if (column != null && column.Name == "Delete2")
                {
                    // Now it's safe to retrieve the cell value.
                    var cell = dataGridView1.Rows[e.RowIndex].Cells["MaBN"];
                    if (cell != null)
                    {
                        string maBN = Convert.ToString(cell.Value); // Using Convert.ToString to handle null values.
                        DeletePatient(maBN);
                    }
                }

                if (column != null && column.Name == "Edit2")
                {
                    var cell = dataGridView1.Rows[e.RowIndex].Cells["MaBN"];
                    if (cell != null)
                    {
                        string maBN = cell.Value.ToString();
                        EditPatient(maBN);
                    }
                }
            }

        }

        public PatientInfo SelectedPatientInfo { get; private set; }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Assuming your select column is a button column and not a checkbox column.
            if (e.ColumnIndex == dataGridView1.Columns["Select2"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                SelectedPatientInfo = new PatientInfo
                {
                    MaBN = row.Cells["MaBN"].Value.ToString(),
                    HoTenBN = row.Cells["HoTenBN"].Value.ToString(),

                    NamSinh = row.Cells["NamSinh"].Value.ToString(),
                    DanToc = row.Cells["DanToc"].Value.ToString(),
                    DiaChi = row.Cells["DiaChi"].Value.ToString(),
                    NgheNghiep = row.Cells["NgheNghiep"].Value.ToString(),
                    TenTinh = row.Cells["TenTinh"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }





        private void EditPatient(string maBN)
        {
            // Logic to open the edit form for the patient with the specified MaBN
            // You could pass maBN to the form to know which record is being edited
            var editForm = new BenhNhan (maBN); // Assuming you have a constructor to handle this
            editForm.ShowDialog();

            // After the edit form is closed, refresh the DataGridView to show updated data
            LoadBenhNhanData();
        }



        private void DeletePatient(string maBN)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Patient", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (dbConnect db = new dbConnect()) // Assuming dbConnect is set up for Oracle connection
                {
                    db.OpenConnection();
                    string query = "DELETE FROM BenhNhan WHERE MaBN = :MaBN";
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaBN", OracleDbType.Varchar2).Value = maBN;

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Patient deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while deleting the patient.");
                        }
                    }
                    db.CloseConnection();
                }

                LoadBenhNhanData();
            }
        }





        // để cho Select bệnh nhâna



   

    

    //

    private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Replace("'", "''"); // Handle single quote in search terms for SQL LIKE clause
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("MaBN LIKE '%{0}%' OR " +
                             "NgheNghiep LIKE '%{0}%' OR " +
                             "DiaChi LIKE '%{0}%' OR " +
                             "TenTinh LIKE '%{0}%' OR " +
                             "HoTenBN LIKE '%{0}%' OR " +
                             "DanToc LIKE '%{0}%' OR " +
                             "Convert(NamSinh, 'System.String') LIKE '%{0}%'", searchTerm);
                dataGridView1.DataSource = dv.ToTable();
            }
            else
            {
                // If the search box is empty, reset the filter and show all rows
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Empty;
                dataGridView1.DataSource = dv.ToTable();
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TTBenhNhanForm_Load(object sender, EventArgs e)
        {
        
        }
    }
}
