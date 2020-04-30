using System;
using System.Windows.Forms;
using System.Drawing;
namespace TesterClient
{
    public partial class GetResult : Form
    {
        public Point mouseLocation;
        public GetResult()
        {
            InitializeComponent();
        }

        private void GetResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        private void GetResult_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            Text = auth.user + ": " + GlobalVars.testName + " Результат.";
            label5.Text = auth.user + ": " + GlobalVars.testName + " Результат.";
            var res = GlobalVars.testLenght / 5;
            var ocenka = 2;
            
            if (GlobalVars.good >= res*2)
            {
                ocenka = 3;
            }
            if (GlobalVars.good >= res*3)
            {
                ocenka = 4;
            }
            if (GlobalVars.good >= res*4)
            {
                ocenka = 5;
            }
            label1.Text = GlobalVars.good + " правильных вопросов из " + GlobalVars.testLenght;
            label2.Text = "Оценка: " + ocenka;
            chart1.Series["s1"].Points.AddXY("Верно", GlobalVars.good);
            chart1.Series["s1"].Points.AddXY("Неверно", GlobalVars.testLenght - GlobalVars.good);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void GetResult_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void GetResult_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }
    }
}