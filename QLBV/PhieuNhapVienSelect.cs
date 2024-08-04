using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBV
{
    public partial class PhieuNhapVienSelect : Form
    {
        public PhieuNhapVienSelect()
        {
            InitializeComponent();
        }

        private void BTNPhieuNhapVien_Click(object sender, EventArgs e)
        {
            PhieuNhapVien phieunhapvien = new PhieuNhapVien();
            phieunhapvien.Show();
        }

        private void BNTXemThongTin_Click(object sender, EventArgs e)
        {
            TTPhieuNhapVien3 phieunhapvientt = new TTPhieuNhapVien3();
            phieunhapvientt.Show();
        }

        private void PhieuNhapVienSelect_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            var document = wordApp.Documents.Add();

            using (TTPhieuNhapVien3 form = new TTPhieuNhapVien3())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    PhieuNhapVienInfo2 info = form.SelectedPhieuNhapVienInfo; // Ensure this property is publicly accessible in TTPhieuKham

                    // Construct the content string for the Word document using the info
                    string docContent = $"MaPhieu: {info.MaPhieuKham}" + Environment.NewLine +
                                        $"Patient: {info.HoTenBN}" + Environment.NewLine +
                                        $"Patient : {info.MaBN}" + Environment.NewLine +
                                        $"Employee Name: {info.HoTenNhanVien}" + Environment.NewLine +
                                        $"Symptoms: {info.TenPhongDieuTri}" + Environment.NewLine +
                                        $"Conclusion: {info.TenKhoa}" + Environment.NewLine +
                                         $"Date of Examination: {info.NgayNhapVien.ToShortDateString()}" + Environment.NewLine +
                                        $"Notes: {info.MaLoaiPhong}";

                    // Set the content to the Word document
                    document.Content.Text = docContent;
                }
            }

            // Save the document
            try
            {
                string filePath = "D:\\PhieuNhapVien.docx";
                document.SaveAs2(filePath);
                MessageBox.Show($"Document saved to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the document: " + ex.Message);
            }
            finally
            {
                document.Close(false);
                wordApp.Quit();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            TTPhieuNhapVien3 phieunhapvientt = new TTPhieuNhapVien3();
            phieunhapvientt.Show();
        }
    }
    
}
