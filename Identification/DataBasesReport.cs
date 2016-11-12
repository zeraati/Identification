using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Interop;
using System.Runtime;

namespace Identification
{
    public partial class DataBasesReport : Form
    {

        Functions functions = new Functions();
        SqlFunctions sqlfunction = new SqlFunctions();

        SetPersonFunctions SPF = new SetPersonFunctions();


        #region Lists Data

        List<string> lstDB = new List<string>();
        List<string> lstTable = new List<string>();
        List<string> lstColumn = new List<string>();

        #endregion


        SqlConnection sqlConnection = new SqlConnection();

        public DataBasesReport(SqlConnection sqlcon)
        {
            InitializeComponent();
            sqlConnection = sqlcon;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            tabControl1.Controls.Clear();

            string strDscDb = "";

            //  sqlchange database
            sqlConnection = sqlfunction.SqlConnectionChangeDB("master", sqlConnection);

            DataGridView dgv;
            TextBox txt;

            //  database count

            for (int i = 0; i < lstbxMain.Items.Count; i++)
            {

                TabPage tabpag = new TabPage();

                //  Create New
                dgv = new DataGridView();
                txt = new TextBox();

                //  tabpage text is database name
                tabpag.Text = lstbxMain.Items[i].ToString();
                //  backccolor
                tabpag.BackColor = Color.White;

                #region Peroperty DataGrid
                dgv.Name = "dgv" + i.ToString();
                dgv.ScrollBars = ScrollBars.Both;
                dgv.Size = new System.Drawing.Size(580, 180);
                dgv.Location = new System.Drawing.Point(6, 49);
                #endregion

                #region Peroperty TextBox
                txt.Name = "txt" + i.ToString();
                txt.Size = new System.Drawing.Size(580, 20);
                txt.Location = new System.Drawing.Point(6, 13);
                txt.ReadOnly = true;
                #endregion


                //  list to string
                strDscDb = functions.ListToString(functions.DataTableToList(sqlfunction.SqlDBExtended(sqlConnection, lstbxMain.Items[i].ToString())), " , ");

                //  add tabpag
                tabControl1.Controls.Add(tabpag);

                //  Description Database
                txt.Text = strDscDb;

                //  
                dgv.DataSource = CreateTable(sqlConnection, lstbxMain.Items[i].ToString(), strDscDb);


                //  add controls
                tabpag.Controls.Add(dgv);
                tabpag.Controls.Add(txt);

            }

            Cursor.Current = Cursors.Default;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;


            //  create sheets
            for (int k = 0; k < lstbxMain.Items.Count; k++)
            {
                int intRowNumber = 1;

                worksheet = workbook.Worksheets.Add();
                app.Visible = true;
                worksheet = workbook.ActiveSheet;
                worksheet.Name = lstbxMain.Items[k].ToString();

                worksheet.Cells[intRowNumber, 1] = functions.ListToString(functions.DataTableToList(sqlfunction.SqlDBExtended(sqlConnection, lstbxMain.Items[k].ToString())), " ; ");

                intRowNumber++;
                worksheet.Cells[intRowNumber, 1] = "ردیف";
                worksheet.Cells[intRowNumber, 2] = "نام";
                worksheet.Cells[intRowNumber, 3] = "توضیحات";
                worksheet.Cells[intRowNumber, 4] = "نوع";

                lstTable = sqlfunction.SqlTableName(sqlConnection, lstbxMain.Items[k].ToString());

                intRowNumber++;
                //  naming
                for (int i = 0; i < lstTable.Count; i++)
                {

                    worksheet.Cells[intRowNumber, 1] = intRowNumber;
                    worksheet.Cells[intRowNumber, 2] = lstTable[i];
                    worksheet.Cells[intRowNumber, 3] = functions.ListToString(functions.DataTableToList(sqlfunction.SqlTableExtended(lstTable[i], sqlConnection, lstbxMain.Items[k].ToString())), " ; ");
                    worksheet.Cells[intRowNumber, 4] = "جدول";
                    intRowNumber++;

                    lstColumn = functions.DataTableToList(sqlfunction.SqlColumnNames(lstTable[i], sqlConnection, lstbxMain.Items[k].ToString()));

                    for (int j = 0; j < lstColumn.Count; j++)
                    {

                        worksheet.Cells[intRowNumber, 1] = intRowNumber;
                        worksheet.Cells[intRowNumber, 2] = lstColumn[j];
                        worksheet.Cells[intRowNumber, 3] = functions.ListToString(functions.DataTableToList(sqlfunction.SqlColumnExtended(lstTable[i], lstColumn[j], sqlConnection, lstbxMain.Items[k].ToString())), " ; ");
                        worksheet.Cells[intRowNumber, 4] = "ستون";
                        intRowNumber++;
                    }

                }

            }


            Cursor.Current = Cursors.Default;

        }

