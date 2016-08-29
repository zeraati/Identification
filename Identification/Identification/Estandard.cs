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

namespace Identification
{
    public partial class Estandard : Form
    {
        Functions Functions = new Functions();
        SqlConnection sqlConnection = new SqlConnection();

        string strFunctionsFile = @"..\Functions";

        Substring frmsubstring = new Substring();

        //string strFieldName;
        string type, strdate;

        int TableName, intTableCheck;


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
            cmbDBName.DataSource = Functions.SqlGetDBName(sqlConnection);


            strdate = solarDate.ToString("yyyyMMdd");
            strdate = strdate.Replace("/", "");

            if (cmbDBName.Text != "") cmbDBName.DropDownWidth = Functions.DropDownWidth(cmbDBName);
        }

        private void خروجToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void بستنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strReport = "";
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbDBName.Text, sqlConnection);

            //  check sql functions
            strReport = Functions.CheckSqlFunctions(strFunctionsFile, cmbDBName.Text, sqlConnection);

            //  report
            if (strReport != "")
            { lstReport(strReport); }

            //  dafault value
            cmbDBName.Text = "Test";

            //  set cmbTBName source    // load table names
            loadTbName();

            if (cmbDBName.Text != "") cmbDBName.DropDownWidth = Functions.DropDownWidth(cmbDBName);
        }

        private void cmbTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load column names
            loadColumn();

            //  displayed count record
            chbDisRec_CheckedChanged(null, null);


            //TName = cmbTB.SelectedIndex;

            btnselectALL2.Text = "انتخاب همه";

            if (cmbTableName.Items.Count != 0) cmbTableName.DropDownWidth = Functions.DropDownWidth(cmbTableName);

        }



        //  txtlen enable & disable
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTypeTab2.Text == "int" | cmbTypeTab2.Text == "tinyint" | cmbTypeTab2.Text == "bigint" | cmbTypeTab2.Text == "smallint" | cmbTypeTab2.Text == "date" |
                cmbTypeTab2.Text == "datetime" | cmbTypeTab2.Text == "bit" | cmbTypeTab2.Text == "float" | cmbTypeTab2.Text == "real" | cmbTypeTab2.Text == "float to int")
            {
                txtLen.Visible = false;
            }
            else txtLen.Visible = true;
        }


        #region TabControl Table
        //  *****   tabcontrol table
        private void btnChangeTableName_Click(object sender, EventArgs e)
        {
            string strFinal;

            //  change table name
            strFinal = Functions.SqlEditTableName(cmbTableNameTab1.Text, txtNewTableName.Text, sqlConnection, true);

            //  conclude final
            lstReport("New Table => " + txtNewTableName + " => " + strFinal);

            // load table names of source
            if (strFinal.Contains("Done")) loadTbName();

        }

        private void btnTBCopy_Click(object sender, EventArgs e)
        {
            string strFinal;

            //  copy table data
            strFinal = Functions.SqlCopyTable(cmbTableNameTab1.Text, txtCopyTableName.Text, sqlConnection);

            //  conclude final
            lstReport(strFinal);

            // load table names of source
            if (strFinal.Contains("Done")) loadTbName();

        }

        private void btnTBDelete_Click(object sender, EventArgs e)
        {
            string strFinal = "";
            DialogResult DR = new DialogResult();
            DR = MessageBox.Show("آیا می خواهید جدول" + cmbTableNameTab1.Text + "حذف شود", "!هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            if (DR == DialogResult.Yes)
            {
                //  drop table
                strFinal = Functions.SqlDropTable(cmbTableNameTab1.Text, sqlConnection);

                //  conclude final
                lstReport(strFinal);

                // load table names of source
                if (strFinal.Contains("Done")) loadTbName();

            }
            else lst1.Items.Add("عملیات لغو شد");
        }

        private void cmbTB2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNewTableName.Text = cmbTableNameTab1.Text;
            txtCopyTableName.Text = cmbTableNameTab1.Text + "_copy";
            cmbTableNameTab1.DropDownWidth = Functions.DropDownWidth(cmbTableNameTab1);
        }

        //**************************************************
        #endregion


        #region TabControl Field
        //  ****    tabcontrol field
        private void btnChangeFieldName_Click(object sender, EventArgs e)
        {

            //  rename column
            string strReport = Functions.SqlRename(" COLUMN ", cmbColumnTab2.Text, txtFildEdit.Text, sqlConnection, cmbTableNameColumn.Text);

            //  report
            lstReport(cmbColumnTab2.Text + " => " + " Rename => " + txtFildEdit.Text + strReport);

            //  load column
            if (strReport.Contains("Done")) loadColumn();

        }

        private void btnEditDataType_Click(object sender, EventArgs e)
        {
            string strDataType = cmbTypeTab2.Text;
            string strNullable = "Not NULL", strReport = "";


            //  nullable is not null
            if (chbxNull.CheckState == CheckState.Checked) strNullable = "Null";

            //  data type is string
            if (txtLen.Visible == true) { strDataType = cmbTypeTab2.Text + "(" + txtLen.Text + ")"; }

            //  run query
            strReport = Functions.SqlEditDataTypeColumn(cmbTableNameColumn.Text, cmbColumnTab2.Text, strDataType, strNullable, sqlConnection, cmbDBName.Text);

            //  report
            lstReport("Edit DataType Column => " + cmbColumnTab2.Text + strReport);

            //  load column
            if (strReport.Contains("Done")) loadColumn();

        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            string strNulable = " NOT NULL ";
            string strDataType = cmbTypeTab2.Text, strReport = "";

            //  checked null or not null
            if (chbxNull.CheckState == CheckState.Checked) strNulable = " NULL ";

            //  data type is string
            if (txtLen.Visible == true) { strDataType = cmbTypeTab2.Text + "(" + txtLen.Text + ")"; }


            if (txtNewField.Text != "")
            {
                //  run query new uniqe column
                if (chbUniqe.CheckState == CheckState.Checked) strReport = Functions.SqlAddNewColumn(cmbTableName.Text, txtNewField.Text, "INT", sqlConnection, 1, 1);

                //  run query new column
                else strReport = Functions.SqlAddNewColumn(cmbTableName.Text, txtNewField.Text, strDataType, strNulable, sqlConnection, cmbDBName.Text);

                //  report
                lstReport("New Colummn => [" + txtNewField.Text + "] " + strReport);
            }
            else MessageBox.Show("نام فیلد را وارد کنید", "هشدار");

            if (strReport.Contains("Done")) loadColumn();

        }

        private void btnDelField_Click(object sender, EventArgs e)
        {
            string strFinaly = "";

            DialogResult DR = new DialogResult();
            DR = MessageBox.Show("آیا فیلد [" + cmbColumnDelete.Text + "]حذف شود ", "!هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

            if (DR == DialogResult.Yes)
            {
                //  run query
                strFinaly = Functions.SqlDropColumn(cmbTableNameColumn.Text, cmbColumnDelete.Text, sqlConnection);

                //  report
                lstReport(cmbColumnDelete.Text + " => Delete => " + strFinaly);

                //  load fields
                if (strFinaly.Contains("Done")) loadColumn();

            }
            else if (DR == DialogResult.No) lst1.Items.Add("عملیات لغو شد");
        }

        private void btnCopyField_Click(object sender, EventArgs e)
        {
            string strFinal = "";

            //  run query
            strFinal = Functions.SqlCopyColumn(cmbTableNameColumn.Text, cmbColumnCopy.Text, sqlConnection);

            //  conclude final 
            lstReport(cmbColumnCopy.Text + " => Copy " + strFinal);

            //  load column
            if (strFinal.Contains("Done")) loadColumn();


        }

        private void cmbTBField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTableNameColumn.Items.Count != 0) cmbTableNameColumn.DropDownWidth = Functions.DropDownWidth(cmbTableNameColumn);

            //  load field name
            loadColumn();

        }

        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtFildEdit.Text = cmbColumnTab2.Text + "_copy";

            if (cmbColumnTab2.Items.Count != 0) cmbColumnTab2.DropDownWidth = Functions.DropDownWidth(cmbColumnTab2);
        }

        private void cmbFieldDel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //**************************************************
        #endregion


        private void btnselectALL2_Click(object sender, EventArgs e)
        {
            Functions.SelectUnselect(chlstbxColumn, btnselectALL2);
        }

        #region TabControl Design




        #endregion







        #region نمایش رکوردهای هر فیلد

        private void chbDisRec_CheckedChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //lst1.Items.Clear();

            DataTable dtColumns = Functions.SqlColumns(cmbTableName.Text, sqlConnection);

            string strCount, strFieldName;


            //  record table
            strCount = Functions.SqlRecordCount(cmbTableName.Text, "*", sqlConnection);


            //  add item
            lst1.Items.Add("تعداد فیلد : " + dtColumns.Rows.Count);
            lst1.Items.Add("تعداد رکورد جدول : " + Functions.StrNum(Convert.ToInt32(strCount)));


            //  checked field
            if (chbDisRec.CheckState == CheckState.Checked)
            {
                lst1.Items.Clear();
                for (int j = 0; j < dtColumns.Rows.Count; j++)
                {
                    //  get field name
                    strFieldName = dtColumns.Rows[j][0].ToString();

                    //  get data type
                    type = dtColumns.Rows[j][2].ToString();


                    if (type == "image" | type == "ntext")
                    {
                        lst1.Items.Add("فیلد " + strFieldName + " قابل شمارش نیست ");
                    }
                    else
                    {
                        //  count fields record
                        strCount = Functions.SqlRecordCount(cmbTableName.Text, "[" + strFieldName + "]", sqlConnection);
                        lst1.Items.Add(strFieldName + " => " + strCount + " Record");
                    }

                }
                tabControl2.SelectedTab = tabControl2.TabPages[1];
            }

            Cursor.Current = Cursors.Default;

        }
        #endregion


        private void گزارشازبانکموردنظرToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  check table '___Report'
            int check = cmbTableName.FindString("___Report");

            if (check == -1)
            {

                if (Functions.SqlCreateReport(sqlConnection) == " => Done!")
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

        private void پشتیبانگیریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLDBbackup sqlbackup = new SQLDBbackup(sqlConnection);
            sqlbackup.ShowDialog();
        }

        private void btnUpdDesign_Click(object sender, EventArgs e)
        {

        }



        //********
        #region loadColumn of DataGridView
        private void loadColumn()
        {

            #region DataGridView Design

            DataTable dtColumns = new DataTable();
            DataGridViewComboBoxColumn dgvCmBxClmType = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn dgvCmBxClmNull = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn dgvTxtClmLen = new DataGridViewTextBoxColumn();


            //****  null or not null
            dgvCmBxClmNull.HeaderText = "Nullable";
            dgvCmBxClmNull.Items.Add("Not Null");
            dgvCmBxClmNull.Items.Add("Null");
            //*******************************
            //****  data type
            dgvCmBxClmType.HeaderText = "DataType";
            DataType(dgvCmBxClmType);
            //*******************************
            //****  Len
            dgvTxtClmLen.HeaderText = "Lenth";
            //*******************************

            //  load columns info            
            dtColumns = Functions.SqlColumns(cmbTableName.Text, sqlConnection);

            //  load column names
            dgvDesign.DataSource = Functions.SqlColumnNames(cmbTableName.Text, sqlConnection);



            //  add columns
            dgvDesign.Columns.Add(dgvCmBxClmNull);
            dgvDesign.Columns.Add(dgvCmBxClmType);
            dgvDesign.Columns.Add(dgvTxtClmLen);

            //  
            //  load column len of source
            for (int i = 0; i < dtColumns.Rows.Count; i++)
            {
                dgvDesign.Rows[i].Cells[4].Value = "tytu";// dtColumns.Rows[i][3].ToString();
            }

            #endregion

            //  load cmb field source
            cmbColumnTab2.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTableNameColumn.Text, sqlConnection));
            cmbColumnCopy.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTableNameColumn.Text, sqlConnection));
            cmbColumnDelete.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTableNameColumn.Text, sqlConnection));

            //  load clb field source
            Functions.LoadColumnInfo(cmbTableName.Text, chlstbxColumn, sqlConnection);

        }
        #endregion
        //********

        #region load TbName
        private void loadTbName()
        {
            cmbTableName.DataSource = Functions.SqlTableName(sqlConnection);
            cmbTableNameTab1.DataSource = Functions.SqlTableName(sqlConnection);
            cmbTableNameColumn.DataSource = Functions.SqlTableName(sqlConnection);
        }
        #endregion

        #region Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            lst1.Items.Add("--------------------------------------");
            string strFieldName = "", strDataType;
            string strColumnData;
            string strQuery = "", strWhere, strFinal = "";
            int intIncerement, intCount, intTableCount;

            //  table records count
            intTableCount = Functions.SqlTableRecordsCount(cmbTableName.Text, sqlConnection);

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
                        if (txtNew.Text == "NULL" | txtNew.Text == "=NULL")
                        { strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "NULL", sqlConnection, strFieldName, " = N'" + txtBefore.Text + "'"); }
                        else { strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],N'" + txtBefore.Text + "',N'" + txtNew.Text + "')", sqlConnection); }

                        //  report
                        if (strFinal.Contains("Done")) { lstReport("Replace =>" + strFinal); }
                    }
                }
            }
            #endregion



            for (int i = 0; i < clb2.Items.Count; i++)
            {
                if (clb2.GetItemCheckState(i) == CheckState.Checked)
                {
                    switch (i)
                    {

                        #region Delete Not Valid
                        case 0:

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
                                        strQuery = "Update [" + cmbTableName.Text + "] Set [" + strFieldName + "] = Null Where [" + strFieldName + "] = '' or [" + strFieldName + "] = '0' or [" + strFieldName + "] = '-' or [" + strFieldName + "] = ' ' and ISNUMERIC([" + strFieldName + "])<>1";
                                    }
                                    else
                                    {
                                        //query
                                        strQuery = "Update [" + cmbTableName.Text + "] Set [" + strFieldName + "] = Null Where [" + strFieldName + "] = 0 and ISNUMERIC([" + strFieldName + "])<>1";
                                    }
                                    //  run query
                                    Functions.SqlExcutCommand(strQuery, sqlConnection);

                                    //query
                                    strQuery = "update [" + cmbTableName.Text + "] Set [" + strFieldName + "] =LTRIM(RTRIM([" + strFieldName + "]))";

                                    //  run query
                                    lst1.Items.Add("حذف مقادیر تهی و نا معتبر" + strFieldName + ": " + Functions.SqlExcutCommand(strQuery, sqlConnection));
                                    strQuery = "";
                                }
                            }
                            break;
                        #endregion

                        #region Edit Data Type Column
                        case 1:
                            EditDataType();
                            loadColumn();
                            break;
                        #endregion

                        #region Replace Character ک و ی
                        case 2:
                            for (int j = 0; j < chlstbxColumn.Items.Count; j++)
                            {
                                //  column name
                                strFieldName = ColumnName(chlstbxColumn.Items[j].ToString());

                                //  run query & report
                                lstReport("جایگزینی ک و ي " + strFieldName + ":" +
                                                Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, " dbo.FixKY([" + strFieldName + "])", sqlConnection, strFieldName, "IS NOT NULL")
                                                );
                            }
                            break;
                        #endregion

                        #region Standard CodeMelli
                        case 3:

                            StandardCodeMelli(strFieldName);

                            break;
                        #endregion

                        #region Standard ShenasCode
                        case 4:

                            strColumnData = "dbo.[CM-Fix]([" + strFieldName + "])";

                            #region Delete Character Not Numeric

                            //  update character not numeric
                            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, strColumnData, sqlConnection);

                            //  report
                            lstReport("Delete Character Not Numeric => " + strFieldName + strFinal);

                            #endregion

                            #region ShenasCode With Len > 7 To CodeMelli
                            //  query
                            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, "CodeMelli", "RIGHT('00'+ " + strColumnData + ",10)," + strFieldName + "= NULL ", sqlConnection, strFieldName, "LEN(" + strColumnData + ")>7 and CodeMelli is null", "");

                            //  report
                            lstReport("ShenasCode With Len > 7 To CodeMelli => " + strFieldName + ": " + strFinal);

                            if (strFinal.Contains("Done"))
                            { StandardCodeMelli("CodeMelli"); }

                            #endregion

                            #region Delete ShenasCode Not Valid

                            //  where query
                            strWhere = strFieldName + "='0' or " + strFieldName + "=' ' or LEN(" + strFieldName + ")>7";

                            //  update
                            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "NULL", sqlConnection, strWhere, "");

                            //  report
                            lstReport("Delete ShenasCode Not Valid => " + strFieldName + ": " + strFinal);

                            #endregion
                            break;
                        #endregion

                        #region Standard Date
                        case 5:
                            for (int l = 0; l < chlstbxColumn.Items.Count; l++)
                            {
                                if (chlstbxColumn.GetItemCheckState(l) == CheckState.Checked)
                                {
                                    //  column name
                                    strFieldName = ColumnName(chlstbxColumn.Items[l].ToString());

                                    #region Standard Formated Date
                                    //  sql update query
                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'-','/')", sqlConnection);
                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'_','/')", sqlConnection);
                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],'.','/')", sqlConnection);
                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "Replace ([" + strFieldName + "],',','/')", sqlConnection);

                                    //  run query                                    
                                    lst1.Items.Add(" Ltrim & Rtrim " +
                                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "ltrim(rtrim([" + strFieldName + "]))", sqlConnection)
                                                    );
                                    //  run query
                                    lst1.Items.Add(" Standard Date " +
                                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, "dbo.[FE_Date]([" + strFieldName + "]) where [" + strFieldName + "] IS NOT NULL", sqlConnection)
                                                    );
                                    #endregion

                                    #region Delete Not Valid Date
                                    //  run query
                                    lst1.Items.Add(" Delete Not Valid Date " +
                                                    Functions.SqlUpdateColumnData(cmbTableName.Text, strFieldName, " NULL where dbo.[CM-Fix]([" + strFieldName + "])= 0 ", sqlConnection)
                                                    );
                                    #endregion

                                }
                            }
                            break;
                        #endregion

                        #region Date Convert
                        case 6:
                            DateConvert frm = new DateConvert(sqlConnection);
                            frm.ShowDialog();
                            break;
                        #endregion

                        #region Save Characters Substring
                        case 7:
                            Substring frmSubstring = new Substring();
                            frmSubstring.ShowDialog();
                            txtTest.Text = Functions.ReturnValue;
                            break;
                        #endregion


                        #region Delete Space Or Zero Value Columns
                        case 8:
                            intTableCheck = 0;
                            for (int j = 0; j < chlstbxColumn.Items.Count; j++)
                            {
                                if (chlstbxColumn.GetItemCheckState(j) == CheckState.Checked)
                                {
                                    //  column name
                                    strFieldName = ColumnName(chlstbxColumn.Items[j].ToString());

                                    //  run query
                                    strFinal = Functions.SqlDropColumnSpace(cmbTableName.Text, strFieldName, sqlConnection);

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
                        case 9:

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
                            intCount = Functions.SqlCountColumn(cmbTableName.Text, sqlConnection, strWhere);

                            if (intCount != 0)
                            {
                                //  run query
                                strFinal = Functions.SqlDropRows(cmbTableName.Text, sqlConnection, strWhere);

                                //  report
                                if (strFinal.Contains("Done"))
                                { lstReport("Count = " + intCount.ToString() + " , " + strFinal); }
                            }
                            else
                            { lstReport(" Rows Is Not Null "); }

                            break;
                        #endregion

                        #region حذف جدول خالی
                        case 10:
                            if (intTableCheck == 0)
                            {
                                //  run query
                                strFinal = Functions.SqlDropTableSpace(cmbTableName.Text, sqlConnection);

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

        #endregion



        //***********************************************************************

        //private void btnUpdate2_Click(object sender, EventArgs e)
        //{
        //strQuery = "[" + sqlConnection.Database + "].dbo.[" + cmbTBField.Text + "]";
        /*
            
        if (cmbType.SelectedIndex == 15 | cmbType.SelectedIndex == 16)
        {
            strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] DECIMAL";  
            if (Functions.SqlExcutCommand(strQuery, sqlConn) == true)
            {
                if (cmbType.SelectedIndex == 15)
                {
                    strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] TINYINT";
                }
                else
                {
                    strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] varchar(max)";
                }
                if (Functions.SqlExcutCommand(strQuery, sqlConn) == true)
                {
                    lst1.Items.Add("تغییر نوع فیلد : " + cmbField.Text + "انجام شد");
                }
                else
                {
                    strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] SMALLINT";
                    if (Functions.SqlExcutCommand(strQuery, sqlConn) == true)
                    {
                        lst1.Items.Add("تغییر نوع فیلد : " + cmbField.Text + "انجام شد");
                    }
                    else
                    {
                        strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] INT";
                        if (Functions.SqlExcutCommand(strQuery, sqlConn) == true)
                        {
                            lst1.Items.Add("تغییر نوع فیلد : " + cmbField.Text + "انجام شد");
                        }
                        else
                        {
                            strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] VARCHAR(max)";
                            lst1.Items.Add("تغییر نوع فیلد " + Functions.SqlExcutCommand(strQuery, sqlConn) + " : " + cmbField.Text);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("! خطا");
            }
        }
        else
        {
            strQuery = "Alter Table " + com + " Alter Column [" + cmbField.Text + "] " + type; //ALTER TABLE dbo.pp ALTER COLUMN k NVARCHAR(13)  
            lst1.Items.Add("تغییر نوع فیلد " + Functions.SqlExcutCommand(strQuery, sqlConn) + " : " + cmbField.Text);
        }
        Functions.LoadColumnNew(sqlConn, cmbTB.Text, chlstbxField);
    }

    private void btnUpdate4_Click(object sender, EventArgs e)
    {
        EditNameTable();
    }

    //************      Table New Name      *******************************
    string strTBName;
    private void btnTBCopy_Click(object sender, EventArgs e)
    {
        if (txtName.Text == "") strTBName = cmbTB2.Text + "_copy";
        else strTBName = txtName.Text;

        strQuery = "SELECT * INTO dbo.[" + strTBName + "] FROM dbo.[" + cmbTB2.Text + "]";

        OK = Functions.SqlExcutCommand(strQuery, sqlConn);
        if (OK == true)
        {
            MessageBox.Show("جدول " + strTBName + " ساخته شد");

            //  set cmbTBName source    // load table names
            loadTbName();

        }
        else
        {
            MessageBox.Show("انجام نشد", "خطا");
            lst1.Items.Add("جدول " + strTBName + " موجود است");
        }
        //    DialogResult DR = new DialogResult();
        //    DR = MessageBox.Show("آیا می خواهید جدول" + cmbTB2.Text + "حذف شود", "!هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        //    if (DR == DialogResult.Yes)
        //    {*/
        //}

        //****************************************************************************


        //*****     Functions



        //********      Using Functions       *************    تغییر دادن نوع فیلد

        private void insert_report()
        {
            string strQuery;
            int intCountColumn = 0, intCountRecord = 0;
            DataTable dtTable = new DataTable();

            //  query
            strQuery = "SELECT name,[object_id] FROM sys.tables WHERE name<>N'___Report'";
            //  run query
            dtTable = Functions.SqlDataAdapter(strQuery, sqlConnection);

            for (int u = 0; u < dtTable.Rows.Count; u++)
            {

                //  record count
                intCountRecord = Functions.SqlTableRecordsCount(dtTable.Rows[u][0].ToString(), sqlConnection);

                //  query
                strQuery = "SELECT COUNT(*) FROM sys.columns WHERE [object_id]=" + dtTable.Rows[u][1].ToString();
                //  run query
                intCountColumn = Functions.SqlDataAdapter(strQuery, sqlConnection).Rows.Count;

                //**************    Insert Fields    Tables ,[Relation 1] ,[Relation 2] ,Description ,TedadRecord ,TedadField ,ShenasNameBank)   
                //  query
                strQuery = "Insert dbo.[___Report](" +
                  "Tables ,[Relation 1] ,[Relation 2] ,Description ,TedadRecord ,TedadField ,ShenasNameBank)" +
                  "VALUES('" + dtTable.Rows[u][0].ToString() + "',NULL,NULL,NULL," + intCountRecord.ToString() + "," + intCountColumn.ToString() + ",NULL)";

                //  run query
                lst1.Items.Add("جدول " + dtTable.Rows[u][0].ToString() + " : " + Functions.SqlExcutCommand(strQuery, sqlConnection));
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
                    strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strColumnName, "LTRIM(RTRIM(REPLACE([" + strColumnName + "],CHAR(160),'')))", sqlConnection);

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
                            strFinal = Functions.SqlEditDataTypeColumn(cmbTableName.Text, strColumnName, "Null", " VARCHAR(max) ", sqlConnection, cmbDBName.Text);

                            //  report
                            lstReport("sqlEditDataTypeColumn " + strColumnName + strFinal);
                        }
                        else
                        {
                            //  max len column data
                            maxlen = Convert.ToInt32(Functions.SqlMaxLenColumnData(cmbTableName.Text, strColumnName, sqlConnection));

                            if (maxlen > 0)
                            {
                                //  edit data type column
                                strFinal = Functions.SqlEditDataTypeColumn(cmbTableName.Text, strColumnName, "VARCHAR(" + maxlen + ")", "Null", sqlConnection, cmbDBName.Text);

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


        #region جستجوی کلمه پیشنهادی
        //private void getData(AutoCompleteStringCollection dataCollection)
        //{
        //    string connetionString = null;
        //    SqlConnection connection;
        //    SqlstrQuery strQuery;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    DataSet ds = new DataSet();
        //    connetionString = "Data Source=.;Initial Catalog=LogFile;Integrated Security=True";
        //    string sql = "SELECT [WorldEnglish] FROM [LogFile].dbo.[Dictionery]";
        //    connection = new SqlConnection(connetionString);
        //    try
        //    {
        //        connection.Open();
        //        strQuery = new SqlstrQuery(sql, connection);
        //        adapter.SelectstrQuery = strQuery;
        //        adapter.Fill(ds);
        //        adapter.Dispose();
        //        strQuery.Dispose();
        //        connection.Close();
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            dataCollection.Add(row[0].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Can not open connection ! ");
        //        Application.Exit();
        //    }
        //}
        #endregion

        #region column name
        private string ColumnName(string strRow)
        {
            return strRow.Substring(0, strRow.IndexOf("["));
        }
        #endregion

        #region ColumnDataType
        private string ColumnDataType(string strColumnName)
        {
            string strcolumn = strColumnName;
            int intStart = strcolumn.IndexOf("[") + 1;
            int intEnd = strcolumn.IndexOf("]");

            strcolumn = strcolumn.Substring(intStart, intEnd - intStart).Trim();
            string strNull = strcolumn.Substring(0, strcolumn.IndexOf("-") + 1).Trim();

            return strcolumn.Replace(strNull, "").Trim();

        }
        #endregion

        #region Public Standard CodeMelli
        public void StandardCodeMelli(string strColumnNameInput)
        {
            string strColumnData, strQuery, strFinal, strWhere;
            strColumnData = "dbo.[CM-Fix]([" + strColumnNameInput + "])";

            #region Create New Column For Standard CodeMelli

            //  query
            strQuery = "SELECT name FROM sys.columns WHERE object_id=(SELECT object_id FROM sys.tables WHERE name=N'" + cmbTableName.Text + "')";

            //  check column
            int intCheckColumn = Functions.CheckField(Functions.SqlDataAdapter(strQuery, sqlConnection), "CodeMelli2");

            if (intCheckColumn == 0)
            {
                //  create codemelli2
                strFinal = Functions.SqlAddNewColumn(cmbTableName.Text, "CodeMelli2", "varchar(10)", "Null", sqlConnection, cmbDBName.Text);

                //  report
                lstReport("Create CodeMelli2 " + strFinal);
            }

            #endregion

            #region Delete Not Valid

            //  delete not valid  
            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, strColumnNameInput, strColumnData, sqlConnection);

            //  report
            lstReport("Delete Not Valid " + strColumnNameInput + strFinal);

            #endregion

            #region Valid Codemellis (Lens = 8,9,10)

            //  valid codemellis (lens = 8,9,10)
            strWhere = " LEN(" + strColumnNameInput + ") BETWEEN 8 AND 10 AND ISNUMERIC(" + strColumnNameInput + ")=1"
                     + " and CodeMelli2 is null and dbo.Ch_Codemelli(RIGHT('00'+ " + strColumnNameInput + ",10))=1";

            //  run query
            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, "CodeMelli2", "RIGHT('00'+ " + strColumnNameInput + ",10)", sqlConnection, strWhere, "");

            //  report
            lstReport("Valid Codemellis (Lens = 8,9,10) " + strFinal);

            #endregion

            #region CodeMellis With Len = 11,12

            //  where query
            strWhere = " len(" + strColumnData + ") between 11 and 12 and dbo.Ch_Codemelli(LEFT(" + strColumnData + ",10)) = 1 and CodeMelli2 is null";

            //  update column data - LEFT
            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, "CodeMelli2", "LEFT(" + strColumnData + ",10)", sqlConnection, strWhere, "");

            //  report
            lstReport("LEFT CodeMellis With Len = 11,12 " + strColumnNameInput + ": " + strFinal);

            //  where query
            strWhere = " len(" + strColumnData + ") between 11 and 12 and dbo.Ch_Codemelli(RIGHT(" + strColumnData + ",10)) = 1 and CodeMelli2 is null";

            //  update column data - RIGHT
            strFinal = Functions.SqlUpdateColumnData(cmbTableName.Text, "CodeMelli2", "RIGHT(" + strColumnData + ",10)", sqlConnection, strWhere, "");

            //  report
            lstReport("RIGHT CodeMellis With Len = 11,12 " + strColumnNameInput + ": " + strFinal);

            #endregion

            #region Finaly

            //  report & update data
            lstReport("CodeMelli2 To CodeMelli " +
                            Functions.SqlUpdateColumnData(cmbTableName.Text, strColumnNameInput, "CodeMelli2", sqlConnection));

            //  edit column data type
            lstReport("CodeMelli Data Type " + Functions.SqlEditField(cmbTableName.Text, strColumnNameInput, sqlConnection, strColumnNameInput, "VARCHAR", "10"));

            //  drop column     'CodeMelli2'
            Functions.SqlDropColumn(cmbTableName.Text, "CodeMelli2", sqlConnection);

            #endregion
        }
        #endregion


        #region DataGridView

        private void DataType(DataGridViewComboBoxColumn cmbClm)
        {
            string[] strDataType = { "bit", "tinyint", "smallint", "int", "bigint", "char", "nchar", "varchar", "nvarchar", "text", "ntext", "date", "datetime", "real", "float" };
            for (int i = 0; i < 14; i++)
            {
                cmbClm.Items.Add(strDataType[i]);
            }
            cmbClm.Items[2].ToString();
        }

        private void Design(DataTable dtDesign)
        {
            DataGridViewColumn dgvClm = new DataGridViewColumn();

            dgvClm.HeaderText = "Column Name";
            //dgvCmBxClm.HeaderText = "Column Name";

            for (int i = 1; i < dtDesign.Rows.Count; i++)
            {

                //dgvDesign.Rows[i].Cells[1].Value = "dfdsf";

            }
            //dgvDesign.Columns.Add(dgvCmBxClm);
        }



        #endregion


        private void btnTest_Click(object sender, EventArgs e)
        {


            //string strquery = "SELECT * FROM dbo.[" + cmbTB.Text + "] SELECT @@ROWCOUNT";
            //int i = Functions.SqlRunQueryInt(strquery, sqlConnection);

            //txtTest.Text = i.ToString();
            //List<string> lstSearchColumn = new List<string>();
            //string[,] strArray = new string[,] {
            //                                        { "", "", "" }, 
            //                                        { "", "", "" } 
            //                                    };

            //cmbType.SelectedIndex = 2;




            //int intSelected = chlstbxField.SelectedIndex;
            //string strcolumn = chlstbxField.Items[intSelected].ToString();
            //txtTest.Text = Functions.ColumnName(chlstbxField.Items[intSelected].ToString()).Trim();
            //int start = strcolumn.IndexOf("[") + 1;
            //int end = strcolumn.IndexOf("]");

            //strcolumn = strcolumn.Substring(start, end - start).Trim();
            //string strNull = strcolumn.Substring(0, strcolumn.IndexOf("-") + 1).Trim();
            //strcolumn = strcolumn.Replace(strNull, "").Trim();

            //txtTest.Text = strcolumn;
            //if (strcolumn.Contains("(")) txtTest.Text = strcolumn.Substring(0, strcolumn.IndexOf("(") - 1);
            //txtTest.Text = strcolumn.IndexOf("-").ToString() + " , " + strcolumn.Length.ToString() + " , " + strcolumn.Substring(0, strcolumn.IndexOf("-")).Trim().Length.ToString();
            //txtTest.Text += strcolumn.Substring(strcolumn.IndexOf("-"),(strcolumn.Length-1)-strcolumn.Substring(0, strcolumn.IndexOf("-")).Trim().Length).Trim();


        }

        private void dgvDesign_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTest.Text = dgvDesign.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void تبدیلتاریخToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateConvert frm = new DateConvert(sqlConnection);
            frm.ShowDialog();
        }



    }
}
