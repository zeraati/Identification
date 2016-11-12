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
        Functions functions = new Functions();
        SqlFunctions sqlfunction = new SqlFunctions();


        SqlConnection sqlConnectionMain = new SqlConnection();
        SqlConnection sqlConnectionSecond = new SqlConnection();
        SetPersonFunctions spf = new SetPersonFunctions();


        string query, strWhere, strSelect, strUpdate, strGroup;
        string strFrom, strON, strSwitch_on, strLColumn, strRColumn, strQuery;

        string strCell1, strCell2, strLbl2, strMain, strSecond, strSlt;
        string strField, strDescription;

        List<string> lstDataGrid = new List<string>();

        int intStepOver;

        bool bEnableCreateClm = false;

        #endregion

        #region Defualt Values
        string strCmbMainDB = "Test";
        string strCmbSecondDB = "Test";
        string strCmbMainTbl = "Person_TeST";
        string strCmbSecondTbl = "Person_TeST_copy";

        #endregion



        #region Text Estandard Event
        string[] strJoinEvent =
        {
            "کدمرکزمدیریت",
            "کدمرکزخدمات",
            "کدملی",
            "شماره شناسنامه"
        };

        private string strPersentEvent(int i)
        {
            string strRtn = "";

            switch (i)
            {
                case 0:
                    strRtn = "100";
                    break;
                case 1:
                    strRtn = "95";
                    break;
                case 2:
                    strRtn = "90";
                    break;
                case 3:
                    strRtn = "85";
                    break;
                case 4:
                    strRtn = "80";
                    break;
                case 5:
                    strRtn = "75";
                    break;
                case 6:
                    strRtn = "70";
                    break;
            }
            return strRtn;
        }


        private string strTextEvent(int i)
        {
            string strRtn = "";

            switch (i)
            {
                case 0:
                    strRtn = "نام " + strPersentEvent(0);
                    break;
                case 1:
                    strRtn = "نام خانوادگی " + strPersentEvent(0);
                    break;
                case 2:
                    strRtn = "نام پدر " + strPersentEvent(0);
                    break;
                case 3:
                    strRtn = "کدملی";
                    break;
                case 4:
                    strRtn = "100 درصد";
                    break;
            }
            return strRtn;
        }
        #endregion


        string strEventListPath = @"../Event.txt";

        public SetPerson(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnectionMain = sqlCon;
            sqlConnectionSecond = sqlCon;
        }

        private void SetPerson_Load(object sender, EventArgs e)
        {
            //  Sql Server Connection Info
            this.Text = "Identification" + "  -  Server Name = " + sqlConnectionMain.DataSource;

            //  clear list
            lstDataGrid.Clear();

            //  disable button
            btnChange.Enabled = btnShow.Enabled = false;

            //  load database name source
            cmbMainDB.DataSource = sqlfunction.SqlGetDBName(sqlConnectionMain);
            cmbSecondDB.DataSource = sqlfunction.SqlGetDBName(sqlConnectionSecond);


            //  check exists file event.txt
            string sss = functions.CreateFile(strEventListPath);

            //  combobox read file text
            functions.ListToCmb(functions.ReadTxt(strEventListPath), cmbEventList);

            //  clear checklistbox
            clbEhraz.Items.Clear();

            //  default value
            intStepOver = 0;


            // defult
            cmbMainDB.Text = strCmbMainDB;
            cmbSecondDB.Text = strCmbSecondDB;
            cmbMainTbl.Text = strCmbMainTbl;
            cmbSecondTbl.Text = strCmbSecondTbl;


        }

        private void cmbDBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {

            //  change data base name
            sqlConnectionMain = sqlfunction.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnectionMain);

            //  load table name
            cmbMainTbl.DataSource = sqlfunction.SqlTableName(sqlConnectionMain);


            //  default value            
            //cmbMainTbl.Text = "TBL_Student";
        }

        private void cmbDBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnectionSecond = sqlfunction.SqlConnectionChangeDB(cmbSecondDB.Text, sqlConnectionSecond);

            //  load table name
            cmbSecondTbl.DataSource = sqlfunction.SqlTableName(sqlConnectionSecond);

            //  default value
            //cmbSecondTbl.Text = "TBL_MKsarparast";
        }

        private void cmbTBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbMainClmnUniq.DataSource = functions.DataTableToList(sqlfunction.SqlColumnNames(cmbMainTbl.Text, sqlConnectionSecond));

            //  load column name to list                          
            lstMainClmns.DataSource = functions.DataTableToList(sqlfunction.SqlColumns(cmbMainTbl.Text, sqlConnectionMain, cmbMainDB.Text), 1);


            //  enable btn barresi
            btnCheck.Enabled = true;

        }

        private void cmbSecondTbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSecndClmns.DataSource = functions.DataTableToList(sqlfunction.SqlColumns(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text), 1);
        }


        private void btnCheck_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int intCount = 0, intCheck = 0;

            string strFunctionsFile = @"../Functions";
            string strFinal;

            //  clear list
            richtxtReport.Text = "";


            #region Sql Functions From Text File

            //  list sql functions
            List<string> lstSqlFunction = new List<string>();

            //  check sql functions

            List<string> lstSqlFile = new List<string>();
            bool bol = false;


            //  change data base name
            sqlConnectionMain = sqlfunction.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnectionMain);

            //  list functions files to sql
            lstSqlFile = lstSqlFile = functions.DataTableToList(sqlfunction.SqlFunctionsFiles(sqlConnectionMain));

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
                if (bol == false) { sqlfunction.SqlExcutCommand(File.ReadAllText(file), sqlConnectionMain); richtxtReport.Text = "Create Function Is Sql : " + Path.GetFileNameWithoutExtension(file) + Environment.NewLine; ; }
            }

            #endregion


            intCheck = 0;

            DataTable dtTotal = new DataTable();


            //if (cmbMainDB.Text == cmbSecondDB.Text & cmbMainTbl.Text == cmbSecondTbl.Text)
            //{
            //    MessageBox.Show("جداول نباید مثل هم باشد", "!خطا");
            //}
            //else
            //{


            #region Data Bases Info

            ////  table main record count
            //intCount = sqlfunction.SqlRecordCount(cmbMainTbl.Text, sqlConnectionMain);

            ////  report count
            //richtxtReport.Text+="تعداد رکورد جدول از بانک اصلی : " + functions.StrNum(intCount)+Environment.NewLine;

            ////  table second count
            //intCount = sqlfunction.SqlRecordCount(cmbSecondTbl.Text, sqlConnectionSecond);

            ////  report count
            //richtxtReport.Text+="تعداد رکورد جدول از بانک فرعی : " + functions.StrNum(intCount) + Environment.NewLine;

            ////  cmb main join cmb second and total
            //intCount = sqlfunction.SqlJoin(cmbMainTbl.Text, cmbSecondTbl.Text, cmbMainClmnJoin.Text, cmbSecndClmnJoin.Text, sqlConnectionMain, cmbMainDB.Text, cmbSecondDB.Text).Rows.Count;

            ////  report count
            //richtxtReport.Text+="تعداد کل رکورد ها : " + functions.StrNum(intCount) + Environment.NewLine;

            ////  cmb main join cmb second and filter
            //intCount = sqlfunction.SqlJoin(cmbMainTbl.Text, cmbSecondTbl.Text, cmbMainClmnJoin.Text, cmbSecndClmnJoin.Text, sqlConnectionMain, cmbMainDB.Text, cmbSecondDB.Text, "").Rows.Count;

            ////  report count
            //richtxtReport.Text+="تعداد کل رکورد ها از بین دو بانک فوق : " + functions.StrNum(intCount) + Environment.NewLine;

            #endregion

            //*****         END

            //  datagridview combobox column source
            functions.ComboBoxSource(clmSecond, sqlfunction.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text));




            #region Create New Field

            richtxtReport.Text += spf.CreateNewField(cmbMainClmnUniq.Text, cmbSecondTbl.Text, cmbSecondTbl, sqlConnectionSecond) + Environment.NewLine;

            #endregion

            //}

            #region DataGridView
            DataTable dtDgv = new DataTable();

            //  column names
            dtDgv = sqlfunction.SqlColumnNames(cmbMainTbl.Text, sqlConnectionMain, cmbMainDB.Text, "فیلد جدول اصلی");


            //  add item to checklistbox
            AddItemCheckList(functions.DataTableToList(dtDgv), Variable.strArray, clbEhraz);


            //  remove row if has 'stid'
            #region Remove One Row            
            //for (int i = 0; i < dtDgv.Rows.Count; i++)
            //{
            //    string strtxt = dtDgv.Rows[i][0].ToString().ToUpper();

            //    if (dtDgv.Rows[i][0].ToString().ToUpper() == cmbMainClmnJoin.Text.ToUpper())
            //    {
            //        dtDgv.Rows.Remove(dtDgv.Rows[i]);
            //        break;
            //    }
            //}
            #endregion


            //  send dtDgv to dgv
            dgv.DataSource = dtDgv;

            #region Default Value
            for (int j = 0; j < dtDgv.Rows.Count; j++)
            {
                //  column radif identity(0,1)
                dgv.Rows[j].Cells[1].Value = j.ToString();
                //  check item is false
                dgv.Rows[j].Cells[0].Value = false;
            }
            #endregion


            dtDgv = sqlfunction.SqlColumns(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text);
            //clmSecond.Items.Clear();
            clmSecond.HeaderText = "فیلد جدول فرعی";
            #endregion


            #region Defualt Value

            //  column names to list
            List<string> lstColumnNames = functions.DataTableToList(sqlfunction.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text));
            //  test list
            List<string> lsttest = new List<string>();
            bool bBreak = false;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < lstColumnNames.Count; j++)
                {
                    string str1 = dgv.Rows[i].Cells[6].Value.ToString();
                    string str2 = lstColumnNames[j].ToString();

                    if (dgv.Rows[i].Cells[6].Value.ToString() == lstColumnNames[j].ToString())
                    {
                        //  defualt value in combobox
                        dgv.Rows[i].Cells[2].Value = lstColumnNames[j].ToString();
                        break;
                    }
                    else
                    {
                        for (int l = 0; l < Variable.strArray.Length / 4; l++)
                        {
                            if (Variable.strArray[l, 0] == lstColumnNames[j].ToString()
                                || Variable.strArray[l, 1] == lstColumnNames[j].ToString()
                                || Variable.strArray[l, 2] == lstColumnNames[j].ToString())
                            {
                                //  defualt value in combobox
                                dgv.Rows[i].Cells[2].Value = Variable.strArray[l, 0];
                                string strtest = Variable.strArray[l, 0];
                                lsttest.Add(Variable.strArray[l, 0]);
                                bBreak = true;
                                goto Br;

                            }
                        Br: if (bBreak == true) { bBreak = false; break; }
                        }
                    }
                }
            }

            #endregion

            cmbMainClmnJoin_SelectedIndexChanged(null, null);

            btnChange.Enabled = btnShow.Enabled = true;
            Cursor.Current = Cursors.Default;


            //  check table log
            intCheck = functions.ListFind(sqlfunction.SqlTableName(sqlConnectionSecond, cmbSecondDB.Text), "LogFile");

            if (intCheck == -1)
            {
                // create query
                string strQuery = "CREATE TABLE LogFile (ID INT NOT NULL IDENTITY(1,1),[date] DATE NULL,logfile NVARCHAR(max) NOT NULL)";

                //  create table log
                //  run query
                strFinal = sqlfunction.SqlExcutCommand(strQuery, sqlConnectionSecond);

                //  report
                if (strFinal.Contains("Done"))
                { richtxtReport.Text += "جدول logfile ساخته شد" + Environment.NewLine; }
                else richtxtReport.Text += "ساختن جدول logfile با مشکل مواجه شد" + Environment.NewLine;

            }

            //  disable btn barresi
            btnCheck.Enabled = false;

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
            Form1 frmLogin = new Form1();
            frmLogin.ShowDialog();
        }

        //**********************************************************
        #endregion


        private void btnDisplay_Click(object sender, EventArgs e)
        {

            //  wait mode enable
            Cursor.Current = Cursors.WaitCursor;

            main();


            //  datagridview run query2
            dgvSearch.DataSource = sqlfunction.SqlDataAdapter(strQuery, sqlConnectionMain);

            //  report counter
            richtxtReport.Text += "رکورد نتیجه: " + functions.StrNum(dgvSearch.RowCount - 1) + Environment.NewLine;



            Cursor.Current = Cursors.Default;

        }

        private void btnEslah_Click(object sender, EventArgs e)
        {
            //  wait mode enable
            Cursor.Current = Cursors.WaitCursor;

            //  function filters
            main();

            //if (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0) { strDescription += " ,join=" + cmbMainClmnJoin.Text; }
            //strDescription += (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0) ? " ,join=" + cmbMainClmnJoin.Text : strDescription;

            //  create query

            query = strUpdate + strDescription + "' FROM " + strFrom + strON + strWhere + " SELECT @@ROWCOUNT";
            richtxtUpdQuery.Text = query;
            //  save query
            File.WriteAllText(@"../Command2.txt", query);

            //  report
            //richtxtReport.Text += "رکورد نتیجه : " + sqlfunction.SqlRunQuery(query, sqlConnectionSecond) + Environment.NewLine;

            //  default value
            int ResultCount = 0;

            //  count not used
            //ResultCount = sqlfunction.SqlCountColumn(cmbSecondTbl.Text, sqlConnectionSecond, "[" + cmbMainClmnUniq.Text + "_vld] is null");

            //  report
            //richtxtReport.Text += "رکورد مانده : " + functions.StrNum(ResultCount) + Environment.NewLine;
            //richtxtReport.SelectedIndex = richtxtReport.Items.Count - 1;

            //  wait mode disable
            Cursor.Current = Cursors.Default;
        }


        private void dgvSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string strQuery = "";

            if (e.ColumnIndex == 0)
            {
                strQuery = "UPDATE [" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld] = " + dgvSearch.Rows[e.RowIndex].Cells[1].Value.ToString() +
                            " , [Description]=N'" + strDescription + " , آپدیت دستی'" + " WHERE STID =" + dgvSearch.Rows[e.RowIndex].Cells[2].Value.ToString() + " AND [" + cmbMainClmnUniq.Text + "_vld] IS NULL SELECT @@ROWCOUNT";

                richtxtReport.Text += "رکورد نتیجه: " + sqlfunction.SqlRunQuery(strQuery, sqlConnectionSecond, cmbSecondDB.Text) + Environment.NewLine;

                dgvSearch.Rows.Remove(dgvSearch.Rows[e.RowIndex]);
            }
        }

        private void cmbMainClmnJoin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDgv = new DataTable();

            //  column names
            dtDgv = sqlfunction.SqlColumnNames(cmbMainTbl.Text, sqlConnectionMain, cmbMainDB.Text, "فیلد جدول اصلی");

            //  add item to checklistbox
            AddItemCheckList(functions.DataTableToList(dtDgv), Variable.strArray, clbEhraz);

            for (int i = 0; i < Variable.strArray.Length / 4; i++)
            {
                for (int o = 0; o < 3; o++)
                {
                    string strs = Variable.strArray[i, o];

                }
            }
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            List<GroupBox> GroupBoxes = new List<GroupBox>();
            List<ComboBox> Caja = new List<ComboBox>();
            for (int i = 0; i < 3; i++)
            {
                ComboBox cb = new ComboBox();
                cb.Location = new System.Drawing.Point(51, 21);
                cb.Name = "comboBox" + i.ToString();
                cb.Size = new System.Drawing.Size(121, 21);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                Caja.Add(cb);
                GroupBox gb = new GroupBox();
                gb.Location = new System.Drawing.Point(51, 21);
                gb.Size = new System.Drawing.Size(203, 56);
                gb.Text = "رویداد " + (i + 1).ToString();
                gb.Name = "GroupBox" + i.ToString();
                gb.Controls.Add(cb);
                GroupBoxes.Add(gb);
                this.flowLayoutPanel1.Controls.Add(gb);
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        #region Event
        private void cmbEventList_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        #endregion


        private void فعالکردنبررسیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  enable btn barresi
            btnCheck.Enabled = true;
        }

        private void btnStepOver_Click(object sender, EventArgs e)
        {

            intStepOver++;
        }


        //*****************************           

        #region Functions
        //***           functions

        //  function is defined
        private void Defined(int i)
        {
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }


        public void main()
        {
            strFrom = "";
            strWhere = "";
            strSelect = "";
            strON = "";
            strLColumn = "";
            strRColumn = "";
            strQuery = "";

            //  label tables
            string a = txtLbl1.Text, b = txtLbl2.Text;

            richtxtReport.Text += "****************************" + Environment.NewLine;

            //  description insert to sql
            strDescription = "";


            //  from
            strFrom = " dbo.[" + cmbMainTbl.Text + "] " + a + " JOIN dbo.[" + cmbSecondTbl.Text + "] " + b + " ";

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgv.Rows[i].Cells[0].Value) != false)
                {

                    strSwitch_on = (dgv.Rows[i].Cells[5].Value != null) ? dgv.Rows[i].Cells[5].Value.ToString().ToUpper().Trim() : "";

                    strLColumn = "Replace(" + a + ".[" + dgv.Rows[i].Cells[6].Value.ToString() + "],' ','')";
                    strRColumn = "Replace(" + b + ".[" + dgv.Rows[i].Cells[2].Value.ToString() + "],' ','')";

                    #region Select
                    if (strSelect == "")
                    {
                        strSelect = " " + a + ".[" + dgv.Rows[i].Cells[6].Value.ToString() + "] [" + a + "_" + dgv.Rows[i].Cells[6].Value.ToString() + "]," +
                              b + ".[" + dgv.Rows[i].Cells[2].Value.ToString() + "] [" + b + "_" + dgv.Rows[i].Cells[2].Value.ToString() + "] ";
                    }
                    else
                    {
                        strSelect += "," + a + ".[" + dgv.Rows[i].Cells[6].Value.ToString() + "] [" + a + "_" + dgv.Rows[i].Cells[6].Value.ToString() + "]," +
                        b + ".[" + dgv.Rows[i].Cells[2].Value.ToString() + "] [" + b + "_" + dgv.Rows[i].Cells[2].Value.ToString() + "] ";
                    }
                    #endregion

                    #region Case

                    switch (strSwitch_on)
                    {
                        case "JOIN":
                            strON = (strON == "") ? " ON " + strLColumn + " = " + strRColumn : strON;

                            //  report
                            strDescription += strSwitch_on + "=" + dgv.Rows[i].Cells[2].Value.ToString() + " , ";
                            richtxtReport.Text += strSwitch_on + "=" + dgv.Rows[i].Cells[2].Value.ToString() + " " + Environment.NewLine;

                            break;
                        case "=":
                            strLColumn = "Replace(" + a + ".[" + dgv.Rows[i].Cells[6].Value.ToString() + "],' ','')";
                            strRColumn = "Replace(" + b + ".[" + dgv.Rows[i].Cells[2].Value.ToString() + "],' ','')";

                            if (dgv.Rows[i].Cells[5].Value.ToString() == "=")
                            {
                                strWhere += (strWhere == "") ? " " + strLColumn + " = " + strRColumn : " AND " + strLColumn + " = " + strRColumn;
                                strDescription += " " + dgv.Rows[i].Cells[2].Value.ToString() + " , ";
                                richtxtReport.Text += " " + dgv.Rows[i].Cells[2].Value.ToString() + " , " + Environment.NewLine;
                            }

                            break;
                        case "سيد":

                            break;
                        case "مير":

                            break;
                        case "اله-الله":

                            break;

                        case "خالی":
                            break;

                    }

                    #endregion

                }

            }


            #region Coment
            ////  a is tablename main & b is tablename second
            //a = txtLbl1.Text; b = txtLbl2.Text;

            //  create update query
            strUpdate = "Update [" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld]=" + a + "." + cmbMainClmnUniq.Text + " , [Description]=N'";

            ////  report
            //richtxtReport.Text += "احراز هویت بر اساس " + Environment.NewLine;

            ////  checked counter
            //int clbEhrazCnt = clbEhraz.CheckedItems.Count;

            ////  clear variables
            //strWhere = strSelect = strJoin = strSlt = "";

            //Cursor.Current = Cursors.WaitCursor;

            ////  Naming Table
            //string comMain = "[" + cmbMainDB.Text + "].dbo.[" + cmbMainTbl.Text + "] " + a;
            //string com2 = "[" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] " + b;

            ////  column info from table main & table second
            //#region column info

            //if (b != "" & a != "")
            //{
            //    for (int l = 0; l < dgv.Rows.Count; l++)
            //    {

            //        if (Convert.ToBoolean(dgv.Rows[l].Cells[0].Value) != false)
            //        {
            //            #region اطلاعات فیلد از جدول اصلی

            //            strCell1 = dgv.Rows[l].Cells[5].Value.ToString();
            //            strMain = "[" + a + "].[" + strCell1 + "] " + a + "_" + strCell1;
            //            strGroup += (strGroup == "") ? " [" + a + "].[" + strCell1 + "] " : ",[" + a + "].[" + strCell1 + "] ";
            //            #endregion

            //            #region اطلاعات فیلد از جدول فرعی

            //            if (dgv.Rows[l].Cells[2].Value != null)
            //            {
            //                strCell2 = dgv.Rows[l].Cells[2].Value.ToString();

            //                if (dgv.Rows[l].Cells[3].Value != null) { strLbl2 = dgv.Rows[l].Cells[3].Value.ToString(); }
            //                else strLbl2 = dgv.Rows[l].Cells[2].Value.ToString();
            //                strSecond = "[" + b + "].[" + strCell2 + "] " + b + "_" + strLbl2;
            //                strGroup += (strGroup == "") ? "[" + b + "].[" + strCell2 + "] " : ",[" + b + "].[" + strCell2 + "] ";
            //            }
            //            else strSecond = "";


            //            if (strSlt == "") { if (strSecond == "") strSlt = strMain; else strSlt = strMain + "," + strSecond; }
            //            else { if (strSecond == "") strSlt += "," + strMain; else strSlt += "," + strMain + "," + strSecond; }

            //            #endregion
            //        }
            //    }
            //}
            //else MessageBox.Show("خطا");

            #endregion
            #endregion

            #region Create Select Query & string Where & Join & Group by

            //**********************

            #region chbUniq
            if (chbUniq.CheckState == CheckState.Checked)
            {
                if (strWhere != "")
                {
                    strWhere += " AND " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL AND " + txtLbl2.Text + ".Description is NULL";
                    strWhere = " WHERE " + strWhere;
                }
                else strWhere += " Where " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL AND " + txtLbl2.Text + ".Description is NULL";
            }
            else
            {
                strWhere = " WHERE " + strWhere;
            }
            #endregion

            //  create query

            strQuery = (strWhere == "") ? "SELECT " + strSelect + " FROM " + strFrom + strON : "SELECT " + strSelect + " FROM " + strFrom + strON + strWhere;

            //  richtextbox <= query
            richTxtQuery.Text = strQuery;

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
            string strMath = " AND ";

            for (int i = 0; i < lstLike.Count; i++)
            {
                if (i > 1)
                { strMath = " OR "; }

                if (lstLike[i] == "NULL")
                {
                    if (strWhere == "")
                    {
                        strWhere = " WHERE  (" + strLblA + ".[" + strType + "] IS " + lstLike[i] + " AND " + strLblB + ".[" + strType + "] IS NOT " + lstLike[i] + " )" +
                            " OR (" + strLblA + ".[" + strType + "] IS NOT " + lstLike[i] + " AND " + strLblB + ".[" + strType + "] IS " + lstLike[i] + " )";
                    }
                    else
                    {
                        strWhere = strWhere + strMath + " (" + strLblA + ".[" + strType + "] IS " + lstLike[i] + " AND " + strLblB + ".[" + strType + "] IS NOT " + lstLike[i] + " )" +
                            " OR (" + strLblA + ".[" + strType + "] IS NOT " + lstLike[i] + " AND " + strLblB + ".[" + strType + "] IS " + lstLike[i] + " )";
                    }
                }
                else if (lstLike[i] == "اله-الله")
                {
                    if (strWhere == "")
                    {
                        strWhere = " WHERE (REPLACE(REPLACE(" + strLblA + ".[" + strType + "], N'الله', ''), N'اله', '') = REPLACE(REPLACE(" + strLblB + ".[" + strType + "], N'الله', ''), N'اله', '') AND " +

                            "(" + strLblA + ".[" + strType + "] LIKE N'%اله%' OR " + strLblA + ".[" + strType + "] LIKE N'%الله%' AND " +
                            strLblB + ".[" + strType + "] LIKE N'%اله%' OR " + strLblB + ".[" + strType + "] LIKE N'%الله%') AND " +
                            strLblA + ".[" + strType + "]<> " + strLblB + ".[" + strType + "])";
                    }
                    else
                    {
                        strWhere = strWhere + strMath +
                                   " (REPLACE(REPLACE(" + strLblA + ".[" + strType + "], N'الله', ''), N'اله', '') = REPLACE(REPLACE(" + strLblB + ".[" + strType + "], N'الله', ''), N'اله', '') AND " +
                                   "(" + strLblA + ".[" + strType + "] LIKE N'%اله%' OR " + strLblA + ".[" + strType + "] LIKE N'%الله%' AND " +
                                   strLblB + ".[" + strType + "] LIKE N'%اله%' OR " + strLblB + ".[" + strType + "] LIKE N'%الله%') AND " +
                                   strLblA + ".[" + strType + "]<> " + strLblB + ".[" + strType + "])";
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
                        strWhere = strWhere + strMath + " (Replace(" + strLblA + ".[" + strType + "] ,'" + lstLike[i] + "','') = Replace(" + strLblB + ".[" + strType + "],'" + lstLike[i] + "','')" +
                                    " AND (" + strLblA + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%' OR " + strLblB + ".[" + strType + "] LIKE N'%" + lstLike[i] + "%') AND " + strLblA + ".[" + strType + "]<>" + strLblB + ".[" + strType + "])";
                    }
                }
            }

            return strWhere;
        }

        private string Selection(string strType, DataGridViewComboBoxColumn cmbPersent, string strLike, string strWhere, string strLblA, string strLblB)
        {
            string strReturn = "";

            if (strLike == "")
            {
                //  < 100%
                if (cmbPersent.ValueMember != "100" & cmbPersent.ValueMember != "")
                {
                    //  check strWhere
                    strReturn = "Where (dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))>=" +// + cmbPersent.Items[cmbPersent.].ToString() +
                                  " AND dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))<";// + cmbPersent.Items[cmbPersent.SelectedIndex - 1].ToString() + ")";

                    if (strWhere != "") strReturn = strWhere + "AND (dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))>=" +// + cmbPersent.Items[cmbPersent.SelectedIndex].ToString() +
                                        " AND dbo.GetPercentageOfTwoStringMatching(REPLACE(" + strLblA + ".[" + strType + "],' ',''),REPLACE(" + strLblB + ".[" + strType + "],' ',''))<";// + cmbPersent.Items[].ToString() + ")";
                }
            }
            else
            {
                strReturn = loopWord(stringtoList(strLike), strType, strLblA, strLblB, cmbMainClmnUniq.Text, strWhere);
            }

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








        private string DeleteFreeSpace(string strItem)
        { return "Replace(" + strItem + ", ' ', '')"; }

        private void AddItemCheckList(List<string> lstItem, string[,] strArr, CheckedListBox clbEhraz)
        {
            clbEhraz.Items.Clear();
            for (int i = 0; i < lstItem.Count; i++)
            {
                for (int o = 0; o < strArr.Length / 4; o++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (lstItem[i].ToUpper() == strArr[o, j].ToUpper())
                        {
                            string strs = strArr[o, j];
                            clbEhraz.Items.Add(strArr[o, 2]);
                        }
                    }
                }
            }
        }

        //*******************************************

    }
}


