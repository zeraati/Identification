using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace EstandardSazi
{
    class Functions1 : clsFunField
    {
        FunctionsSQL sqldal = new FunctionsSQL();
        Variables var = new Variables();
        int retInt;

        #region test sql connection

        /// <summary>
        /// test sql connection
        /// </summary>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>open connection is true</returns>
        public Boolean testConn(SqlConnection sqlConnection)
        {
            Boolean bolTestConn = false;


            //  test connection
            try
            {
                sqlConnection.Open();
                bolTestConn = true;
                sqlConnection.Close();
            }
            catch (Exception) { }


            return bolTestConn;
        }

        #endregion



        #region LoadColumn==> In CheckedListBox & ComboBox

        /// <summary>
        /// اضافه کردن نام ستون ها به چک لیست
        /// </summary>
        /// <param name="DBName">نام دیتابیس</param>
        /// <param name="cmbxTbl">کمبو لیست نام جداول</param>
        /// <param name="ChkLstBx">چک لیست باکس</param>
        public void LoadColumnNew(SqlConnection sqlCon, string cmbxTbl, CheckedListBox ChkLstBx)
        {
            ChkLstBx.Items.Clear();

            DataTable dt = sqldal.sqlColumnsNameCLBNew(cmbxTbl, sqlCon);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][4].ToString() == "")
                {
                    if (dt.Rows[i][2].ToString() == "NO")
                    {
                        ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + "] not null");
                    }
                    else
                    {
                        ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + "] null");
                    }
                }
                else
                {
                    if (dt.Rows[i][4].ToString() == "-1")
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (max)] not null");
                        }
                        else
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (max)] null");
                        }
                    }
                    else
                    {
                        if (dt.Rows[i][2].ToString() == "NO")
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (" + dt.Rows[i][4].ToString() + ")] not null");
                        }
                        else
                        {
                            ChkLstBx.Items.Add(dt.Rows[i][1].ToString() + " [" + dt.Rows[i][3].ToString() + " (" + dt.Rows[i][4].ToString() + ")] null");
                        }
                    }

                }

            }
        }

        //***********   new 95-05-19
        public List<string> lstLoadClm(SqlConnection sqlCon, string strTblName)
        {
            List<string> lstClm = new List<string>();
            DataTable dt = sqldal.sqlColumnsName(sqlCon, strTblName);
            return lstClm = DataTableToList(dt);
        }
        //******************************
        #endregion LoadColumn==> In CheckedListBox & ComboBox

        //***********   new 95-05-19
        //  convert datatable to list
        public List<string> DataTableToList(DataTable dt)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst.Add(dt.Rows[i][0].ToString());
            }
            return lst;
        }

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
        /// <summary>
        /// ذخیره اطلاعات لاگین از لیست
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="Path">مسیر فایل</param>
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



        #region LoadColumn==> In CheckListBox For DBName

        public void ChlbDB(SqlConnection sqlCon, CheckedListBox ChhLstBx)
        {
            ChhLstBx.Items.Clear();
            List<string> lst=sqldal.lstSqlGetDBName(sqlCon);

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
            retInt = 0;
            for (int i = 0; i < dtCK.Rows.Count; i++)
            {
                if (dtCK.Rows[i][0].ToString().ToUpper() == txtCK1.ToUpper())
                {
                    retInt++;
                }
            }
            return retInt;
        }

        string rtn;
        string strChar;
        string strCmb;
        string strMaster = " master.dbo.ReplaceAB(";
        public string clbEhrazChecked(string Type, string Persent, string cmbLike, string strWhere, string a, string b)
        {

            strCmb = cmbLike;
            while (strCmb.IndexOf(",") != -1)
            {
                strChar = strCmb.Substring(0, cmbLike.IndexOf(",")).Trim();
                if (Persent == "100" & strChar != "") // cmbLike
                {
                    //      خالی - سید - میر - اله
                    if (strWhere != "")
                    {
                        // " AND replace(a.Name,N'" + cmbLike + "','')=replace(b.Name,N'" + cmbLike + "','')AND (a.Name LIKE N'%" + cmbLike + "%' or b.Name LIKE N'%" + cmbLike + "%') AND b.Name<>a.Name";
                        if (strChar == "خالی") strWhere += " OR (" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='') ";
                        if (strChar != "خالی") strWhere += var.WhereReplace1(strChar, Type, a, b);
                    }
                    //" Where replace(a.Name,N'" + cmbLike + "','')=replace(b.Name,N'" + cmbLike + "','') AND b.Name<>a.Name";
                    else
                    {
                        if (strChar == "خالی") strWhere += " WHERE ((" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='') AND " + b + ".[STID_vld] IS NULL) ";
                        if (strChar != "خالی") strWhere += var.WhereReplace2(strChar, Type, a, b);
                    }
                }
                else if (Persent != "100" & strChar != "") // Persent Name
                {

                }
                strCmb = strCmb.Replace(strChar + ",", "");
            }
            if (strCmb.IndexOf(",") == -1 & cmbLike != "")
            {
                if (strWhere != "")
                {
                    // " AND replace(a.Name,N'" + cmbLike + "','')=replace(b.Name,N'" + cmbLike + "','')AND (a.Name LIKE N'%" + cmbLike + "%' or b.Name LIKE N'%" + cmbLike + "%') AND b.Name<>a.Name";
                    if (strCmb == "خالی") strWhere += " OR ((" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='')  AND " + b + ".[STID_vld] IS NULL) ";
                    if (strCmb != "خالی") strWhere += var.WhereReplace1(strCmb, Type, a, b);
                }
                //" Where replace(a.Name,N'" + cmbLike + "','')=replace(b.Name,N'" + cmbLike + "','') AND b.Name<>a.Name";
                else
                {
                    if (strCmb == "خالی") strWhere = " WHERE ((" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='')  AND " + b + ".[STID_vld] IS NULL)";
                    if (strCmb != "خالی") strWhere = var.WhereReplace2(strCmb, Type, a, b);
                }
            }
            if (strCmb.IndexOf(",") == -1 & cmbLike == "")
            {
                if (strWhere != "")
                {
                    strWhere += " AND " + a + "." + Type + " = " + b + "." + Type;
                }
                else
                {
                    strWhere = "WHERE " + a + "." + Type + " = " + b + "." + Type;
                }
            }
            return rtn = strWhere;
        }

        string Where;

        public string SelectAll(string Type, string Persent, string cmbLike, string strSelect, string a, string b)
        {
            string SelectAll = "";
            strCmb = cmbLike;
            while (strCmb.IndexOf(",") != -1)
            {
                strChar = strCmb.Substring(0, cmbLike.IndexOf(","));

                if (Persent == "100" & strChar != "") // cmbLike
                {
                    //      خالی - سید - میر - اله
                    if (strChar == "خالی") Where += " WHERE ((" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='') AND " + b + ".[STID_vld] IS NULL) ";
                    if (strChar != "خالی") Where += var.WhereReplace2(strChar, Type, a, b);
                }
                if (SelectAll == "")
                {
                    SelectAll = strSelect + Where;
                }
                else
                {
                    SelectAll += " UNION " + strSelect + Where;
                }
                strCmb = strCmb.Replace(strChar + ",", "");
                Where = "";
            }
            strChar = strCmb;
            if (strCmb.IndexOf(",") == -1 & cmbLike != "")
            {
                if (Persent == "100" & strChar != "") // cmbLike
                {
                    //      خالی - سید - میر - اله
                    if (strChar == "خالی") Where += " WHERE ((" + a + "." + Type + " is null or " + b + "." + Type + " is null OR " + a + "." + Type + " ='' or " + b + "." + Type + " ='') AND " + b + ".[STID_vld] IS NULL) ";
                    if (strChar != "خالی") Where += var.WhereReplace2(strChar, Type, a, b);
                }
                if (SelectAll == "")
                {
                    SelectAll = strSelect + Where;
                }
                else
                {
                    SelectAll += " UNION " + strSelect + Where;
                }
                Where = "";
            }
            return rtn = SelectAll;
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












    }
}
