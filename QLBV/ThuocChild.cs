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
    public partial class ThuocChild : Form
    {
        private DataTable dtThuoc;

        public ThuocChild()
        {
            InitializeComponent();
            dtThuoc = new DataTable();
            LoadThuocData();
            dgvUser.CellContentClick += dgvUser_CellContentClick;
            this.dgvUser.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUser_CellContentClick);
        }
        //
        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on a valid row and column index.
            if (e.RowIndex >= 0 && e.RowIndex < dgvUser.Rows.Count && e.ColumnIndex >= 0 && e.ColumnIndex < dgvUser.Columns.Count)
            {
                var column = dgvUser.Columns[e.ColumnIndex];

                if (column != null && column.Name == "Delete5")
                {
                    // Now it's safe to retrieve the cell value.
                    var cell = dgvUser.Rows[e.RowIndex].Cells["MaThuoc"];
                    if (cell != null)
                    {
                        string maThuoc = Convert.ToString(cell.Value); // Using Convert.ToString to handle null values.
                        DeleteThuoc(maThuoc);
                    }
                }

                if (column != null && column.Name == "Edit5")
                {
                    var cell = dgvUser.Rows[e.RowIndex].Cells["MaThuoc"];
                    if (cell != null)
                    {
                        string maThuoc = cell.Value.ToString();
                        EditThuoc(maThuoc);
                    }
                }
            }
        }

        private void EditThuoc(string maThuoc)
        {
            ThemThuoc form = new ThemThuoc(maThuoc);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadThuocData(); // Reload data to reflect changes
            }
        }

        private void DeleteThuoc(string maThuoc)
        {
            // Confirm the deletion
            if (MessageBox.Show("Are you sure you want to delete this thuốc?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    string query = "DELETE FROM Thuoc WHERE MaThuoc = :MaThuoc";
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaThuoc", OracleDbType.Varchar2).Value = maThuoc;
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thuốc has been deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error deleting thuốc.");
                        }
                    }
                    db.CloseConnection();
                }
                LoadThuocData();
            }
        }


        //





        public new ThuocMainForm ParentForm { get; set; }


        //
        private void LoadThuocData()
        {
            foreach (DataGridViewColumn column in dgvUser.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            // Define your query to select data from the Thuoc table
            string query = "SELECT MaThuoc, TenThuoc, DonGia, DonViTinh FROM Thuoc";

            // Clear the existing data
            dtThuoc.Clear();

            // Use your dbConnect class to interact with the database
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dtThuoc);
                }
                db.CloseConnection();
            }

            // Assuming dataGridView is your DataGridView control on the TTBenhNhanForm
            dgvUser.DataSource = dtThuoc;
            dgvUser.Dock = DockStyle.Fill;
        }

        private void btnAdd_Click(object sender, EventArgs e)   
        {
            using (ThemThuoc themThuoc = new ThemThuoc())
            {
                if (themThuoc.ShowDialog() == DialogResult.OK)
                {
                    LoadThuocData(); // Reload the data grid
                }
            }
        }
    }
}
