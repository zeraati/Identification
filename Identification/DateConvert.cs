using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Identification
{
    public partial class DateConvert : Form
    {
        #region Global

        Functions Functions = new Functions();
        SqlFunctions sqlfunction = new SqlFunctions();

        SqlConnection sqlConnection = new SqlConnection();

        string strTableName="";

        #endregion
        public DateConvert(SqlConnection sqlCon,string strTBLName)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
            strTableName = strTBLName;
        }

        private void بستنپنجرهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DateConvert_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            //  name form
            this.Text = "Convert Date - Server : " + sqlConnection.DataSource + " - DataBase : " + sqlConnection.Database;

            //  load table name of source
            cmbTableName.DataSource = sqlfunction.SqlTableName(sqlConnection);

            //  default value
            cmbTableName.Text = strTableName;
        }

        private void cmbTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load column names of source
            cmbFirst.DataSource = Functions.DataTableToList(sqlfunction.SqlColumnNames(cmbTableName.Text, sqlConnection));
            cmbConvertion.DataSource = Functions.DataTableToList(sqlfunction.SqlColumnNames(cmbTableName.Text, sqlConnection));
        }

        //  bottun milady to persion
        private void btnG2P_Click(object sender, EventArgs e)
        {
            string strFinal = "";
            //  check column data type
            if (checkDataType(cmbFirst.Text, "date") == false | checkDataType(cmbConvertion.Text, "varchar") == false)
            { MessageBox.Show("Not Standard " + cmbFirst.Text + " OR " + cmbConvertion.Text); }
            else
            {
                //  update convert milady to persion
                strFinal = sqlfunction.SqlUpdateMiladyToPersion(cmbTableName.Text, cmbFirst.Text, cmbConvertion.Text, sqlConnection);


                #region Standard Formated Date
                //  sql update query
                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "Replace ([" + cmbConvertion.Text + "],'-','/')", sqlConnection);
                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "Replace ([" + cmbConvertion.Text + "],'_','/')", sqlConnection);
                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "Replace ([" + cmbConvertion.Text + "],'.','/')", sqlConnection);
                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "Replace ([" + cmbConvertion.Text + "],',','/')", sqlConnection);

                //  run query                                    
                lstReport.Items.Add(" Ltrim & Rtrim " +
                                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "ltrim(rtrim([" + cmbConvertion.Text + "]))", sqlConnection)
                                );
                //  run query
                lstReport.Items.Add(" Standard Date " +
                                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, "dbo.[FE_Date]([" + cmbConvertion.Text + "]) where [" + cmbConvertion.Text + "] IS NOT NULL", sqlConnection)
                                );
                #endregion

                #region Delete Not Valid Date
                //  run query
                lstReport.Items.Add(" Delete Not Valid Date " +
                                sqlfunction.SqlUpdateColumnData(cmbTableName.Text, cmbConvertion.Text, " NULL where dbo.[CM-Fix]([" + cmbConvertion.Text + "])= 0 ", sqlConnection)
                                );
                #endregion

                //  save persion column

                SaveFinalPersionDate(strFinal, cmbConvertion.Text);

            }
        }

        //  bottun persion to milady
        private void btnP2G_Click(object sender, EventArgs e)
        {
            string strFinal = "";

            //  check column data type
            if (checkDataType(cmbFirst.Text, "varchar") == false | checkDataType(cmbConvertion.Text, "date") == false)
            { MessageBox.Show("Not Standard"); }
            else
            {
                //  update date after converter persion to milady
                strFinal = sqlfunction.SqlUpdatePersionToMilady(cmbTableName.Text, cmbConvertion.Text, cmbFirst.Text, sqlConnection);

                //  report
                lstReport.Items.Add(strFinal);

                //  save persion column

                SaveFinalPersionDate(strFinal, cmbFirst.Text);
            }

        }


        private void تبدیلتاریخمیلادیبهشمسیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnG2P_Click(null, null);
        }

        private void تبدیلتاریخشمسیبهمیلادیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnP2G_Click(null, null);
        }

        #region Functions

        #region save final Persion Date

        public void SaveFinalPersionDate(string strFinal, string strPersion)
        {

            //  conclude final
            if (strFinal.Contains("Done"))
            {
                //  report
                lstReport.Items.Add(
                    //  save only year of persion date
                                    sqlfunction.SqlUpdateCharacter(cmbTableName.Text, strPersion, 7, 4, sqlConnection, "SqlUpdateCharacter ")
                                    );

                //  report
                lstReport.Items.Add(
                    //  convert column persion varchar to smallint
                                    "SqlEditDataTypeColumn " + sqlfunction.SqlEditDataTypeColumn(cmbTableName.Text, strPersion, " smallint ", " NULL ", sqlConnection, sqlConnection.Database)
                                    );

                //  report
                lstReport.Items.Add(
                    //  standard column persion
                                    sqlfunction.SqlUpdateColumnData(cmbTableName.Text, strPersion, "NULL", sqlConnection, " < 1300 ", " Nullable Date<1300 ").Replace("Error", "does not have")
                                    );

            }
            else MessageBox.Show("Error!");
        }

        #endregion


        #region checkDataType
        public bool checkDataType(string strColumn, string strType)
        {
            bool bolReturn = false;
            List<string> lst = Functions.DataTableToList(sqlfunction.SqlColumns(cmbTableName.Text, sqlConnection,"", strColumn));
            if (sqlfunction.SqlColumns(cmbTableName.Text, sqlConnection, "",strColumn).Rows[0][2].ToString().ToUpper().Contains(strType.ToUpper())) bolReturn = true;
            return bolReturn;
        }
        #endregion



        #endregion

    }
}
