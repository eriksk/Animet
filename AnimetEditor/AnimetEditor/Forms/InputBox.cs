using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimetEditor.Forms
{
    public partial class InputBox : Form
    {
        public string[] Result;

        public InputBox(string[] lines)
        {
            InitializeComponent();
            textBox1.Lines = lines;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Result = textBox1.Lines;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
