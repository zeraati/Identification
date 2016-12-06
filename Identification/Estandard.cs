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
    public partial class Estandard : Form
    {
        Functions Functions = new Functions();
        SqlFunctions sqlfunction = new SqlFunctions();
        ClsStandard clsfunction = new ClsStandard();


        SqlConnection sqlConnection = new SqlConnection();

        FontDialog FD = new FontDialog();

        string strPathFont = @"../Font.txt";
        string strFunctionsFile = @"../Functions";
        string strType, strDate;
        int intTableCheck;


        public Estandard(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void Estandard_Load(object sender, EventArgs e)
        {
            Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(DateTime.Now);
            CenterToScreen();
            this.Text = "استاندارد سازی" + "   -   " + sqlConnection.DataSource;

            //  load dbname
            cmbDBName.DataSource = sqlfunction.SqlGetDBName(sqlConnection);

            //  defualt value
            cmbDBName.Text = "Ehraz";

            strDate = solarDate.ToString("yyyyMMdd");
            strDate = strDate.Replace("/", "");

            //  auto change color lblText
            timer1.Interval = 2000;
            timer1.Start();


        }

        #region Buttons

        //  button report database
        private void btnReportDB_Click(object sender, EventArgs e)
        {
            //  check table '___Report'
            int check = cmbTableName.FindString("___Report");

            if (check == -1)
            {

                if (sqlfunction.SqlCreateReport(sqlConnection) == " => Done!")
                {

                    //  set cmbTBName source    // load table names
                    loadTbName();

                    insert_report();
                }
            }
            else
            {
                insert_report();
            }
        }

        //  button backup database
        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            SQLDBbackup sqlbackup = new SQLDBbackup(sqlConnection);
            sqlbackup.ShowDialog();
        }

        //button convert date
        private void btnConvertDate_Click(object sender, EventArgs e)
        {
            DateConvert frm = new DateConvert(sqlConnection, cmbTableName.Text);
            frm.ShowDialog();
        }

        //  button exit
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        #endregion




        #region GroupBox1

        //  combobox database name
        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> lstSqlFile = new List<string>();
            bool bol = false;


            //  change data base name
            sqlConnection = sqlfunction.SqlConnectionChangeDB(cmbDBName.Text, sqlConnection);

            //  list functions files to sql
            lstSqlFile = lstSqlFile = Functions.DataTableToList(sqlfunction.SqlFunctionsFiles(sqlConnection));

            //  list functions files is not sql

            //  check sql functions
            string[] files = Directory.GetFiles(strFunctionsFile, "*.txt");
            foreach (string file in files)
            {
                bol = false;
                for (int i = 0; i < lstSqlFile.Count; i++)
                {
                    if (Path.GetFileNameWithoutExtension(file) == lstSqlFile[i])
                    { bol = true; break; }
                }

                //  create function is not sql
                if (bol == false) { sqlfunction.SqlExcutCommand(File.ReadAllText(file), sqlConnection); lst1.Items.Add("Create Function Is Sql : " + Path.GetFileNameWithoutExtension(file)); }
            }

            //  select tabcontrol report
            tabControl2.SelectedTab = tabControl2.TabPages[1];


            //  set cmbTBName source    // load table names
            loadTbName();

        }


        //  combobox table name
        private void cmbTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //  load column names
            loadColumn();

            //  displayed count record
            chbDisRec_CheckedChanged(null, null);

            cmbTableNameTab1.Text = cmbTableName.Text;

            Cursor.Current = Cursors.Default;
        }


        #region نمایش رکوردهای هر فیلد

        private void chbDisRec_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //lst1.Items.Clear();

            DataTable dtColumns = sqlfunction.SqlColumns(cmbTableName.Text, sqlConnection, cmbDBName.Text);

            //List<string> lstTest = Functions.DataTableToList(dtColumns,1);
            string strFieldName;
            int intCount;

            //  record table
            intCount = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection);


            //  add item
            lst1.Items.Add("***************************************");
            lst1.Items.Add("تعداد فیلد : " + dtColumns.Rows.Count);
            lst1.Items.Add("تعداد رکورد جدول : " + Functions.StrNum(intCount));


            //  checked field
            if (chbDisRec.CheckState == CheckState.Checked)
            {
                lst1.Items.Clear();
                for (int j = 0; j < dtColumns.Rows.Count; j++)
                {
                    //  get field name
                    strFieldName = dtColumns.Rows[j][1].ToString();

                    //  get data type
                    strType = dtColumns.Rows[j][3].ToString();


                    if (strType == "image" | strType == "ntext")
                    { lst1.Items.Add("فیلد " + strFieldName + " قابل شمارش نیست "); }

                    else
                    {
                        //  count columns record
                        intCount = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection, strFieldName, cmbDBName.Text);
                        lst1.Items.Add(strFieldName + " => " + intCount + " Record");
                    }

                }
                tabControl2.SelectedTab = tabControl2.TabPages[1];
            }

            Cursor.Current = Cursors.Default;

        }
        #endregion


        #region TabControl

        #region Tab Page 1

        #region Update
        /*
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (clb2.CheckedIndices.Count != 0)
            {
                //  richtxt clear
                richTxt.Text = "";

                Cursor.Current = Cursors.WaitCursor;
                lst1.Items.Add("--------------------------------------");

                string strFieldName = "", strDataType;
                string strColumnData;
                string strQuery = "", strWhere, strFinal = "";
                int intIncerement, intCount, intTableCount;


                //  table records count
                intTableCount = sqlfunction.SqlRecordCount(cmbTableName.Text, sqlConnection, "*", cmbDBName.Text);

                lst1.Items.Add("تعداد فیلد : " + chlstbxColumn.Items.Count.ToString());
                lst1.Items.Add("تعداد رکورد جدول : " + Functions.StrNum(intTableCount));

                #region نام فیلدی که تیک زده شده را به ما میدهد
                for (int r = 0; r < chlstbxColumn.Items.Count; r++)
                {
                    if (chlstbxColumn.GetItemCheckState(r) == CheckState.Checked)
                    {
                        strFieldName = ColumnName(chlstbxColumn.Items[r].ToString()).Trim();
                    }
                }
                #endregion

                #region Replace
                if (cbReplace.CheckState == CheckState.Checked)
                {
                    int l;
                    for (l = 0; l < chlstbxColumn.Items.Count; l++)
                    {
                        if (chlstbxColumn.GetItemCheckState(l) == CheckState.Checked)
                        {
                            //  column name
                            strFieldName = ColumnName(chlstbxColumn.Items[l].ToString());

                            //  update column
                            if (txtNew.Text.ToUpper().Contains("NULL") == true)
                            {
                                strFinal = sqlfunction.SqlUpdateData(cmbTableName.Text,
                                                                          strFieldName,
                                                                          "NULL",
                                                                          cmbDBName.Text,
                                                                          sqlConnection,
                                                                          "[" + strFieldName + "] = N'" + txtBefore.Text + "'",
                                                                          " Replace data ");
                            }
                            else
                            {
                                strFinal = sqlfunction.SqlUpdateData(cmbTableName.Text,
                                                                       strFieldName,
                                                                       "Replace (" + strFieldName + "),N'" + txtBefore.Text + "',N'" + txtNew.Text + "')",
                                                                       cmbDBName.Text,
                                                                       sqlConnection,
                                                                       "[" + strFieldName + "] = N'" + txtBefore.Text + "'",
                                                                       " Replace data ");
                            }

                        }
                    }
                }
                #endregion


                for (int i = 0; i < clb2.Items.Count; i++)
                {
                    if (clb2.GetItemCheckState(i) == CheckState.Checked)
                    {
                        string strChecked = clb2.Items[i].ToString();

                        switch (strChecked)
                        {

                            #region Delete Not Valid
                            case "حذف مقدار تهی":

                                for (int j = 0; j < chlstbxColumn.Items.Count; j++)
                                {
                                    if (chlstbxColumn.GetItemCheckState(j) == CheckState.Checked)
                                    {
                                        //  column name
                                        strFieldName = ColumnName(chlstbxColumn.Items[j].ToString()).Trim();

                                        //  column data type
                                        strDataType = ColumnDataType(chlstbxColumn.Items[j].ToString());

                                        //  check string & numeric
                                        if (strDataType.Contains("("))
                                        {
                                            //query
                                            strQuery = "Update [" + cmbTableName.Text + "] Set [" + strFieldName + "] = Null Where [" + strFieldName + "] = N'' or [" + strFieldName + "] = N'0' or [" + strFieldName + "] = N'-' or [" + strFieldName + "] = N' ' and ISNUMERIC([" + strFieldName + "])<>1";
                                        }
                                        else
                                        {
                                            //query
                                            strQuery = "Update [" + cmbTableName.Text + "] Set [" + strFieldName + "] = Null Where [" + strFieldName + "] = 0 and ISNUMERIC([" + strFieldName + "])<>1";
                                        }
                                        //  run query
                                        richTxt.Text = (richTxt.Text != "") ? "/r/n" + strQuery : richTxt.Text;
                                        sqlfunction.SqlExcutCommand(strQuery, sqlConnection);

                                        //query
                                        strQuery = "update [" + cmbTableName.Text + "] Set [" + strFieldName + "] =LTRIM(RTRIM([" + strFieldName + "]))";

                                        //  run query
                                        lst1.Items.Add("حذف مقادیر تهی و نا معتبر" + strFieldName + ": " + sqlfunction.SqlExcutCommand(strQuery, sqlConnection));
                                        strQuery = "";
                                    }
                                }
                                break;
                            #endregion

                            #region Edit Data Type Column
                            case "نوع و اندازه فیلد":
                                EditDataType();
                                loadColumn();
                                break;
                            #endregion

                            #region Replace Character ک و ی
                            case "اصلاح ي و ک":
                                for (int j = 0; j < chlstbxColumn.Items.Count; j++)
                                {
                                    //  column name
                                    strFieldName = ColumnName(chlstbxColumn.Items[j].ToString());

                                    //  run query
                                    //strFinal = sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, " dbo.FixKY([" + strFieldName + "])", sqlConnection, strFieldName);

                                    //  report
                                    if (strFinal.Contains("Done"))
                                    { lstReport("Replace(K Or Y) [" + strFieldName + "]:" + strFinal); }
                                    else lstReport("Replace(K Or Y) [" + strFieldName + "]: Is Not Replace ");
                                }
                                break;
                            #endregion

                            #region Standard CodeMelli
                            case "اصلاح کد ملی":

                                //StandardCodeMelli(strFieldName);

                                break;
                            #endregion

                            #region Standard ShenasCode
                            case "اصلاح شناسنامه":

                                //  data
                                //strColumnData = "dbo.[CM-Fix]([" + strFieldName + "])";

                                #region Delete Character Not Numeric

                                ////  update character not numeric
                                //strFinal = sqlfunction.SqlUpdateData(cmbTableName.Text, strFieldName, strColumnData, sqlConnection);

                                ////  report
                                //lstReport("Delete Character Not Numeric => " + strFieldName + strFinal);

                                #endregion

                                #region ShenasCode With Len > 7 To CodeMelli
                                //  query
                                //strFinal = sqlfunction.SqlUpdateColumnData(cmbTableName.Text, "CodeMelli", 
                                //    "RIGHT('00'+ " + strColumnData + ",10)," + strFieldName + "= NULL ", 
                                //    sqlConnection, strFieldName, "LEN(" + strColumnData + ")>7 and CodeMelli is null", "");

                                ////  report
                                //lstReport("ShenasCode With Len > 7 To CodeMelli => " + strFieldName + ": " + strFinal);

                                //if (strFinal.Contains("Done"))
                                //{ StandardCodeMelli("CodeMelli"); }

                                #endregion

                                #region Delete ShenasCode Not Valid

                                //  where query
                                //strWhere = strFieldName + "='0' or " + strFieldName + "=' ' or LEN(" + strFieldName + ")>7";

                                ////  update
                                //strFinal = sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "NULL", sqlConnection, strWhere, "");

                                ////  report
                                //lstReport("Delete ShenasCode Not Valid => " + strFieldName + ": " + strFinal);

                                #endregion
                                break;
                            #endregion

                            #region Standard Date
                            case "استاندارد کردن تاریخ":
                                for (int l = 0; l < chlstbxColumn.Items.Count; l++)
                                {
                                    if (chlstbxColumn.GetItemCheckState(l) == CheckState.Checked)
                                    {
                                        //  column name
                                        strFieldName = ColumnName(chlstbxColumn.Items[l].ToString());

                                        #region Standard Formated Date
                                        //  sql update query
                                        //sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'-','/')", sqlConnection);
                                        //sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'_','/')", sqlConnection);
                                        //sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'.','/')", sqlConnection);
                                        //sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],',','/')", sqlConnection);

                                        ////  run query                                    
                                        //lst1.Items.Add(" Ltrim & Rtrim " +
                                        //                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "ltrim(rtrim([" + strFieldName + "]))", sqlConnection)
                                        //                );
                                        ////  run query
                                        //lst1.Items.Add(" Standard Date " +
                                        //                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "dbo.[FE_Date]([" + strFieldName + "]) where [" + strFieldName + "] IS NOT NULL", sqlConnection)
                                        //                );
                                        #endregion

                                        #region Delete Not Valid Date
                                        //  run query
                                        //lst1.Items.Add(" Delete Not Valid Date " +
                                        //                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strFieldName, " NULL where dbo.[CM-Fix]([" + strFieldName + "])= 0 ", sqlConnection)
                                        //                );
                                        #endregion

                                    }
                                }
                                break;
                            #endregion

                            #region Date Convert
                            case "تبدیل تاریخ":
                                DateConvert frm = new DateConvert(sqlConnection, cmbTableName.Text);
                                frm.ShowDialog();
                                break;
                            #endregion

                            #region Save Characters Substring
                            case "ذخیره چند کاراکتر":
                                Substring frmSubstring = new Substring();
                                frmSubstring.ShowDialog();
                                break;
                            #endregion

                            #region Delete Space Or Zero Value Columns
                            case "حذف فیلد های خالی":
                                intTableCheck = 0;
                                for (int j = 0; j < chlstbxColumn.Items.Count; j++)
                                {
                                    if (chlstbxColumn.GetItemCheckState(j) == CheckState.Checked)
                                    {
                                        //  column name
                                        strFieldName = ColumnName(chlstbxColumn.Items[j].ToString());

                                        //  run query
                                        strFinal = sqlfunction.SqlDropColumnSpace(cmbTableName.Text, strFieldName, sqlConnection);

                                        //  report
                                        if (strFinal.Contains("Done"))
                                        { lstReport(strFinal); }

                                    }
                                }

                                //  load columns
                                loadColumn();

                                break;
                            #endregion

                            #region Delete Space Rows
                            case "حذف رکوردهای خالی":

                                //  defualt value
                                strWhere = "";
                                intIncerement = 0;

                                //  string where
                                foreach (int Index in chlstbxColumn.CheckedIndices)
                                {
                                    if (intIncerement == 0) { strWhere = "[" + ColumnName(chlstbxColumn.Items[Index].ToString()) + "] IS NULL "; intIncerement++; }
                                    else { strWhere += " AND " + "[" + ColumnName(chlstbxColumn.Items[Index].ToString()) + "] IS NULL "; }
                                }

                                //  count rows where is null
                                intCount = sqlfunction.SqlCountColumn(cmbTableName.Text, sqlConnection, strWhere);

                                if (intCount != 0)
                                {
                                    //  run query
                                    strFinal = sqlfunction.SqlDropRows(cmbTableName.Text, sqlConnection, strWhere);

                                    //  report
                                    if (strFinal.Contains("Done"))
                                    { lstReport("Count = " + intCount.ToString() + " , " + strFinal); }
                                }
                                else
                                { lstReport(" Rows Is Not Null "); }

                                break;
                            #endregion

                            #region Delete Tables Space
                            case "حذف جدول خالی":
                                if (intTableCheck == 0)
                                {
                                    //  run query
                                    strFinal = sqlfunction.SqlDropTableSpace(cmbTableName.Text, sqlConnection);

                                    //  report
                                    if (strFinal.Contains("Done")) { lstReport(strFinal); }
                                }

                                //  set cmbTBName source    // load table names
                                loadTbName();

                                break;
                                #endregion

                        }
                    }
                }


                lst1.SelectedIndex = lst1.Items.Count - 1;

                //  select tabcontrol
                tabControl2.SelectedTab = tabControl2.TabPages[1];

                Cursor.Current = Cursors.Default;
            }
        }
        */
        #endregion




        #endregion


        #region Tab Page 2

        #region Update 2
        private void btnStd2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.AppStarting;

            #region Variable

            string strFieldName = "", strDataType;
            string strColumnData, strColumnCodemelli = "";
            string strFinal = "", strWhere = "";
            int intIncerement, intCount, intTableCount;

            List<string> lstFinal = new List<string>();
            List<string> lstClmName = new List<string>();

            #endregion


            #region پیدا کردن نام ستون کدملی

            if (dgvStd2 != null)
            {
                for (int i = 0; i < dgvStd2.RowCount - 1; i++)
                {
                    //  checked rows
                    if (dgvStd2.Rows[i].Cells[2].Value.ToString() != "")
                    {

                        lst1.Items.Add("استاندارد 2");
                        lst1.Items.Add("--------------------------------------");


                        string strColumn = dgvStd2.Rows[i].Cells[1].Value.ToString();
                        string strType = dgvStd2.Rows[i].Cells[2].Value.ToString();


                        if (strType == "کدملی") strColumnCodemelli = strColumn;

                        //  exit from for
                        break;
                    }
                }
            }
            #endregion


            #region Standard Sazi New

            if (dgvStd2 != null)
            {
                #region Standard from DataGridView
                for (int i = 0; i < dgvStd2.RowCount - 1; i++)
                {
                    //  checked rows
                    if (dgvStd2.Rows[i].Cells[2].Value.ToString() != "")
                    {
                        string strColumn = dgvStd2.Rows[i].Cells[1].Value.ToString();
                        string strType = dgvStd2.Rows[i].Cells[2].Value.ToString();

                        //  function standard sazi
                        lstFinal = clsfunction.Standard(cmbTableName.Text, strColumn, strType, strColumnCodemelli, sqlConnection, cmbDBName.Text);

                        //  report
                        lst1.Items.Add(lstFinal);
                    }
                }
                #endregion

                //  checked list box item
                #region Checked List Box
                for (int j = 0; j < dgvStd2.RowCount - 1; j++)
                {
                    strFieldName = dgvStd2.Rows[j].Cells[1].Value.ToString();
                    for (int k = 0; k < chlstbxEvent.Items.Count; k++)
                    {
                        if (chlstbxEvent.GetItemCheckState(k) == CheckState.Checked)
                        {
                            switch (k)
                            {
                                //  type & size column
                                case 0:
                                    EditDataType();
                                    loadColumn();
                                    break;

                                //  drop column free from data
                                case 1:
                                    //  run query
                                    strFinal = sqlfunction.SqlDropColumnSpace(cmbTableName.Text, strFieldName, cmbDBName.Text, sqlConnection);

                                    //  report
                                    if (strFinal.Contains("Done"))
                                    { lstReport(strFinal); }
                                    break;

                                //  delete row free
                                case 2:

                                    lstClmName = Functions.DataTableToList(sqlfunction.SqlColumnNames(cmbTableName.Text, sqlConnection, cmbDBName.Text));

                                    for (int l = 0; l < lstClmName.Count; l++)
                                    {
                                        strWhere += (l == 0) ? "[" + lstClmName[l] + "] IS NULL " : " AND [" + lstClmName[l] + "] IS NULL ";
                                    }

                                    //  run query
                                    intCount = sqlfunction.SqlDropRows(cmbTableName.Text, cmbDBName.Text, sqlConnection, strWhere);

                                    if (intCount != 0)
                                    { lstReport(" delete row : " + intCount.ToString()); }
                                    else
                                    { lstReport(" Rows Is Not Null "); }
                                    break;
                            }
                        }
                    }
                }
                #endregion


            }
            #endregion


            Cursor.Current = Cursors.Default;
        }
        #endregion


        #endregion


        #region Tab Page 3


        #endregion


        #region Tab Page 4

        private void btnUpdDesign_Click(object sender, EventArgs e)
        {
            string strDataType = "", strFinal = "", strId;
            bool bGoLoadColumn = false;


            if (dgvDesign != null)
            {
                for (int i = 0; i < dgvDesign.RowCount; i++)
                {
                    //  checked rows
                    if (Convert.ToBoolean(dgvDesign.Rows[i].Cells[1].Value) == true)
                    {

                        //  data type is string
                        strDataType = dgvDesign.Rows[i].Cells[4].Value.ToString();
                        //  len is null => numberic
                        if (dgvDesign.Rows[i].Cells[5].Value != null)
                        { strDataType = dgvDesign.Rows[i].Cells[4].Value.ToString() + "(" + dgvDesign.Rows[i].Cells[5].Value.ToString() + ")"; }

                        //  warrning if is null
                        if (
                            dgvDesign.Rows[i].Cells[2].Value != null |
                            dgvDesign.Rows[i].Cells[3].Value != null |
                            dgvDesign.Rows[i].Cells[4].Value != null |
                            dgvDesign.Rows[i].Cells[5].Value != null
                            )
                        {
                            //  id is null
                            #region Id Is Null & Add New Column

                            if (dgvDesign.Rows[i].Cells[0].Value == null)
                            {

                                //  create new column
                                strFinal = sqlfunction.SqlAddNewColumn
                                    (
                                    cmbTableName.Text,                              //  table name
                                    dgvDesign.Rows[i].Cells[2].Value.ToString(),    //  column name
                                    strDataType,                                    //  datatype
                                    dgvDesign.Rows[i].Cells[3].Value.ToString(),    //  nullable
                                    sqlConnection,                                  //  connection
                                    cmbDBName.Text
                                    );

                                //  enable load column
                                bGoLoadColumn = true;

                                //  report
                                lst1.Items.Add(strFinal + dgvDesign.Rows[i].Cells[2].Value.ToString());
                            }
                            #endregion

                            #region Id Using => Edit or Delete Column

                            //  id using
                            else
                            {
                                //  get Id
                                strId = dgvDesign.Rows[i].Cells[0].Value.ToString();

                                //  edit column
                                if (dgvDesign.Rows[i].Cells[6].Value.ToString() == "ویرایش")
                                {

                                    strFinal = sqlfunction.SqlEditColumn
                                                                    (
                                                                    cmbTableName.Text,
                                                                    lblOldColumn.Text,
                                                                    dgvDesign.Rows[i].Cells[2].Value.ToString(),
                                                                    strDataType,
                                                                    sqlConnection,
                                                                    cmbDBName.Text
                                                                    );
                                    lblOldColumn.Text = "";
                                }
                                //  delete column
                                else
                                {
                                    DialogResult DR = MessageBox.Show("؟آیا می خواهید فیلدها را حذف کنید", "!هشدار", MessageBoxButtons.YesNo);
                                    if (DR == DialogResult.Yes)
                                    {
                                        strFinal = sqlfunction.SqlDropColumn
                                                                    (cmbTableName.Text,
                                                                    dgvDesign.Rows[i].Cells[2].Value.ToString(),
                                                                    cmbDBName.Text,
                                                                    sqlConnection);
                                    }
                                    else lst1.Items.Add(".عملیات لغو شد");
                                }

                                //  report
                                lstReport(strFinal);

                                //  enable load column
                                bGoLoadColumn = true;
                            }

                            #endregion

                        }
                    }
                }
            }

            //  load column  
            if (bGoLoadColumn == true) { loadColumn(); bGoLoadColumn = false; }
        }


        //  standard automatic
        private void btnAuto_Click(object sender, EventArgs e)
        {   //
            List<string> lsttest = new List<string>();

            #region Edit Column Names

            for (int i = 0; i < Variable.strArray.Length / 4; i++)
            {
                for (int j = 0; j < dgvDesign.Rows.Count - 1; j++)
                {
                    string str = dgvDesign.Rows[j].Cells[2].Value.ToString().ToUpper().Replace(" ", "");
                    string strs = Variable.strArray[i, 0].ToUpper().Replace(" ", "") + " , " + Variable.strArray[i, 1].ToUpper().Replace(" ", "") + " , " + Variable.strArray[i, 2].ToUpper().Replace(" ", "");
                    //  check column Same              
                    if (dgvDesign.Rows[j].Cells[2].Value.ToString() == Variable.strArray[i, 0])
                    { continue; }

                    if (dgvDesign.Rows[j].Cells[2].Value.ToString().ToUpper().Replace(" ", "") == Variable.strArray[i, 0].ToUpper().Replace(" ", "") ||
                        dgvDesign.Rows[j].Cells[2].Value.ToString().ToUpper().Replace(" ", "") == Variable.strArray[i, 1].ToUpper().Replace(" ", "") ||
                        dgvDesign.Rows[j].Cells[2].Value.ToString().ToUpper().Replace(" ", "") == Variable.strArray[i, 2].ToUpper().Replace(" ", "") ||
                        dgvDesign.Rows[j].Cells[2].Value.ToString().ToUpper().Replace(" ", "") == Variable.strArray[i, 3].ToUpper().Replace(" ", ""))
                    {
                        lsttest.Add(Variable.strArray[i, 0] + " , " + j.ToString());
                        lstReport(sqlfunction.SqlRename("COLUMN", dgvDesign.Rows[j].Cells[2].Value.ToString(), Variable.strArray[i, 0], sqlConnection, cmbTableName.Text));
                        break;
                    }
                }
            }

            #endregion

            //  load column
            cmbTB_SelectedIndexChanged(null, null);

        }

        #endregion

        #region Tab Page 5

        private void btnChangeTableName_Click(object sender, EventArgs e)
        {
            string strFinal;

            //  change table name
            strFinal = sqlfunction.SqlEditTableName(cmbTableNameTab1.Text, txtNewTableName.Text, sqlConnection, true);

            //  conclude final
            lstReport(" TableName => [" + txtNewTableName.Text + "] => " + strFinal);

            // load table names of source
            if (strFinal.Contains("Done")) loadTbName();

        }

        private void btnTBCopy_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string strFinal;

            //  copy table data
            strFinal = sqlfunction.SqlCopyTable(cmbTableNameTab1.Text, txtCopyTableName.Text, sqlConnection);

            //  conclude final
            lstReport(strFinal);

            // load table names of source
            if (strFinal.Contains("Done")) loadTbName();

            Cursor.Current = Cursors.Default;
        }

        private void btnTBDelete_Click(object sender, EventArgs e)
        {
            string strFinal = "";
            DialogResult DR = new DialogResult();
            DR = MessageBox.Show("حذف شود" + " [" + cmbTableNameTab1.Text + "] " + "آیا می خواهید جدول", "!هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            if (DR == DialogResult.Yes)
            {
                //  drop table
                strFinal = sqlfunction.SqlDropTable(cmbTableNameTab1.Text, sqlConnection);

                //  conclude final
                lstReport(strFinal);

                // load table names of source
                if (strFinal.Contains("Done")) loadTbName();

            }
            else lst1.Items.Add("عملیات لغو شد");
        }

        private void cmbTableNameTab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNewTableName.Text = cmbTableNameTab1.Text;
            txtCopyTableName.Text = cmbTableNameTab1.Text + "_copy";
        }


        #endregion

        #region Tab Page 6

        #endregion


        private void dgvDesign_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // My combobox column is the second one so I hard coded a 1, flavor to taste
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvDesign.Rows[e.RowIndex].Cells[3];
            if (cb.Value != null & dgvDesign.Rows[e.RowIndex].Cells[4].Value != null)
            {
                dgvDesign.Rows[e.RowIndex].Cells[5].ReadOnly = ReadOnly(dgvDesign.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (ReadOnly(dgvDesign.Rows[e.RowIndex].Cells[4].Value.ToString()) == true) dgvDesign.Rows[e.RowIndex].Cells[5].Value = null;
            }
        }



        #endregion


        #region Cell Mouse Click

        Label lblOldColumn = new Label();
        Label lblRowIndex = new Label();
        Label lblCells = new Label();

        private void dgvDesign_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvDesign.Rows[e.RowIndex].Cells[2].Value != null & lblOldColumn.Text == "")
            {
                lblOldColumn.Text = dgvDesign.Rows[e.RowIndex].Cells[2].Value.ToString();

                lblCells.Text = 2.ToString(); lblRowIndex.Text = e.RowIndex.ToString();
            }

        }

        #endregion


        #endregion



        //********
        #region loadColumn of DataGridView
        private void loadColumn()
        {
            //  clear item
            dgvDesign.Columns.Clear();
            dgvStd2.Columns.Clear();
            chlstbxColumn.Items.Clear();

            DataTable dtColumns = new DataTable();
            //  list count is not null
            List<string> lstCount = new List<string>();
            //  list count is null
            List<string> lstCountNull = new List<string>();

            #region DataGridView Design

            //  datagrid columns
            DataGridViewTextBoxColumn dgvTxBxClmOrd = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxBxClmOrd2 = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn dgvChBxClm = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClm = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dgvTxtBxClm2 = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClmType = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClmType2 = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClmNull = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn dgvTxtClmLen = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClmEvent = new DataGridViewComboBoxColumn(); // copy , delete
            DataGridViewTextBoxColumn dgvTxtClmCount = new DataGridViewTextBoxColumn(); //  count columns
            DataGridViewTextBoxColumn dgvTxtClmCountNull = new DataGridViewTextBoxColumn(); //  count columns is null

            #endregion

            //  header text
            #region HeadersText & ToolTipText

            dgvTxBxClmOrd.HeaderText = "ID";
            dgvTxBxClmOrd.ToolTipText = "ID";
            dgvTxBxClmOrd2.HeaderText = "ID";
            dgvTxBxClmOrd2.ToolTipText = "ID";
            dgvTxtBxClm.HeaderText = "نام فیلد";
            dgvTxtBxClm.ToolTipText = "نام فیلد";
            dgvTxtBxClm2.HeaderText = "نام فیلد";
            dgvTxtBxClm2.ToolTipText = "نام فیلد";
            dgvCmBxClmNull.HeaderText = "Nullable";
            dgvCmBxClmNull.ToolTipText = "Nullable";
            dgvCmBxClmType.HeaderText = "نوع";
            dgvCmBxClmType.ToolTipText = "نوع";
            dgvCmBxClmType2.HeaderText = "نوع";
            dgvCmBxClmType2.ToolTipText = "نوع";
            dgvTxtClmLen.HeaderText = "Lenth";
            dgvTxtClmLen.ToolTipText = "Lenth";
            dgvCmBxClmEvent.HeaderText = "عملیات";
            dgvCmBxClmEvent.ToolTipText = "عملیات";
            dgvTxtClmCount.HeaderText = "تعداد رکورد پر";
            dgvTxtClmCount.ToolTipText = "تعداد رکورد پر";
            dgvTxtClmCountNull.HeaderText = "رکورد خالی";
            dgvTxtClmCountNull.ToolTipText = "رکورد خالی";

            #endregion

            //****  null or not null
            dgvCmBxClmNull.Items.Add("Not Null");
            dgvCmBxClmNull.Items.Add("Null");
            //*******************************

            //  add item copy , delete            
            dgvCmBxClmEvent.Items.Add("ویرایش");
            dgvCmBxClmEvent.Items.Add("حذف");
            //*******************************

            //****  data types standard
            DataType(dgvCmBxClmType);
            //*******************************     

            //****  select standard column

            #region Select Standard Column
            dgvCmBxClmType2.Items.Add("");
            dgvCmBxClmType2.Items.Add("اصلاح ی و ک");
            dgvCmBxClmType2.Items.Add("حروفی");
            dgvCmBxClmType2.Items.Add("عددی");
            dgvCmBxClmType2.Items.Add("کدملی");
            dgvCmBxClmType2.Items.Add("شماره شناسنامه");
            dgvCmBxClmType2.Items.Add("تاریخ شمسی");
            dgvCmBxClmType2.Items.Add("تاریخ میلادی");
            #endregion

            //*******************************

            //  load columns info            
            dtColumns = sqlfunction.SqlColumns(cmbTableName.Text, sqlConnection, cmbDBName.Text);

            //  count columns to list
            lstCount = sqlfunction.SqlRecordCountColumn(cmbTableName.Text, sqlConnection, cmbDBName.Text, " IS NOT NULL ");

            //  count columns is null to list
            lstCountNull = sqlfunction.SqlRecordCountColumn(cmbTableName.Text, sqlConnection, cmbDBName.Text);

            //  list type test
            List<string> lsttype = Functions.DataTableToList(dtColumns, 3);

            //  add columns & default value
            #region Add Column & Default Value

            #region Width

            dgvTxBxClmOrd.Width = 30;
            dgvTxBxClmOrd2.Width = 30;
            dgvChBxClm.Width = 30;
            dgvTxtClmLen.Width = 40;
            dgvCmBxClmEvent.Width = 60;
            dgvTxtClmCount.Width = 50;
            dgvCmBxClmNull.Width = 70;
            dgvTxtClmCountNull.Width = 50;

            #endregion

            #region Add

            //  add column id
            dgvDesign.Columns.Add(dgvTxBxClmOrd);
            dgvTxBxClmOrd.ReadOnly = true;
            dgvStd2.Columns.Add(dgvTxBxClmOrd2);
            dgvTxBxClmOrd2.ReadOnly = true;
            //  add checkbox column
            dgvDesign.Columns.Add(dgvChBxClm);
            dgvChBxClm.Frozen = true;
            //  add column name field
            dgvDesign.Columns.Add(dgvTxtBxClm);
            dgvStd2.Columns.Add(dgvTxtBxClm2);
            //  add column nullable
            dgvDesign.Columns.Add(dgvCmBxClmNull);
            //  add column datatype
            dgvDesign.Columns.Add(dgvCmBxClmType);
            dgvStd2.Columns.Add(dgvCmBxClmType2);
            //  add column length
            dgvDesign.Columns.Add(dgvTxtClmLen);
            //  add column event
            dgvDesign.Columns.Add(dgvCmBxClmEvent);
            //  add column count
            dgvDesign.Columns.Add(dgvTxtClmCount);
            //  add column count is null
            dgvDesign.Columns.Add(dgvTxtClmCountNull);

            #endregion

            //  default value
            #region Default Value

            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                dgvDesign.Rows.Add(dgvTxBxClmOrd.ToolTipText = dtColumns.Rows[i][0].ToString(),
                false,
                dgvTxtBxClm.ToolTipText = dtColumns.Rows[i][1].ToString(),
                dgvCmBxClmNull.DisplayMember = dtColumns.Rows[i][2].ToString(),
                dgvCmBxClmType.DisplayMember = dtColumns.Rows[i][3].ToString(),
                dgvTxtClmLen.ToolTipText = dtColumns.Rows[i][4].ToString().Replace("(", "").Replace(")", ""),
                dgvCmBxClmEvent.ToolTipText = "ویرایش",
                lstCount[i], lstCountNull[i]);

                //  datagrid standard2
                dgvStd2.Rows.Add(dgvTxBxClmOrd2.ToolTipText = dtColumns.Rows[i][0].ToString(),
                dgvTxtBxClm2.ToolTipText = dtColumns.Rows[i][1].ToString(),
                dgvCmBxClmType2.ToolTipText = ""
                                );


                chlstbxColumn.Items.Add
                    (
                    dtColumns.Rows[i][1].ToString() +
                    " [" + dtColumns.Rows[i][2].ToString() + "] - " +
                    dtColumns.Rows[i][3].ToString() +
                    dtColumns.Rows[i][4].ToString()
                    );

                //  disable text column len is numberic

                dgvDesign.Rows[i].Cells[5].ReadOnly = ReadOnly(dgvDesign.Rows[i].Cells[4].Value.ToString());
                if (ReadOnly(dgvDesign.Rows[i].Cells[4].Value.ToString()) == true) dgvDesign.Rows[i].Cells[5].Value = null;

            }

            #endregion

            #endregion

        }
        #endregion
        //********

        #region load TbName
        private void loadTbName()
        {
            cmbTableName.DataSource = sqlfunction.SqlTableName(sqlConnection);
            cmbTableNameTab1.DataSource = sqlfunction.SqlTableName(sqlConnection);
        }

        #endregion


        private void insert_report()
        {
            string strQuery;
            int intCountColumn = 0, intCountRecord = 0;
            DataTable dtTable = new DataTable();

            //  query
            strQuery = "SELECT name,[object_id] FROM sys.tables WHERE name<>N'___Report'";
            //  run query
            dtTable = sqlfunction.SqlDataAdapter(strQuery, sqlConnection, cmbDBName.Text);

            for (int u = 0; u < dtTable.Rows.Count; u++)
            {

                //  record count
                intCountRecord = sqlfunction.SqlRecordCount(dtTable.Rows[u][0].ToString(), sqlConnection);

                //  query
                strQuery = "SELECT COUNT(*) FROM sys.columns WHERE [object_id]=" + dtTable.Rows[u][1].ToString();
                //  run query
                intCountColumn = sqlfunction.SqlDataAdapter(strQuery, sqlConnection, cmbDBName.Text).Rows.Count;

                //**************    Insert Fields    Tables ,[Relation 1] ,[Relation 2] ,Description ,TedadRecord ,TedadField ,ShenasNameBank)   
                //  query
                strQuery = "Insert dbo.[___Report](" +
                  "Tables ,[Relation 1] ,[Relation 2] ,Description ,TedadRecord ,TedadField ,ShenasNameBank)" +
                  "VALUES('" + dtTable.Rows[u][0].ToString() + "',NULL,NULL,NULL," + intCountRecord.ToString() + "," + intCountColumn.ToString() + ",NULL)";

                //  run query
                lst1.Items.Add("جدول " + dtTable.Rows[u][0].ToString() + " : " + sqlfunction.SqlExcutCommand(strQuery, sqlConnection));
            }
        }


        public void EditDataType()
        {
            string strColumnName = "";
            string strDataType = "";
            string strFinal = "";
            Int64 maxlen;

            for (int k = 0; k < chlstbxColumn.Items.Count; k++)
            {
                if (chlstbxColumn.GetItemCheckState(k) == CheckState.Checked)
                {
                    //  column name
                    strColumnName = ColumnName(chlstbxColumn.Items[k].ToString());

                    //  return data type
                    strDataType = ColumnDataType(chlstbxColumn.Items[k].ToString());

                    //  run query                    
                    strFinal = sqlfunction.SqlUpdateData(cmbTableName.Text, strColumnName, "LTRIM(RTRIM(REPLACE([" + strColumnName + "],CHAR(160),'')))", cmbDBName.Text, sqlConnection);

                    //  report
                    lst1.Items.Add(strColumnName + " : " + strFinal);


                    //  check string & numeric
                    if (strDataType.Contains("("))
                    {
                        strDataType = strDataType.Substring(0, strDataType.IndexOf("(") - 1);

                        if (strDataType == "image")
                        {
                            lst1.Items.Add("فیلد " + strDataType + "قابل تغییر نمی باشد ");
                        }
                        else if (strDataType == "ntext")
                        {
                            //  run query
                            strFinal = sqlfunction.SqlEditDataTypeColumn(cmbTableName.Text, strColumnName, "Null", " VARCHAR(max) ", sqlConnection, cmbDBName.Text);

                            //  report
                            lstReport("sqlEditDataTypeColumn " + strColumnName + strFinal);
                        }
                        else
                        {
                            //  max len column data
                            maxlen = Convert.ToInt32(sqlfunction.SqlMaxLenColumnData(cmbTableName.Text, strColumnName, sqlConnection));

                            if (maxlen > 0)
                            {
                                //  edit data type column
                                strFinal = sqlfunction.SqlEditDataTypeColumn(cmbTableName.Text, strColumnName, "VARCHAR(" + maxlen + ")", "Null", sqlConnection, cmbDBName.Text);

                                //  report
                                lstReport("SqlEditDataTypeColumn => " + strColumnName + strFinal);
                            }
                        }
                    }
                    else
                    {
                        lstReport("column is Standard => " + strColumnName);
                    }
                }
            }
        }


        private void lstReport(string strReport)
        {
            //lst1.Items.Clear();
            lst1.Items.Add(strReport);
            lst1.SelectedIndex = lst1.Items.Count - 1;

            //  select tabcontrol report
            tabControl2.SelectedTab = tabControl2.TabPages[1];
        }


        #region column name
        private string ColumnName(string strRow)
        {
            return strRow.Substring(0, strRow.IndexOf("[") - 1);
        }
        #endregion


        #region ColumnDataType
        private string ColumnDataType(string strColumnName)
        {
            string strcolumn = strColumnName;
            int intStart = strcolumn.IndexOf("[") + 1;
            int intEnd = strcolumn.IndexOf("]");

            string strNull = strcolumn.Substring(intStart, intEnd - intStart).Trim();
            strcolumn = strcolumn.Substring(0, strcolumn.IndexOf("-") + 1).Trim();

            return strColumnName.Replace(strcolumn, "").Trim();
        }
        #endregion


        #region DataGridView

        private void DataType(DataGridViewComboBoxColumn cmbClm)
        {
            string[] strDataType =
                { "bit", "tinyint", "smallint", "int", "bigint",
                "char", "nchar", "varchar", "nvarchar",
                "text", "ntext", "date", "datetime","smalldatetime", "real", "float" };


            for (int i = 0; i < 14; i++)
            {
                cmbClm.Items.Add(strDataType[i]);
            }
            cmbClm.Items[2].ToString();
        }


        #endregion

        /// <summary>
        /// readonly for datatypes numberic
        /// </summary>
        /// <param name="strDataType"></param>
        /// <returns></returns>
        private bool ReadOnly(string strDataType)
        {
            bool b = false;
            if (strDataType == "bit" | strDataType == "int" | strDataType == "smallint" | strDataType == "bigint" | strDataType == "datetime"
                | strDataType == "date" | strDataType == "smalldate" | strDataType == "image" | strDataType == "tinyint" | strDataType == "real"
                | strDataType == "float")
            { b = true; }
            return b;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            lblText.ForeColor = randomColor;

        }


    }
}
