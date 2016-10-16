using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identification
{
    public class clsUpdate
    {
        #region Global
        string str1 = "";
        string type;
        int maxlen;
        string fld;
        FunctionsSQL FunSql = new FunctionsSQL();
        //clsDAL clsdal = new clsDAL();
        Functions clshlp = new Functions();
        #endregion

        #region CheckDataType
        //public string ReturnData(string Fildname, string TBName, string DBName)
        //{
        //    string query = "Select ISNULL(max(Len([" + Fildname + "])),0) From [" + TBName + "]";

        //    SqlCommand cmd = new SqlCommand(query, Functions.SqlConnection());
        //    try
        //    {
        //        if (cmd.Connection.State == ConnectionState.Closed)
        //        {
        //            cmd.Connection.Open();
        //            fld = cmd.ExecuteScalar().ToString();
        //        }
        //        return fld;
        //    }
        //    catch (Exception ex)
        //    {
        //        return fld;
        //    }
        //    finally
        //    {
        //        if (cmd.Connection.State == ConnectionState.Open)
        //        {
        //            cmd.Connection.Close();
        //            cmd.Dispose();
        //        }
        //    }
        //}
        #endregion
    }
}
