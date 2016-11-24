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
        SqlFunctions sqlfunction = new SqlFunctions();


        SqlConnection sqlConnection = new SqlConnection();
        private string intInValid;
        private string intValid;
        private string intRepeat;

        public InfoColumns(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<int> lstInt = new List<int>();

            //
            string strSwitch = "";
            int intValid = 0, intInValid = 0, intRepeat = 0;
            int intCountTotal = 0, intCount = 0, intCountNull = 0;
            //  percent
            double dbPercent = 0;


            //  clear item
            DGVSearch.Columns.Clear();

            #region DataGridView Design

            DataGridViewTextBoxColumn dgvTxtBxClmOrd = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmName = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmTotal = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmUsed = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmSpace = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmPercent = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmValid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmNotValid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClmRepeatCount = new DataGridViewTextBoxColumn();

            #endregion

            //  column names
            List<string> lstClmName = Functions.DataTableToList(sqlfunction.SqlColumnNames(cmbTableName.Text, sqlConnection, cmbDBName.Text));

            //  count total
            intCountTotal = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection);

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
            dgvTxtBxClmValid.HeaderText = "تعداد معتبر";
            dgvTxtBxClmValid.ToolTipText = "تعداد معتبر";
            dgvTxtBxClmNotValid.HeaderText = "تعداد نامعتبر";
            dgvTxtBxClmNotValid.ToolTipText = "تعداد نامعتبر";
            dgvTxtBxClmRepeatCount.HeaderText = "تعداد تکراری";
            dgvTxtBxClmRepeatCount.ToolTipText = "تعداد تکراری";

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
            DGVSearch.Columns.Add(dgvTxtBxClmValid);
            dgvTxtBxClmValid.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmNotValid);
            dgvTxtBxClmNotValid.ReadOnly = true;
            DGVSearch.Columns.Add(dgvTxtBxClmRepeatCount);
            dgvTxtBxClmRepeatCount.ReadOnly = true;
            #endregion


            //  default value
            #region Default Value

            for (int i = 0; i < lstClmName.Count; i++)
            {
                intCount = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection, lstClmName[i], "");
                intCountNull = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection, lstClmName[i]);

                dbPercent = (intCountTotal != 0) ? Convert.ToDouble((Convert.ToDouble(intCount) / Convert.ToDouble(intCountTotal)) * 100) : 0;
                string strPercent = (dbPercent != 0) ? dbPercent.ToString("###.##") : "0";

                strSwitch = dgvColumn.Rows[i].Cells[1].Value.ToString();

                //  validator function
                lstInt = Validator(strSwitch, lstClmName[i]);


                DGVSearch.Rows.Add
                    (
                    i,
                    lstClmName[i],
                    Functions.StrNum(intCountTotal),
                    Functions.StrNum(intCount),
                    intCountNull,
                    strPercent,
                    Functions.StrNum(lstInt[0]),
                    Functions.StrNum(lstInt[1]),
                    Functions.StrNum(lstInt[2])
                    );

            }

            #endregion            


            Cursor.Current = Cursors.Default;
        }

        //  valid , invalid , repeat
        private List<int> Validator(string strSwitch, string strClmName)
        {
            List<int> lstInt = new List<int>();


            switch (strSwitch)
            {
                case "هیچکدام":
                    sqlfunction.SqlValidatorCount(sqlConnection, 0, strClmName, cmbTableName.Text, lstInt, cmbDBName.Text);
                    break;
                case "نام،فامیل،نام پدر":
                    sqlfunction.SqlValidatorCount(sqlConnection, 1, strClmName, cmbTableName.Text, lstInt, cmbDBName.Text);
                    break;
                case "کدملی":
                    sqlfunction.SqlValidatorCount(sqlConnection, 2, strClmName, cmbTableName.Text, lstInt, cmbDBName.Text);
                    break;
                case "شماره شناسنامه":
                    sqlfunction.SqlValidatorCount(sqlConnection, 3, strClmName, cmbTableName.Text, lstInt, cmbDBName.Text);
                    break;
                case "تاریخ میلادی":
                    break;
                case "تاریخ شمسی":
                    break;
                case "عددی":
                    sqlfunction.SqlValidatorCount(sqlConnection, 6, strClmName, cmbTableName.Text, lstInt, cmbDBName.Text);
                    break;
            }

            return lstInt;
        }

        private void NullSearch_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            this.Text = "جستجوی اطلاعات خالی" + "  -  نام سرور = " + sqlConnection.DataSource;

            //  load database name of source
            cmbDBName.DataSource = sqlfunction.SqlGetDBName(sqlConnection);

            //  dafault value
            cmbDBName.Text = "Ehraz";

            btnDelField.Enabled = false;
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {

            //  change data base name
            sqlConnection = sqlfunction.SqlConnectionChangeDB(cmbDBName.Text, sqlConnection);

            //  load table name
            cmbTableName.DataSource = sqlfunction.SqlTableName(sqlConnection);

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

            dgvColumn.Columns.Clear();
            DataTable dtColumn = sqlfunction.SqlColumnNames(cmbTableName.Text, sqlConnection, cmbDBName.Text);

            DataGridViewTextBoxColumn dgvTxBxClm = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClm = new DataGridViewComboBoxColumn();

            //  header text
            #region HeadersText & ToolTipText
            dgvTxBxClm.HeaderText = "نام فیلد";
            dgvTxBxClm.ToolTipText = "نام فیلد";
            dgvCmBxClm.HeaderText = "نوع";
            dgvCmBxClm.ToolTipText = "نوع";
            #endregion

            //  readonly
            dgvTxBxClm.ReadOnly = true;

            dgvCmBxClm.Width = 100;

            //  combobox items
            #region Items
            dgvCmBxClm.Items.Add("هیچکدام");
            dgvCmBxClm.Items.Add("نام،فامیل،نام پدر");
            dgvCmBxClm.Items.Add("کدملی");
            dgvCmBxClm.Items.Add("شماره شناسنامه");
            dgvCmBxClm.Items.Add("تاریخ میلادی");
            dgvCmBxClm.Items.Add("تاریخ شمسی");
            dgvCmBxClm.Items.Add("عددی");
            #endregion


            //  add
            dgvColumn.Columns.Add(dgvTxBxClm);
            dgvColumn.Columns.Add(dgvCmBxClm);


            for (int i = 0; i < dtColumn.Rows.Count; i++)
            {
                dgvColumn.Rows.Add(dtColumn.Rows[i][0].ToString(), dgvCmBxClm.DisplayMember = "هیچکدام");
            }


        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;


            app.Visible = true;
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Sheet1";

            #region First Cells
            worksheet.Cells[1, 1] = "ID";
            worksheet.Cells[1, 2] = "نام فیلد";
            worksheet.Cells[1, 3] = "کل رکورد";
            worksheet.Cells[1, 4] = "رکورد پر";
            worksheet.Cells[1, 5] = "رکورد خالی";
            worksheet.Cells[1, 6] = "درصد";
            worksheet.Cells[1, 7] = "تعداد معتبر";
            worksheet.Cells[1, 8] = "تعداد نامعتبر";
            worksheet.Cells[1, 9] = "تعداد تکراری";
            #endregion


            for (int k = 2; k < DGVSearch.RowCount + 2; k++)
            {
                for (int j = 0; j < DGVSearch.ColumnCount; j++)
                {
                    worksheet.Cells[k, j + 1] = (DGVSearch.Rows[k - 2].Cells[j].Value == null) ? "Null" : DGVSearch.Rows[k - 2].Cells[j].Value.ToString();
                }
            }

            Cursor.Current = Cursors.Default;
        }



    }
}
