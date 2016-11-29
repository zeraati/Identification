using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Identification
{
    public class ClsStandard
    {
        SqlFunctions sqlfunction = new SqlFunctions();
        Functions Functions = new Functions();

        public List<string> Standard(string strTableName, string strColumnName, string strType, string strColumnCodemelli, SqlConnection sqlconnection, string strDbName = "")
        {
            List<string> lstRtn = new List<string>();


            switch (strType)
            {
                case "":
                    break;
                case "حروفی":
                    lstRtn = sqlfunction.SqlAlphasOnly(strTableName, strColumnName, sqlconnection, strDbName);
                    break;
                case "عددی":
                    lstRtn = sqlfunction.SqlNumberic(strTableName, strColumnName, sqlconnection, strDbName);
                    break;
                case "کدملی":
                    lstRtn = StandardCodeMelli(strTableName, strColumnName, sqlconnection, strDbName);
                    break;
                case "شماره شناسنامه":
                    lstRtn = StandardShenascode(strTableName, strColumnName, strColumnCodemelli, sqlconnection, strDbName);
                    break;
                case "تاریخ شمسی":
                    break;
                case "تاریخ میلادی":
                    break;
            }

            return lstRtn;
        }


        #region Public Standard CodeMelli
        /// <summary>
        /// Standard CodeMelli
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <param name="sqlconnection"></param>
        /// <param name="strDbName"></param>
        /// <returns>lstRtn</returns>
        public List<string> StandardCodeMelli(string strTableName, string strColumnName, SqlConnection sqlconnection, string strDbName = "")//(string strColumnNameInput)
        {
            string strColumnData, strQuery, strWhere;
            List<string> lstRtn = new List<string>();

            strColumnData = "dbo.[CM-Fix]([" + strColumnName + "])";

            #region Create New Column For Standard CodeMelli

            //  query
            strQuery = "SELECT name FROM sys.columns WHERE object_id=(SELECT object_id FROM sys.tables WHERE name=N'" + strTableName + "')";

            //  check column
            int intCheckColumn = Functions.CheckField(sqlfunction.SqlDataAdapter(strQuery, sqlconnection, strDbName), "CodeMelli2");

            //  create codemelli2
            if (intCheckColumn == 0)
            { sqlfunction.SqlAddNewColumn(strTableName, "CodeMelli2", "varchar(10)", "Null", sqlconnection, strDbName); }

            #endregion

            #region Delete Not Valid

            //  delete not valid  
            sqlfunction.SqlUpdateData(strTableName, strColumnName, strColumnData, strDbName, sqlconnection);

            #endregion

            #region Valid Codemellis (Lens = 8,9,10)

            //  valid codemellis (lens = 8,9,10)
            strWhere = " LEN(" + strColumnName + ") BETWEEN 8 AND 10 AND ISNUMERIC(" + strColumnName + ")=1"
                     + " and CodeMelli2 is null and dbo.Ch_Codemelli(RIGHT('00'+ " + strColumnName + ",10))=1";

            //  run query
            lstRtn.Add(
                        sqlfunction.SqlUpdateColumn(strTableName,
                                                    "CodeMelli2",
                                                    "RIGHT('00'+ " + strColumnName + ",10)",
                                                    strDbName,
                                                    sqlconnection,
                                                    strWhere,
                                                    "Valid Codemellis (Lens = 8,9,10) "));

            #endregion

            #region CodeMellis With Len = 11,12

            //  where query
            strWhere = " len(" + strColumnData + ") between 11 and 12 and dbo.Ch_Codemelli(LEFT(" + strColumnData + ",10)) = 1 and CodeMelli2 is null";

            //  update column data - LEFT
            lstRtn.Add(
                        sqlfunction.SqlUpdateColumn(strTableName,
                                                    "CodeMelli2",
                                                    "LEFT(" + strColumnData + ",10)",
                                                    strDbName,
                                                    sqlconnection,
                                                    strWhere,
                                                    "CodeMellis With Len = 11,12 - LEFT - "));
            //  where query
            strWhere = " len(" + strColumnData + ") between 11 and 12 and dbo.Ch_Codemelli(RIGHT(" + strColumnData + ",10)) = 1 and CodeMelli2 is null";

            //  update column data - RIGHT
            lstRtn.Add(
                        sqlfunction.SqlUpdateColumn(strTableName,
                                                    "CodeMelli2",
                                                    "RIGHT(" + strColumnData + ",10)",
                                                    strDbName,
                                                    sqlconnection,
                                                    strWhere,
                                                    "CodeMellis With Len = 11,12 - RIGHT - "));

            #endregion

            #region Finaly

            //  report & update data
            sqlfunction.SqlUpdateColumn(strTableName, strColumnName, "CodeMelli2", strDbName, sqlconnection);

            //  edit column data type
            sqlfunction.SqlEditColumn(strTableName, strColumnName, sqlconnection, strColumnName, "VARCHAR", "10", strDbName);
            lstRtn.Add("Edit Column Datatype VARCHAR(10)");

            //  drop column     'CodeMelli2'
            sqlfunction.SqlDropColumn(strTableName, "CodeMelli2", sqlconnection, strDbName);

            #endregion

            return lstRtn;

        }
        #endregion

        #region Public Standard ShensNameh

        public List<string> StandardShenascode(string strTableName, string strColumnName, string strColumnCodemelli, SqlConnection sqlconnection, string strDbName = "")
        {
            List<string> lstRtn = new List<string>();

            strColumnName = "[" + strColumnName + "]";
            string strColumnData = "dbo.[CM-Fix](" + strColumnName + ")", strWhere = "";

            #region Delete Character Not Numeric

            //  update character not numeric - report
            lstRtn.Add("Delete Character Not Numeric => " + strColumnName +
                        sqlfunction.SqlUpdateData(strTableName, strColumnName, strColumnData, strDbName, sqlconnection)
                        );

            #endregion

            #region ShenasCode With Len > 7 To CodeMelli

            //  where 
            strWhere = "LEN(" + strColumnData + ")>7 and CodeMelli is null";

            //  query - report
            lstRtn.Add(
                        sqlfunction.SqlUpdateColumn(strTableName,
                        strColumnCodemelli, "RIGHT('00'+ " + strColumnData + ",10), " + strColumnName + " = NULL ",
                        strDbName,
                        sqlconnection,
                        strWhere,
                        "ShenasCode With Len > 7 To CodeMelli => " + strColumnName + ": ")
                        );

            #endregion

            #region Delete ShenasCode Not Valid

            //  where query
            strWhere = strColumnName + "='0' or " + strColumnName + "=' ' or LEN(" + strColumnName + ")>7";

            //  update
            lstRtn.Add(
                        sqlfunction.SqlUpdateColumn(strTableName,
                        strColumnName,
                        "NULL",
                        strDbName,
                        sqlconnection, strWhere, "Delete ShenasCode Not Valid => " + strColumnName + ": ")
                        );

            #endregion

            return lstRtn;
        }
        #endregion
    }
}
