using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Identification
{
    class FunctionsSQL
    {

        #region DatabaseStruct
        /// <summary>
        /// نام پایگاه داده و جدول
        /// </summary>
        public struct DatabaseStruct
        {
            #region DataBaseName
            private string _DataBaseName;
            /// <summary>
            /// نام پایگاه اطلاعاتی
            /// </summary>
            public string DataBaseName
            {
                get { return _DataBaseName; }
                set { _DataBaseName = value; }
            }
            #endregion

            #region TableName
            private string _TableName;
            /// <summary>
            /// نام جدول
            /// </summary>
            public string TableName
            {
                get { return _TableName; }
                set { _TableName = value; }
            }
            #endregion
        }
        public int count;
        #endregion

        string strcon;


        public List<string> DataTableToList(DataTable dt, int IndexColumn = 0)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                lst.Add(dt.Rows[i][IndexColumn].ToString());

            }
            return lst;
        }

        public DataTable ListToDataTable(List<string> lst)
        {
            DataTable dtRtn = new DataTable();
            for (int i = 0; i < lst.Count; i++)
            {
                dtRtn.Rows.Add(lst[i]);
            }
            return dtRtn;
        }




        //*********     Function SQL


        #region SqlConnection

        /// <summary>
        /// test sql connection
        /// </summary>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>open connection is true</returns>
        /// 

        public bool SqlConnectionTest(SqlConnection sqlConnection)
        {
            bool bSqlcon = false;
            try
            {
                sqlConnection.Open();
                bSqlcon = true;
                sqlConnection.Close();
            }
            catch (Exception) { }

            return bSqlcon;
        }

        public SqlConnection SqlConnection(SqlConnection sqlConnection)
        {

            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
            }
            catch (Exception) { }

            return sqlConnection;
        }
        public SqlConnection SqlConnection(string strServer, string strUser, string strPass)
        {
            SqlConnection sqlConnection = new SqlConnection();
            if (strUser != "" & strPass != "") sqlConnection.ConnectionString = "Data Source=" + strServer + ";Initial Catalog=master;Persist Security Info=True;User ID=" + strUser + ";Password=" + strPass + "";
            else sqlConnection.ConnectionString = "Data Source=" + strServer + ";Initial Catalog=master;Integrated Security=True";
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
            }
            catch (Exception) { }

            return sqlConnection;
        }


        #endregion

        #region SqlConnectionChangeDB

        public SqlConnection SqlConnectionChangeDB(SqlConnection sqlConnection, string newDB)
        {
            sqlConnection.Close();

            SqlConnection sqlConn = new SqlConnection(sqlConnection.ConnectionString.Replace(sqlConnection.Database, newDB));

            return sqlConn;
        }

        #endregion

        #region SqlGetDBName
        /// <summary>
        /// بر روی سیستم SQL لیستی شامل تمام پایگاه  داده های
        /// </summary>
        /// <returns>List<string></returns>

        public List<string> SqlGetDBName(SqlConnection sqlConnection)
        {
            List<string> lstRtn = new List<string>();
            string qry = "SELECT name FROM sys.databases WHERE database_id>0 order by name";
            DataTable dt = SqlDataAdapter(qry, sqlConnection);
            return lstRtn = DataTableToList(dt); ;
        }

        #endregion

        #region SqlDataAdapter

        public DataTable SqlDataAdapter(string query, SqlConnection sqlConnection, string stat = "sqlDataAdapter")
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                if (stat != "sqlDataAdapter")
                    MessageBox.Show(stat + Environment.NewLine + e.Message, "خطا");
                return dt;
            }
        }


        #endregion

        #region SqlGetTableNameInDB
        /// <summary>
        /// لیستی شامل جداول موجود در پایگاه داده انتخاب شده 
        /// </summary>
        /// <param name="DBName">نام پایگاه داده</param>
        /// <returns>list table name of database</returns>
        public List<string> SqlGetTableNameInDB(SqlConnection sqlCon)
        {
            List<string> lstRtn = new List<string>();
            string Query = " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' GROUP BY TABLE_NAME";
            DataTable dt = SqlDataAdapter(Query, sqlCon);
            return lstRtn = DataTableToList(dt);
        }

        #endregion

        #region SqlColumns

        public DataTable SqlColumns(SqlConnection sqlConnection, string strTableName)
        {
            DataTable dtRtn = new DataTable();
            string Query =
                    " SELECT Name,[Null],[Type]+[Len] [Type] FROM" +
                    " (" +
                        " SELECT COLUMN_NAME Name," +
                            " CASE  WHEN IS_NULLABLE = 'YES' THEN ' Null' ELSE ' Not Null' END [Null],DATA_TYPE [Type]," +
                            " CASE  WHEN CHARACTER_MAXIMUM_LENGTH IS NULL THEN ' ' ELSE ' ('+CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(50))+')' END AS [Len]" +
                        " FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "'" +
                    " ) Columns";
            dtRtn = SqlDataAdapter(Query, sqlConnection);
            return dtRtn;
        }

        #endregion

        #region SqlExecuteCmd
        public Boolean SqlExecuteCmd(string query, SqlConnection sqlCon)
        {
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            try
            {
                cmd.CommandTimeout = 3600;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteReader();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
        }
        #endregion

        #region SqlCount
        public int SqlCount(string strQry, SqlConnection sqlCon)
        {
            SqlCommand cmd = new SqlCommand(strQry, sqlCon);

            int count;
            cmd.Connection.Open();
            cmd.CommandTimeout = 3600;
            count = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Connection.Close();
            return count;
        }
        #endregion



        //*********        End

    }
}
