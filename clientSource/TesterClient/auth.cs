using System;
using System.Drawing;
using System.Windows.Forms;

namespace TesterClient
{
    public partial class auth : Form
    {
        public Point mouseLocation;
        public auth()
        {
            InitializeComponent();
        }
        public static string group = null;
        public static string user = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 1 & textBox2.Text.Length > 1)
            {
                group = textBox1.Text;
                user = textBox2.Text;
                user = char.ToUpper(user[0]) + user.Substring(1);
                string token = Requests.POST("auth", group, user);
                if (token == "False")
                {
                     label4.Visible = true;
                }
                else
                {
                    GlobalVars.clientToken = token;
                    GetTest GetTestForm = new GetTest();
                    Hide();
                    GetTestForm.Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void auth_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void auth_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            settings Setting = new settings();
            Setting.Show();
        }
    }
}
