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
    public partial class Uniqe : Form
    {
        Functions Functions = new Functions();
        SqlConnection sqlConnection = new SqlConnection();

        string strQuery;

        public Uniqe(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void Uniqe_Load(object sender, EventArgs e)
        {
            this.Text = "یکتا سازی" + "  -  نام سرور = " + sqlConnection.DataSource;

            //  load database name of source
            cmbDBName.DataSource = Functions.SqlGetDBName(sqlConnection);
            
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  set cmbTBName soutce    // load table names
            //cmbTBName.DataSource = Functions.SqlGetTableNameInDB(sqlConn);
            
        }

        private void cmbTBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbField.DataSource = Functions.SqlColumnNames(cmbTBName.Text, sqlConnection);

            Functions.LoadColumnInfo(cmbTBName.Text, clbField, sqlConnection);

            btnAllSelect.Text = "انتخاب همه";
        }
        int strIndex;
        string field;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            lstReport.Items.Clear();
            DataTable dt;
            int check=0;
            if (chCreateID.CheckState == CheckState.Checked)
            {
                //dt = Functions.SqlColumns(sqlConn, cmbTBName.Text);
                //check = Functions.CheckField(dt, "ID");
                if (check == 0)
                {
                    strQuery = "ALTER TABLE [" + cmbDBName.Text + "].dbo.[" + cmbTBName.Text + "] ADD [ID] INT IDENTITY(0,1)";

                    //if (Functions.SqlExcutCommand(query, sqlConn) == true)
                    //{
                    //    Functions.LoadColumnNew(sqlConn, cmbTBName.Text, clbField);

                    //    //  load cmb field source
                    //    cmbField.DataSource = Functions.lstLoadClm(sqlConn, cmbTBName.Text);

                    //    btnAllSelect.Text = "انتخاب همه";
                    //}
                }
            }

            for (int i = 0; i < clbField.Items.Count; i++)
            {
                if (clbField.GetItemCheckState(i) == CheckState.Checked)
                {
                    field = clbField.Items[i].ToString();
                    strIndex = field.IndexOf("[");
                    field = field.Substring(0, strIndex).Trim();
                    if (cmbField.Text != field)
                    {
                        strQuery = "SELECT COUNT(*) FROM (SELECT [" + field + "],COUNT([" + cmbField.Text + "])coun";
                        strQuery += " FROM [" + cmbDBName.Text + "].dbo.[" + cmbTBName.Text + "] Where [" + cmbField.Text + "] is not null";
                        strQuery += " GROUP BY [" + field + "] HAVING COUNT([" + cmbField.Text + "])<>1)a";
                        //check= Convert.ToInt32( Functions.sqlcount(query,Variables.sever));
                        //lstReport.Items.Add(field + " = " + Functions.SqlRecordCount(query, sqlConn));
                    }
                }
            }
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            Functions.SelectUnselect(clbField, btnAllSelect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "ssfsdf" + Environment.NewLine +"fgfdgsdf";
            textBox1.Text = str;
            lstReport.Items.Add(str);
        }
        //*****     Functions

    }
}
