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
        Functions functions = new Functions();

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
            int intLenght = Convert.ToInt32(txtLenght.Text);
            int intStart = Convert.ToInt32(txtStart.Text);
            intLenght++;
                        
            this.functions.ReturnValue= intStart.ToString() + "," + intLenght.ToString();
            
            Close();
        }
    }
}
