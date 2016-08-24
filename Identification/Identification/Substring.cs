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
    public partial class Substring : Form
    {
        public Substring()
        {
            InitializeComponent();
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(txtChar.Text);
            int St = Convert.ToInt32(txtStart.Text);
            i++;
            Close();
        }
    }
}
