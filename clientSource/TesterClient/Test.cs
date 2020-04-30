using System;
using System.Drawing;
using System.Windows.Forms;

namespace TesterClient
{
    public partial class Test : Form
    {
        public Point mouseLocation;
        public Test()
        {
            InitializeComponent();
        }
        public static int selectedAnswer;
        public static int actualQuestion;
        public static int answered;
        public void drawCells(int k)
        {
            groupBox1.Controls.Clear();

            int posX = 6;
            int posY = 22;
            PictureBox[] picBox = new PictureBox[GlobalVars.testLenght];
            for (int i = 0; i < GlobalVars.testLenght; i++)
            {
                SolidBrush mybrush = new SolidBrush(Color.Silver);
                if (GlobalVars.userAnswers[i] == 3)
                {
                    mybrush = new SolidBrush(Color.Silver);
                }
                if (GlobalVars.userAnswers[i] == 1)
                {
                    mybrush = new SolidBrush(Color.Lime);
                }
                if (GlobalVars.userAnswers[i] == 0)
                {
                    mybrush = new SolidBrush(Color.Red);
                }
                if (i == k)
                {
                    if (GlobalVars.userAnswers[i] == 3)
                    {
                        button2.Enabled = true;
                        mybrush = new SolidBrush(Color.DimGray);
                    }
                    if (GlobalVars.userAnswers[i] == 1)
                    {
                        button2.Enabled = false;
                        mybrush = new SolidBrush(Color.Lime);
                    }
                    if (GlobalVars.userAnswers[i] == 0)
                    {
                        button2.Enabled = false;
                        mybrush = new SolidBrush(Color.Red);
                    }
                    Bitmap tempImage = new Bitmap(1528 / (GlobalVars.testLenght + 6), 58);
                    Graphics tempG = Graphics.FromImage(tempImage);
                    tempG.FillRectangle(mybrush, 0, 0, Size.Width / GlobalVars.testLenght, 58);
                    PictureBox temp_pc = new PictureBox
                    {
                        Name = "temp_pc",
                        Width = 1528 / (GlobalVars.testLenght + 6),
                        Height = 24,
                        Location = new Point(posX, posY),
                        Image = tempImage,
                        Visible = true
                    };
                    groupBox1.Controls.Add(temp_pc);
                }
                int currentI = i;
                Bitmap image = new Bitmap(1528 / (GlobalVars.testLenght + 6), 48);
                Graphics g = Graphics.FromImage(image);
                g.FillRectangle(mybrush, 0, 0, Size.Width / GlobalVars.testLenght, 48);
                picBox[i] = new PictureBox
                {
                    Name = i.ToString(),
                    Width = 1528 / (GlobalVars.testLenght + 6),
                    Height = 16,
                    Location = new Point(posX, posY)
                };
                posX += 1528 / GlobalVars.testLenght;
                picBox[i].Image = image;
                picBox[i].Visible = true;
                picBox[i].Click += (sender, e) => selectQuestion(sender, e, currentI);
                groupBox1.Controls.Add(picBox[i]);
            }
        }

        public void selectQuestion(object sender, EventArgs e, int i)
        {
            if (GlobalVars.userAnswers[i] == 3)
            {
                button2.Enabled = true;
                listBox1.Enabled = true;
            }
            if (GlobalVars.userAnswers[i] == 1)
            {
                button2.Enabled = false;
                listBox1.Enabled = false;
            }
            if (GlobalVars.userAnswers[i] == 0)
            {
                button2.Enabled = false;
                listBox1.Enabled = false;
            }
            actualQuestion = i;
            label1.Text = "Вопрос " + (actualQuestion + 1).ToString() + " из " + GlobalVars.testLenght;
            drawCells(i);
            loadQuestion(i);
        }
        public void loadQuestion(int x)
        {
            label2.Text = GlobalVars.testData.testData[x].Question;
            listBox1.Items.Clear();
            listBox1.Items.Add(GlobalVars.testData.testData[x].Answer1);
            listBox1.Items.Add(GlobalVars.testData.testData[x].Answer2);
            listBox1.Items.Add(GlobalVars.testData.testData[x].Answer3);
            listBox1.Items.Add(GlobalVars.testData.testData[x].Answer4);
        }

        private void Test_Load(object sender, EventArgs e)
        {
            actualQuestion = GlobalVars.userAnswers.FindIndex(x => x == 3);
            if (actualQuestion == -1) actualQuestion = GlobalVars.testLenght - 1;
            if (GlobalVars.testData.ended == 1) {
                timer1.Enabled = false;
                GetResult getResult = new GetResult();
                getResult.Show();
                Close();
            }
            label1.Text = "Вопрос " + (actualQuestion + 1).ToString() + " из " + GlobalVars.testLenght;
            Text = "Группа " + auth.group + ": " + auth.user + ", " + GlobalVars.testName;
            label5.Text = "Группа " + auth.group + ": " + auth.user + ", " + GlobalVars.testName;
            label3.Text = "Осталось: " + GlobalVars.avaliableTime + " минут";
            if (GlobalVars.userAnswers[actualQuestion] != 3) {
                button2.Enabled = false;
            }
            if (GlobalVars.testData.ended == 0) {
                loadQuestion(actualQuestion);
                drawCells(actualQuestion);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            selectedAnswer = -1;
            selectedAnswer = listBox1.SelectedIndex;
            string questionString = label2.Text;
            if (selectedAnswer != -1)
            {
                answered++;
                bool good = Requests.POST("send_answer", questionString, selectedAnswer);
                _ = good ? GlobalVars.userAnswers[actualQuestion] = 1 : GlobalVars.userAnswers[actualQuestion] = 0;
                if (good) GlobalVars.good++;
                if (actualQuestion < GlobalVars.testLenght - 1) actualQuestion++;
                label1.Text = "Вопрос " + (actualQuestion + 1).ToString() + " из " + GlobalVars.testLenght;
                actualQuestion = GlobalVars.userAnswers.FindIndex(x => x == 3);
                if (actualQuestion == -1) actualQuestion = GlobalVars.testLenght - 1;
                loadQuestion(actualQuestion);
                drawCells(actualQuestion);
                if (answered == GlobalVars.testLenght) button2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Requests.POST("end_test");
            timer1.Enabled = false;
            GetResult GetResultForm = new GetResult();
            Close();
            GetResultForm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GlobalVars.avaliableTime--;
            Requests.POST("sync_time");
            if (GlobalVars.avaliableTime <= 1)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                listBox1.Enabled = false;
                button2.Enabled = false;
                timer1.Enabled = false;
            }
            label3.Text = "Осталось: " + GlobalVars.avaliableTime + " минут";
        }

        private void Test_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void Test_MouseMove(object sender, MouseEventArgs e)
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

        private void Button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
