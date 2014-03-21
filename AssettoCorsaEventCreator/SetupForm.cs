using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AssettoCorsaEventCreator
{
    public partial class SetupForm : Form
    {
        public string InstallPath
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public SetupForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
