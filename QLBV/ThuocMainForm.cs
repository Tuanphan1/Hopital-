using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace QLBV
{
    public partial class ThuocMainForm : Form
    {
        public ThuocMainForm()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.ThuocMainForm_Load);

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        public void OpenChildForm(Form childForm, System.Windows.Forms.Panel panel)
        {
            // Close any previously opened child forms in the panel
            panel.Controls.Clear();

            // Set the child form's properties
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Add the form to the panel and show it
            panel.Controls.Add(childForm);
            childForm.Show();
        }
        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void ThuocMainForm_Load(object sender, EventArgs e)
        {
            ThuocChild thuocChildForm = new ThuocChild();

            // Open the child form inside the panel
            OpenChildForm(thuocChildForm, panelMain);
        }
    }
}
