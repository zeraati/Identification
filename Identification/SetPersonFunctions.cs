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
    class SetPersonFunctions
    {

        Functions functions = new Functions();
        SqlFunctions sqlfunction = new SqlFunctions();


        #region Create New Field

        public string CreateNewField(string cmbColumnUniq, string strCmbSecondTbl, ComboBox clmSecondTbl, SqlConnection sqlConnectionSecond)
        {

            string strReturn = "";
            string strColumn, strFinal;
            DialogResult dr;
            int intCheck;


            //  new field
            strColumn = cmbColumnUniq + "_vld";
            //  check new field
            intCheck = CheckField(sqlfunction.SqlColumnNames(strCmbSecondTbl, sqlConnectionSecond), strColumn);

            if (intCheck == 0)
            {
                dr = MessageBox.Show(" بسازم ؟" + cmbColumnUniq + "_vld آیا فیلد ", "! هشدار", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    //  add new field
                    strFinal = sqlfunction.SqlAddNewColumn(strCmbSecondTbl, strColumn, "INT", "NULL", sqlConnectionSecond);

                    if (strFinal.Contains("Done"))
                    {
                        //  report
                        strReturn = strColumn + " با موفقیت انجام شد " + Environment.NewLine;

                        //  datagridview combobox column source
                        functions.ComboBoxSource(clmSecondTbl, sqlfunction.SqlColumnNames(strCmbSecondTbl, sqlConnectionSecond));

                    }
                    else
                    {
                        //  report
                        strReturn = " با مشکل مواجه شد " + strColumn + Environment.NewLine;
                    }


                    intCheck = 0;

                    //  check field "Description"
                    intCheck = CheckField(sqlfunction.SqlColumnNames(strCmbSecondTbl, sqlConnectionSecond), "Description");

                    if (intCheck == 0)
                    {
                        // add column   
                        strFinal = sqlfunction.SqlAddNewColumn(strCmbSecondTbl, "Description", "NVARCHAR(MAX)", "NULL", sqlConnectionSecond);

                        if (strFinal.Contains("Done"))
                        {
                            //  report
                            strReturn += "Description با موفقیت انجام شد " + Environment.NewLine;

                            //  datagridview combobox column source
                            functions.ComboBoxSource(clmSecondTbl, sqlfunction.SqlColumnNames(strCmbSecondTbl, sqlConnectionSecond));
                        }
                        else strReturn += " با مشکل مواجه شد " + Environment.NewLine;
                    }

                }
                else if (dr == DialogResult.No)
                { strReturn = "لغو شد" + Environment.NewLine; }
            }

            return strReturn;
        }

        #endregion


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


    }
}
