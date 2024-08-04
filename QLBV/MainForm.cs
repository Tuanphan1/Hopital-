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
    public partial class MainForm : Form
    {
        Timer heartbeatTimer;
        List<Point> heartbeatPoints = new List<Point>(); // Holds the points for the heartbeat line
        int currentX = 0;
        public MainForm()
        {
            InitializeComponent();

            // Initialize the timer and its properties here
            heartbeatTimer = new Timer
            {
                Interval = 100 // Set the interval to 100 milliseconds
            };

            heartbeatTimer.Tick += new EventHandler(UpdateHeartbeat); // Subscribe to the Tick event
            heartbeatTimer.Start(); // Start the Timer
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        
        public void OpenChildForm(Form childForm, Panel panelchild)
        {
            // Close any previously opened child forms in the panel
            panelchild.Controls.Clear();

            // Set the child form's properties
            childForm.TopLevel = false; // This allows the form to be treated as a control
            childForm.FormBorderStyle = FormBorderStyle.None; // Remove the border/title bar
            childForm.Dock = DockStyle.Fill; // Fill the panel

            // Add the form to the panel and show it
            panelchild.Controls.Add(childForm);
            childForm.Show();
        }
        private void panelchild_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            TTNhanVienForm ttNhanVienForm = new TTNhanVienForm();
            OpenChildForm(ttNhanVienForm, panelChild);
        }

        private void btnBenhNhan_Click(object sender, EventArgs e)
        {
            TTBenhNhanForm ttBenhNhanForm = new TTBenhNhanForm();
            OpenChildForm(ttBenhNhanForm, panelChild);
        }



        private void UpdateHeartbeat(object sender, EventArgs e)
        {
            // Calculate the next points for the heartbeat line
            // This is where you would add your logic for the "beat" of the heart
            Point nextPoint = new Point(currentX, CalculateNextY());
            heartbeatPoints.Add(nextPoint);

            // If the end of the panel is reached, reset to start
            if (currentX >= PanelHeart.Width)
            {
                heartbeatPoints.Clear();
                currentX = 0;
            }

            // Redraw the heartbeat line
            PanelHeart.Invalidate(); // Causes the panel to be redrawn
            currentX += 3;
        }
        private int CalculateNextY()
        {
            // This method would contain your logic to calculate the Y position 
            // of the next point based on the heartbeat pattern you want to create
            return new Random().Next(PanelHeart.Height); // For example purposes only
        }
        private void PanelHeart_Paint(object sender, PaintEventArgs e)
        {
            if (heartbeatPoints.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Red, heartbeatPoints.ToArray());
            }
        }

        private void btnPhieuKham_Click(object sender, EventArgs e)
        {
            PhieuKhamSelect phieukhamSL = new PhieuKhamSelect();
            phieukhamSL.Show();
        }

        private void guna2Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            PhieuNhapVienSelect phieunhapvienSL = new PhieuNhapVienSelect();
            phieunhapvienSL.Show();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            TTThuoc tTThuoc = new TTThuoc();
            tTThuoc.Show();
        }

        private void panelChild_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Rating rating = new Rating();
            rating.Show();
        }
    }
}