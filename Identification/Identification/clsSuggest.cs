using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Identification
{
    class clsSuggest
    {
        string sub, str2, query, str3;
        int count;
        DataTable dtsql = new DataTable();
        DataTable dtreurn = new DataTable();
        Functions Functions = new Functions();

        public DataTable Suggest(string str,SqlConnection sqlCon)
        {
            DataTable dtret = new DataTable();
            dtret.Columns.Add("p");
            dtret.Columns.Add("e");
            str = str2 = str.Trim();
            count = 0;
            if (str.IndexOf(" ") != -1)
            {
                str3 = str.Substring(0, str.IndexOf(" "));
                query = "SELECT [WorldPersion],[WorldEnglish] FROM [LogFile].[dbo].[Dictionery] WHERE [WorldPersion] = N'" + str + "' order by [WorldPersion]";
                dtsql = Functions.SqlDataAdapter(query,sqlCon);
                if (dtsql.Rows.Count != 0)
                {
                    dtreurn = dtsql;
                }
                else
                {
                    while ((count = str.IndexOf(" ", count)) != -1)
                    {

                        query = "SELECT [WorldPersion],[WorldEnglish] FROM [LogFile].[dbo].[Dictionery] WHERE [WorldPersion] like N'%" + str3 + "%' order by [WorldPersion]";
                        dtsql = Functions.SqlDataAdapter(query, sqlCon);
                        for (int i = 0; i < dtsql.Rows.Count; i++)
                        {
                            DataRow dr = dtret.NewRow();
                            dr[0] = dtsql.Rows[i][0].ToString();
                            dr[1] = dtsql.Rows[i][1].ToString();
                            dtret.Rows.Add(dr);
                        }
                        str3 = str.Substring(count);
                        count++;
                    }
                    dtreurn = dtret;
                }
            }
            else if (count == 0)
            {
                query = "SELECT [WorldPersion],[WorldEnglish] FROM [LogFile].[dbo].[Dictionery] WHERE [WorldPersion] like N'%" + str + "%' order by [WorldPersion]";
                DataRow dr = dtsql.NewRow();
                dtsql = Functions.SqlDataAdapter(query, sqlCon);
                for (int i = 0; i < dtsql.Rows.Count; i++)
                {
                    dr[dtsql.Columns[0].ColumnName] = dtsql.Rows[i][0].ToString();
                    dr[dtsql.Columns[1].ColumnName] = dtsql.Rows[i][1].ToString();
                }
                dtreurn = dtsql;
            }
            return dtreurn;
        }
    }
}
