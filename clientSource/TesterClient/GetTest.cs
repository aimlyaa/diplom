using System;
using System.Drawing;
using System.Windows.Forms;

namespace TesterClient
{
    public partial class GetTest : Form
    {
        public Point mouseLocation;
        public GetTest()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    public class RootObject
    {
        public tst[] Jsons { get; set; }
    }
        public class tst
        {
            public string[] answers { get; set; }
            public string[] quetsions { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GlobalVars.testName = listBox1.SelectedItem.ToString();
            if (GlobalVars.testName != null)
            {
                Requests.GET(GlobalVars.testName);
                Test test = new Test();
                Close();
                test.Show();
            }
        }

        private void GetTest_Load(object sender, EventArgs e)
        {
            string jsonString = Requests.GET();
            foreach (string item in jsonString.Split(','))
            {
                listBox1.Items.Add(item);
            }
            listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GetTest_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void GetTest_MouseMove(object sender, MouseEventArgs e)
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
