using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace QLBV
{
    public partial class TTThuoc : Form
    {
        public TTThuoc()
        {
            InitializeComponent();
        }

        private void BTNPhieuKham_Click(object sender, EventArgs e)
        {
          ThuocMainForm thuocMainForm = new ThuocMainForm();
            thuocMainForm.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void TTThuoc_Load(object sender, EventArgs e)
        {

        }
    }
}
