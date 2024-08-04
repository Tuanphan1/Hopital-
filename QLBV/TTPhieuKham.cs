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
    public partial class TTPhieuKham : Form
    {
        public TTPhieuKham()
        {
            InitializeComponent();
            dgvPhieuKham.CellContentClick += new DataGridViewCellEventHandler(dgvPhieuKham_CellContentClick);

        }
        private void TTPhieuKham_Load(object sender, EventArgs e)
        {
            LoadPhieuKhamData();
        }


       private void LoadPhieuKhamData()
{
            string query = @"
    SELECT 
        pk.MaPhieuKham,
        bn.HoTenBN AS PatientName,
        bn.MaBN AS PatientCode,
        nv.HoTenNhanVien AS EmployeeName,
        pk.MaPhongKham,
        pk.TrieuChung AS Symptoms,
        pk.KetLuan AS Conclusion,
        pk.NgayKham AS DateOfExamination,
        pk.GhiChu AS Notes
    FROM 
        PhieuKham pk
        INNER JOIN BenhNhan bn ON pk.MaBN = bn.MaBN
        INNER JOIN NhanVien nv ON pk.MaNhanVien = nv.MaNhanVien
    ORDER BY 
        pk.MaPhieuKham";

            DataTable dt = new DataTable();

    try
    {
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }

        dgvPhieuKham.DataSource = dt;
        dgvPhieuKham.Columns["PatientName"].HeaderText = "Bệnh Nhân";
        dgvPhieuKham.Columns["PatientCode"].HeaderText = "Mã BN";
        dgvPhieuKham.Columns["EmployeeName"].HeaderText = "Nhân Viên";
        dgvPhieuKham.Columns["Symptoms"].HeaderText = "Triệu Chứng";
        dgvPhieuKham.Columns["Conclusion"].HeaderText = "Kết Luận";
        dgvPhieuKham.Columns["DateOfExamination"].HeaderText = "Ngày Khám";
        dgvPhieuKham.Columns["Notes"].HeaderText = "Ghi Chú";
            }
    catch (Exception ex)
    {
        MessageBox.Show("An error occurred: " + ex.Message);
    }
}
        private void dgvPhieuKham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPhieuKham.Columns[e.ColumnIndex].Name == "Delete3")
            {
                string maPhieuKham = dgvPhieuKham.Rows[e.RowIndex].Cells["MaPhieuKham"].Value.ToString();
                if (!string.IsNullOrEmpty(maPhieuKham))
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            DeletePhieuKham(maPhieuKham);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while deleting the record: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void DeletePhieuKham(string maPhieuKham)
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                OracleTransaction transaction = db.GetConnection().BeginTransaction();

                try
                {
                    // First, delete related ChiTietDonThuoc records
                    string deleteChiTietDonThuocQuery = "DELETE FROM ChiTietDonThuoc WHERE MaDonThuoc IN (SELECT MaDonThuoc FROM DonThuoc WHERE MaPhieuKham = :MaPhieuKham)";
                    using (OracleCommand cmdChiTiet = new OracleCommand(deleteChiTietDonThuocQuery, db.GetConnection()))
                    {
                        cmdChiTiet.Transaction = transaction;
                        cmdChiTiet.Parameters.Add("MaPhieuKham", OracleDbType.Varchar2).Value = maPhieuKham;
                        cmdChiTiet.ExecuteNonQuery();
                    }

                    // Second, delete related DonThuoc records
                    string deleteDonThuocQuery = "DELETE FROM DonThuoc WHERE MaPhieuKham = :MaPhieuKham";
                    using (OracleCommand cmdDonThuoc = new OracleCommand(deleteDonThuocQuery, db.GetConnection()))
                    {
                        cmdDonThuoc.Transaction = transaction;
                        cmdDonThuoc.Parameters.Add("MaPhieuKham", OracleDbType.Varchar2).Value = maPhieuKham;
                        cmdDonThuoc.ExecuteNonQuery();
                    }
                    // TIep
                    string deletePhieuNhapVienQuery = "DELETE FROM PhieuNhapVien WHERE MaPhieuKham = :MaPhieuKham";
                    using (OracleCommand cmdPhieuNhapVien = new OracleCommand(deletePhieuNhapVienQuery, db.GetConnection()))
                    {
                        cmdPhieuNhapVien.Transaction = transaction;
                        cmdPhieuNhapVien.Parameters.Add("MaPhieuKham", OracleDbType.Varchar2).Value = maPhieuKham;
                        cmdPhieuNhapVien.ExecuteNonQuery();
                    }

                    // Finally, delete the PhieuKham record
                    string deletePhieuKhamQuery = "DELETE FROM PhieuKham WHERE MaPhieuKham = :MaPhieuKham";
                    using (OracleCommand cmdPhieuKham = new OracleCommand(deletePhieuKhamQuery, db.GetConnection()))
                    {
                        cmdPhieuKham.Transaction = transaction;
                        cmdPhieuKham.Parameters.Add("MaPhieuKham", OracleDbType.Varchar2).Value = maPhieuKham;
                        int resultPhieuKham = cmdPhieuKham.ExecuteNonQuery();
                        if (resultPhieuKham > 0)
                        {
                            MessageBox.Show("PhieuKham and related records deleted successfully.");
                            transaction.Commit();
                        }
                        else
                        {
                            MessageBox.Show("No PhieuKham record was found with the specified ID.");
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }








        public PhieuKhamInfo2 SelectedPhieuKhamInfo { get; private set; }
        private void dgvPhieuKham_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPhieuKham.Columns["Select3"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPhieuKham.Rows[e.RowIndex];
                SelectedPhieuKhamInfo = new PhieuKhamInfo2
                {
                    MaPhieuKham = row.Cells["MaPhieuKham"].Value.ToString(),
                    HoTenBN = row.Cells["PatientName"].Value.ToString(),
                    MaBN = row.Cells["PatientCode"].Value.ToString(),
                    HoTenNhanVien = row.Cells["EmployeeName"].Value.ToString(),
                    MaPhongKham = row.Cells["MaPhongKham"].Value.ToString(),
                    TrieuChung = row.Cells["Symptoms"].Value.ToString(),
                    KetLuan = row.Cells["Conclusion"].Value.ToString(),
                    NgayKham = Convert.ToDateTime(row.Cells["DateofExamination"].Value),
                    GhiChu = row.Cells["Notes"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void btnCash_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
