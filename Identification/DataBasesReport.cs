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


        private void DataBasesReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;

            string strQuery = "SELECT [name] [نام بانک] FROM sys.databases";
            List<string> lstDatabase = functions.DataTableToList(sqlfunction.SqlDataAdapter(strQuery, sqlConnection));
            List<string> lstTableName = new List<string>();
            List<string> lstColumnName = new List<string>();

            //  node main
            string strDatasurce = (sqlConnection.DataSource == ".") ? "Local" : sqlConnection.DataSource;
            trvDatabase.Nodes.Add(strDatasurce);

            //  server set image
            trvDatabase.Nodes[0].ImageIndex = 0;


            for (int i = 0; i < lstDatabase.Count; i++)
            {
                trvDatabase.Nodes[0].Nodes.Add(lstDatabase[i].ToString());
                trvDatabase.Nodes[0].Nodes[i].ImageIndex = 1;
                lstTableName = sqlfunction.SqlTableName(sqlConnection, lstDatabase[i].ToString());
                for (int j = 0; j < lstTableName.Count; j++)
                {
                    trvDatabase.Nodes[0].Nodes[i].Nodes.Add(lstTableName[j].ToString());
                    trvDatabase.Nodes[0].Nodes[i].Nodes[j].ImageIndex = 2;

                    lstColumnName = functions.DataTableToList(sqlfunction.SqlColumnNames(lstTableName[j].ToString(), sqlConnection, lstDatabase[i].ToString()));
                    for (int k = 0; k < lstColumnName.Count; k++)
                    {
                        trvDatabase.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(lstColumnName[k].ToString());
                        trvDatabase.Nodes[0].Nodes[i].Nodes[j].Nodes[k].ImageIndex = 3;
                    }

                }
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

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            bool bEnable = false;
            string strSelectNode = "";


            if (trvDatabase.SelectedNode.Level == 1)
            {
                strSelectNode = trvDatabase.SelectedNode.Text;

                for (int j = 0; j < lstbxMain.Items.Count; j++)
                {
                    if (lstbxMain.Items[j].ToString() == strSelectNode)
                    { bEnable = true; break; }
                }

                if (bEnable == false) { lstbxMain.Items.Add(strSelectNode); bEnable = true; }
            }
        }

        private void btnDelFromList_Click(object sender, EventArgs e)
        {
            if (lstbxMain.SelectedIndex != -1) { lstbxMain.Items.RemoveAt(lstbxMain.SelectedIndex); }
        }

        private void btnAddToList_KeyDown(object sender, KeyEventArgs e)
        {
            string s = e.ToString();
            if (e.Modifiers == Keys.Alt)
            {
                //Show the form
            }
        }

        private void DataBasesReport_KeyDown(object sender, KeyEventArgs e)
        {
            string s = e.ToString();
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Alt)
            {
                //Show the form
            }
        }

        private void tvDatabase_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode nodeToDropIn = this.trvDatabase.GetNodeAt(this.trvDatabase.PointToClient(new Point(e.X, e.Y)));
            if (nodeToDropIn == null) { return; }
            if (nodeToDropIn.Level == 1)
            {
                nodeToDropIn = nodeToDropIn.Parent;
            }

            object data = e.Data.GetData(typeof(DateTime));
            if (data == null) { return; }
            nodeToDropIn.Nodes.Add(data.ToString());
            this.lstbxMain.Items.Add(data);
        }

        private void lstbxMain_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void tvDatabase_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }


        private void trvDatabase_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
