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
    public partial class InfoColumns : Form
    {
        Functions Functions = new Functions();
        SqlConnection sqlConnection = new SqlConnection();


        public InfoColumns(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //  percent
            double dbPercent =0;
            //  clear item
            DGVSearch.Columns.Clear();

            #region DataGridView Design

            DataGridViewTextBoxColumn dgvTxtBxClmOrd = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmName = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmTotal = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmUsed = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmSpace = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmPercent = new DataGridViewTextBoxColumn();

            #endregion

            //  column names
            List<string> lstClmName = Functions.DataTableToList(Functions.SqlColumnNames(cmbTableName.Text, sqlConnection, cmbDBName.Text));

            //  count total
            int intCountTotal = Functions.SqlRecordCount(cmbTableName.Text, sqlConnection);

            //  header text
            #region HeadersText & ToolTipText

            dgvTxtBxClmOrd.HeaderText = "ID";
            dgvTxtBxClmOrd.ToolTipText = "ID";
            dgvTxtBxClmName.HeaderText = "نام فیلد";
            dgvTxtBxClmName.ToolTipText = "نام فیلد";
            dgvTxtBxClmTotal.HeaderText = "کل رکورد";
            dgvTxtBxClmTotal.ToolTipText = "کل رکورد";
            dgvTxtBxClmUsed.HeaderText = "رکورد پر";
            dgvTxtBxClmUsed.ToolTipText = "رکورد پر";
            dgvTxtBxClmSpace.HeaderText = "رکورد خالی";
            dgvTxtBxClmSpace.ToolTipText = "رکورد خالی";
            dgvTxtBxClmPercent.HeaderText = "درصد";
            dgvTxtBxClmPercent.ToolTipText = "درصد";

            #endregion

            #region Add

            //  add column id
            DGVSearch.Columns.Add(dgvTxtBxClmOrd);
            dgvTxtBxClmOrd.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmName);
            dgvTxtBxClmName.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmTotal);
            dgvTxtBxClmTotal.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmUsed);
            dgvTxtBxClmUsed.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmSpace);
            dgvTxtBxClmSpace.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmPercent);
            dgvTxtBxClmPercent.ReadOnly = true;

            #endregion

            //  default value
            #region Default Value

            for (int i = 0; i < lstClmName.Count; i++)
            {
                if (intCountTotal != 0) dbPercent = Convert.ToDouble((Convert.ToDouble(Functions.SqlRecordCount(cmbTableName.Text, sqlConnection, lstClmName[i], "")) / Convert.ToDouble(intCountTotal)) * 100);
                //intpercent = (intCountTotal != 0) ? Convert.ToInt32(lstCountColumn[i]) / intCountTotal * 100 : 0;

                DGVSearch.Rows.Add
                    (
                    i,
                    lstClmName[i],
                    intCountTotal,
                    Functions.SqlRecordCount(cmbTableName.Text, sqlConnection, lstClmName[i], ""),
                    Functions.SqlRecordCount(cmbTableName.Text, sqlConnection, lstClmName[i]),
                    dbPercent.ToString("###")
                    );

            }

            #endregion

            Cursor.Current = Cursors.Default;
        }

        private void NullSearch_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            this.Text = "جستجوی اطلاعات خالی" + "  -  نام سرور = " + sqlConnection.DataSource;

            //  load database name of source
            cmbDBName.DataSource = Functions.SqlGetDBName(sqlConnection);

            //  dafault value
            cmbDBName.Text = "Ehraz";

            btnDelField.Enabled = false;
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {

            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbDBName.Text, sqlConnection);

            //  load table name
            cmbTableName.DataSource = Functions.SqlTableName(sqlConnection);

            //  dafault value
            cmbTableName.Text = "TBL_MKsarparast_95-02-21";

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
            //Column2.DataSource = Functions.SqlColumnNames(cmbTableName.Text, sqlConnection);
        }
    }
}
