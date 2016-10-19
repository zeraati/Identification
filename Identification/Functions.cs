using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace Identification
{

    public class Variable
    {
        #region Enable & Disable from item clbEhraz
        public static string[,] strArray =
            {
            {"Name","FirstName","نام", },
            { "Family","LastName","نام خانوادگی"},
            {"Father","FatherName","نام پدر"},
            {"ShenasCode","ShNo","شماره شناسنامه" },
            { "PBirthDate","BirthDate","سال تولد" },
            { "CodeMelli","MelliCode","کد ملی" },
            { "HomeCity","Cityname","شهر محل تولد" },
            { "SodorCity","","شهر محل صدور" },
            { "HomeOstan","OstanName","استان محل تولد" },
            { "SodorOstan","Sodor","استان محل صدور" },
            { "STID","CodeMarkazModiriat","کد مرکز مدیریت" },
            { "MKID","CodeMarkazKhadamat","کد مرکز خدمات" }
            };

        #endregion
    }

    class Functions
    {

        #region Create File Text Not Exists

        public string CreateFile(string strFilePath)
        {
            string strReturn = "";

            if (File.Exists(strFilePath))
            {
                strReturn = "OK";
            }
            else
            {
                File.CreateText(strFilePath);
                strReturn = "Create";
            }
            return strReturn;
        }

        #endregion


        #region LoadColumn==> In CheckedListBox & ComboBox

        /// <summary>
        /// اضافه کردن نام ستون ها به چک لیست
        /// </summary>
        /// <param name="DBName">نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو لیست نام جداول</param>
        /// <param name="ChkLstBx">چک لیست باکس</param>
        public void LoadColumnInfo(string strTableName, CheckedListBox ChkLstBx, SqlConnection sqlConnection)
        {
            ChkLstBx.Items.Clear();
            List<string> lstColumns = DataTypeToList(SqlColumns(strTableName, sqlConnection, ""));
            for (int i = 0; i < lstColumns.Count; i++)
            {
                ChkLstBx.Items.Add(lstColumns[i]);
            }
        }

        //******************************
        #endregion LoadColumn==> In CheckedListBox & ComboBox


        public List<string> LoadSvrName(string PathLogin)
        {
            #region load server names

            //  load server info
            List<string> lst = new List<string>();
            //  return server names
            List<string> lst2 = new List<string>();

            //  load server names into combobox
            if (File.Exists(PathLogin))
            {
                lst = readTxt(PathLogin);

                //  add server local
                lst2.Add(".");

                //  get server from read line
                for (int i = 0; i < lst.Count; i++)
                { lst2.Add(lst[i].Substring(0, lst[i].IndexOf(","))); }

            }

            #endregion

            // return server info
            return lst2;
        }


        #region save list to text
        public void saveList(List<string> lst, string Path)
        {
            File.WriteAllText(Path, lst[0]);
            for (int i = 1; i < lst.Count; i++)
            {
                File.WriteAllText(Path, File.ReadAllText(Path) + "\r\n" + lst[i]);
            }
        }
        #endregion



        #region listFind
        /// <summary>
        /// جستجو عبارت از لیست
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="match">عبارت جستجو</param>
        /// <returns>ایندکس</returns>
        public int listFind(List<string> lst, string match)
        {
            int intRtn = -1;

            //  find math
            for (int i = 0; i < lst.Count; i++)
                if (lst[i].Contains(match)) intRtn = i;


            return intRtn;
        }

        #endregion

        public List<string> readTxt(string PathLogin)
        {

            //read text file
            FileStream fileStream = new FileStream(PathLogin, FileMode.Open, FileAccess.Read);


            // read line & add to list
            List<string> lst = new List<string>();
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string strline;

                while ((strline = streamReader.ReadLine()) != null)
                { lst.Add(strline); }
            }
            return lst;
        }

        public void ReadText(string strpath, ComboBox cmb)
        {
            cmb.Items.Clear();
            string[] lines;
            string server;
            var list = new List<string>();
            var fileStream = new FileStream(strpath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    server = line.Substring(0, line.IndexOf(","));
                    cmb.Items.Add(server);
                }
            }
            lines = list.ToArray();
        }


        #region List to Combobox
        public void ListToCmb(List<string> lst, ComboBox cmb)
        {
            for (int i = 0; i < lst.Count; i++)
            { cmb.Items.Add(lst[i]); }
        }
        #endregion


        #region LoadColumn==> In CheckListBox For DBName

        public void ChlbDB(SqlConnection sqlCon, CheckedListBox ChhLstBx)
        {
            ChhLstBx.Items.Clear();
            List<string> lst = SqlGetDBName(sqlCon);

            for (int i = 0; i < lst.Count; i++)
            {
                ChhLstBx.Items.Add(lst[i]);
            }
        }
        #endregion

        #region Select & Unselect
        /// <summary>
        /// انتخاب و حذف انتخاب همه رکوردها
        /// </summary>
        /// <param name="ChkLstBx">چک لیست</param>
        /// <param name="btnSelect">نام دکمه</param>
        public void SelectUnselect(CheckedListBox ChkLstBx, Button btnSelect)
        {
            if (btnSelect.Text == "انتخاب همه")
            {
                for (int i = 0; i < ChkLstBx.Items.Count; i++)
                { ChkLstBx.SetItemChecked(i, true); }
                btnSelect.Text = "بدون انتخاب";
            }
            else
            {
                for (int i = 0; i < ChkLstBx.Items.Count; i++)
                { ChkLstBx.SetItemChecked(i, false); }
                btnSelect.Text = "انتخاب همه";
            }
        }
        #endregion Select & Unselect

        /// <summary>
        /// چک کردن فیلد مورد نظر آیا در جدول موجود هست؟
        /// </summary>
        /// <param name="dtCK"></param>
        /// <param name="txtCK">عبارت جستجو شونده</param>
        /// <returns>reti</returns>
        public int CheckField(DataTable dtCK, string txtCK1)
        {

            int retInt = 0;
            for (int i = 0; i < dtCK.Rows.Count; i++)
            {
                if (dtCK.Rows[i][0].ToString().ToUpper() == txtCK1.ToUpper())
                {
                    retInt++;
                }
            }
            return retInt;
        }




        public string StrNum(int Number) { string ret = string.Format("{0:n0}", Number); return ret; }

        public int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        #region functions

        //**************    functions


        #region CheckSQLFunctions

        public List<string> CheckSqlFunctions(string strFilePath, string strDataBaseName, SqlConnection sqlConnection)
        {
            List<string> lstRetrun = new List<string>();
            List<string> lstSql = new List<string>();

            bool bol = false;

            string strQuery = "SELECT name FROM [" + strDataBaseName + "].sys.objects WHERE type='FN'";

            //  list sql functions of source
            lstSql = DataTableToList(SqlDataAdapter(strQuery, sqlConnection, strDataBaseName));

            //  function file
            string[] files = Directory.GetFiles(strFilePath, "*.txt");

            foreach (string file in files)
            {
                bol = false;


                //  check functions in sql & file
                for (int i = 0; i < lstSql.Count; i++)
                {
                    if (Path.GetFileNameWithoutExtension(file) == lstSql[i])
                    { bol = true; break; }
                }

                //  create function is not sql
                if (bol == false) { SqlExcutCommand(File.ReadAllText(file), sqlConnection); lstRetrun.Add("Create Function Is Sql : " + Path.GetFileNameWithoutExtension(file)); }
            }
            return lstRetrun;
        }
        #endregion


        #region DataTypeToList
        public List<string> DataTypeToList(DataTable dt)
        {
            List<string> lst = new List<string>();

            //  convert data to list
            for (int i = 0; i < dt.Rows.Count; i++)
            { lst.Add(dt.Rows[i][1].ToString() + " [ " + dt.Rows[i][2].ToString() + " - " + dt.Rows[i][3].ToString() + dt.Rows[i][4].ToString() + "]"); }

            return lst;
        }
        #endregion

        #region DataTableToList

        public List<string> DataTableToList(DataTable dt, int intIndexColumn = 0)
        {
            List<string> lst = new List<string>();

            //  convert data to list
            for (int i = 0; i < dt.Rows.Count; i++)
            { lst.Add(dt.Rows[i][intIndexColumn].ToString()); }

            return lst;
        }
        #endregion


        #region ComboBoxSource
        /// <summary>
        /// set combobox source
        /// </summary>
        /// <param name="ComboBox">combobox name</param>
        /// <param name="Table">table name</param>
        /// <param name="ColumnIndex">index of column show in combobox</param>
        /// <param name="State">show in problem massage</param>
        public void ComboBoxSource(ComboBox ComboBox, DataTable Table, int ColumnIndex = 0, string State = "State")
        {
            try
            {
                ComboBox.BindingContext = new BindingContext();
                ComboBox.DataSource = Table;
                ComboBox.ValueMember = Table.Columns[ColumnIndex].ColumnName;
            }

            catch (Exception e)
            {
                if (State != "State")
                    MessageBox.Show(State + Environment.NewLine + e.Message);
            }
        }
        public void ComboBoxSource(DataGridViewComboBoxColumn dgvcmbColumn, DataTable Table, int ColumnIndex = 0, string State = "State")
        {
            try
            {
                dgvcmbColumn.DataSource = Table;
                dgvcmbColumn.DisplayMember = Table.Columns[ColumnIndex].ColumnName;
                dgvcmbColumn.ValueMember = Table.Columns[ColumnIndex].ColumnName;
            }

            catch (Exception e)
            {
                if (State != "State")
                    MessageBox.Show(State + Environment.NewLine + e.Message);
            }
        }

        #endregion


        #region ListFind
        /// <summary>
        /// جستجو عبارت از لیست
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="match">عبارت جستجو</param>
        /// <returns>ایندکس</returns>
        public int ListFind(List<string> lst, string match)
        {
            int intRtn = -1;

            //  find math
            for (int i = 0; i < lst.Count; i++)
                if (lst[i].Contains(match)) intRtn = i;


            return intRtn;
        }

        #endregion


        #region SaveListToText
        /// <summary>
        /// ذخیره اطلاعات لاگین از لیست
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="Path">مسیر فایل</param>
        public void SaveListToText(List<string> lst, string Path)
        {
            File.WriteAllText(Path, lst[0]);
            for (int i = 1; i < lst.Count; i++)
            {
                File.WriteAllText(Path, File.ReadAllText(Path) + "\r\n" + lst[i]);
            }
        }
        #endregion


        #region CheckedListBoxSource


        public void CheckedListBoxSource(List<string> lstSource, CheckedListBox checkedListBox)
        {
            // clear list box
            checkedListBox.Items.Clear();

            // set list box source
            for (int i = 0; i < lstSource.Count; i++) checkedListBox.Items.Add(lstSource[i]);

        }

        #endregion


        #region GetQueryName
        /// <summary>
        ///  read querys name
        /// </summary>
        /// <param name="queryPath"></param>
        /// <param name="type"></param>
        /// <returns>list</returns>
        public List<string> GetQueryNameList(string strQueryPath, string strType)
        {
            List<string> lst = new List<string>();


            //  get all query file name
            string[] qryFileName = Directory.GetFiles(strQueryPath, "*.sql");


            //  file name with type
            foreach (string fileName in qryFileName)
            {
                //  type field
                if (strType != "جدول" && fileName.Contains("جدول") == false)
                { lst.Add(Path.GetFileNameWithoutExtension(fileName)); }

                //  type table
                if (strType == "جدول" && fileName.Contains("جدول"))
                { lst.Add(Path.GetFileNameWithoutExtension(fileName)); }
            }

            return lst;
        }

        public DataTable GetQueryNameeDatatable(string strQueryPath, string strType)
        {
            DataTable dt = new DataTable();
            string strFindTbName = "";

            dt.Columns.Add("FileQuery", typeof(string));
            dt.Columns.Add("TbName", typeof(string));

            //  get all query file name
            string[] qryFileName = Directory.GetFiles(strQueryPath, "*.sql");


            //  file name with type
            foreach (string fileName in qryFileName)
            {

                //  type field
                if (strType != "جدول" && fileName.Contains("جدول") == false)
                { dt.Rows.Add(Path.GetFileNameWithoutExtension(fileName)); }

                //  type table
                if (strType == "جدول" && fileName.Contains("جدول"))
                {
                    string strQry = File.ReadAllText(fileName);

                    //  tbl name index
                    int i = strQry.ToUpper().IndexOf("INTO [") + 6, b = strQry.ToUpper().IndexOf("] FROM") - i;

                    //  get table name
                    if (b > 0) strFindTbName = strQry.Substring(i, b);

                    //  add query file name
                    dt.Rows.Add(Path.GetFileNameWithoutExtension(fileName), strFindTbName);
                }
            }

            return dt;
        }

        public string GetQueryNameString(string strQuery, string strTbNmNew)
        {

            //  return query when substring
            string strQryRtn = "";
            int intStart = 0, intLen = 0;

            //  tbl name index
            if (strQryRtn == "" & strQuery.Contains("INTO ["))
            {
                intStart = strQuery.ToUpper().IndexOf("INTO [") + 6;
                intLen = strQuery.ToUpper().IndexOf("] FROM") - intStart;
                strQryRtn = strQuery.Substring(intStart, intLen);
            }

            if (strQryRtn == "" & strQuery.Contains("ALTER TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("ALTER TABLE [") + 13;
                intLen = strQuery.ToUpper().IndexOf("] ADD") - intStart;
                strQryRtn = strQuery.Substring(intStart, intLen);
            }

            if (strQryRtn == "" & strQuery.Contains("DROP TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("DROP TABLE [") + 12;
                intLen = strQuery.Length - (intStart + 1);

                strQryRtn = strQuery.Substring(intStart, intLen);
            }




            //  tb name new
            strQryRtn = strQuery.Substring(intStart, intLen);

            //  create query new
            strQryRtn = strQuery.Replace(strQryRtn, strTbNmNew);

            return strQryRtn;

        }

        #endregion


        #region TableNameOfQuery
        public string TableNameOfQuery(string strQuery)
        {
            string strName = "";
            int intStart = 0, intLen = 0;

            //  tbl name index
            if (strName == "" & strQuery.Contains("INTO ["))
            {
                intStart = strQuery.ToUpper().IndexOf("INTO [") + 6;
                intLen = strQuery.ToUpper().IndexOf("] FROM") - intStart;
                strName = strQuery.Substring(intStart, intLen);
            }

            if (strName == "" & strQuery.Contains("ALTER TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("ALTER TABLE [") + 13;
                intLen = strQuery.ToUpper().IndexOf("] ADD") - intStart;
                strName = strQuery.Substring(intStart, intLen);
            }
            if (strName == "" & strQuery.Contains("DROP TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("DROP TABLE [") + 12;
                intLen = strQuery.Length - (intStart + 1);

                strName = strQuery.Substring(intStart, intLen);
            }



            //  return tb name

            return strName = strQuery.Substring(intStart, intLen); ;
        }
        #endregion


        #region ChangeTableName

        //  change table name
        public DataTable ChangeTableName(DataGridView dgvQrys, DataTable dtSource)
        {

            DataTable dt = new DataTable();

            //  add columns
            dt.Columns.Add("QueryName", typeof(string));
            dt.Columns.Add("TbNameOld", typeof(string));
            dt.Columns.Add("TbNameNew", typeof(string));


            //  get row checked
            for (int i = 0; i < dgvQrys.Rows.Count; i++)
            {
                //  checked item
                if (Convert.ToBoolean(dgvQrys.Rows[i].Cells[0].Value) == true)
                {
                    //  add row
                    string ss = dtSource.Rows[i][1].ToString();           //  old table name 
                    dt.Rows.Add(
                                dgvQrys.Rows[i].Cells[1].Value.ToString(),//  query name
                                dtSource.Rows[i][1].ToString(),           //  old table name 
                                dgvQrys.Rows[i].Cells[2].Value.ToString() //  new table name
                              );

                }

            }


            return dt;
        }
        #endregion


        #region ServerUserPass
        /// <summary>
        /// serverUserPass
        /// </summary>
        /// <param name="loginPath">مسیر فایل اطلاعات ورود به سرور</param>
        /// <param name="strServer">سرور</param>
        /// <returns>لیست شامل نام سرور ، نام کاربری و رمز عبور سرور</returns>
        public List<string> ServerUserPass(string loginPath, string strServer)
        {
            //  return list 
            List<string> lst = new List<string>();
            lst.Add(strServer); // add server



            //  all servers login info
            List<string> lstLogin = new List<string>();
            lstLogin = ReadTxt(loginPath);


            //  get this server user & pass add to lst
            for (int i = 0; i < lstLogin.Count; i++)
            {
                //  find this server info
                if (lstLogin[i].Contains(strServer))
                {
                    //  finde index of separator
                    List<int> lstIndexOf = new List<int>();
                    lstIndexOf = IndexOfAll(lstLogin[i], ",");

                    //  add user & pass to lst
                    lst.Add(lstLogin[i].Substring(lstIndexOf[0], (lstIndexOf[1] - lstIndexOf[0]) - 1));
                    lst.Add(lstLogin[i].Substring(lstIndexOf[1], lstLogin[i].Length - lstIndexOf[1]));
                }
            }


            //  set user & pass when server is local
            if (strServer == ".") lst[1] = lst[2] = "";

            //  decrypt pass
            try { lst[2] = CryptorEngine.Decrypt(lst[2], true); }
            catch (Exception) { }


            return lst;
        }

        #endregion


        #region ListServerName
        /// <summary>
        /// get server name from file
        /// </summary>
        /// <param name="strPathLogin">login file path</param>
        /// <returns>List</returns>
        public List<string> ListServerName(string strPathLogin)
        {
            List<string> lstSvrName = new List<string>();

            //  load server names into cmbServer
            if (File.Exists(strPathLogin))
            {
                lstSvrName = ReadTxt(strPathLogin);

                lstSvrName.Insert(0, "., ,");

                //  get server from read line
                for (int i = 0; i < lstSvrName.Count; i++)
                { lstSvrName[i] = lstSvrName[i].Substring(0, lstSvrName[i].IndexOf(",")); }

            }

            return lstSvrName;
        }
        #endregion


        #region GetListBoxSelectedItems

        public List<string> GetListBoxSelectedItemsText(ListBox listBox)
        {
            List<string> lstSelectedItems = new List<string>();

            foreach (var item in listBox.SelectedItems)
                lstSelectedItems.Add(listBox.GetItemText(item));

            return lstSelectedItems;
        }


        #endregion


        #region ListBoxSelectAllWithRollBack

        List<int> lstSelectedIndex = new List<int>();

        private void ListBoxSelectAllWithRollBack(bool bolCheckAll, ListBox listBox)
        {

            //  select all
            if (bolCheckAll == true)
            {
                //  for roll back
                foreach (var item in listBox.SelectedItems)
                { lstSelectedIndex.Add(listBox.Items.IndexOf(item)); }

                //  select all
                for (int i = 0; i < listBox.Items.Count; i++)
                { listBox.SetSelected(i, true); }
            }



            // select roll back
            else
            {
                //  unselect all
                for (int i = 0; i < listBox.Items.Count; i++)
                    listBox.SetSelected(i, false);

                //  roll back
                for (int i = 0; i < lstSelectedIndex.Count; i++)
                    listBox.SetSelected(lstSelectedIndex[i], true);

                // clear roll back items
                lstSelectedIndex.Clear();
            }
        }

        #endregion


        #region GetSelectedItemsText
        public List<string> GetSelectedItemsText(ListBox listBox)
        {
            int intSelectedIndex = 0;
            string strSelectedText = "";
            List<string> lstSelectedItemsText = new List<string>();


            foreach (var item in listBox.SelectedItems)
            {
                // index of select
                intSelectedIndex = listBox.Items.IndexOf(item);

                // text of select
                strSelectedText = listBox.GetItemText(listBox.Items[intSelectedIndex]);

                //  add selected text to list
                lstSelectedItemsText.Add(strSelectedText);
            }

            return lstSelectedItemsText;
        }
        #endregion


        #region ListToString

        public string ListToString(List<string> lst, string strSeperator, string strAddToRow = "", bool bolQuery = false, string strTable = "")
        {
            string strListRows = "";

            //  normal string
            for (int i = 0; i < lst.Count; i++)
            {
                //  just exist one row
                if (lst.Count == 1)
                { strListRows = strAddToRow + lst[0]; break; }



                //  multi row
                // first row
                if (i == 0)
                { strListRows = strAddToRow + lst[0] + strSeperator; }

                //  between first & last row
                if (i > 0 && i < lst.Count - 1)
                { strListRows = strListRows + strAddToRow + lst[i] + strSeperator; }

                //  last row
                if (i > 0 && i == lst.Count - 1)
                { strListRows = strListRows + strAddToRow + lst[i]; }
            }


            // query string
            if (bolQuery)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst.Count == 1)
                    { strListRows = strAddToRow + ".[" + lst[0] + "]"; break; }

                    if (i == 0)
                    { strListRows = strAddToRow + ".[" + lst[0] + "]" + strSeperator; }

                    if (i > 0 && i < lst.Count - 1)
                    { strListRows = strListRows + strAddToRow + ".[" + lst[i] + "]" + strSeperator; }

                    if (i > 0 && i == lst.Count - 1)
                    { strListRows = strListRows + strAddToRow + ".[" + lst[i] + "]"; }
                }
            }


            //  qury string with alies
            if (bolQuery == true && strTable != "")
            {


                for (int i = 0; i < lst.Count; i++)
                {
                    //  set alies
                    strTable = "] [" + lst[i] + "_" + strTable + "]";

                    if (lst.Count == 1)
                    { strListRows = strAddToRow + ".[" + lst[0] + strTable; break; }

                    if (i == 0)
                    { strListRows = strAddToRow + ".[" + lst[0] + strTable + strSeperator; }

                    if (i > 0 && i < lst.Count - 1)
                    { strListRows = strListRows + strAddToRow + ".[" + lst[i] + strTable + strSeperator; }

                    if (i > 0 && i == lst.Count - 1)
                    { strListRows = strListRows + strAddToRow + ".[" + lst[i] + strTable; }
                }
            }


            return strListRows;
        }

        #endregion


        #region TbNameOfQuery
        public string TbNmQry(string strQuery)
        {
            string strName = "";
            int intStart = 0, intLen = 0;

            //  tbl name index
            if (strName == "" & strQuery.Contains("INTO ["))
            {
                intStart = strQuery.ToUpper().IndexOf("INTO [") + 6;
                intLen = strQuery.ToUpper().IndexOf("] FROM") - intStart;
                strName = strQuery.Substring(intStart, intLen);
            }

            if (strName == "" & strQuery.Contains("ALTER TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("ALTER TABLE [") + 13;
                intLen = strQuery.ToUpper().IndexOf("] ADD") - intStart;
                strName = strQuery.Substring(intStart, intLen);
            }
            if (strName == "" & strQuery.Contains("DROP TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("DROP TABLE [") + 12;
                intLen = strQuery.Length - (intStart + 1);

                strName = strQuery.Substring(intStart, intLen);
            }



            //  return tb name

            return strName = strQuery.Substring(intStart, intLen); ;
        }
        #endregion


        #region ChangeTbName

        //  change table name
        public DataTable ChangeTbName(DataGridView dgvQrys, DataTable dtSource)
        {

            DataTable dt = new DataTable();

            //  add columns
            dt.Columns.Add("QueryName", typeof(string));
            dt.Columns.Add("TbNameOld", typeof(string));
            dt.Columns.Add("TbNameNew", typeof(string));


            //  get row checked
            for (int i = 0; i < dgvQrys.Rows.Count; i++)
            {
                //  checked item
                if (Convert.ToBoolean(dgvQrys.Rows[i].Cells[0].Value) == true)
                {
                    //  add row
                    string ss = dtSource.Rows[i][1].ToString();           //  old table name 
                    dt.Rows.Add(
                                dgvQrys.Rows[i].Cells[1].Value.ToString(),//  query name
                                dtSource.Rows[i][1].ToString(),           //  old table name 
                                dgvQrys.Rows[i].Cells[2].Value.ToString() //  new table name
                              );

                }

            }


            return dt;
        }
        #endregion


        #region GetQueryFileName

        public DataTable GetQueryFileName(string strQueryPath, string strType)
        {
            DataTable dtReturn = new DataTable();

            dtReturn.Columns.Add("QueryFileName", typeof(string));
            dtReturn.Columns.Add("IntoTableName", typeof(string));


            //  get all query file name
            string[] qryFileName = Directory.GetFiles(strQueryPath, "*.sql");


            //  file name with type
            foreach (string fileName in qryFileName)
            {

                //  type field
                if (strType != "جدول" && fileName.Contains("جدول") == false)
                { dtReturn.Rows.Add(Path.GetFileNameWithoutExtension(fileName)); }


                //  type table
                if (strType == "جدول" && fileName.Contains("جدول"))
                {
                    //  get table name from query
                    string strTableName = SqlQueryGetTableName(File.ReadAllText(fileName));

                    //  add query file name & table name
                    dtReturn.Rows.Add(Path.GetFileNameWithoutExtension(fileName), strTableName);
                }
            }

            return dtReturn;
        }

        #endregion

        #region ReadTxt
        /// <summary>
        /// read text file
        /// </summary>
        /// <param name="Path">مسیر فایل متنی</param>
        /// <returns></returns>
        public List<string> ReadTxt(string Path)
        {

            //read text file
            FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);


            // read line & add to list
            List<string> lst = new List<string>();
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string strline;

                while ((strline = streamReader.ReadLine()) != null)
                { lst.Add(strline); }
            }


            //  return
            return lst;
        }

        #endregion


        #region IndexOfAll

        /// <summary>
        /// IndexOfAll
        /// </summary>
        /// <param name="txt">متن اصلی</param>
        /// <param name="search">عبارت جستجو</param>
        /// <returns></returns>
        public List<int> IndexOfAll(string txt, string search)
        {
            List<int> lst = new List<int>();

            for (int i = txt.IndexOf(search); i > -1; i = txt.IndexOf(search, i + 1))
            { lst.Add(i + 1); }

            return lst;
        }
        #endregion


        //*********     helper


        #region GetQryName
        /// <summary>
        ///  read querys name
        /// </summary>
        /// <param name="queryPath"></param>
        /// <param name="type"></param>
        /// <returns>list</returns>
        public List<string> GetQryNameList(string strQueryPath, string strType)
        {
            List<string> lst = new List<string>();


            //  get all query file name
            string[] qryFileName = Directory.GetFiles(strQueryPath, "*.sql");


            //  file name with type
            foreach (string fileName in qryFileName)
            {
                //  type field
                if (strType != "جدول" && fileName.Contains("جدول") == false)
                { lst.Add(Path.GetFileNameWithoutExtension(fileName)); }

                //  type table
                if (strType == "جدول" && fileName.Contains("جدول"))
                { lst.Add(Path.GetFileNameWithoutExtension(fileName)); }
            }

            return lst;
        }

        public DataTable GetQryNameDt(string strQueryPath, string strType)
        {
            DataTable dt = new DataTable();
            string strFindTbName = "";

            dt.Columns.Add("FileQuery", typeof(string));
            dt.Columns.Add("TbName", typeof(string));

            //  get all query file name
            string[] qryFileName = Directory.GetFiles(strQueryPath, "*.sql");


            //  file name with type
            foreach (string fileName in qryFileName)
            {

                //  type field
                if (strType != "جدول" && fileName.Contains("جدول") == false)
                { dt.Rows.Add(Path.GetFileNameWithoutExtension(fileName)); }

                //  type table
                if (strType == "جدول" && fileName.Contains("جدول"))
                {
                    string strQry = File.ReadAllText(fileName);

                    //  tbl name index
                    int i = strQry.ToUpper().IndexOf("INTO [") + 6, b = strQry.ToUpper().IndexOf("] FROM") - i;

                    //  get table name
                    if (b > 0) strFindTbName = strQry.Substring(i, b);

                    //  add query file name
                    dt.Rows.Add(Path.GetFileNameWithoutExtension(fileName), strFindTbName);
                }
            }

            return dt;
        }

        public string GetQryStr(string strQuery, string strTbNmNew)
        {

            //  return query when substring
            string strQryRtn = "";
            int intStart = 0, intLen = 0;

            //  tbl name index
            if (strQryRtn == "" & strQuery.Contains("INTO ["))
            {
                intStart = strQuery.ToUpper().IndexOf("INTO [") + 6;
                intLen = strQuery.ToUpper().IndexOf("] FROM") - intStart;
                strQryRtn = strQuery.Substring(intStart, intLen);
            }

            if (strQryRtn == "" & strQuery.Contains("ALTER TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("ALTER TABLE [") + 13;
                intLen = strQuery.ToUpper().IndexOf("] ADD") - intStart;
                strQryRtn = strQuery.Substring(intStart, intLen);
            }

            if (strQryRtn == "" & strQuery.Contains("DROP TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("DROP TABLE [") + 12;
                intLen = strQuery.Length - (intStart + 1);

                strQryRtn = strQuery.Substring(intStart, intLen);
            }




            //  tb name new
            strQryRtn = strQuery.Substring(intStart, intLen);

            //  create query new
            strQryRtn = strQuery.Replace(strQryRtn, strTbNmNew);

            return strQryRtn;

        }

        #endregion


        #endregion


        //****************************************************************
        #region sql functions
        //****************      sql functions

        #region Sql Update Character Save
        public string SqlUpdateCharacter(string strTableName, string strColumnName, int intStartIndex, int intLength, SqlConnection sqlConnection, string strState = "")
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "] = SUBSTRING([" + strColumnName + "]," + intStartIndex + "," + intLength + ") WHERE [" + strColumnName + "] IS NOT NULL ";

            return SqlExcutCommand(strQuery, sqlConnection, strState);
        }
        #endregion

        #region Sql Return Function Name



        #endregion


        #region Date

        #region Sql Update Persion To Milady

        public string SqlUpdatePersionToMilady(string strTableName, string strColumnMilady, string strColumnPersion, SqlConnection sqlConnection)
        {
            string strQuery = "Update dbo.[" + strTableName + "] SET [" + strColumnMilady + "] = " +
                "CAST(dbo.UDF_Julian_To_Gregorian(dbo.UDF_Persian_To_Julian(CAST(RIGHT([" + strColumnPersion + "],4) AS INT),CONVERT(INT,SUBSTRING([" + strColumnPersion + "],4,2)),CAST(LEFT([" + strColumnPersion + "],2)AS INT))) AS DATE)" +
                " where [" + strColumnMilady + "] is null";
            return SqlExcutCommand(strQuery, sqlConnection, "Sql Update Persion To Milady");
        }

        #endregion

        #region Sql Update Milady To Persion

        public string SqlUpdateMiladyToPersion(string strTableName, string strColumnMilady, string strColumnPersion, SqlConnection sqlConnection)
        {
            string strQuery = "Update dbo.[" + strTableName + "] SET [" + strColumnPersion + "] = dbo.UDF_Gregorian_To_Persian([" + strColumnMilady + "]) where [" + strColumnPersion + "] is null";
            return SqlExcutCommand(strQuery, sqlConnection, "Sql Update Milady To Persion ");
        }

        #endregion

        #endregion


        #region Sql MaxLen Column Data
        public string SqlMaxLenColumnData(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT ISNULL(MAX(LEN([" + strColumnName + "])),0) FROM dbo.[" + strTableName + "]";
            return SqlRunQuery(strQuery, sqlConnection);
        }
        #endregion


        #region Sql Edit DataType Column
        public string SqlEditDataTypeColumn(string strTableName, string strColumnName, string strNewDataType, string strNulable, SqlConnection sqlConnection, string strDBName = "")
        {
            string strQuery = "";
            SqlConnection sqlConn = new SqlConnection();
            sqlConn = sqlConnection;

            if (strDBName == sqlConnection.Database)
            {
                strQuery = "ALTER TABLE dbo.[" + strTableName + "] ALTER COLUMN [" + strColumnName + "] " + strNewDataType + " " + strNulable;
            }
            else
            {
                sqlConn = SqlConnectionChangeDB(strDBName, sqlConnection);
                strQuery = "ALTER TABLE dbo.[" + strTableName + "] ALTER COLUMN [" + strColumnName + "] " + strNewDataType + " " + strNulable;
            }
            return SqlExcutCommand(strQuery, sqlConn);
        }

        #endregion


        #region SqlRunQuery

        public string SqlRunQuery(string strQuery, SqlConnection sqlConnection, string strDBName = "")
        {
            //  change sqlconnection new database name
            if (strDBName != "")
            { sqlConnection = SqlConnectionChangeDB(strDBName, sqlConnection); }

            SqlCommand sqlCmd = new SqlCommand(strQuery, sqlConnection);
            string strExecute;
            sqlCmd.Connection.Close();
            sqlCmd.Connection.Open();
            sqlCmd.CommandTimeout = 3600;
            strExecute = sqlCmd.ExecuteScalar().ToString();
            sqlCmd.Connection.Close();
            return strExecute;
        }

        #endregion


        #region SqlEditTableName
        public string SqlEditTableName(string strTableName, string strNewTableName, SqlConnection sqlConnection, bool bolExtended = false)
        {
            string strQuery;
            if (bolExtended == true)
            {
                strQuery = @"EXEC [" + sqlConnection.Database + "].sys.sp_addextendedproperty @name = N'Title',@value = N'" + strTableName + "'," +
                    "@level0type = N'SCHEMA',@level0name = N'dbo',@level1type = N'TABLE',@level1name = N'" + strTableName + "'";
                SqlExcutCommand(strQuery, sqlConnection);
            }
            strQuery = "EXEC [" + sqlConnection.Database + "].sys.sp_rename @objname = N'" + strTableName + "',@newname = N'" + strNewTableName + "'";
            return SqlExcutCommand(strQuery, sqlConnection, " SqlEditTableName ");
        }
        #endregion


        #region SqlCopyTable
        public string SqlCopyTable(string strTableName, string strCopyTableName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT * INTO dbo.[" + strCopyTableName + "] FROM dbo.[" + strTableName + "]";

            return SqlExcutCommand(strQuery, sqlConnection, " CopyTable [" + strCopyTableName + "]");

        }
        #endregion


        #region SqlDropTable
        public string SqlDropTable(string strTableName, SqlConnection sqlConnection, string strState = "")
        {
            string strQry = "DROP TABLE [" + strTableName + "]";

            string strResult = SqlExcutCommand(strQry, sqlConnection, strState + "DropTable");

            return strTableName + " => " + strResult;
        }
        #endregion


        #region SqlAddNewColumn

        public string SqlAddNewColumn(string strTableName, string strColumnName, string strDataType, string strNulable, SqlConnection sqlConnection, string strDBName = "")
        {
            string strQuery = "";


            if (strDBName != "")
            { sqlConnection = SqlConnectionChangeDB(strDBName, sqlConnection); }

            strQuery = "ALTER TABLE dbo.[" + strTableName + "] ADD [" + strColumnName + "] " + strDataType.ToUpper() + " " + strNulable;

            return SqlExcutCommand(strQuery, sqlConnection, "AddNewField");
        }

        public string SqlAddNewColumn(string strTableName, string strColumnName, string strDataType, SqlConnection sqlConnection, int intSeed = 1, int intStart = 1, string strDBName = "")
        {
            string strQuery = "";
            SqlConnection sqlConn = new SqlConnection();
            sqlConn = sqlConnection;

            if (strDBName == sqlConnection.Database)
            {
                strQuery = "ALTER TABLE dbo.[" + strTableName + "] ADD [" + strColumnName + "] " + strDataType + " IDENTITY(" + intStart + "," + intSeed + ") PRIMARY KEY ";
            }
            else
            {
                sqlConn = SqlConnectionChangeDB(strDBName, sqlConnection);
                strQuery = "ALTER TABLE dbo.[" + strTableName + "] ADD [" + strColumnName + "] " + strDataType + " IDENTITY(" + intStart + "," + intSeed + ") PRIMARY KEY ";
            }
            return SqlExcutCommand(strQuery, sqlConnection, "AddNewField");
        }

        #endregion


        #region SqlRename
        public string SqlRename(string strType, string strOldName, string strNewName, SqlConnection sqlConnection, string strTable = "")
        {
            string strQuery = "";

            // renam table
            if (strType.ToUpper().Trim() == "TABLE")
            { strQuery = "EXEC sp_rename '" + strOldName + "', '" + strNewName + "'"; }

            //  rename column
            if (strType.ToUpper().Trim() == "COLUMN" && strTable != "")
            { strQuery = "EXEC sp_rename '" + strTable + "." + strOldName + "', '" + strNewName + "' , 'COLUMN'"; }


            return SqlExcutCommand(strQuery, sqlConnection, "SqlRename => " + strOldName + " => " + strNewName);
        }
        #endregion


        #region SqlCopyColumn
        public string SqlCopyColumn(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            DataTable dtClmInfo = SqlColumns(strTableName, sqlConnection, strColumnName);

            //  check data type - string & not string
            if (dtClmInfo.Rows[0][3].ToString() == "")

            //  add new field
            { SqlAddNewColumn(strTableName, strColumnName + "_Copy", dtClmInfo.Rows[0][3].ToString(), dtClmInfo.Rows[0][2].ToString(), sqlConnection); }
            else
            { SqlAddNewColumn(strTableName, strColumnName + "_Copy", dtClmInfo.Rows[0][3].ToString() + dtClmInfo.Rows[0][4].ToString(), dtClmInfo.Rows[0][2].ToString(), sqlConnection); }

            //  copy column
            return SqlUpdateColumnData(strTableName, strColumnName + "_Copy", strColumnName, sqlConnection, "CopyColumn");

        }
        #endregion


        #region SqlUpdateColumn

        public string SqlUpdateColumnData(string strTableName, string strColumnName, string strData, SqlConnection sqlConnection)
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]= " + strData;
            return SqlExcutCommand(strQuery, sqlConnection, "UpdateColumn");
        }
        public string SqlUpdateColumnData(string strTableName, string strColumnName, string strColumnData, SqlConnection sqlConnection, string strState = "")
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]=[" + strColumnData + "]";
            return SqlExcutCommand(strQuery, sqlConnection, strState);
        }
        public string SqlUpdateColumnData(string strTableName, string strColumnName, string strColumnData, SqlConnection sqlConnection, string strWhereQuery = "", string strState = "")
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]=" + strColumnData + " Where " + strWhereQuery;
            return SqlExcutCommand(strQuery, sqlConnection, strState);
        }
        public string SqlUpdateColumnData(string strTableName, string strColumnName, string strColumnData, SqlConnection sqlConnection, string strWhereColumn = "", string strWhere = "", string strState = "")
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]=" + strColumnData + " Where " + strWhere;
            return SqlExcutCommand(strQuery, sqlConnection, strState);
        }


        #endregion


        #region SqlCreateReport

        public string SqlCreateReport(SqlConnection sqlConnection)
        {
            string strQuery = "CREATE TABLE [" + sqlConnection.Database + "].[dbo].[___Report](" +
             "[Id] [INT] IDENTITY(1,1) NOT NULL," +
              "[Tables] [VARCHAR](255) NULL," +
              "[Relation 1] [VARCHAR](255) NULL," +
              "[Relation 2] [VARCHAR](255) NULL," +
              "[Description] [VARCHAR](255) NULL," +
              "[TedadRecord] [INT] NULL," +
              "[TedadField] [INT] NULL," +
              "[ShenasNameBank] [VARCHAR](255) NULL," +
              "CONSTRAINT [PK ___Report] PRIMARY KEY CLUSTERED ([Id] ASC" +
              ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," +
              "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]";

            return SqlExcutCommand(strQuery, sqlConnection);
        }

        #endregion


        #region SqlCountColumn
        public int SqlCountColumn(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT COUNT([" + strColumnName + "]) FROM dbo.[" + strTableName + "]";

            return SqlExecuteScalar(strQuery, sqlConnection);
        }

        public int SqlCountColumn(string strTableName, SqlConnection sqlConnection, string strWhere = "")
        {
            string strQuery = "SELECT COUNT(*) FROM dbo.[" + strTableName + "] WHERE " + strWhere;

            return SqlExecuteScalar(strQuery, sqlConnection);
        }

        public int SqlCountColumnKey(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT COUNT(*) FROM (SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME=N'" + strTableName + "' AND COLUMN_NAME=N'" + strColumnName + "') K";

            return SqlExecuteScalar(strQuery, sqlConnection, "Column Key");
        }

        #endregion


        #region SqlDropColumnSpace
        public string SqlDropColumnSpace(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            string strReturn = "";

            int intCount = SqlCountColumn(strTableName, strColumnName, sqlConnection);

            if (intCount == 0)
            {
                strReturn = SqlDropColumn(strTableName, strColumnName, sqlConnection);
            }
            return strReturn;
        }

        #endregion


        #region SqlDropTableSpace
        public string SqlDropTableSpace(string strTableName, SqlConnection sqlConnection)
        {
            int intCount = 0;
            string strReturn = "";

            intCount = SqlRecordCount(strTableName, sqlConnection);

            if (intCount == 0) { strReturn = SqlDropTable(strTableName, sqlConnection); }

            return strReturn;
        }
        #endregion


        #region SqlDropRows
        public string SqlDropRows(string strTableName, SqlConnection sqlConnection, string strWhere = "")
        {
            string strQuery = "";

            if (strWhere == "")
            { strQuery = "DELETE FROM " + strTableName; }
            else strQuery = "DELETE FROM " + strTableName + " WHERE " + strWhere;
            return SqlExcutCommand(strQuery, sqlConnection, " DropRows ");
        }
        #endregion


        #region SqlEditColumn
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strOldColumn"></param>
        /// <param name="strNewColumn"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="str">Field Or DataType</param>
        /// <returns></returns>
        public string SqlEditColumn(string strTableName, string strOldColumn, SqlConnection sqlConnection, string strNewColumn = "", string strDataType = "", string strLen = "")
        {
            string strQuery = "";
            int intSelect = 0;

            if (strDataType == "")
            {
                //  edit field name
                strQuery = "[" + sqlConnection.Database + "].sys.sp_rename @objname = N'" + strTableName + "." + strOldColumn + "', @newname = N'" + strNewColumn.Trim() + "'";
            }
            else
            {
                //  edit field data type
                if (strLen == "")
                {
                    if (strDataType.Contains("float"))
                    {
                        string[] strArrType = { "bit", "tinyint", "smallint", "int", "bigint", "char(200)", "nchar(200)", "varchar(max)", "nvarchar(max)", "text", "ntext", "date", "datetime", "real", "float" };

                        while (SqlExcutCommand(strQuery, sqlConnection, "EditField") == " => Error!")
                        {

                            strQuery = "Alter Table [" + strTableName + "] Alter Column [" + strOldColumn + "] " + strArrType[intSelect];

                            intSelect++;

                        }

                    }
                }
                else
                {
                    strQuery = "Alter Table [" + strTableName + "] Alter Column [" + strOldColumn + "] ";
                }

            }

            return "Done!";
        }

        public string SqlEditColumn(string strTableName, string strOldColumn, string strNewColumn, string strDataType, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "", strFinal = "", strReport = "", strFinal2 = "";
            //  connection change database name
            if (strDbName != "") { sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection); }

            strQuery = "[" + sqlConnection.Database + "].sys.sp_rename @objname = N'" + strTableName + "." + strOldColumn + "', @newname = N'" + strNewColumn.Trim() + "'";

            strFinal = SqlExcutCommand(strQuery, sqlConnection, "Rename Column => " + strOldColumn);
            if (strFinal.Contains("Done"))
            {
                //  change data type
                strQuery = "Alter Table [" + strTableName + "] Alter Column [" + strNewColumn + "] " + strDataType;

                //  sql command
                strFinal2 = SqlExcutCommand(strQuery, sqlConnection, "=> Datatype Column => " + strOldColumn);

                if (strFinal2.Contains("Done"))
                {
                    strReport = strFinal + strFinal2;
                }
            }
            else strReport = "Error !";

            return strReport;
        }

        #endregion


        #region SqlConnection
        public SqlConnection SqlConnect(string server = ".", string user = "", string pass = "", string dbName = "")
        {
            string strconn, strdbName;


            //  set data base name
            if (dbName == "") { strdbName = "master"; } else { strdbName = dbName; }


            // create connection string
            if (user != "" && pass != "")
            { strconn = "Data Source=" + server + ";Initial Catalog=" + strdbName + ";Persist Security Info=True;User ID=" + user + ";Password=" + pass; }

            else strconn = "Data Source=.;Initial Catalog=" + strdbName + ";Integrated Security=True;Connect Timeout=5";


            SqlConnection sqlConnection = new SqlConnection(strconn);
            return sqlConnection;
        }
        #endregion


        #region SqlExecuteScalar

        public int SqlExecuteScalar(string strQuery, SqlConnection sqlConnection, string strState = "")
        {
            SqlCommand cmd = new SqlCommand(strQuery, sqlConnection);
            int intCount;
            cmd.Connection.Close();
            cmd.Connection.Open();
            cmd.CommandTimeout = 3600;
            intCount = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();
            return intCount;
        }

        #endregion


        #region SqlExcutCommand
        public string SqlExcutCommand(string strQuery, SqlConnection sqlConnection, string strState = "")
        {

            SqlCommand cmd = new SqlCommand(strQuery, sqlConnection);
            try
            {
                // conection timeoute
                cmd.CommandTimeout = 3600;

                //  open conection
                if (cmd.Connection.State == ConnectionState.Closed)
                { cmd.Connection.Open(); }

                // execute query
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return strState + " => Done!";
            }

            catch (Exception) { return strState + " => Error!"; }
        }

        #endregion


        #region SqlExcutCommandWithGO
        public List<string> SqlExcutCommandWithGO(SqlConnection sqlConnection, string strQuery, string strState = "")
        {
            //  for substring
            int intStart, intLenght;

            List<string> lstQry = new List<string>();
            List<string> lstResult = new List<string>();


            // index of all GO in query
            List<int> lstIndexGo = IndexOfAll(strQuery, "GO");


            //  create evry query
            for (int i = 0; i < lstIndexGo.Count; i++)
            {

                //  first GO
                if (i == 0)
                { intLenght = lstIndexGo[0] - 1; lstQry.Add(strQuery.Substring(0, intLenght)); }


                //  midel Go    // GO between first & last 
                if (lstIndexGo.Count - 1 > i)
                {
                    intStart = lstIndexGo[i] + 2; intLenght = (lstIndexGo[i + 1] - 1) - (lstIndexGo[i] + 2);
                    lstQry.Add(strQuery.Substring(intStart, intLenght));
                }


                //  last GO
                if (lstIndexGo.Count - 1 == i)
                {
                    intStart = lstIndexGo[i] + 2; intLenght = strQuery.Length - intStart;
                    lstQry.Add(strQuery.Substring(intStart, intLenght));
                }

            }


            // run evry query & set result
            for (int i = 0; i < lstQry.Count; i++)
            { lstResult.Add(SqlExcutCommand(lstQry[i].ToString(), sqlConnection, "lstQry[" + i.ToString() + "]")); }


            return lstResult;
        }
        #endregion


        #region SqlGetDBName
        /// <summary>
        /// بر روی سیستم SQL لیستی شامل تمام پایگاه  داده های
        /// </summary>
        /// <returns>DataTable</returns>

        public List<string> SqlGetDBName(SqlConnection sqlConnection, string strLabel = "")
        {
            List<string> lstReturn = new List<string>();
            string strQuery = "SELECT name FROM sys.databases ";

            if (strLabel != "")
            { strQuery = "SELECT name [" + strLabel + "] FROM sys.databases "; }

            DataTable dtAdapter = SqlDataAdapter(strQuery, sqlConnection, "");

            for (int i = 0; i < dtAdapter.Rows.Count; i++)
            { lstReturn.Add(dtAdapter.Rows[i][0].ToString()); }

            return lstReturn;
        }

        #endregion


        #region SqlDataAdapter

        public DataTable SqlDataAdapter(string strQuery, SqlConnection sqlConnection, string strTable = "", string strNumberTop = "")
        {
            DataTable dt = new DataTable();


            if (strNumberTop != "" && strTable != "")
            { strQuery = "SELECT TOP 100 * FROM [" + strTable + "]"; }

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(strQuery, sqlConnection);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            { return dt; }
        }


        #endregion


        #region SqlTableName

        public List<string> SqlTableName(SqlConnection sqlConnection, string strDbName = "", string strLabel = "")
        {
            string strQuery = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

            if (strLabel != "")
            { strQuery = "SELECT TABLE_NAME [" + strLabel + "] FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"; }

            List<string> lstReturn = new List<string>();
            DataTable dtAdapter = SqlDataAdapter(strQuery, sqlConnection, strDbName);

            for (int i = 0; i < dtAdapter.Rows.Count; i++)
            {
                lstReturn.Add(dtAdapter.Rows[i][0].ToString());
            }
            return lstReturn;
        }

        #endregion


        #region SqlColumnsInfo

        /// <summary>
        /// name , nullable , type , len
        /// list of columns name
        /// </summary>
        /// <param name="sqlConnection">SqlConnection</param>
        /// <param name="strTableName">TableName</param>
        /// <returns>data table</returns>
        public DataTable SqlColumns(string strTableName, SqlConnection sqlConnection, string strDbName = "")
        {
            string Query = "SELECT  ORDINAL ," +
                           "Name ," +
                           " [Null] ," +
                           " [Type] , [Len]" +
                           " FROM    ( 	" +
                                        " SELECT ORDINAL_POSITION ORDINAL , COLUMN_NAME Name," +
                                        " CASE  WHEN IS_NULLABLE = 'YES' THEN 'Null' ELSE 'Not Null' END [Null],DATA_TYPE [Type]," +
                                        " CASE  WHEN CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' ('+CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(50))+')' END AS [Len]" +
                                        " FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "'" +
                                    " ) Columns;";
            //  change database name
            if (strDbName != "") sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection);

            return SqlDataAdapter(Query, sqlConnection, strDbName);
        }

        public DataTable SqlColumns(string strTableName, SqlConnection sqlConnection, string strDbName = "", string strColumnName = "")
        {
            string Query = "SELECT  Name ," +
                           " [Null] ," +
                           " [Type] , [Len]" +
                           " FROM    ( 	" +
                                        " SELECT COLUMN_NAME Name," +
                                        " CASE  WHEN IS_NULLABLE = 'YES' THEN 'Null' ELSE 'Not Null' END [Null],DATA_TYPE [Type]," +
                                        " CASE  WHEN CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' ('+CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(50))+')' END AS [Len]" +
                                        " FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "' AND COLUMN_NAME=N'" + strColumnName + "'" +
            " ) Columns;";

            //  change database name
            if (strDbName != "") sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection);

            return SqlDataAdapter(Query, sqlConnection, strDbName);
        }

        public DataTable SqlColumnNames(string strTableName, SqlConnection sqlConnection, string strDbName = "", string strLable = "")
        {
            string strQuery = " SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "'";

            if (strLable != "")
            { strQuery = " SELECT COLUMN_NAME [" + strLable + "] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "'"; }

            return SqlDataAdapter(strQuery, sqlConnection, strDbName);

        }


        #endregion


        #region SqlTableRecordsCount

        public int SqlRecordCount(string strTableName, SqlConnection sqlConnection, string strColumnName = "", string strNull = "NULL", string strLabel = "")
        {
            int count;
            strNull = (strNull != "NULL") ? "WHERE [" + strColumnName + "] IS NOT NULL" : "WHERE [" + strColumnName + "] IS NULL";

            //  query
            string strQuery = "SELECT COUNT(*) FROM dbo.[" + strTableName + "]";

            //  add label
            if (strColumnName != "")
            { strQuery = (strLabel != "") ? "SELECT COUNT(*) [" + strLabel + "] FROM dbo.[" + strTableName + "] " + strNull : "SELECT COUNT(*) FROM dbo.[" + strTableName + "] " + strNull; }

            SqlCommand cmd = new SqlCommand(strQuery, sqlConnection);
            cmd.Connection.Close();
            cmd.Connection.Open();
            cmd.CommandTimeout = 3600;
            count = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();
            return count;

        }

        public List<string> SqlRecordCountColumn(string strTableName, SqlConnection sqlConnection, string strDbName = "")
        {
            //  column name
            List<string> lstClm = DataTableToList(SqlColumnNames(strTableName, sqlConnection, strDbName));

            //  return
            List<string> lstRtn = new List<string>();

            for (int i = 0; i < lstClm.Count; i++)
            { lstRtn.Add(StrNum(SqlRecordCount(strTableName, sqlConnection, lstClm[i], strDbName))); }

            return lstRtn;

        }

        public List<string> SqlRecordsNull(string strTableName, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "";

            //  return
            List<string> lstRtn = new List<string>();

            //  column name
            List<string> lstClm = DataTableToList(SqlColumnNames(strTableName, sqlConnection, strDbName));

            for (int i = 0; i < lstClm.Count; i++)
            {
                strQuery = "select COUNT(*) from dbo.[" + strTableName + "] WHERE [" + lstClm[i] + "] IS NULL";
                lstRtn.Add(StrNum(SqlExecuteScalar(strQuery, sqlConnection)));
            }

            return lstRtn;

        }
        #endregion


        #region SqlConnectionChangeDB


        public SqlConnection SqlConnectionChangeDB(string newDB, SqlConnection sqlConnection)
        {
            sqlConnection.Close();

            SqlConnection sqlConn = new SqlConnection(sqlConnection.ConnectionString.Replace(sqlConnection.Database, newDB));

            return sqlConn;
        }


        #endregion

        #region SqlCheckUniqColumn
        public string SqlCheckUniqColumn(SqlConnection sqlConnection, string strTable, string strColumn, string strDbName = "")
        {
            // create query
            string strQry = "SELECT[" + strColumn + "],COUNT(*)cnt FROM[" + strTable + "] GROUP BY[" + strColumn + "] HAVING COUNT(*) > 1";

            // check result
            if (SqlDataAdapter(strQry, sqlConnection, strDbName).Rows.Count > 0)
            { return strTable + " ==>" + strColumn + " ==> its not uniq"; }

            return strTable + " ==>" + strColumn + " ==> its uniq";

        }

        #endregion

        #region SqlGetUniqData
        public string SqlGetUniqData(SqlConnection sqlConnection, string strTable, string strColumn, out string strAllQuery)
        {
            string strQry, strAllQry;


            //  remove old table
            strQry = " IF OBJECT_ID('t01', 'U') IS NOT NULL BEGIN DROP TABLE t01 END";
            strAllQry = strQry;


            //  remove old table
            strQry = " IF OBJECT_ID('" + strTable + "_uniq', 'U') IS NOT NULL BEGIN DROP TABLE [" + strTable + "_uniq] END";
            strAllQry = strAllQry + strQry;

            //  add RowNumber & uniq datat
            strQry =
                        " SELECT c.* INTO t01 FROM"
                        + " (SELECT [" + strColumn + "], MAX(RowNumber)RowNumber FROM(SELECT ROW_NUMBER() OVER(ORDER BY [" + strColumn + "])RowNumber, [" + strColumn + "] FROM [" + strTable + "])a GROUP BY [" + strColumn + "])b"
                        + " JOIN(SELECT ROW_NUMBER() OVER(ORDER BY [" + strColumn + "])RowNumber, * FROM [" + strTable + "])c ON b.RowNumber = c.RowNumber";
            strAllQry = strAllQry + strQry;

            //  new table in to table orginal uniq
            strQry = " DROP TABLE[" + strTable + "] SELECT * INTO [" + strTable + "_uniq] FROM dbo.t01 DROP TABLE dbo.t01";
            strAllQry = strAllQry + strQry;



            //  remove RowNumber column
            strQry = " ALTER TABLE [" + strTable + "_uniq] DROP COLUMN RowNumber";
            SqlExcutCommand(strQry, sqlConnection, "GetUniqData");

            // strAllQuery  out
            strAllQuery = strAllQry + strQry;


            return strTable + " ==> " + strColumn + " ==> " + SqlExcutCommand(strAllQry, sqlConnection, "GetUniqData");

        }
        #endregion


        #region SqlDropColumn
        public string SqlDropColumn(string strTable, string strColumn, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "ALTER TABLE dbo.[" + strTable + "] DROP COLUMN [" + strColumn + "]";

            //  connection change database name
            if (strDbName != "") { sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection); }

            return SqlExcutCommand(strQuery, sqlConnection, strColumn + " ==> DropColumn");
        }
        #endregion


        #region SqlJoin
        public DataTable SqlJoin(string strFirstTable, string strSecendTable, string strJoinColumn1, string strJoinColumn2, SqlConnection sqlConnection, string strFirstDbName = "", string strSecondDbName = "", string strType = "*")
        {
            string strQuery = "";

            if (strType == "*")
            {
                strQuery = "SELECT a.[" + strJoinColumn1 + "] FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b" +
                    " ON a.[" + strJoinColumn1 + "] = b.[" + strJoinColumn2 + "] WHERE a." + strJoinColumn1 + " IS NOT NULL OR b." + strJoinColumn2 + " IS NOT NULL";

                if (strFirstDbName != "" & strSecondDbName != "")
                {
                    strQuery = "SELECT a.[" + strJoinColumn1 + "] FROM [" + strFirstDbName + "].dbo.[" + strFirstTable + "] a LEFT JOIN [" + strSecondDbName + "].dbo.[" + strSecendTable + "] b" +
                                " ON a.[" + strJoinColumn1 + "] = b.[" + strJoinColumn2 + "] WHERE a." + strJoinColumn1 + " IS NOT NULL OR b." + strJoinColumn2 + " IS NOT NULL";
                }
            }
            else
            {
                strQuery = "SELECT a.[" + strJoinColumn1 + "] FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b" +
                    " ON a.[" + strJoinColumn1 + "] = b.[" + strJoinColumn2 + "] WHERE a." + strJoinColumn1 + " IS NOT NULL OR b." + strJoinColumn2 + " IS NOT NULL" +
                    " GROUP BY a.[" + strJoinColumn1 + "]";

                if (strFirstDbName != "" & strSecondDbName != "")
                {
                    strQuery = "SELECT a.[" + strJoinColumn1 + "] FROM [" + strFirstDbName + "].dbo.[" + strFirstTable + "] a LEFT JOIN [" + strSecondDbName + "].dbo.[" + strSecendTable + "] b" +
                                " ON a.[" + strJoinColumn1 + "] = b.[" + strJoinColumn2 + "] WHERE a." + strJoinColumn1 + " IS NOT NULL OR b." + strJoinColumn2 + " IS NOT NULL" +
                                " GROUP BY a.[" + strJoinColumn1 + "]";
                }
            }
            return SqlDataAdapter(strQuery, sqlConnection, "SqlJoin");
        }

        public DataTable SqlJoin(string strFirstTable, string strSecendTable, string strJointColumn, List<string> lstFirsTableColumn, List<string> lstSecendTableColumn, SqlConnection sqlConnection, out string strQuery, bool bolCreat = false)
        {
            string strFirstColumn = ListToString(lstFirsTableColumn, ",", "a", true, strFirstTable);
            string strSecendColumn = ListToString(lstSecendTableColumn, ",", "b", true, strSecendTable);

            string Query = strQuery = "";

            if (bolCreat == true)
            { Query = strQuery = "SELECT " + strFirstColumn + "," + strSecendColumn + " INTO " + strFirstTable + "_" + strSecendTable + " FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b ON b.[" + strJointColumn + "] = a.[" + strJointColumn + "]"; }

            if (bolCreat == false)
            { Query = strQuery = "SELECT " + strFirstColumn + "," + strSecendColumn + " FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b ON b.[" + strJointColumn + "] = a.[" + strJointColumn + "]"; }

            return SqlDataAdapter(Query, sqlConnection, "SqlJoin");
        }


        #endregion


        #region SqlRename
        public void SqlRename(SqlConnection sqlConnection, string strType, string strOldName, string strNewName, string strTable = "")
        {
            string strQuery = "";

            // renam table
            if (strType.ToUpper() == "TABLE")
            { strQuery = "EXEC sp_rename '" + strOldName + "', '" + strNewName + "'"; }

            //  rename column
            if (strType.ToUpper() == "COLUMN" && strTable != "")
            { strQuery = "EXEC sp_rename '" + strTable + "." + strOldName + "', '" + strNewName + "' , 'COLUMN'"; }


            SqlExcutCommand(strQuery, sqlConnection);
        }
        #endregion


        #region SqlQueryGetTableName

        public string SqlQueryGetTableName(string strQuery)
        {

            string strTableName = "";
            int intStart = 0, intLen = 0;


            // get table name when contains "INTO"
            if (strQuery.Contains("INTO ["))
            {
                intStart = strQuery.ToUpper().IndexOf("INTO [") + 6;
                intLen = strQuery.ToUpper().IndexOf("] FROM") - intStart;
                strTableName = strQuery.Substring(intStart, intLen);
            }

            // get when contains "ALTER"
            else if (strQuery.Contains("ALTER TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("ALTER TABLE [") + 13;
                intLen = strQuery.ToUpper().IndexOf("] ADD") - intStart;
                strTableName = strQuery.Substring(intStart, intLen);
            }

            // get when contains "DROP TABLE"
            else if (strQuery.Contains("DROP TABLE ["))
            {
                intStart = strQuery.ToUpper().IndexOf("DROP TABLE [") + 12;
                intLen = strQuery.Length - (intStart + 1);

                strTableName = strQuery.Substring(intStart, intLen);
            }


            return strTableName;

        }

        #endregion


        #region SqlQueryChangeTableName

        public string SqlQueryChangeTableName(string strQuery, string strNewTableName)
        {
            // old table name
            string strTableName = SqlQueryGetTableName(strQuery);

            //  change table name in query
            return strQuery.Replace(strTableName, strNewTableName);

        }

        #endregion


        #region SqlConnectionTest

        public Boolean SqlConnectionTest(SqlConnection sqlConnection)
        {
            Boolean bolSqlConnectionTest = false;


            //  test connection
            try
            {
                sqlConnection.Open();
                bolSqlConnectionTest = true;
                sqlConnection.Close();
            }
            catch (Exception) { }


            return bolSqlConnectionTest;
        }

        #endregion

        //****************************************************************
        #endregion

        //****************************************************************




    }
}
