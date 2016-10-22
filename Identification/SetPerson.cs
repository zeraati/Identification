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
        SqlConnection sqlConnectionMain = new SqlConnection();
        SqlConnection sqlConnectionSecond = new SqlConnection();

        string query, query2, strJoin, strWhere, strSelect, strUpdate, strGroup;
        string strCell1, strCell2, strLbl2, strMain, strSecond, strSlt;
        string strField, strDescription;

        int intStepOver;

        bool bEnableCreateClm = false;

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

            //  disable button
            btnChange.Enabled = btnShow.Enabled = false;

            //  load database name source
            cmbMainDB.DataSource = functions.SqlGetDBName(sqlConnectionMain);
            cmbSecondDB.DataSource = functions.SqlGetDBName(sqlConnectionSecond);


            //  check exists file event.txt
            string sss = functions.CreateFile(strEventListPath);

            //  combobox read file text
            functions.ListToCmb(functions.ReadTxt(strEventListPath), cmbEventList);

            //  clear checklistbox
            clbEhraz.Items.Clear();

            //  default value
            intStepOver = 0;


            // defult
            cmbMainDB.Text = cmbSecondDB.Text = "_Main";
            cmbMainTbl.Text = "TBL_Student_Main_C";
            cmbSecondTbl.Text = "TBL_Student_Iden";
        }

        private void cmbDBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnectionMain = functions.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnectionMain);

            //  load table name
            cmbMainTbl.DataSource = functions.SqlTableName(sqlConnectionMain);

            //  default value            
            //cmbMainTbl.Text = "TBL_Student";
        }

        private void cmbDBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnectionSecond = functions.SqlConnectionChangeDB(cmbSecondDB.Text, sqlConnectionSecond);

            //  load table name
            cmbSecondTbl.DataSource = functions.SqlTableName(sqlConnectionSecond);

            //  default value
            //cmbSecondTbl.Text = "TBL_MKsarparast";
        }

        private void cmbTBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbMainClmnJoin.DataSource = functions.DataTableToList(functions.SqlColumnNames(cmbMainTbl.Text, sqlConnectionMain));
            cmbMainClmnUniq.DataSource = functions.DataTableToList(functions.SqlColumnNames(cmbMainTbl.Text, sqlConnectionSecond));

            //  default value
            cmbMainClmnJoin.Text = "CodeMelli";
        }

        private void cmbTBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load cmb field source
            cmbSecndClmnJoin.DataSource = functions.DataTableToList(functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond));

            //  default value
            cmbSecndClmnJoin.Text = "CodeMelli";
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            //  select all & unselect all
            functions.SelectUnselect(clbEhraz, btnSelectAll);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int intCount = 0, intCheck = 0;

            string strFunctionsFile = @"../Functions";
            string strFinal;

            List<string> lst = new List<string>();

            //  clear list
            lstReport.Items.Clear();

            //  check sql functions
            lst = functions.CheckSqlFunctions(strFunctionsFile, cmbMainDB.Text, sqlConnectionMain);

            //  report from functions
            if (lst.Count != 0)
            {
                for (int j = 0; j < lst.Count; j++)
                { lstReport.Items.Add(lst[j]); }
            }

            intCheck = 0;

            DataTable dtTotal = new DataTable();
            DialogResult dr;

            //if (cmbMainDB.Text == cmbSecondDB.Text & cmbMainTbl.Text == cmbSecondTbl.Text)
            //{
            //    MessageBox.Show("جداول نباید مثل هم باشد", "!خطا");
            //}
            //else
            //{
            #region Data Bases Info

            //  table main record count
            intCount = functions.SqlRecordCount(cmbMainTbl.Text, sqlConnectionMain);

            //  report count
            lstReport.Items.Add("تعداد رکورد جدول از بانک اصلی : " + functions.StrNum(intCount));

            //  table second count
            intCount = functions.SqlRecordCount(cmbSecondTbl.Text, sqlConnectionSecond);

            //  report count
            lstReport.Items.Add("تعداد رکورد جدول از بانک فرعی : " + functions.StrNum(intCount));

            //  cmb main join cmb second and total
            intCount = functions.SqlJoin(cmbMainTbl.Text, cmbSecondTbl.Text, cmbMainClmnJoin.Text, cmbSecndClmnJoin.Text, sqlConnectionMain, cmbMainDB.Text, cmbSecondDB.Text).Rows.Count;

            //  report count
            lstReport.Items.Add("تعداد کل رکورد ها : " + functions.StrNum(intCount));

            //  cmb main join cmb second and filter
            intCount = functions.SqlJoin(cmbMainTbl.Text, cmbSecondTbl.Text, cmbMainClmnJoin.Text, cmbSecndClmnJoin.Text, sqlConnectionMain, cmbMainDB.Text, cmbSecondDB.Text, "").Rows.Count;

            //  report count
            lstReport.Items.Add("تعداد کل رکورد ها از بین دو بانک فوق : " + functions.StrNum(intCount));

            #endregion

            //*****         END

            //  datagridview combobox column source
            functions.ComboBoxSource(clmSecond, functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text));

            //  list column names                
            lstMainClmns.DataSource = functions.DataTableToList(functions.SqlColumns(cmbMainTbl.Text, sqlConnectionMain, cmbMainDB.Text), 1);
            lstSecndClmns.DataSource = functions.DataTableToList(functions.SqlColumns(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text), 1);


            #region Create New Field

            //  new field
            strField = cmbMainClmnUniq.Text + "_vld";
            //  check new field
            intCheck = CheckField(functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond), strField);

            if (intCheck == 0)
            {
                dr = MessageBox.Show(" بسازم ؟" + cmbMainClmnUniq.Text + "_vld آیا فیلد ", "! هشدار", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    //  add new field
                    strFinal = functions.SqlAddNewColumn(cmbSecondTbl.Text, strField, "INT", "NULL", sqlConnectionSecond, cmbSecondDB.Text);

                    if (strFinal.Contains("Done"))
                    {
                        //  report
                        lstReport.Items.Add(strField + " با موفقیت انجام شد ");

                        //  datagridview combobox column source
                        functions.ComboBoxSource(clmSecond, functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond));

                        //  enable for create where
                        bEnableCreateClm = true;
                    }
                    else
                    {
                        //  disable for create where
                        bEnableCreateClm = false;

                        //  report
                        lstReport.Items.Add(" با مشکل مواجه شد " + strField);
                    }

                    //    

                    intCheck = 0;

                    //  check field "Description"
                    intCheck = CheckField(functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond), "Description");

                    if (intCheck == 0)
                    {
                        // add column   
                        strFinal = functions.SqlAddNewColumn(cmbSecondTbl.Text, "Description", "NVARCHAR(MAX)", "NULL", sqlConnectionSecond, cmbSecondDB.Text);

                        if (strFinal.Contains("Done"))
                        {
                            //  report
                            lstReport.Items.Add("Description با موفقیت انجام شد ");

                            //  datagridview combobox column source
                            functions.ComboBoxSource(clmSecond, functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond));
                        }
                        else lstReport.Items.Add(" با مشکل مواجه شد ");
                    }

                }
                else if (dr == DialogResult.No)
                {
                    //  disable for create where
                    bEnableCreateClm = false;

                    lstReport.Items.Add("لغو شد");
                }

            }
            #endregion

            //}

            #region DataGridView
            DataTable dtDgv = new DataTable();

            //  column names
            dtDgv = functions.SqlColumnNames(cmbMainTbl.Text, sqlConnectionMain, "", "فیلد جدول اصلی");


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


            dtDgv = functions.SqlColumns(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text);
            //clmSecond.Items.Clear();
            clmSecond.HeaderText = "فیلد جدول فرعی";
            #endregion


            #region Defualt Value

            //  column names to list
            List<string> lstColumnNames = functions.DataTableToList(functions.SqlColumnNames(cmbSecondTbl.Text, sqlConnectionSecond, cmbSecondDB.Text));
            //  test list
            List<string> lsttest = new List<string>();
            bool bBreak = false;

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
                    else
                    {
                        for (int l = 0; l < 12; l++)
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
                    /*
                 | strArray[j, 0] == lstColumnNames[j].ToString()
                        | strArray[j, 1] == lstColumnNames[j].ToString()
                        | strArray[j, 2] == lstColumnNames[j].ToString()    
                 */
                }
            }

            #endregion

            cmbMainClmnJoin_SelectedIndexChanged(null, null);

            btnChange.Enabled = btnShow.Enabled = true;
            Cursor.Current = Cursors.Default;


            //  check table log
            intCheck = functions.ListFind(functions.SqlTableName(sqlConnectionSecond, cmbSecondDB.Text), "LogFile");

            if (intCheck == -1)
            {
                // create query
                string strQuery = "CREATE TABLE LogFile (ID INT NOT NULL IDENTITY(1,1),[date] DATE NULL,logfile NVARCHAR(max) NOT NULL)";

                //  create table log
                //  run query
                strFinal = functions.SqlExcutCommand(strQuery, sqlConnectionSecond);

                //  report
                if (strFinal.Contains("Done"))
                { lstReport.Items.Add("جدول logfile ساخته شد"); }
                else lstReport.Items.Add("ساختن جدول logfile با مشکل مواجه شد");

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

            //  create query
            main();

            //  richTxt
            richTxt.Text = query2;
            richTxt.WordWrap = true;

            //  datagridview run query2
            dgvSearch.DataSource = functions.SqlDataAdapter(query2, sqlConnectionMain);

            //  report counter
            lstReport.Items.Add("رکورد نتیجه: " + functions.StrNum(dgvSearch.RowCount - 1));
            lstReport.SelectedIndex = lstReport.Items.Count - 1;

            Cursor.Current = Cursors.Default;

        }

        private void btnEslah_Click(object sender, EventArgs e)
        {
            //  wait mode enable
            Cursor.Current = Cursors.WaitCursor;

            //  function filters
            main();

            //  richTxt
            richTxt.Text = query;
            richTxt.WordWrap = true;

            //if (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0) { strDescription += " ,join=" + cmbMainClmnJoin.Text; }
            strDescription += (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0) ? " ,join=" + cmbMainClmnJoin.Text : strDescription;

            //  create query
            query = strUpdate + strDescription + "' FROM " + strJoin + strWhere + " AND  " + txtLbl2.Text + ".Description is NULL SELECT @@ROWCOUNT";

            //  save query
            File.WriteAllText(@"../Command2.txt", query);

            //  report
            lstReport.Items.Add("رکورد نتیجه : " + functions.SqlRunQuery(query, sqlConnectionSecond));

            //  default value
            int ResultCount = 0;

            //  count not used
            ResultCount = functions.SqlCountColumn(cmbSecondTbl.Text, sqlConnectionSecond, "[" + cmbMainClmnUniq.Text + "_vld] is null");

            //  report
            lstReport.Items.Add("رکورد مانده : " + functions.StrNum(ResultCount));
            lstReport.SelectedIndex = lstReport.Items.Count - 1;

            //  wait mode disable
            Cursor.Current = Cursors.Default;
        }

        private void cmbPersentName_SelectedIndexChanged(object sender, EventArgs e)
        {


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
            string strQuery = "";

            if (e.ColumnIndex == 0)
            {
                if (strDescription.IndexOf(cmbMainClmnJoin.Text) <= 0)
                { strDescription += " ,join=" + cmbMainClmnJoin.Text; }

                strQuery = "UPDATE [" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld] = " + dgvSearch.Rows[e.RowIndex].Cells[1].Value.ToString() +
                            " , [Description]=N'" + strDescription + " , آپدیت دستی'" + " WHERE STID =" + dgvSearch.Rows[e.RowIndex].Cells[2].Value.ToString() + " AND [" + cmbMainClmnUniq.Text + "_vld] IS NULL SELECT @@ROWCOUNT";

                lstReport.Items.Add("رکورد نتیجه: " + functions.SqlRunQuery(strQuery, sqlConnectionSecond, cmbSecondDB.Text));

                dgvSearch.Rows.Remove(dgvSearch.Rows[e.RowIndex]);

            }
        }

        private void cmbMainClmnJoin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDgv = new DataTable();

            //  column names
            dtDgv = functions.SqlColumnNames(cmbMainTbl.Text, sqlConnectionMain, cmbMainDB.Text, "فیلد جدول اصلی");

            //  add item to checklistbox
            AddItemCheckList(functions.DataTableToList(dtDgv), Variable.strArray, clbEhraz);

            for (int i = 0; i < 12; i++)
            {
                for (int o = 0; o < 3; o++)
                {
                    string strs = Variable.strArray[i, o];
                    if (Variable.strArray[i, o].ToUpper() == cmbMainClmnJoin.Text.ToUpper())
                    {
                        string strtest = Variable.strArray[i, 2]; clbEhraz.Items.Remove(Variable.strArray[i, 2]);
                    }

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

        private void cmbNameLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNameLike.Text != "")
            {
                for (int i = 0; i < clbEhraz.Items.Count; i++)
                {
                    string sss = clbEhraz.Items[i].ToString();
                    if (clbEhraz.Items[i].ToString() == "نام")
                    {
                        clbEhraz.SetItemChecked(i, true);
                        break;
                    }
                }
            }


        }

        private void cmbFamilyLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilyLike.Text != "")
            {
                for (int i = 0; i < clbEhraz.Items.Count; i++)
                {
                    string sss = clbEhraz.Items[i].ToString();
                    if (clbEhraz.Items[i].ToString() == "نام خانوادگی")
                    {
                        clbEhraz.SetItemChecked(i, true);
                        break;
                    }
                }
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
            for (int i = 0; i < clbEhraz.Items.Count; i++)
            { clbEhraz.SetItemChecked(i, false); }

            switch (intStepOver)
            {
                case 0:
                    clbEhraz.SetItemChecked(intStepOver, true);
                    cmbPersentName.Text = strPersentEvent(intStepOver);
                    break;
                case 1:
                    clbEhraz.SetItemChecked(intStepOver, true);
                    break;
                case 2:
                    clbEhraz.SetItemChecked(intStepOver, true);
                    break;
                case 3:
                    intStepOver = 0;
                    break;
            }

            intStepOver++;
        }

        private void cmbFatherLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFatherLike.Text != "")
            {
                for (int i = 0; i < clbEhraz.Items.Count; i++)
                {
                    string sss = clbEhraz.Items[i].ToString();
                    if (clbEhraz.Items[i].ToString() == "نام پدر")
                    {
                        clbEhraz.SetItemChecked(i, true);
                        break;
                    }
                }
            }
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
            string a, b;

            lstReport.Items.Add("****************************");

            //  description insert to sql
            strDescription = "";

            strWhere = "";

            //  a is tablename main & b is tablename second
            a = txtLbl1.Text; b = txtLbl2.Text;

            //  create update query
            strUpdate = "Update [" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld]=" + a + "." + cmbMainClmnUniq.Text + " , [Description]=N'";

            //  report
            lstReport.Items.Add("احراز هویت بر اساس ");

            //  checked counter
            int clbEhrazCnt = clbEhraz.CheckedItems.Count;

            //  clear variables
            strWhere = strSelect = strJoin = strSlt = "";

            Cursor.Current = Cursors.WaitCursor;

            //  Naming Table
            string comMain = "[" + cmbMainDB.Text + "].dbo.[" + cmbMainTbl.Text + "] " + a;
            string com2 = "[" + cmbSecondDB.Text + "].dbo.[" + cmbSecondTbl.Text + "] " + b;

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
                        strGroup += (strGroup == "") ? " [" + a + "].[" + strCell1 + "] " : ",[" + a + "].[" + strCell1 + "] ";
                        #endregion

                        #region اطلاعات فیلد از جدول فرعی

                        if (dgv.Rows[l].Cells[2].Value != null)
                        {
                            strCell2 = dgv.Rows[l].Cells[2].Value.ToString();

                            if (dgv.Rows[l].Cells[3].Value != null) { strLbl2 = dgv.Rows[l].Cells[3].Value.ToString(); }
                            else strLbl2 = dgv.Rows[l].Cells[2].Value.ToString();
                            strSecond = "[" + b + "].[" + strCell2 + "] " + b + "_" + strLbl2;
                            strGroup += (strGroup == "") ? "[" + b + "].[" + strCell2 + "] " : ",[" + b + "].[" + strCell2 + "] ";
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


            #region Create Select Query & string Where & Join & Group by

            strSelect = "SELECT [" + txtLbl1.Text + "].[" + cmbMainClmnJoin.Text + "] " + txtLbl1.Text + "_" + cmbMainClmnJoin.Text +
                ", [" + txtLbl2.Text + "].[" + cmbSecndClmnJoin.Text + "] " + txtLbl2.Text + "_" + cmbSecndClmnJoin.Text + "," + strSlt + " FROM ";

            if (strSlt == "")
            {
                strSelect = "SELECT [" + txtLbl1.Text + "].[" + cmbMainClmnJoin.Text + "] " + txtLbl1.Text + "_" + cmbMainClmnJoin.Text +
                                ", [" + txtLbl2.Text + "].[" + cmbSecndClmnJoin.Text + "] " + txtLbl2.Text + "_" + cmbSecndClmnJoin.Text + " FROM ";
            }

            strJoin = comMain + " JOIN " + com2 + " ON " + DeleteFreeSpace(a + ".[" + cmbMainClmnJoin.Text + "]") + " = " + DeleteFreeSpace(b + ".[" + cmbSecndClmnJoin.Text + "]");

            //  group by
            strGroup = " GROUP BY [" + txtLbl1.Text + "].[" + cmbMainClmnJoin.Text + "], [" + txtLbl2.Text + "].[" + cmbSecndClmnJoin.Text + "] " + strGroup;

            //**********************

            //  checked from checklistbox
            foreach (int intChecked in clbEhraz.CheckedIndices)
            {

                string strChecked = clbEhraz.Items[intChecked].ToString();

                switch (strChecked)
                {
                    #region Name -> varchar
                    case "نام":

                        //  create where for query
                        strWhere = Selection("Name", cmbPersentName, cmbNameLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription("نام ", cmbPersentName, cmbNameLike.Text);
                        lstReport.Items.Add(PersentDescription(" نام ", cmbPersentName, cmbNameLike.Text));

                        break;
                    #endregion

                    #region Family -> varchar
                    case "نام خانوادگی":

                        //  create where for query
                        strWhere = Selection("Family", cmbPersentFamily, cmbFamilyLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription(" نام خانوادگی ", cmbPersentFamily, cmbFamilyLike.Text);
                        lstReport.Items.Add(PersentDescription(" نام خانوادگی ", cmbPersentFamily, cmbFamilyLike.Text));

                        break;
                    #endregion

                    #region Father -> varchar
                    case "نام پدر":

                        //  create where for query
                        strWhere = Selection("Father", cmbPersentFather, cmbFatherLike.Text, strWhere, a, b);

                        //  report
                        strDescription += PersentDescription(" نام پدر ", cmbPersentFather, cmbFatherLike.Text);
                        lstReport.Items.Add(PersentDescription(" نام پدر ", cmbPersentFather, cmbFatherLike.Text));

                        break;
                    #endregion

                    #region ShenasCode -> int
                    case "شماره شناسنامه":

                        //  create where for query
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null"; }
                        else strWhere += " Where " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null";

                        strDescription += " شماره شناسنامه ";
                        lstReport.Items.Add(" شماره شناسنامه ");

                        break;
                    #endregion

                    #region PBirthDate -> smallint                        
                    case "سال تولد":
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null "; }
                        else strWhere += " Where " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null ";

                        strDescription += " سال تولد ";
                        lstReport.Items.Add(" سال تولد ");
                        break;
                    #endregion

                    #region CodeMelli -> char(10)
                    case "کد ملی":
                        if (strWhere != "")
                        { strWhere += " AND " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null "; }
                        else strWhere += " Where " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null ";

                        strDescription += " کد ملی ";
                        lstReport.Items.Add(" کد ملی ");

                        break;
                    #endregion

                    #region HomeCity -> varchar
                    case "شهر محل تولد":
                        if (strWhere != "")
                        { strWhere += " and Replace(" + a + ".HomeCity,' ','')=Replace(" + b + ".HomeCity,' ','')"; }
                        else strWhere += " Where Replace(" + a + ".HomeCity,' ','')=Replace(" + b + ".HomeCity,' ','')";

                        strDescription += " شهر محل تولد ";
                        lstReport.Items.Add(" شهر محل تولد ");

                        break;
                    #endregion

                    #region SodorCity -> varchar
                    case "شهر محل صدور":
                        if (strWhere != "")
                        { strWhere += " and Replace(" + a + ".SodorCity,' ','')=Replace(" + b + ".SodorCity,' ','')"; }
                        else strWhere += " Where Replace(" + a + ".SodorCity,' ','')=Replace(" + b + ".SodorCity,' ','')";

                        strDescription += " شهر محل صدور ";
                        lstReport.Items.Add(" شهر محل صدور ");

                        break;
                    #endregion

                    #region SodorOstan -> varchar
                    case "استان محل تولد":
                        break;
                    #endregion

                    #region HomeOstan -> varchar
                    case "استان محل صدور":
                        break;
                    #endregion

                    #region Code Modiriat                        
                    case "کد مرکز مدیریت":
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null "; }
                        else strWhere += " Where " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null ";

                        strDescription += " کد مرکز مدیریت ";
                        lstReport.Items.Add(" کد مرکز مدیریت ");

                        break;
                    #endregion

                    #region Code Khadamat                        
                    case "کد مرکز خدمات":
                        if (strWhere != "")
                        { strWhere += " AND " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null "; }
                        else strWhere += " Where " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null ";

                        strDescription += " کد مرکز خدمات ";
                        lstReport.Items.Add(" کد مرکز خدمات ");

                        break;
                    #endregion

                }
            }

            //if (bEnableCreateClm == true)
            //{
            if (chbUniq.CheckState == CheckState.Checked)
            {
                if (strWhere != "")
                {
                    strWhere += " AND " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";
                }
                else strWhere += " Where " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";
            }
            else
            {
                if (strWhere != "")
                {
                    strWhere += " AND " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NOT NULL ";
                }
                else strWhere += " Where " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NOT NULL ";
            }
            //}


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
                //if (dtInput.Rows[i][0].ToString() == strSearchField) { intReturn = 1; }
                intReturn = (dtInput.Rows[i][0].ToString() == strSearchField) ? 1 : intReturn;
            }
            return intReturn;
        }

        private string DeleteFreeSpace(string strItem)
        { return "Replace(" + strItem + ", ' ', '')"; }

        private void AddItemCheckList(List<string> lstItem, string[,] strArr, CheckedListBox clbEhraz)
        {
            clbEhraz.Items.Clear();
            for (int i = 0; i < lstItem.Count; i++)
            {
                for (int o = 0; o < 12; o++)
                {
                    for (int j = 0; j < 3; j++)
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
        #endregion
    }
}


