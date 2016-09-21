using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Identification
{
    public partial class SetPerson : Form
    {

        #region Global
        Functions Functions = new Functions();
        SqlConnection sqlConnection = new SqlConnection();

        string query, query2, strJoin, strWhere, strSelect, strUpdate;
        string strCell1, strCell2, strLbl2, strMain, strSecond, strSlt;
        string strField, strDescription;
        #endregion


        public SetPerson(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
        }

        private void SetPerson_Load(object sender, EventArgs e)
        {
            //  Sql Server Connection Info
            this.Text = "Identification" + "  -  Server Name = " + sqlConnection.DataSource + " - DataBase Name = " + sqlConnection.Database;

            //  disable button
            btnChange.Enabled = btnShow.Enabled = false;

            //  load database name source
            cmbMainDB.DataSource = Functions.SqlGetDBName(sqlConnection);
            cmbSecndDB.DataSource = Functions.SqlGetDBName(sqlConnection);

            //  default value
            cmbMainDB.Text = cmbSecndDB.Text = "Ehraz";
        }

        private void cmbDBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnection);

            //  load table name
            cmbMainTbl.DataSource = Functions.SqlTableName(sqlConnection);

            //  default value            
            cmbMainTbl.Text = "TBL_Student_95-02-21_copy";
        }

        private void cmbDBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbSecndDB.Text, sqlConnection);

            //  load table name
            cmbSecndTbl.DataSource = Functions.SqlTableName(sqlConnection);

            //  default value
            cmbSecndTbl.Text = "TBL_MKsarparast_95-02-21_copy";
        }

        private void cmbTBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbMainClmnJoin.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbMainTbl.Text, sqlConnection));
            cmbMainClmnUniq.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbMainTbl.Text, sqlConnection));

            //  default value
            cmbMainClmnJoin.Text = "STID";
        }

        private void cmbTBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbSecndClmnJoin.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection));

            //  default value
            cmbSecndClmnJoin.Text = "STID";
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            //  select all & unselect all
            Functions.SelectUnselect(clbEhraz, btnSelectAll);
        }


        private void btnCheck_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor; this.Enabled = false;
            int intCount = 0, intCheck = 0;

            string strFunctionsFile = @"../Functions";
            string strFinal, strReport;


            lstReport.Items.Clear();

            //  check sql functions
            strReport = Functions.CheckSqlFunctions(strFunctionsFile, cmbMainDB.Text, sqlConnection);

            //  report from functions
            if (strReport != "")
            { lstReport.Items.Add(strReport); }

            intCheck = 0;

            DataTable dtTotal = new DataTable();
            DialogResult dr;

            if (cmbMainDB.Text == cmbSecndDB.Text & cmbMainTbl.Text == cmbSecndTbl.Text)
            {
                MessageBox.Show("جداول نباید مثل هم باشد", "!خطا");
            }
            else
            {
                #region Data Bases Info

                //  table main record count
                intCount = Functions.SqlTableRecordsCount(cmbMainTbl.Text, sqlConnection, cmbMainDB.Text);

                //  report count
                lstReport.Items.Add("تعداد رکورد جدول از بانک اصلی : " + Functions.StrNum(intCount));

                //  table second count
                intCount = Functions.SqlTableRecordsCount(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text);

                //  report count
                lstReport.Items.Add("تعداد رکورد جدول از بانک فرعی : " + Functions.StrNum(intCount));

                //  cmb main join cmb second
                intCount = Functions.SqlJoin(cmbMainTbl.Text, cmbSecndTbl.Text, cmbMainClmnJoin.Text, cmbSecndClmnJoin.Text, sqlConnection, cmbMainDB.Text, cmbSecndDB.Text).Rows.Count;

                //  report count
                lstReport.Items.Add("تعداد کل رکورد ها : " + Functions.StrNum(intCount));

                #endregion

                //*****         END

                //  datagridview combobox column source
                Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text));

                //  list column names                
                lstMainClmns.DataSource = Functions.DataTableToList(Functions.SqlColumns(cmbMainTbl.Text, sqlConnection, cmbMainDB.Text));
                lstSecndClmns.DataSource = Functions.DataTableToList(Functions.SqlColumns(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text));


                #region Create New Field

                //  new field
                strField = cmbMainClmnUniq.Text + "_vld";
                //  check new field
                intCheck = CheckField(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text), strField);

                if (intCheck == 0)
                {
                    dr = MessageBox.Show(" بسازم ؟" + cmbMainClmnUniq.Text + "_vld آیا فیلد ", "! هشدار", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        //  add new field
                        strFinal = Functions.SqlAddNewColumn(cmbSecndTbl.Text, strField, "INT", "NULL", sqlConnection, cmbSecndDB.Text);

                        if (strFinal.Contains("Done"))
                        {
                            lstReport.Items.Add(strField + "با موفقیت انجام شد");

                            //  datagridview combobox column source
                            Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text, ""));
                        }
                        else lstReport.Items.Add("با مشکل مواجه شد " + strField);

                        //    

                        intCheck = 0;

                        //  check field "Desription"
                        intCheck = CheckField(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text, ""), "Desription");

                        if (intCheck == 0)
                        {
                            // add column   
                            strFinal = Functions.SqlAddNewColumn(cmbSecndTbl.Text, "Desription", "NVARCHAR(MAX)", "NULL", sqlConnection, cmbSecndDB.Text);

                            if (strFinal.Contains("Done"))
                            {
                                lstReport.Items.Add("با موفقیت انجام شد Desription");

                                //  datagridview combobox column source
                                Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text));
                            }
                            else lstReport.Items.Add("با مشکل مواجه شد");
                        }

                    }
                    else if (dr == DialogResult.No)
                    {
                        lstReport.Items.Add("لغو شد");
                    }
                }
                #endregion
            }

            #region DataGridView
            DataTable dtDgv = new DataTable();

            //  column names
            dtDgv = Functions.SqlColumnNames(cmbMainTbl.Text, sqlConnection, cmbMainDB.Text, "فیلد جدول اصلی");

            //  send dtDgv to dgv
            dgv.DataSource = dtDgv;

            for (int j = 0; j < dtDgv.Rows.Count; j++)
            {
                //  column radif identity(0,1)
                dgv.Rows[j].Cells[1].Value = j.ToString();
                //  check item is false
                dgv.Rows[j].Cells[0].Value = false;
            }

            dtDgv = Functions.SqlColumns(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text);
            //clmSecond.Items.Clear();
            clmSecond.HeaderText = "فیلد جدول فرعی";
            #endregion


            #region Defualt Value

            //  column names to list
            List<string> lstColumnNames = Functions.DataTableToList(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection, cmbSecndDB.Text));

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < lstColumnNames.Count; j++)
                {
                    string str1 = dgv.Rows[i].Cells[5].Value.ToString();
                    string str2 = lstColumnNames[j].ToString();

                    if (dgv.Rows[i].Cells[5].Value.ToString() == lstColumnNames[j].ToString())
                    {
                        //  defualt value in combobox
                        dgv.Rows[i].Cells[2].Value = lstColumnNames[j].ToString();
                        break;
                    }
                }
            }

            #endregion

            btnChange.Enabled = btnShow.Enabled = true;
            Cursor.Current = Cursors.Default; this.Enabled = true;

        }

        #region ToolStripMenuItem
        //  ToolStripMenuItem

        private void tsmiClose_Click(object sender, EventArgs e)
        { Close(); }

        private void tsmiExit_Click(object sender, EventArgs e)
        { Application.Exit(); }

        private void اضافهکردنآیتمToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void اصلاحToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void بررسیToolStripMenuItem_Click(object sender, EventArgs e)
        { btnCheck_Click(null, null); }

        private void تغییراتصالبهبانکگToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //**********************************************************
        #endregion




        private void btnDisplay_Click(object sender, EventArgs e)
        {

            //  create query
            main();

            //  change database name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnection);

            //  datagridview run query2
            dgvSearch.DataSource = Functions.SqlDataAdapter(query2, sqlConnection);

            //  report counter
            lstReport.Items.Add("رکورد نتیجه: " + Functions.StrNum(dgvSearch.RowCount - 1));
            lstReport.SelectedIndex = lstReport.Items.Count - 1;

            Cursor.Current = Cursors.Default;

        }






        private void btnEslah_Click(object sender, EventArgs e)
        {
            //  wait mode enable
            Cursor.Current = Cursors.WaitCursor; this.Enabled = false;

            int strTC = 0;
            main();
            if (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0)
            {
                strDescription += " ,join=" + cmbMainClmnJoin.Text;
            }
            //strTC = Functions.SqlRecordCount("select count(*) FROM " + strJoin + strWhere + " AND  b.Desription is NULL", sqlConn);
            query = strUpdate + strDescription + "' FROM " + strJoin + strWhere + " AND  b.Desription is NULL";
            File.WriteAllText(@"../Command2.txt", query);
            lstReport.Items.Add("نتیجه دستور : " + Functions.SqlExcutCommand(query, sqlConnection));
            //strTC = clsSql.SqlRecordCount("select count(*) from [" + cmbDBName2.Text + "].dbo.[" + cmbTBName2.Text + "] Where dbo.[" + cmbTBName2.Text + "].[" + cmbUnicOfMain.Text + "_vld] is not null", Variables.sever);
            int ResultCount = 0;
            //ResultCount = Functions.SqlRecordCount("select count(*) from [" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] Where dbo.[" + cmbSecndTbl.Text + "].[" + cmbMainClmnUniq.Text + "_vld] is null", sqlConn);
            lstReport.Items.Add("رکورد نتیجه: " + Functions.StrNum(Convert.ToInt32(strTC)));
            lstReport.Items.Add("رکورد مانده: " + Functions.StrNum(ResultCount));
            lstReport.SelectedIndex = lstReport.Items.Count - 1;
            Cursor.Current = Cursors.Default; this.Enabled = true;
        }













        private void cmbPersentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intBack = 0, intNext = 0;

            textBox1.Text = cmbPersentName.Text + " درصد ";

            if (cmbPersentName.SelectedIndex != 0)
            {
                intBack = cmbPersentName.SelectedIndex - 1;
                intNext = cmbPersentName.SelectedIndex;
                textBox1.Text = " بین " + cmbPersentName.Items[intNext].ToString() + " تا " + cmbPersentName.Items[intBack].ToString() + " درصد ";
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            //this.Enabled = false;
            //dgvMainClmns.DataSource = Functions.SqlColumns(sqlConn, cmbMainTbl.Text);
            //fun.LoadColumn(cmbMainDB.Text, cmbMainTbl.Text, dgvClmField, Variables.server);
            //fun.LoadColumn(cmbSecndDB.Text, cmbSecndTbl.Text, dgvClmField2, Variables.server);
            // dgvClmField.DisplayIndex = 1;
            // dgvClmField2.DisplayIndex = 1;
            //dgvMainClmns.Enabled = true;
        }

        private void lstReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = "";
            foreach (string lst in lstReport.Items)
            {
                text += lst + Environment.NewLine;
            }
            File.WriteAllText(@"../Report.txt", text);
        }

        private void dgvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            string a = "", b = "";
            textBox1.Text = "";
            strSlt = "";
            string comMain = "[" + cmbMainDB.Text + "].dbo.[" + cmbMainTbl.Text + "] " + a;
            string com2 = "[" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] " + b;
            //**********************
            if (b != "" & a != "")
            {
                for (int l = 0; l < dgv.Rows.Count; l++)
                {

                    if (Convert.ToBoolean(dgv.Rows[l].Cells[0].Value) != false)
                    {
                        #region اطلاعات فیلد از جدول اصلی

                        strCell1 = dgv.Rows[l].Cells[5].Value.ToString();
                        strMain = "[" + a + "].[" + strCell1 + "] " + a + "_" + strCell1;
                        #endregion

                        #region اطلاعات فیلد از جدول فرعی

                        if (dgv.Rows[l].Cells[2].Value != null)
                        {
                            strCell2 = dgv.Rows[l].Cells[2].Value.ToString();

                            if (dgv.Rows[l].Cells[3].Value != null) { strLbl2 = dgv.Rows[l].Cells[3].Value.ToString(); }
                            else strLbl2 = dgv.Rows[l].Cells[2].Value.ToString();
                            strSecond = "[" + b + "].[" + strCell2 + "] " + b + "_" + strLbl2;
                        }
                        else strSecond = "";

                        #endregion

                        if (strSlt == "") { if (strSecond == "") strSlt = strMain; else strSlt = strMain + "," + strSecond; }
                        else { if (strSecond == "") strSlt += "," + strMain; else strSlt += "," + strMain + "," + strSecond; }
                    }

                }
            }
            else MessageBox.Show("خطا");
            //**********************
            //textBox1.Text = "SELECT " + strSlt + " FROM " + comMain + " JOIN " + com2 + " ON " + strMaster + " " + a + ".[" + cmbMainClmnJoin.Text + "])=" + strMaster + " " + b + ".[" + cmbSecndClmnJoin.Text + "])";
        }

        private void cmbNameLike_SelectedIndexChanged(object sender, EventArgs e)
        { if (cmbNameLike.Text != "") clbEhraz.SetItemChecked(0, true); }

        private void cmbFamilyLike_SelectedIndexChanged(object sender, EventArgs e)
        { if (cmbFamilyLike.Text != "") clbEhraz.SetItemChecked(1, true); }

        private void cmbFatherLike_SelectedIndexChanged(object sender, EventArgs e)
        { if (cmbFatherLike.Text != "") clbEhraz.SetItemChecked(2, true); }


        //*****************************           

        #region Functions
        //***           functions


        public void main()
        {
            string a, b;

            lstReport.Items.Add("****************************");

            //  description insert to sql
            strDescription = "";

            strWhere = "";

            //  a is tablename main & b is tablename second
            a = txtLbl1.Text; b = txtLbl2.Text;

            //  create update query
            strUpdate = "Update [" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld]=" + a + "." + cmbMainClmnUniq.Text + " , [Desription]=N'";

            lstReport.Items.Add("احراز هویت بر اساس ");

            //  checked counter
            int clbEhrazCnt = clbEhraz.CheckedItems.Count;

            //  clear variables
            strWhere = strSelect = strJoin = strSlt = textBox1.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            //  Naming Table
            string comMain = "[" + cmbMainDB.Text + "].dbo.[" + cmbMainTbl.Text + "] " + a;
            string com2 = "[" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] " + b;

            //  column info from table main & table second
            #region column info

            if (b != "" & a != "")
            {
                for (int l = 0; l < dgv.Rows.Count; l++)
                {

                    if (Convert.ToBoolean(dgv.Rows[l].Cells[0].Value) != false)
                    {
                        #region اطلاعات فیلد از جدول اصلی

                        strCell1 = dgv.Rows[l].Cells[5].Value.ToString();
                        strMain = "[" + a + "].[" + strCell1 + "] " + a + "_" + strCell1;
                        #endregion

                        #region اطلاعات فیلد از جدول فرعی

                        if (dgv.Rows[l].Cells[2].Value != null)
                        {
                            strCell2 = dgv.Rows[l].Cells[2].Value.ToString();

                            if (dgv.Rows[l].Cells[3].Value != null) { strLbl2 = dgv.Rows[l].Cells[3].Value.ToString(); }
                            else strLbl2 = dgv.Rows[l].Cells[2].Value.ToString();
                            strSecond = "[" + b + "].[" + strCell2 + "] " + b + "_" + strLbl2;
                        }
                        else strSecond = "";


                        if (strSlt == "") { if (strSecond == "") strSlt = strMain; else strSlt = strMain + "," + strSecond; }
                        else { if (strSecond == "") strSlt += "," + strMain; else strSlt += "," + strMain + "," + strSecond; }

                        #endregion
                    }
                }
            }
            else MessageBox.Show("خطا");

            #endregion


            #region Create Select Query & string Where & Join

            strSelect = "SELECT " + strSlt + " FROM ";
            strJoin = comMain + " JOIN " + com2 + " ON " + DeleteFreeSpace(a + ".[" + cmbMainClmnJoin.Text + "]") + " = " + DeleteFreeSpace(b + ".[" + cmbSecndClmnJoin.Text + "]");

            //**********************

            //  checked from checklistbox
            foreach (int Checked in clbEhraz.CheckedIndices)
            {
                switch (Checked)
                {
                    #region Name -> varchar
                    case 0:

                        //  create where for query
                        strWhere = Selection("Name", cmbPersentName, cmbNameLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription("نام", cmbPersentName, cmbNameLike.Text);
                        lstReport.Items.Add(PersentDescription("نام", cmbPersentName, cmbNameLike.Text));

                        break;
                    #endregion

                    #region Family -> varchar
                    case 1:

                        //  create where for query
                        strWhere = Selection("Family", cmbPersentFamily, cmbFamilyLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription(" نام خانوادگی", cmbPersentFamily, cmbFamilyLike.Text);
                        lstReport.Items.Add(PersentDescription(" نام خانوادگی", cmbPersentFamily, cmbFamilyLike.Text));

                        break;
                    #endregion

                    #region Father -> varchar
                    case 2:

                        //  create where for query
                        strWhere = Selection("Father", cmbPersentFather, cmbFatherLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription("نام پدر", cmbPersentFather, cmbFatherLike.Text);
                        lstReport.Items.Add(PersentDescription("نام پدر", cmbPersentFather, cmbFatherLike.Text));

                        break;
                    #endregion

                    #region ShenasCode -> int
                    case 3:

                        //  create where for query
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null"; }
                        else strWhere += " Where " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null";

                        strDescription += " شناسنامه ";
                        lstReport.Items.Add(" شناسنامه ");

                        break;
                    #endregion

                    #region PBirthDate -> smallint                        
                    case 4:
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null "; }
                        else strWhere += " Where " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null ";

                        strDescription += " سال تولد ";
                        lstReport.Items.Add(" سال تولد ");
                        break;
                    #endregion

                    #region CodeMelli -> char(10)
                    case 5:
                        if (strWhere != "")
                        { strWhere += " AND " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null "; }
                        else strWhere += " Where " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null ";

                        strDescription += " کد ملی ";
                        lstReport.Items.Add(" کد ملی ");

                        break;
                    #endregion

                    #region HomeCity -> varchar
                    case 6:
                        if (strWhere != "")
                        { strWhere += " and Replace(" + a + ".HomeCity,' ','')=Replace(" + b + ".HomeCity,' ','')"; }
                        else strWhere += " Where Replace(" + a + ".HomeCity,' ','')=Replace(" + b + ".HomeCity,' ','')";

                        strDescription += " شهر محل تولد ";
                        lstReport.Items.Add(" شهر محل تولد ");

                        break;
                    #endregion

                    #region SodorCity -> varchar
                    case 7:
                        if (strWhere != "")
                        { strWhere += " and Replace(" + a + ".SodorCity,' ','')=Replace(" + b + ".SodorCity,' ','')"; }
                        else strWhere += " Where Replace(" + a + ".SodorCity,' ','')=Replace(" + b + ".SodorCity,' ','')";

                        strDescription += " شهر محل تولد ";
                        lstReport.Items.Add(" شهر محل تولد ");

                        break;
                    #endregion

                    #region SodorOstan -> varchar
                    case 8:
                        break;
                    #endregion

                    #region HomeOstan -> varchar
                    case 9:
                        break;
                    #endregion

                    #region Code Modiriat                        
                    case 12:
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null "; }
                        else strWhere += " Where " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null ";

                        strDescription += " کد مرکز مدیریت ";
                        lstReport.Items.Add("کد مرکز مدیریت");

                        break;
                    #endregion

                    #region Code Khadamat                        
                    case 13:
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null "; }
                        else strWhere += " Where " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null ";

                        strDescription += " کد مرکز خدمات ";
                        lstReport.Items.Add("کد مرکز خدمات");

                        break;
                        #endregion

                }

            }


            if (strWhere != "")
            {
                strWhere += " AND " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";
            }
            else strWhere += " Where " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";

            //  create query finaly
            query2 = strSelect + strJoin + strWhere;

            //  save query2 to file command.txt
            File.WriteAllText(@"../Command.txt", query2);


            #endregion
        }


        #region ComboBox to List<string>
        private List<string> stringtoList(string strCmb)
        {
            List<string> lst = new List<string>();

            //  input string
            string strIn = strCmb.Trim(), strChar;

            while (strIn.IndexOf(",") != -1)
            {
                strChar = strIn.Substring(0, strIn.IndexOf(",")).Trim();

                //  replace 'خالی' to null
                if (strChar.Trim() == "خالی")
                { lst.Add("NULL"); }

                else if (strChar.Trim() == "اله-الله") { lst.Add("اله-الله"); }
                else lst.Add(strChar.Trim());


                strIn = strIn.Replace(strChar.Trim() + ",", "");
            }
            if (strIn.IndexOf(",") == -1)
            {
                //  replace 'خالی' to null
                if (strIn.Trim() == "خالی")
                { lst.Add("NULL"); }

                else if (strIn.Trim() == "اله-الله") { lst.Add("اله-الله"); }
                else lst.Add(strIn.Trim());
            }

            return lst;
        }
        #endregion

        //  
        private string loopWord(List<string> lstLike, string strType, string strLblA, string strLblB, string strColumnUniq, string strWhere)
        {

            for (int i = 0; i < lstLike.Count; i++)
            {
                if (lstLike[i] == "NULL")
                {
                    if (strWhere == "")
                    {
                        strWhere = " WHERE " + strLblA + ".[" + strType + "] IS " + lstLike[i] + " OR " + strLblB + ".[" + strType + "] IS " + lstLike[i] + " ";
                    }
                    else
                    {
                        strWhere = strWhere + " OR ((" + strLblA + ".[" + strType + "] IS " + lstLike[i] + " OR " + strLblB + ".[" + strType + "] IS " + lstLike[i] + " )";
                    }
                }
                else if (lstLike[i] == "اله-الله")
                {
                    if (strWhere == "")
                    {
                        strWhere = " WHERE (REPLACE(REPLACE(" + strLblA + ".[" + strType + "], N'الله', ''), N'اله', '') = REPLACE(REPLACE(" + strLblB + ".[" + strType + "], N'الله', ''), N'اله', '') AND " +
                                   "(" + strLblA + ".[" + strType + "] LIKE N'%اله%' OR " + strLblB + ".[" + strType + "] LIKE N'%اله%' or " + strLblA + ".[" + strType + "] LIKE N'%الله%' or " + strLblB + ".[" + strType + "] LIKE N'%الله%') AND " + strLblA + ".[" + strType + "]<> " + strLblB + ".[" + strType + "])";
                    }
                    else
                    {
                        strWhere = strWhere +
                                   " OR (REPLACE(REPLACE(" + strLblA + ".[" + strType + "], N'الله', ''), N'اله', '') = REPLACE(REPLACE(" + strLblB + ".[" + strType + "], N'الله', ''), N'اله', '') AND " +
                                   "(" + strLblA + ".[" + strType + "] LIKE N'%اله%' OR " + strLblB + ".[" + strType + "] LIKE N'%اله%' or " + strLblA + ".[" + strType + "] LIKE N'%الله%' or " + strLblB + ".[" + strType + "] LIKE N'%الله%') AND " + strLblA + ".[" + strType + "]<> " + strLblB + ".[" + strType + "])";
                    }
                }
                else
                {
                    if (strWhere == "")
                    {
                        strWhere = " WHERE (Replace(" + strLblA + ".[" + strType + "] ,'" + lstLike[i] + "','') = Replace(" + strLblB + ".[" + strType + "],'" + lstLike[i] + "','')" +
                                    " AND (" + strLblA + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%' OR " + strLblB + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%') AND " + strLblA + ".[" + strType + "]<>" + strLblB + ".[" + strType + "])";
                    }
                    else
                    {
                        strWhere = strWhere + " OR (Replace(" + strLblA + ".[" + strType + "] ,'" + lstLike[i] + "','') = Replace(" + strLblB + ".[" + strType + "],'" + lstLike[i] + "','')" +
                                    " AND (" + strLblA + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%' OR " + strLblB + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%') AND " + strLblA + ".[" + strType + "]<>" + strLblB + ".[" + strType + "])";
                    }
                }
            }

            return strWhere;
        }


        private string Selection(string strType, ComboBox cmbPersent, string strLike, string strWhere, string strLblA, string strLblB)
        {
            string strReturn = "";

            if (strLike == "")
            {
                //  ==  100%
                if (cmbPersent.Text == "100")
                {
                    //  check strWhere
                    strReturn = " Where REPLACE(" + strLblA + ".[" + strType + "],' ','') = REPLACE(" + strLblB + ".[" + strType + "],' ','') ";
                    if (strWhere != "")
                    { strReturn = strWhere + " AND REPLACE(" + strLblA + ".[" + strType + "],' ','') = REPLACE(" + strLblB + ".[" + strType + "],' ','') "; }

                }
                //  < 100%
                else
                {
                    //  check strWhere
                    strReturn = "Where (dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))>=" + cmbPersent.Items[cmbPersent.SelectedIndex].ToString() +
                                  " AND dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))<" + cmbPersent.Items[cmbPersent.SelectedIndex - 1].ToString() + ")";

                    if (strWhere != "") strReturn = strWhere + "AND (dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))>=" + cmbPersent.Items[cmbPersent.SelectedIndex].ToString() +
                                        " AND dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))<" + cmbPersent.Items[cmbPersent.SelectedIndex - 1].ToString() + ")";
                }
            }
            else
            {
                strReturn = loopWord(stringtoList(strLike), strType, strLblA, strLblB, cmbMainClmnUniq.Text, strWhere);
            }


            //  100%
            //if (cmbPersent.Text == "100")
            //{
            //    if (strLike == "")
            //    {
            //        strReturn = " Where REPLACE(" + strLblA + ".[" + strType + "],' ','') = REPLACE(" + strLblB + ".[" + strType + "],' ','') ";
            //        if (strWhere != "") strReturn = strWhere + " AND REPLACE(" + strLblA + ".[" + strType + "],' ','') = REPLACE(" + strLblB + ".[" + strType + "],' ','') ";
            //    }
            //    else
            //    {
            //        strReturn = loopWord(stringtoList(strLike), strType, strLblA, strLblB, cmbMainClmnUniq.Text, strWhere, cmbPersent.Text);
            //    }
            //}
            ////  <100%
            //else
            //{
            //    if (strLike == "")
            //    {
            //        strPersent = cmbPersent.Items[cmbPersent.SelectedIndex].ToString();


            //    }
            //    else
            //    {

            //    }
            //}
            // else 


            return strReturn;
        }

        //  report to description
        private string PersentDescription(string strType, ComboBox cmbPersent, string strDeleteWord = "")
        {
            string strReturn = "";
            int intBack = 0, intNext = 0;

            //  persent 100%
            strReturn = strType + cmbPersent.Text + " درصد ";

            //  if strDeleteWord not null
            if (strDeleteWord != "") strReturn = strType + cmbPersent.Text + " درصد " + " , " + strDeleteWord;

            if (cmbPersent.SelectedIndex != 0)
            {
                intBack = cmbPersent.SelectedIndex - 1;
                intNext = cmbPersent.SelectedIndex;
                strReturn = strType + " بین " + cmbPersent.Items[intNext].ToString() + " تا " + cmbPersent.Items[intBack].ToString() + " درصد ";
                if (strDeleteWord != "") strReturn = strType + " بین " + cmbPersent.Items[intNext].ToString() + " تا " + cmbPersent.Items[intBack].ToString() + " درصد " + " , " + strDeleteWord;

            }
            return strReturn;
        }

        private int CheckField(DataTable dtInput, string strSearchField)
        {
            int intReturn = 0;
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                if (dtInput.Rows[i][0].ToString().Contains(strSearchField))
                {
                    intReturn = 1;
                }
            }
            return intReturn;
        }

        private string DeleteFreeSpace(string strItem)
        { return "Replace(" + strItem + ", ' ', '')"; }

        //*******************************************
        #endregion
    }
}


