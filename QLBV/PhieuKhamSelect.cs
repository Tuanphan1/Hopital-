using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace QLBV
{
    public partial class PhieuKhamSelect : Form
    {
        public PhieuKhamSelect()
        {
            InitializeComponent();
        }

        private void PhieuKhamSelect_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            TTPhieuKham phieukhamTT = new TTPhieuKham();
            phieukhamTT.Show();
        }

        private void BTNPhieuKham_Click(object sender, EventArgs e)
        {
            PhieuKham phieukham = new PhieuKham();
            phieukham.Show();
        }

        private void guna2buttomInPhieuKham_Click(object sender, EventArgs e)
        {
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            Document document = null;
            try
            {
                document = wordApp.Documents.Add();

                using (TTPhieuKham form = new TTPhieuKham())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        PhieuKhamInfo2 info = form.SelectedPhieuKhamInfo;

                        string docContent = $"MaPhieuKham: {info.MaPhieuKham}" + Environment.NewLine +
                                            $"Patient Name: {info.HoTenBN}" + Environment.NewLine +
                                            $"Patient Code: {info.MaBN}" + Environment.NewLine +
                                            $"Employee Name: {info.HoTenNhanVien}" + Environment.NewLine +
                                            $"Symptoms: {info.TrieuChung}" + Environment.NewLine +
                                            $"Conclusion: {info.KetLuan}" + Environment.NewLine +
                                            $"Date of Examination: {info.NgayKham.ToShortDateString()}" + Environment.NewLine +
                                            $"Notes: {info.GhiChu}";

                        document.Content.Text = docContent;

                        // Show save file dialog to user
                        SaveFileDialog sfd = new SaveFileDialog
                        {
                            Filter = "Word Documents (*.docx)|*.docx",
                            DefaultExt = "docx",
                            Title = "Save Phieu Kham As"
                        };

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            document.SaveAs2(sfd.FileName);
                            MessageBox.Show($"Document saved to {sfd.FileName}");
                            // Optionally open the document for the user
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (document != null) document.Close(false);
                if (wordApp != null) wordApp.Quit();
                Marshal.ReleaseComObject(document);
                Marshal.ReleaseComObject(wordApp);
            }
        }
    }
}
