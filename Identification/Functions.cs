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
            { "Name","FirstName","نام","" },
            { "Family","LastName","نام خانوادگی","فامیل"},
            { "Father","FatherName","نام پدر",""},
            { "ShenasCode","ShNo","شماره شناسنامه","ش ش" },
            { "PBirthDate","BirthDate","سال تولد" ,""},
            { "CodeMelli","MelliCode","کد ملی","شماره ملی" },
            { "HomeCity","Cityname","شهر محل تولد","" },
            { "SodorCity","","شهر محل صدور","" },
            { "HomeOstan","OstanName","استان محل تولد","" },
            { "SodorOstan","Sodor","استان محل صدور","" },
            { "STID","CodeMarkazModiriat","کد مرکز مدیریت","" },
            { "MKID","CodeMarkazKhadamat","کد مرکز خدمات","" },
            {"Mobile","همراه","موبایل","شماره همراه" }
            };

        #endregion
    }

    
    public class Functions
    {
        //SqlFunctions sqlfunctions = new SqlFunctions();

        #region Function

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
        public void LoadColumnInfo(string strTableName, CheckedListBox ChkLstBx, SqlConnection sqlConnection,DataTable dtSqlColumns)
        {
            ChkLstBx.Items.Clear();
            List<string> lstColumns = DataTypeToList(dtSqlColumns);
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

        public void ChlbDB( CheckedListBox ChhLstBx,List<string> lstGetDBName)
        {
            ChhLstBx.Items.Clear();            

            for (int i = 0; i < lstGetDBName.Count; i++)
            {
                ChhLstBx.Items.Add(lstGetDBName[i]);
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

        public void DataTableToListbox(DataTable dt, ListBox lstbx, int intIndexColumn = 0)
        {

            lstbx.Items.Clear();

            //  convert data to list
            for (int i = 0; i < dt.Rows.Count; i++)
            { lstbx.Items.Add(dt.Rows[i][intIndexColumn].ToString()); }

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
        /*
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
                    string strTableName = sqlfunctions.SqlQueryGetTableName(File.ReadAllText(fileName));

                    //  add query file name & table name
                    dtReturn.Rows.Add(Path.GetFileNameWithoutExtension(fileName), strTableName);
                }
            }

            return dtReturn;
        }
        */
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
        #endregion

        //****************************************************************
        #region sql functions
        //****************      sql functions








        
































    










        //****************************************************************
        #endregion

        //****************************************************************




    }
}
