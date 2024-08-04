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
 
        public partial class Login : Form
        {
            SqlConnection cn;
            SqlCommand cm;
            public Login()
            {
                InitializeComponent();
            }

            private void label2_Click(object sender, EventArgs e)
            {

            }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtName.Text; // Get the username from the textbox
            string password = txtPassword.Text; // Get the password from the textbox

            using (dbConnect db = new dbConnect())
            {
                bool loginSuccess = db.CheckLogin(username, password);
                if (loginSuccess)
                {
                    // If login is successful, show the main form and hide the login form
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    // If login fails, show an error message
                    MessageBox.Show("Incorrect username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void Login_Load(object sender, EventArgs e)
            {

            }

            private void txtName_TextChanged(object sender, EventArgs e)
            {

            }

            private void txtPassword_TextChanged(object sender, EventArgs e)
            {

            }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
    }
