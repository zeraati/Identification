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

        Dictionary<int, string> dicDBName = new Dictionary<int, string>();
        SqlConnection sqlConnection = new SqlConnection();

        string query, query2, strJoin, strWhere, strSelect, strUpdate, TotalCount;
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

            this.Text = "احراز هویت اشخاص" + "  -  نام سرور = " + sqlConnection.DataSource;

            //  disable button
            btnChange.Enabled = btnShow.Enabled = false;

            //  load database name source
            cmbMainDB.DataSource = Functions.SqlGetDBName(sqlConnection);
            cmbSecndDB.DataSource = Functions.SqlGetDBName(sqlConnection);

            //  defult value
            cmbMainDB.Text = cmbSecndDB.Text = "Ehraz";
        }

        private void cmbDBNameMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnection);

            //  load table name
            cmbMainTbl.DataSource = Functions.SqlTableName(sqlConnection);

            //  default value            
            cmbMainTbl.Text = "TBL_Student_95-02-21";
        }

        private void cmbDBName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  change data base name
            sqlConnection = Functions.SqlConnectionChangeDB(cmbSecndDB.Text, sqlConnection);

            //  load table name
            cmbSecndTbl.DataSource = Functions.SqlTableName(sqlConnection);

            //  default value
            cmbSecndTbl.Text = "TBL_MKsarparast_95-02-21";
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

            string strPathFile = @"..\Functions";
            string strTextFile = "", strFinal;
            string[] files = Directory.GetFiles(strPathFile);

            lstReport.Items.Clear();

            foreach (string file in files)
            {
                //  read function files
                if (Functions.SqlCountColumn("master.sys.objects", sqlConnection, "type_desc='SQL_SCALAR_FUNCTION' AND name='" + Path.GetFileNameWithoutExtension(file) + "'") == 0)
                {
                    //  read text query from path file
                    strTextFile = File.ReadAllText(file, UTF8Encoding.UTF8);
                    //  run query
                    Functions.SqlExcutCommand(strTextFile, sqlConnection);
                }
            }

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
                intCount = Functions.SqlTableRecordsCount(cmbMainTbl.Text, sqlConnection);

                //  report count
                lstReport.Items.Add("تعداد رکورد جدول از بانک اصلی : " + Functions.StrNum(intCount));

                //  table second count
                intCount = Functions.SqlTableRecordsCount(cmbSecndTbl.Text, sqlConnection);

                //  report count
                lstReport.Items.Add("تعداد رکورد جدول از بانک فرعی : " + Functions.StrNum(intCount));

                //  cmb main join cmb second
                intCount = Functions.SqlJoin(cmbMainTbl.Text, cmbSecndTbl.Text, cmbMainClmnUniq.Text, sqlConnection).Rows.Count;

                //  report count
                lstReport.Items.Add("تعداد کل رکورد ها : " + Functions.StrNum(intCount));
                #endregion

                //*****         END

                //  datagridview combobox column source
                Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection));

                //  list column names                
                lstMainClmns.DataSource = Functions.DataTableToList(Functions.SqlColumns(cmbMainTbl.Text, sqlConnection));
                lstSecndClmns.DataSource = Functions.DataTableToList(Functions.SqlColumns(cmbSecndTbl.Text, sqlConnection));


                #region Create New Field

                //  new field
                strField = cmbMainClmnUniq.Text + "_vld";
                //  check new field
                intCheck = CheckField(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection), strField);

                if (intCheck == 0)
                {
                    dr = MessageBox.Show(" بسازم ؟" + cmbMainClmnUniq.Text + "_vld آیا فیلد ", "! هشدار", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        //  add new field
                        strFinal = Functions.SqlAddNewColumn(cmbSecndTbl.Text, strField, "INT", "NULL", sqlConnection);

                        if (strFinal.Contains("Done"))
                        {
                            lstReport.Items.Add(strField + "با موفقیت انجام شد");

                            //  datagridview combobox column source
                            Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection));
                        }
                        else lstReport.Items.Add("با مشکل مواجه شد " + strField);

                        //    

                        intCheck = 0;

                        //  check field "Desription"
                        intCheck = CheckField(Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection), "Desription");

                        if (intCheck == 0)
                        {
                            // add column   
                            strFinal = Functions.SqlAddNewColumn(cmbSecndTbl.Text, "Desription", "NVARCHAR(MAX)", "NULL", sqlConnection, cmbSecndDB.Text);

                            if (strFinal.Contains("Done"))
                            {
                                lstReport.Items.Add("با موفقیت انجام شد Desription");

                                //  datagridview combobox column source
                                Functions.ComboBoxSource(clmSecond, Functions.SqlColumnNames(cmbSecndTbl.Text, sqlConnection));
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

            #region New DataGridView
            DataTable dtddgv = new DataTable();
            dtddgv = Functions.SqlColumnNames(cmbMainTbl.Text, sqlConnection);
            dgv.DataSource = dtddgv;
            for (int j = 0; j < dtddgv.Rows.Count; j++)
            {
                dgv.Rows[j].Cells[1].Value = j.ToString();
                dgv.Rows[j].Cells[0].Value = false;
            }

            //**********    advanced search column

            //**********

            dtddgv = Functions.SqlColumns(cmbSecndTbl.Text, sqlConnection);
            //clmSecond.Items.Clear();
            clmSecond.HeaderText = "فیلد فرعی";
            for (int i = 0; i < dtddgv.Rows.Count; i++)
            {
                //clmSecond.Items.Add(dtddgv.Rows[i][0].ToString());
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

        private void جستجوToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find frmFind = new Find();
            frmFind.ShowDialog();
        }

        private void اصلاحToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void بررسیToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void تغییراتصالبهبانکگToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //**********************************************************
        #endregion



        private void btnDisplay_Click(object sender, EventArgs e)
        {

            //btnCheck_Click(null, null);
            main();
            //lstReport.Items.Add(strDescription);

            sqlConnection = Functions.SqlConnectionChangeDB(cmbMainDB.Text, sqlConnection);

            dgvSearch.DataSource = Functions.SqlDataAdapter(query2, sqlConnection);


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

        private void dgvMainClmns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Boolean chk;
            //chk = Convert.ToBoolean(dgvMainClmns.Rows[e.RowIndex].Cells[0].Value);
            //if (chk == true)
            //{
            //    dgvMainClmns.Rows[e.RowIndex].Cells[0].Value = false;
            //}
            //else dgvMainClmns.Rows[e.RowIndex].Cells[0].Value = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string strMaster = " master.dbo.ReplaceAB(", a = "", b = "";
            textBox1.Text = "";
            listBox1.Items.Clear();
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

                        strCell1 = dgv.Rows[l].Cells[4].Value.ToString();
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
                        else { if (strSecond == "")strSlt += "," + strMain; else strSlt += "," + strMain + "," + strSecond; }
                    }

                }
            }
            else MessageBox.Show("خطا");
            //**********************
            //textBox1.Text = "SELECT " + strSlt + " FROM " + comMain + " JOIN " + com2 + " ON " + strMaster + " " + a + ".[" + cmbMainClmnJoin.Text + "])=" + strMaster + " " + b + ".[" + cmbSecndClmnJoin.Text + "])";
        }

        private void cmbNameLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNameLike.Text != "") clbEhraz.SetItemChecked(0, true);
        }

        private void cmbFamilyLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilyLike.Text != "") clbEhraz.SetItemChecked(1, true);
        }

        private void cmbFatherLike_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFatherLike.Text != "") clbEhraz.SetItemChecked(2, true);
        }



        //*****************************
        //*****     Functions   *******
        public void main()
        {
            string strMaster, a, b;

            strMaster = " master.dbo.ReplaceAB(";
            lstReport.Items.Add("*****************");

            strDescription = "";
            strWhere = "";

            a = txtLbl1.Text; b = txtLbl2.Text;

            strUpdate = "Update [" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] SET [" + cmbMainClmnUniq.Text + "_vld]=" + a + "." + cmbMainClmnUniq.Text + " , [Desription]=N'";

            lstReport.Items.Add("احراز هویت بر اساس ");

            int clbEhrazCnt = clbEhraz.CheckedItems.Count;

            strWhere = strSelect = strJoin = "";
            Cursor.Current = Cursors.WaitCursor;
            textBox1.Text = "";
            strSlt = "";
            string comMain = "[" + cmbMainDB.Text + "].dbo.[" + cmbMainTbl.Text + "] " + a;
            string com2 = "[" + cmbSecndDB.Text + "].dbo.[" + cmbSecndTbl.Text + "] " + b;

            if (b != "" & a != "")
            {
                for (int l = 0; l < dgv.Rows.Count; l++)
                {

                    if (Convert.ToBoolean(dgv.Rows[l].Cells[0].Value) != false)
                    {
                        #region اطلاعات فیلد از جدول اصلی

                        strCell1 = dgv.Rows[l].Cells[4].Value.ToString();
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
                        else { if (strSecond == "")strSlt += "," + strMain; else strSlt += "," + strMain + "," + strSecond; }

                        #endregion
                    }
                }
            }
            else MessageBox.Show("خطا");


            strSelect = "SELECT " + strSlt + " FROM ";
            strJoin = comMain + " JOIN " + com2 + " ON " + strMaster + " " + a + ".[" + cmbMainClmnJoin.Text + "])=" + strMaster + " " + b + ".[" + cmbSecndClmnJoin.Text + "])";

            //**********************
            if (cmbFamilyLike.Text != "" | cmbNameLike.Text != "" | cmbFatherLike.Text != "")
            {
                #region Union

                foreach (int action in clbEhraz.CheckedIndices)
                {
                    switch (action)
                    {
                        case 0:

                            if (cmbPersentName.Text == "100" & cmbNameLike.Text != "") // cmbNameLike
                            {
                                //      خالی - سید - میر - اله
                                strSlt = Functions.SelectAll("Name", cmbPersentName.Text, cmbNameLike.Text, strSelect + strJoin, a, b);
                                strDescription += " نام 100 درصد و " + cmbNameLike.Text;
                                lstReport.Items.Add(" نام 100 درصد و " + cmbNameLike.Text);
                            }
                            break;
                        case 1:
                            if (cmbPersentFamily.Text == "100" & cmbFamilyLike.Text != "") // cmbFamilyLike
                            {
                                //      خالی - سید - میر - اله
                                strSlt = Functions.SelectAll("Family", cmbPersentFamily.Text, cmbFamilyLike.Text, strSelect + strJoin, a, b);
                                strDescription += " نام خانوادگی 100 درصد و " + cmbFamilyLike.Text;
                                lstReport.Items.Add(" نام خانوادگی 100 درصد و " + cmbFamilyLike.Text);
                                if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            }
                            break;
                        case 2:
                            if (cmbPersentFather.Text == "100" & cmbFatherLike.Text != "") // cmbFatherLike
                            {
                                //      خالی - سید - میر - اله
                                strSlt = Functions.SelectAll("Father", cmbPersentFather.Text, cmbFatherLike.Text, strSelect + strJoin, a, b);
                                strDescription += " نام پدر 100 درصد و " + cmbFatherLike.Text; lstReport.Items.Add(" نام پدر 100 درصد و " + cmbFatherLike.Text);
                                if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            }
                            break;

                    }
                }
                #endregion

                query2 = strSlt;
            }
            else
            {
                #region Not Union

                #region Case
                foreach (int action in clbEhraz.CheckedIndices)
                {
                    switch (action)
                    {
                        case 0:
                            if (cmbPersentName.Text == "100" & cmbNameLike.Text == "")
                            {
                                strWhere = Functions.clbEhrazChecked("Name", cmbPersentName.Text, cmbNameLike.Text, strWhere, a, b);
                                strDescription += " نام 100 درصد "; lstReport.Items.Add("نام 100 درصد ");
                                if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            }
                            clbEhrazCnt--;
                            break;
                        case 1:
                            if (cmbPersentFamily.Text == "100" & cmbFamilyLike.Text == "")
                            {
                                strWhere = Functions.clbEhrazChecked("Family", cmbPersentFamily.Text, cmbFamilyLike.Text, strWhere, a, b);
                                strDescription += " نام خانوادگی 100 درصد ";
                                lstReport.Items.Add("نام خانوادگی 100 درصد");
                                if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            }
                            clbEhrazCnt--;
                            break;
                        case 2:
                            if (cmbPersentFather.Text == "100" & cmbFatherLike.Text == "")
                            {
                                strWhere = Functions.clbEhrazChecked("Father", cmbPersentFather.Text, cmbFatherLike.Text, strWhere, a, b);
                                strDescription += " نام پدر 100 درصد "; lstReport.Items.Add("نام پدر 100 درصد");
                                if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            }
                            clbEhrazCnt--;
                            break;
                        case 3:
                            if (strWhere != "")
                            {
                                strWhere += " AND " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null";
                            }
                            else strWhere += " Where " + b + ".ShenasCode=" + a + ".ShenasCode AND " + b + ".ShenasCode is not null";
                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " شناسنامه ";
                            lstReport.Items.Add(" شناسنامه ");
                            clbEhrazCnt--;
                            break;
                        case 4:
                            if (strWhere != "")
                            {
                                strWhere += " AND " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null ";
                            }
                            else strWhere += " Where " + b + ".PBirthDate=" + a + ".PBirthDate AND " + b + ".PBirthDate is not null ";

                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " سال تولد ";
                            lstReport.Items.Add(" سال تولد ");
                            clbEhrazCnt--;
                            break;
                        case 5:
                            if (strWhere != "")
                            {
                                strWhere += " AND " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null ";
                            }
                            else strWhere += " Where " + a + ".CodeMelli=" + b + ".CodeMelli AND " + b + ".CodeMelli is not null ";
                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " کد ملی ";
                            lstReport.Items.Add(" کد ملی ");
                            clbEhrazCnt--;
                            break;
                        //شهر محل تولد
                        case 6:
                            if (strWhere != "")
                            {
                                strWhere += " and " + strMaster + a + ".HomeCity)=" + strMaster + b + ".HomeCity)";
                            }
                            else strWhere += " Where " + strMaster + a + ".HomeCity)=" + strMaster + b + ".HomeCity)";
                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " شهر محل تولد ";
                            lstReport.Items.Add(" شهر محل تولد ");
                            clbEhrazCnt--;
                            break;
                        //شهر محل صدور
                        case 7:
                            if (strWhere != "")
                            {
                                strWhere += " and " + strMaster + a + ".SodorCity)=" + strMaster + b + ".SodorCity)";
                            }
                            else strWhere += " Where " + strMaster + a + ".SodorCity)=" + strMaster + b + ".SodorCity)";
                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " شهر محل تولد ";
                            lstReport.Items.Add(" شهر محل تولد ");
                            clbEhrazCnt--;
                            break;
                        // استان محل تولد
                        case 8:
                            break;
                        // استان محل صدور
                        case 9:
                            break;
                        case 12:
                            if (strWhere != "")
                            {
                                strWhere += " AND " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null ";
                            }
                            else strWhere += " Where " + b + ".STID=" + a + ".STID AND " + b + ".STID is not null ";

                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " کد مرکز مدیریت ";
                            lstReport.Items.Add("کد مرکز مدیریت");
                            clbEhrazCnt--;
                            break;
                        case 13:
                            if (strWhere != "")
                            {
                                strWhere += " AND " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null ";
                            }
                            else strWhere += " Where " + b + ".MKID=" + a + ".MKID AND " + b + ".MKID is not null ";
                            if (clbEhrazCnt > 0) { query += " AND "; query2 += " AND "; }
                            strDescription += " کد مرکز خدمات ";
                            lstReport.Items.Add("کد مرکز خدمات");
                            clbEhrazCnt--;
                            break;
                    }
                }
                #endregion  //  End Case

                #endregion

                if (strWhere != "")
                {
                    strWhere += " AND " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";
                }
                else strWhere += " Where " + b + ".[" + cmbMainClmnUniq.Text + "_vld] IS NULL ";

                query2 = strSelect + strJoin + strWhere;

                File.WriteAllText(@"../Command.txt", query2);
            }


        }







        //***           function new

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

        //*******************************************
    }
}