        private void lstDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (sqlConnection.Database == "")
            {
                sqlConnection = sqlfunction.SqlConnectionChangeDB("master", sqlConnection);
                sqlConnection = sqlfunction.SqlConnectionChangeDB(lstDatabase.Text, sqlConnection);
            }

            TableSelectedIndexChanged(sqlConnection, lstDatabase, lstTableName);

        }

        #region TableSelectedIndexChanged

        private void TableSelectedIndexChanged(SqlConnection sqlConnection, ListBox lstbxDataBase, ListBox lstbxTable)
        {
            //string strWork = lstbxWorks.GetItemText(lstbxWorks.SelectedItem);
            string strDataBase = lstbxDataBase.GetItemText(lstbxDataBase.SelectedItem);


            // lstbxTable source
            sqlfunction.SqlTableName(sqlConnection, lstbxTable, strDataBase);

        }

        #endregion

        private void lstTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTableName = lstTableName.GetItemText(lstTableName.SelectedItem);


            //  lstbxColumnName
            functions.DataTableToListbox(sqlfunction.SqlColumnNames(strTableName, sqlConnection, lstDatabase.Text), lstColumnName);


        }

        private void DataBasesReport_Load(object sender, EventArgs e)
        {
            string strQuery = "SELECT [name] [نام بانک] FROM sys.databases";

            functions.DataTableToListbox(sqlfunction.SqlDataAdapter(strQuery, sqlConnection), lstDatabase);

        }

        private void lstDatabase_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstDatabase.Items.Count != 0)
            {
                //string strMain = lstDatabase.GetItemText(lstDatabase.SelectedItem);
                if (lstDatabase.Items.Count != 0) lstbxMain.Items.Add(lstDatabase.GetItemText(lstDatabase.SelectedItem));

                if (lstDatabase.SelectedIndex != -1) { lstDatabase.Items.RemoveAt(lstDatabase.SelectedIndex); }
            }
        }

        private void lstbxMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstbxMain.Items.Count != 0)
            {

                if (lstbxMain.Items.Count != 0) lstDatabase.Items.Add(lstbxMain.GetItemText(lstbxMain.SelectedItem));

                if (lstbxMain.SelectedIndex != -1) { lstbxMain.Items.RemoveAt(lstbxMain.SelectedIndex); }

            }
        }

        #region CreateTable

        //  return tables , columns
        /// <summary>
        /// return tables , columns
        /// </summary>
        /// <param name="sqlconnection"></param>
        /// <param name="lstBxDB">ListBox From Database</param>
        /// <returns>datatabe</returns>
        private DataTable CreateTable(SqlConnection sqlconnection, string strDb, string strDscDb = "")
        {
            DataTable dtOut = new DataTable();

            string strDscTable = "", strDscColumn = "";

            #region Create Column

            dtOut.Columns.Add("ردیف", typeof(int));
            dtOut.Columns.Add("نام", typeof(string));
            dtOut.Columns.Add("توضیحات", typeof(string));
            dtOut.Columns.Add("نوع", typeof(string));

            #endregion


            strDscTable = "";

            //
            lstTable = sqlfunction.SqlTableName(sqlConnection, strDb);

            //  create length

            for (int j = 0; j < lstTable.Count; j++)
            {
                strDscTable = functions.ListToString(functions.DataTableToList(sqlfunction.SqlTableExtended(lstTable[j], sqlConnection, strDb)), " , ");

                //  save to datatable
                dtOut.Rows.Add(drSelect(0, j, lstTable[j], strDscTable, dtOut));


                lstColumn = functions.DataTableToList(sqlfunction.SqlColumnNames(lstTable[j], sqlconnection, strDb));


                for (int k = 0; k < lstColumn.Count; k++)
                {

                    strDscColumn = functions.ListToString(functions.DataTableToList(sqlfunction.SqlColumnExtended(lstTable[j], lstColumn[k], sqlconnection, strDb)), " , ");

                    //  save to datatable
                    dtOut.Rows.Add(drSelect(1, k, lstColumn[k], strDscColumn, dtOut));

                }
            }


            return dtOut;

        }

        #endregion

        private DataRow drSelect(int intType, int intNumber, string strName, string strDescription, DataTable dtOut)
        {
            DataRow dr = dtOut.NewRow();

            #region switch
            switch (intType)
            {
                case 0:
                    dr["ردیف"] = intNumber;
                    dr["نام"] = strName;
                    dr["توضیحات"] = strDescription;
                    dr["نوع"] = "جدول";
                    break;
                case 1:
                    dr["ردیف"] = intNumber;
                    dr["نام"] = strName;
                    dr["توضیحات"] = strDescription;
                    dr["نوع"] = "ستون";
                    break;
            }
            #endregion

            return dr;
        }

    }
}
