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
    public partial class TTPhieuNhapVien3 : Form
    {
        public TTPhieuNhapVien3()
        {
            InitializeComponent();
        }

        public PhieuNhapVienInfo2 SelectedPhieuNhapVienInfo { get; private set; }
        private void dgvTTPK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                // Assuming 'Delete4' is the name of the delete button column
            if (dgvTTPNV.Columns[e.ColumnIndex].Name == "Delete4" && e.RowIndex >= 0)
            {
                string maPhieuNhapVien = dgvTTPNV.Rows[e.RowIndex].Cells["MaPhieuNhapVien"].Value.ToString();
                if (MessageBox.Show("Are you sure you want to delete record " + maPhieuNhapVien + "?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeletePhieuNhapVien(maPhieuNhapVien);
                }
            }
            if (e.ColumnIndex == dgvTTPNV.Columns["Select4"].Index && e.RowIndex >= 0)
            {
               DataGridViewRow row = dgvTTPNV.Rows[e.RowIndex];
               SelectedPhieuNhapVienInfo = new PhieuNhapVienInfo2
                {
                    MaPhieuNhapVien = row.Cells["MaPhieuNhapVien"].Value.ToString(),
                   NgayNhapVien = Convert.ToDateTime(row.Cells["NgayNhapVien"].Value),
                   TienTamUng = row.Cells["TienTamUng"].Value.ToString(),
                    HoTenNhanVien = row.Cells["HoTenNhanVien"].Value.ToString(),
                    TenGiuongDieuTri = row.Cells["TenGiuongDieuTri"].Value.ToString(),
                   TenKhoa = row.Cells["TenKhoa"].Value.ToString(),
                    TenPhongDieuTri = row.Cells["TenPhongDieuTri"].Value.ToString()
                
            };
                AddSelectedPhieuNhapVienToXuatVien(SelectedPhieuNhapVienInfo);
                this.DialogResult = DialogResult.OK;
               this.Close();
            }
        }
        private void DeletePhieuNhapVien(string maPhieuNhapVien)
        {
            using (dbConnect db = new dbConnect())
            {
                db.OpenConnection();
                string query = "DELETE FROM PhieuNhapVien WHERE MaPhieuNhapVien = :MaPNV";
                using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.Add("MaPNV", OracleDbType.Varchar2).Value = maPhieuNhapVien;
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Record deleted successfully.");
                        LoadPhieuNhapVienData(); // Refresh the data grid view
                    }
                    else
                    {
                        MessageBox.Show("Error deleting record.");
                    }
                }
                db.CloseConnection();
            }
        }

        private void LoadPhieuNhapVienData()
        {
            string query = @"
        SELECT 
            pnv.MaPhieuNhapVien,
            pnv.NgayNhapVien,
            pnv.TienTamUng,
            pnv.MaPhieuKham,
            k.TenKhoa,
            nv.HoTenNhanVien,
            pdt.TenPhongDieuTri,
            gdt.TenGiuongDieuTri,
            lp.TenLoai
        FROM 
            PhieuNhapVien pnv
            INNER JOIN Khoa k ON pnv.MaKhoa = k.MaKhoa
            INNER JOIN NhanVien nv ON pnv.MaNhanVien = nv.MaNhanVien
            INNER JOIN PhongDieuTri pdt ON pnv.MaPhongDieuTri = pdt.MaPhongDieuTri
            INNER JOIN GiuongDieuTri gdt ON pnv.MaGiuongDieuTri = gdt.MaGiuongDieuTri
            INNER JOIN LoaiPhong lp ON pnv.MaLoaiPhong = lp.MaLoaiPhong";

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
                    db.CloseConnection();
                }

                dgvTTPNV.DataSource = dt;
                dgvTTPNV.Columns["MaPhieuNhapVien"].HeaderText = "Mã Phiếu Nhập Viện";
                dgvTTPNV.Columns["NgayNhapVien"].HeaderText = "Ngày Nhập Viện";
                dgvTTPNV.Columns["TienTamUng"].HeaderText = "Tiền Tạm Ứng";
                dgvTTPNV.Columns["MaPhieuKham"].HeaderText = "Mã Phiếu Khám";
                dgvTTPNV.Columns["TenKhoa"].HeaderText = "Tên Khoa";
                dgvTTPNV.Columns["HoTenNhanVien"].HeaderText = "Tên Nhân Viên";
                dgvTTPNV.Columns["TenPhongDieuTri"].HeaderText = "Tên Phòng Điều Trị";
                dgvTTPNV.Columns["TenGiuongDieuTri"].HeaderText = "Tên Giường Điều Trị";

                dgvTTPNV.Columns["TenLoai"].HeaderText = "Tên Loại Phòng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void AddSelectedPhieuNhapVienToXuatVien(PhieuNhapVienInfo2 selectedInfo)
        {
            // Generate a new MaPhieuXuatVien
            string newMaPhieuXuatVien = GenerateUniqueMaPhieuXuatVien();

            // Use today's date for NgayXuatVien
            DateTime ngayXuatVien = DateTime.Now;

            // Insert the new record into PHIEUXUATVIEN
            try
            {
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    string query = @"
            INSERT INTO PHIEUXUATVIEN (MaPhieuXuatVien, NgayXuatVien, MaPhieuNhapVien, MaNhanVien)
            VALUES (:MaPXV, :NgayXV, :MaPNV, :MaNV)";

                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        cmd.Parameters.Add("MaPXV", OracleDbType.Varchar2).Value = newMaPhieuXuatVien;
                        cmd.Parameters.Add("NgayXV", OracleDbType.Date).Value = ngayXuatVien;
                        cmd.Parameters.Add("MaPNV", OracleDbType.Varchar2).Value = selectedInfo.MaPhieuNhapVien;
                        cmd.Parameters.Add("MaNV", OracleDbType.Varchar2).Value = selectedInfo.MaNhanVien;

                        cmd.ExecuteNonQuery();
                    }
                    db.CloseConnection();
                    MessageBox.Show("Record added to PHIEUXUATVIEN successfully va dang dc in ra ");
                    SaveDetailsToWord(newMaPhieuXuatVien, ngayXuatVien, selectedInfo.MaPhieuNhapVien, selectedInfo.MaNhanVien);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting into PHIEUXUATVIEN: " + ex.Message);
            }
        }


        private string GenerateUniqueMaPhieuXuatVien()
        {
            string uniqueMaPXV = "PXV01"; // Starting ID
            try
            {
                using (dbConnect db = new dbConnect())
                {
                    db.OpenConnection();
                    // Get the last MaPhieuXuatVien from the table
                    string query = "SELECT MAX(MaPhieuXuatVien) AS LastID FROM PHIEUXUATVIEN";
                    using (OracleCommand cmd = new OracleCommand(query, db.GetConnection()))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string lastId = result.ToString();
                            // Extract the numeric part of the ID, increment it by 1 to make a new ID
                            int numericId = int.Parse(lastId.Substring(3)) + 1;
                            uniqueMaPXV = "PXV" + numericId.ToString("D2"); // "D2" ensures the numeric part is two digits
                        }
                    }
                    db.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating MaPhieuXuatVien: " + ex.Message);
            }
            return uniqueMaPXV;
        }

        private void SaveDetailsToWord(string maPhieuXuatVien, DateTime ngayXuatVien, string maPhieuNhapVien, string maNhanVien)
        {
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document wordDoc = null;

            try
            {
                object missing = System.Reflection.Missing.Value;
                object fileName = @"D:\PhieuXuatVien.docx";

                // Check if the document exists
                if (System.IO.File.Exists((string)fileName))
                {
                    // Open the document
                    wordDoc = wordApp.Documents.Open(ref fileName);
                }
                else
                {
                    // Create a new document
                    wordDoc = wordApp.Documents.Add();
                }

                // Move to the end of the document
                wordApp.Selection.EndKey(Microsoft.Office.Interop.Word.WdUnits.wdStory, ref missing);

                // Insert the details
                string details = $"Ma Phieu Xuat Vien: {maPhieuXuatVien}\n" +
                                 $"Ngay Xuat Vien: {ngayXuatVien.ToShortDateString()}\n" +
                                 $"Ma Phieu Nhap Vien: {maPhieuNhapVien}\n" +
                                 $"Ma Nhan Vien: {maNhanVien}\n\n";

                wordApp.Selection.TypeText(details);

                // Save the document
                wordDoc.SaveAs2(ref fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while writing to Word document: " + ex.Message);
            }
            finally
            {
                // Close the document and Quit Word
                wordDoc?.Close();
                wordApp.Quit();
            }
        }





        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TTPhieuNhapVien3_Load_1(object sender, EventArgs e)
        {
            LoadPhieuNhapVienData();
        }
    }
}
