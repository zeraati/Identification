using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persia;
using PersiaSL;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace Identification
{
    public partial class FindDuplicate : Form
    {
        Functions Functions = new Functions();
        SqlConnection sqlConnection = new SqlConnection();

        public FindDuplicate(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void FindDuplicate_Load(object sender, EventArgs e)
        {
            //  load dbname
            cmbDBName.DataSource = Functions.SqlGetDBName(sqlConnection);
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbDBName.Text, sqlConnection);

            //  load table name
            cmbTableName.DataSource = Functions.SqlTableName(sqlConnection);
        }

        private void cmbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load column name
            cmbColumnName.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTableName.Text, sqlConnection, cmbDBName.Text));
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn dgvTxtBxClmOrd = new DataGridViewTextBoxColumn();

            dgvTxtBxClmOrd.HeaderText = "ID";
            dgvTxtBxClmOrd.ToolTipText = "ID";

            dgvSearch.Columns.Add(dgvTxtBxClmOrd);
            dgvTxtBxClmOrd.ReadOnly = true;


            string strQuery = "SELECT * FROM dbo.[" + cmbTableName.Text + "] a JOIN(" +
                              " SELECT COUNT(*)c, [" + cmbColumnName.Text + "] cmk FROM dbo.[" + cmbTableName.Text + "]" +
                              " GROUP BY [" + cmbColumnName.Text + "]" +
                              " HAVING COUNT(*) > 1)b" +
                              " ON a.[" + cmbColumnName.Text + "] = b.cmk" +
                              " ORDER BY a.[" + cmbColumnName.Text + "]";

            richTextBox1.Text = strQuery;

            dgvSearch.DataSource = Functions.SqlDataAdapter(strQuery, sqlConnection);

            #region Default Value
            for (int j = 0; j < dgvSearch.Rows.Count; j++)
            {
                //  column radif identity(0,1)
                dgvSearch.Rows[j].Cells[0].Value = j.ToString();
            }
            #endregion

        }
    }
}
