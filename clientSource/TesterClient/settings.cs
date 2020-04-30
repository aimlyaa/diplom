using System;
using System.Windows.Forms;

namespace TesterClient
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GlobalVars.serverIp = textBox1.Text;
            GlobalVars.serverPort = textBox2.Text;
            Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox1.Text = GlobalVars.serverIp;
            textBox2.Text = GlobalVars.serverPort;
        }
    }
}
