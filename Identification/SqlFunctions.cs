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
    public class SqlFunctions
    {

        Functions functions = new Functions();


        #region SqlConnection Functions

        //  Connection    
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

        //SqlConnectionChangeDB
        public SqlConnection SqlConnectionChangeDB(string newDB, SqlConnection sqlConnection)
        {
            sqlConnection.Close();

            SqlConnection sqlConn = new SqlConnection(sqlConnection.ConnectionString.Replace(sqlConnection.Database, newDB));

            return sqlConn;
        }

        // SqlConnectionTest
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


        #region DataBase Functions
        //  SqlGetDBName
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
        //  DataBase Extended
        public DataTable SqlDBExtended(SqlConnection sqlconnection, string strDb = "")
        {
            string strQuery = "SELECT [name] +' = '+ CAST([value] AS NVARCHAR(max)) FROM sys.extended_properties WHERE minor_id = 0 AND major_id = 0";

            return SqlDataAdapter(strQuery, sqlconnection, strDb);
        }

        #endregion

        #endregion


        #region Sql Functions

        public DataTable SqlFunctionsFiles(SqlConnection sqlconnection, string strLabel = "")
        {
            string strQuery = "";
            strQuery = (strLabel != "") ? "SELECT name FROM [" + sqlconnection.Database + "].sys.objects [" + strLabel + "] WHERE type='FN'" :
                "SELECT name FROM [" + sqlconnection.Database + "].sys.objects WHERE type='FN'";

            return SqlDataAdapter(strQuery, sqlconnection, "");
        }
        #endregion


        #region Sql Upadte Functions

        // Sql Update Character Save
        public string SqlUpdateCharacter(string strTableName, string strColumnName, int intStartIndex, int intLength, SqlConnection sqlConnection)
        {
            string strQuery = "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "] = SUBSTRING([" + strColumnName + "]," + intStartIndex + "," + intLength + ") WHERE [" + strColumnName + "] IS NOT NULL ";

            return SqlExcutCommand(strQuery, sqlConnection, " SqlUpdateCharacter ");
        }

        //SqlUpdateColumn
        #region SqlUpdateColumn

        public string SqlUpdateData(string strTableName, string strColumnName, string strData, string strDbName, SqlConnection sqlConnection, string strWhereQuery = "", string strState = "")
        {
            string strQuery = "";
            strQuery = (strWhereQuery == "") ? "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]= " + strData :
                "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]= " + strData + " Where " + strWhereQuery;


            return SqlExcutCommand(strQuery, sqlConnection, "UpdateColumn " + strState, strDbName);
        }

        /// <summary>
        /// Update Column From Other Column | Can be provided
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName">Column Name</param>
        /// <param name="strColumnData">Column Name Data</param>
        /// <param name="strDbName">Data Base Name</param>
        /// <param name="sqlConnection"></param>
        /// <param name="strState"></param>
        /// <param name="strWhereQuery">Where</param>
        /// <returns></returns>
        public string SqlUpdateColumn(string strTableName, string strColumnName, string strColumnData, string strDbName, SqlConnection sqlConnection, string strWhereQuery = "", string strState = "")
        {
            string strQuery = "";
            strQuery = (strWhereQuery != "") ? "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]=[" + strColumnData + "]" :
                "UPDATE dbo.[" + strTableName + "] SET [" + strColumnName + "]=" + strColumnData + " Where " + strWhereQuery;


            return SqlExcutCommand(strQuery, sqlConnection, strState, strDbName);
        }

        #endregion

        #region Sql AlphasOnly

        public List<string> SqlAlphasOnly(string strTable, string strColumn, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "UPDATE dbo.[" + strTable + "] SET [" + strColumn + "]=dbo.AlphasOnly([" + strColumn + "]) WHERE [" + strColumn + "] IS NOT NULL SELECT @@ROWCOUNT";

            List<string> lstRtn = new List<string>();

            lstRtn.Add(SqlExecuteScalar(strQuery, sqlConnection, " SqlAlphasOnly ", strDbName).ToString());


            return lstRtn;
        }

        #endregion

        #region Sql Numberic

        public List<string> SqlNumberic(string strTable, string strColumn, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "UPDATE dbo.[" + strTable + "] SET [" + strColumn + "]=dbo.[CM-Check]([" + strColumn + "]) WHERE [" + strColumn + "] IS NOT NULL SELECT @@ROWCOUNT";

            List<string> lstRtn = new List<string>();

            lstRtn.Add(SqlExecuteScalar(strQuery, sqlConnection, " SqlNumberic ", strDbName).ToString());

            return lstRtn;
        }

        #endregion


        #endregion


        #region Date

        // Sql Update Persion To Milady
        public string SqlUpdatePersionToMilady(string strTableName, string strColumnMilady, string strColumnPersion, SqlConnection sqlConnection)
        {
            string strQuery = "Update dbo.[" + strTableName + "] SET [" + strColumnMilady + "] = " +
                "CAST(dbo.UDF_Julian_To_Gregorian(dbo.UDF_Persian_To_Julian(CAST(RIGHT([" + strColumnPersion + "],4) AS INT),CONVERT(INT,SUBSTRING([" + strColumnPersion + "],4,2)),CAST(LEFT([" + strColumnPersion + "],2)AS INT))) AS DATE)" +
                " where [" + strColumnMilady + "] is null";
            return SqlExcutCommand(strQuery, sqlConnection, "Sql Update Persion To Milady");
        }

        // Sql Update Milady To Persion
        public string SqlUpdateMiladyToPersion(string strTableName, string strColumnMilady, string strColumnPersion, SqlConnection sqlConnection)
        {
            string strQuery = "Update dbo.[" + strTableName + "] SET [" + strColumnPersion + "] = dbo.UDF_Gregorian_To_Persian([" + strColumnMilady + "]) where [" + strColumnPersion + "] is null";
            return SqlExcutCommand(strQuery, sqlConnection, "Sql Update Milady To Persion ");
        }

        #endregion


        #region Sql Table Functions

        //  SqlTableInfo
        #region SqlTableInfo
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

        public void SqlTableName(SqlConnection sqlConnection, ListBox lstTableName, string strDbName = "", string strLabel = "")
        {
            lstTableName.Items.Clear();

            string strQuery = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

            if (strLabel != "")
            { strQuery = "SELECT TABLE_NAME [" + strLabel + "] FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"; }



            DataTable dtAdapter = SqlDataAdapter(strQuery, sqlConnection, strDbName);

            for (int i = 0; i < dtAdapter.Rows.Count; i++)
            {
                lstTableName.Items.Add(dtAdapter.Rows[i][0].ToString());
            }
        }

        //  tables extended properties
        public DataTable SqlTableExtended(string strTable, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "SELECT CAST(b.[value] AS NVARCHAR(max)) " +
                            "FROM sys.tables a RIGHT JOIN sys.extended_properties b " +
                            "ON a.object_id = b.major_id " +
                            "WHERE b.major_id <> 0 AND b.minor_id = 0 AND a.name = '" + strTable + "'";



            return SqlDataAdapter(strQuery, sqlConnection, strDbName);

        }
        #endregion

        //SqlQueryChangeTableName
        #region SqlQueryChangeTableName

        public string SqlQueryChangeTableName(string strQuery, string strNewTableName)
        {
            // old table name
            string strTableName = SqlQueryGetTableName(strQuery);

            //  change table name in query
            return strQuery.Replace(strTableName, strNewTableName);

        }

        #endregion

        //  SqlQueryGetTableName
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

        // SqlEditTableName
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

        // SqlCopyTable
        public string SqlCopyTable(string strTableName, string strCopyTableName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT * INTO dbo.[" + strCopyTableName + "] FROM dbo.[" + strTableName + "]";

            return SqlExcutCommand(strQuery, sqlConnection, " CopyTable [" + strCopyTableName + "]");

        }

        // SqlDropTable
        public string SqlDropTable(string strTableName, SqlConnection sqlConnection, string strState = "")
        {
            string strQry = "DROP TABLE [" + strTableName + "]";

            string strResult = SqlExcutCommand(strQry, sqlConnection, strState + "DropTable");

            return strTableName + " => " + strResult;
        }

        //  SqlDropTableSpace
        public string SqlDropTableSpace(string strTableName, SqlConnection sqlConnection)
        {
            int intCount = 0;
            string strReturn = "";

            intCount = SqlRecordCount(strTableName, sqlConnection);

            if (intCount == 0) { strReturn = SqlDropTable(strTableName, sqlConnection); }

            return strReturn;
        }

        #endregion


        #region Sql Table & Column Functions
        // SqlRename
        public string SqlRename(string strType, string strOldName, string strNewName, SqlConnection sqlConnection, string strTable = "")
        {
            string strQuery = "";

            // renam table
            if (strType.ToUpper().Trim() == "TABLE")
            {
                //  extended property
                strQuery = "EXECUTE sp_addextendedproperty " +
                    //  name
                    "@name = N'MS_Description', " +
                    //  old table name
                    "@value = N'" + strOldName + "', " +
                    "@level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', " +
                    //  table name Afterwards rename
                    "@level1name = N'" + strOldName + "'";

                //  run execut command
                SqlExcutCommand(strQuery, sqlConnection);

                //  rename table
                strQuery = "EXEC sp_rename @objname =N'" + strOldName + "', @newname =N'" + strNewName + "' , @objtype ='TABLE'";
            }

            //  rename column
            if (strType.ToUpper().Trim() == "COLUMN" && strTable != "")
            {
                //  extended property
                strQuery = "EXECUTE sp_addextendedproperty " +
                    "@name = N'Title', @value = N'" + strOldName + "', " +
                    //  type
                    "@level0type = N'SCHEMA', @level0name = N'dbo', " +
                    //  table name
                    "@level1type = N'TABLE', @level1name = N'" + strTable + "', " +
                    //  column description
                    "@level2type = N'COLUMN', @level2name = N'" + strOldName + "'";

                //  run execut command
                SqlExcutCommand(strQuery, sqlConnection);

                //  rename column
                strQuery = "EXEC sp_rename @objname =N'" + strTable + "." + strOldName + "', @newname =N'" + strNewName + "' , @objtype ='COLUMN'";
            }


            return SqlExcutCommand(strQuery, sqlConnection, "SqlRename => " + strOldName + " => " + strNewName);
        }
        //  
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

        //  SqlTableRecordsCount
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

        public List<string> SqlRecordCountColumn(string strTableName, SqlConnection sqlConnection, string strDbName = "", string strNull = "NULL")
        {
            //  change database name
            sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection);

            //  column name
            List<string> lstClm = functions.DataTableToList(SqlColumnNames(strTableName, sqlConnection, strDbName));

            //  return
            List<string> lstRtn = new List<string>();

            for (int i = 0; i < lstClm.Count; i++)
            { lstRtn.Add(functions.StrNum(SqlRecordCount(strTableName, sqlConnection, lstClm[i], strNull))); }

            return lstRtn;

        }

        public List<string> SqlRecordsNull(string strTableName, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "";

            //  return
            List<string> lstRtn = new List<string>();

            //  column name
            List<string> lstClm = functions.DataTableToList(SqlColumnNames(strTableName, sqlConnection, strDbName));

            for (int i = 0; i < lstClm.Count; i++)
            {
                strQuery = "select COUNT(*) from dbo.[" + strTableName + "] WHERE [" + lstClm[i] + "] IS NULL";
                lstRtn.Add(functions.StrNum(SqlExecuteScalar(strQuery, sqlConnection)));
            }

            return lstRtn;

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="intSwitch"></param>
        /// <param name="strClm"></param>
        /// <param name="strTable"></param>
        /// <param name="strQuery1">Valid</param>
        /// <param name="intQuery2">InValid</param>
        /// <param name="intQuery3">Repeat</param>
        /// <param name="strDbName"></param>
        //Sql CodeMelli
        public void SqlValidatorCount(SqlConnection sqlConnection, int intSwitch, string strClm, string strTable, List<int> lstInt, string strDbName = "")
        {
            int int1 = 0, int2 = 0, int3 = 0;
            string strQuery3 = "";

            #region Queries
            switch (intSwitch)
            {
                case 0:
                    #region Any
                    int1 = 0; int2 = 0; int3 = 0;
                    #endregion
                    break;
                case 1:
                    #region Name,Family,Father
                    int1 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND dbo.AlphasOnly([" + strClm + "])<>'0' ");
                    int2 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND dbo.AlphasOnly([" + strClm + "])='0' ");
                    strQuery3 = "SELECT COUNT(*) b_count " +
                                "FROM dbo.[" + strTable + "] " +
                                "WHERE [" + strClm + "] IS NOT NULL AND dbo.AlphasOnly([" + strClm + "])<>'0' " +
                                "GROUP BY [" + strClm + "] HAVING COUNT(*)>1 ";
                    int3 = SqlDataAdapter(strQuery3, sqlConnection, strDbName).Rows.Count;
                    #endregion
                    break;
                case 2:
                    #region CodeMelli Count
                    int1 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND dbo.Ch_Codemelli(dbo.[CM-Check]([" + strClm + "])) = 1 ");
                    int2 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND dbo.Ch_Codemelli(dbo.[CM-Check]([" + strClm + "])) = 0 ");

                    strQuery3 = "SELECT COUNT(*) b_count " +
                                "FROM dbo.[" + strTable + "] " +
                                "WHERE [" + strClm + "] IS NOT NULL AND dbo.Ch_Codemelli(dbo.[CM-Check]([" + strClm + "]))=1 " +
                                "GROUP BY [" + strClm + "] HAVING COUNT(*)>1 ";
                    int3 = SqlDataAdapter(strQuery3, sqlConnection, strDbName).Rows.Count;
                    #endregion
                    break;
                case 3:
                    #region ShenasCode
                    int1 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND LEN(dbo.[CM-Check]([" + strClm + "])) < 8 ");
                    int2 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND (LEN(dbo.[CM-Check]([" + strClm + "])) > 7 OR CAST(dbo.[CM-Check]([" + strClm + "]) AS BIGINT) = 0 ) ");

                    strQuery3 = "SELECT COUNT(*) b_count FROM dbo.[" + strTable + "] " +
                                "WHERE [" + strClm + "] IS NOT NULL AND LEN(dbo.[CM - Check]([" + strClm + "])) < 8 AND CAST(dbo.[CM - Check]([" + strClm + "]) AS BIGINT)<> 0  " +
                                "GROUP BY [" + strClm + "] HAVING COUNT(*)> 1";
                    int3 = SqlDataAdapter(strQuery3, sqlConnection, strDbName).Rows.Count;
                    #endregion
                    break;
                case 4:
                    #region Numberic
                    int1 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND CAST(dbo.[CM-Check]([" + strClm + "]) AS BIGINT) <> 0 ");
                    int2 = SqlCountColumn(strTable, sqlConnection, " [" + strClm + "] IS NOT NULL AND CAST(dbo.[CM-Check]([" + strClm + "]) AS BIGINT) = 0 ");

                    strQuery3 = "SELECT COUNT(*) b_count FROM dbo.[" + strTable + "] " +
                                "WHERE [" + strClm + "] IS NOT NULL AND CAST(dbo.[CM-Check]([" + strClm + "]) AS BIGINT) <> 0 " +
                                "GROUP BY [" + strClm + "] HAVING COUNT(*)> 1";
                    int3 = SqlDataAdapter(strQuery3, sqlConnection, strDbName).Rows.Count;
                    #endregion
                    break;
            }

            #endregion

            //  repeat
            //strQuery3 = SqlRunQuery(strQuery3, sqlConnection, strDbName);

            lstInt.Add(int1);
            lstInt.Add(int2);
            lstInt.Add(int3);

        }


        #endregion


        #region Sql Column Functions

        //  SqlColumnsInfo
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


            strQuery = (strLable != "") ? " SELECT COLUMN_NAME [" + strLable + "] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=N'" + strTableName + "'" : strQuery;

            return SqlDataAdapter(strQuery, sqlConnection, strDbName);

        }


        #endregion

        //  columns extended properties
        public DataTable SqlColumnExtended(string strTable, string strColumn, SqlConnection sqlConnection, string strDbName = "")
        {

            string strQuery = "SELECT CAST(d.b_value AS NVARCHAR(max)) FROM sys.tables c JOIN ( " +
                            "SELECT[object_id][a_object_id],[name] [a_name],[column_id] [a_column_id],b.[b_name],b.[b_value] FROM sys.columns a JOIN( " +
                            "SELECT[major_id][b_major_id], [minor_id] [b_minor_id], [name] [b_name], [value] [b_value] FROM sys.extended_properties WHERE [minor_id] <> 0)b " +
                            "ON a.[object_id]=b.[b_major_id] AND a.[column_id]=b.[b_minor_id] " +
                            ") d ON c.[object_id]=d.a_object_id WHERE c.[name]='" + strTable + "'AND d.a_name='" + strColumn + "'";



            return SqlDataAdapter(strQuery, sqlConnection, strDbName);
        }

        //  SqlDropColumn
        #region SqlDropColumn
        public string SqlDropColumn(string strTable, string strColumn, SqlConnection sqlConnection, string strDbName = "")
        {
            string strQuery = "ALTER TABLE dbo.[" + strTable + "] DROP COLUMN [" + strColumn + "]";

            //  connection change database name
            if (strDbName != "") { sqlConnection = SqlConnectionChangeDB(strDbName, sqlConnection); }

            return SqlExcutCommand(strQuery, sqlConnection, strColumn + " ==> DropColumn");
        }
        #endregion

        //  SqlCopyColumn
        public string SqlCopyColumn(string strTableName, string strColumnName,string strDbName, SqlConnection sqlConnection)
        {
            DataTable dtClmInfo = SqlColumns(strTableName, sqlConnection, strColumnName);

            //  check data type - string & not string
            if (dtClmInfo.Rows[0][3].ToString() == "")

            //  add new field
            { SqlAddNewColumn(strTableName, strColumnName + "_Copy", dtClmInfo.Rows[0][3].ToString(), dtClmInfo.Rows[0][2].ToString(), sqlConnection); }
            else
            { SqlAddNewColumn(strTableName, strColumnName + "_Copy", dtClmInfo.Rows[0][3].ToString() + dtClmInfo.Rows[0][4].ToString(), dtClmInfo.Rows[0][2].ToString(), sqlConnection); }

            //  copy column
            return SqlUpdateColumn(strTableName, strColumnName + "_Copy", strColumnName, strDbName, sqlConnection, "CopyColumn ");

        }

        //  SqlCountColumn
        #region SqlCountColumn

        public int SqlCountDataBase(SqlConnection sqlConnection)
        {
            string strQuery = "SELECT COUNT(*) FROM sys.databases";


            return SqlExecuteScalar(strQuery, sqlConnection);
        }

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

        //  SqlAddNewColumn
        #region Sql AddNew Column

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

        //  Sql Edit DataType Column
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

        //  Sql MaxLen Column Data
        #region Sql MaxLen Column Data
        public string SqlMaxLenColumnData(string strTableName, string strColumnName, SqlConnection sqlConnection)
        {
            string strQuery = "SELECT ISNULL(MAX(LEN([" + strColumnName + "])),0) FROM dbo.[" + strTableName + "]";
            return SqlRunQuery(strQuery, sqlConnection);
        }
        #endregion
        //  SqlDropColumnSpace
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
        //  SqlEditColumn
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
        public string SqlEditColumn(string strTableName, string strOldColumn, SqlConnection sqlConnection, string strNewColumn = "", string strDataType = "", string strLen = "", string strDbName = "")
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

                        while (SqlExcutCommand(strQuery, sqlConnection, "EditField", strDbName) == " => Error!")
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


        #endregion

        #region Sql Execute

        //  SqlExecuteScalar
        #region SqlExecuteScalar
        public int SqlExecuteScalar(string strQuery, SqlConnection sqlConnection, string strState = "", string strDbName = "")
        {
            //  connection change db
            sqlConnection = (strDbName != "") ? SqlConnectionChangeDB(strDbName, sqlConnection) : sqlConnection;


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

        //  SqlExcutCommand
        #region SqlExcutCommand
        public string SqlExcutCommand(string strQuery, SqlConnection sqlConnection, string strState = "", string strDbName = "")
        {
            //  connection change db
            sqlConnection = (strDbName != "") ? SqlConnectionChangeDB(strDbName, sqlConnection) : sqlConnection;


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

        //  SqlExcutCommandWithGO
        #region SqlExcutCommandWithGO
        public List<string> SqlExcutCommandWithGO(SqlConnection sqlConnection, string strQuery, string strState = "")
        {
            //  for substring
            int intStart, intLenght;

            List<string> lstQry = new List<string>();
            List<string> lstResult = new List<string>();


            // index of all GO in query
            List<int> lstIndexGo = functions.IndexOfAll(strQuery, "GO");


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

        #endregion

        //  SqlRunQuery
        #region SqlRunQuery

        public string SqlRunQuery(string strQuery, SqlConnection sqlConnection, string strDBName = "")
        {
            //  change sqlconnection new database name
            sqlConnection = (strDBName != "") ? SqlConnectionChangeDB(strDBName, sqlConnection) : sqlConnection;


            SqlCommand sqlCmd = new SqlCommand(strQuery, sqlConnection);
            string strExecute;
            sqlCmd.Connection.Close();
            sqlCmd.Connection.Open();
            sqlCmd.CommandTimeout = 3600;
            strExecute = (sqlCmd.ExecuteScalar().ToString() == null) ? "NULL" : sqlCmd.ExecuteScalar().ToString();
            sqlCmd.Connection.Close();


            return strExecute;
        }

        #endregion

        //SqlCreateReport
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

        //  SqlDropRows
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

        //  SqlDataAdapter
        #region SqlDataAdapter
        public DataTable SqlDataAdapter(string strQuery, SqlConnection sqlConnection, string strDbName, string strTable = "", string strNumberTop = "")
        {
            DataTable dt = new DataTable();

            //  sql connection change dbname
            sqlConnection = (strDbName != "") ? SqlConnectionChangeDB(strDbName, sqlConnection) : sqlConnection;

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

        //  SqlCheckUniqColumn
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

        //  SqlGetUniqData
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

        //  SqlJoin
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
            string strFirstColumn = functions.ListToString(lstFirsTableColumn, ",", "a", true, strFirstTable);
            string strSecendColumn = functions.ListToString(lstSecendTableColumn, ",", "b", true, strSecendTable);

            string Query = strQuery = "";

            if (bolCreat == true)
            { Query = strQuery = "SELECT " + strFirstColumn + "," + strSecendColumn + " INTO " + strFirstTable + "_" + strSecendTable + " FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b ON b.[" + strJointColumn + "] = a.[" + strJointColumn + "]"; }

            if (bolCreat == false)
            { Query = strQuery = "SELECT " + strFirstColumn + "," + strSecendColumn + " FROM [" + strFirstTable + "] a LEFT JOIN [" + strSecendTable + "] b ON b.[" + strJointColumn + "] = a.[" + strJointColumn + "]"; }

            return SqlDataAdapter(Query, sqlConnection, "SqlJoin");
        }


        #endregion

        //  Sql Repeat
        public int SqlRepeatCount(string strColumn, SqlConnection sqlconnection, string strDBName = "")
        {
            int intRtn = 0;


            string strQuery = "SELECT COUNT(*),[" + strColumn + "] FROM dbo.PersonTest_New GROUP BY[" + strColumn + "] HAVING COUNT(*) > 1";


            intRtn = SqlDataAdapter(strQuery, sqlconnection, strDBName).Rows.Count;


            return intRtn;
        }

    }
}
