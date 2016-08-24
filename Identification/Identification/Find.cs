using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Identification
{
    public partial class Find : Form
    {
        public Find()
        {
            InitializeComponent();
        }
        Boolean answer;
        private void button1_Click(object sender, EventArgs e)
        {
            answer = textBox1.Text.Contains("الله");
            textBox2.Text = answer.ToString();
        }
    }
}
