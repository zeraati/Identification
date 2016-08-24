using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Identification
{
    public partial class NullSearch : Form
    {
        Functions Functions = new Functions();
        Dictionary<int, string> dicDBName = new Dictionary<int, string>();
        SqlConnection sqlConnection = new SqlConnection();

        string count;
        public NullSearch(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }
        string com;
        DataTable dtField;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //fun.LoadColumn(sqlConn, cmbTBName.Text, Column2);

            Column2.DataSource = Functions.SqlColumnNames(cmbTBName.Text, sqlConnection);

            Column2.DisplayIndex = 1;
            //com = "[" + cmbDBName.Text + "].dbo.[" + cmbTBName.Text + "]";
            ////count = Functions.sqlcount("select count(*) from " + com,Variables.sever);
            //dtField = Functions.sqlColumnsName(cmbDBName.Text, cmbTBName.Text);

            //dtField.Columns.Add("تعداد رکوردهای خالی");
            //dtField.Columns.Add("تعداد رکوردهای پر");

            //for (int i = 0; i < dtField.Rows.Count; i++)
            //{
            //    dtField.Rows[i][1] = Functions.sqlcount("SELECT COUNT(*) FROM " + com + " WHERE [" + dtField.Rows[i][0].ToString() + "] IS NULL", Variables.sever);
            //    dtField.Rows[i][2] = Functions.sqlcount("SELECT COUNT([" + dtField.Rows[i][0].ToString() + "]) FROM " + com, Variables.sever);
            //}
            DGVSearch.Enabled = true;
            //btnDelField.Enabled = true;
            //DGVSearch.DataSource = dtField;
        }

        private void NullSearch_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            this.Text = "جستجوی اطلاعات خالی" + "  -  نام سرور = " + sqlConnection.DataSource;

            //  load database name of source
            cmbDBName.DataSource = Functions.SqlGetDBName(sqlConnection);

            btnDelField.Enabled = false;
            DGVSearch.Enabled = false;
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            fun.ComboDBEvent(1, dicDBName, cmbDBName, cmbTBName);
        }

        private void btnDelField_Click(object sender, EventArgs e)
        {

        }

        private void DGVSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = DGVSearch.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void cmbTBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Column2.DataSource = Functions.SqlColumnNames(cmbTBName.Text, sqlConnection);
        }
    }
}
