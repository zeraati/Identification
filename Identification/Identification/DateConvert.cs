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

        clsDateConvert clsdate = new clsDateConvert();
        clsDateStandard clsstd = new clsDateStandard();


        Dictionary<int, string> dicDBName = new Dictionary<int, string>();
        SqlConnection sqlConnection = new SqlConnection();


        bool ok;
        string Fild;
        string command;
        #endregion
        public DateConvert(SqlConnection sqlCon)
        {
            InitializeComponent();
            sqlConnection = sqlCon;
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
            cmbTB.DataSource = Functions.SqlTableName(sqlConnection);

        }

        private void cmbTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load column names of source
            cmb1.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTB.Text, sqlConnection));
            cmb2.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTB.Text, sqlConnection));
        }

        //  bottun milady to persion
        private void btnG2P_Click(object sender, EventArgs e)
        {
            string strFinal = "";
            //  check column data type
            if (checkDataType(cmb1.Text, "date") == false | checkDataType(cmb2.Text, "varchar") == false)
            { MessageBox.Show("Not Standard " + cmb1.Text + " OR " + cmb2.Text); }
            else
            {
                //  update convert milady to persion
                strFinal = Functions.SqlUpdateMiladyToPersion(cmbTB.Text, cmb1.Text, cmb2.Text, sqlConnection);


                #region Standard Formated Date
                //  sql update query
                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "Replace ([" + cmb2.Text + "],'-','/')", sqlConnection);
                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "Replace ([" + cmb2.Text + "],'_','/')", sqlConnection);
                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "Replace ([" + cmb2.Text + "],'.','/')", sqlConnection);
                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "Replace ([" + cmb2.Text + "],',','/')", sqlConnection);

                //  run query                                    
                lstReport.Items.Add(" Ltrim & Rtrim " +
                                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "ltrim(rtrim([" + cmb2.Text + "]))", sqlConnection)
                                );
                //  run query
                lstReport.Items.Add(" Standard Date " +
                                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "dbo.[FE_Date]([" + cmb2.Text + "]) where [" + cmb2.Text + "] IS NOT NULL", sqlConnection)
                                );
                #endregion

                #region Delete Not Valid Date
                //  run query
                lstReport.Items.Add(" Delete Not Valid Date " +
                                Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, " NULL where dbo.[CM-Fix]([" + cmb2.Text + "])= 0 ", sqlConnection)
                                );
                #endregion

                #region save final Persion Date

                //  conclude final
                if (strFinal.Contains("Done"))
                {
                    //  report
                    lstReport.Items.Add(
                        //  save only year of persion date
                                        Functions.SqlUpdateCharacter(cmbTB.Text, cmb2.Text, 7, 4, sqlConnection, "SqlUpdateCharacter ")
                                        );

                    //  report
                    lstReport.Items.Add(
                        //  convert column persion varchar to smallint
                                        "SqlEditDataTypeColumn " + Functions.SqlEditDataTypeColumn(cmbTB.Text, cmb2.Text, " smallint ", " NULL ", sqlConnection, sqlConnection.Database)
                                        );

                    //  report
                    lstReport.Items.Add(
                        //  standard column persion
                                        Functions.SqlUpdateColumnData(cmbTB.Text, cmb2.Text, "NULL", sqlConnection, " < 1300 ", " SqlUpdateColumnData ")
                                        );

                }
                #endregion

                else MessageBox.Show("Error!");

            }
        }








        private void btnP2G_Click(object sender, EventArgs e)
        {
            string strFinal = "";

            //  check column data type
            if (checkDataType(cmb2.Text, "date") == false | checkDataType(cmb1.Text, "varchar") == false)
            { MessageBox.Show("Not Standard"); }
            else
            {
                //  update date after converter persion to milady
                strFinal = Functions.SqlUpdatePersionToMilady(cmbTB.Text, cmb2.Text, cmb1.Text, sqlConnection);

                //  report
                lstReport.Items.Add(strFinal);


                #region save final Persion Date

                //  conclude final
                if (strFinal.Contains("Done"))
                {
                    //  report
                    lstReport.Items.Add(
                        //  save only year of persion date
                                        Functions.SqlUpdateCharacter(cmbTB.Text, cmb1.Text, 7, 4, sqlConnection, "SqlUpdateCharacter ")
                                        );

                    //  report
                    lstReport.Items.Add(
                        //  convert column persion varchar to smallint
                                        "SqlEditDataTypeColumn " + Functions.SqlEditDataTypeColumn(cmbTB.Text, cmb1.Text, " smallint ", " NULL ", sqlConnection, sqlConnection.Database)
                                        );

                    //  report
                    lstReport.Items.Add(
                        //  standard column persion
                                        Functions.SqlUpdateColumnData(cmbTB.Text, cmb1.Text, "NULL", sqlConnection, " < 1300 ", " SqlUpdateColumnData ")
                                        );

                }
                #endregion
            }

        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }



        #region Functions

        #region checkDataType
        public bool checkDataType(string strColumn, string strType)
        {
            bool bolReturn = false;
            if (Functions.SqlColumns(cmbTB.Text, sqlConnection, strColumn).Rows[0][2].ToString().ToUpper().Contains(strType.ToUpper())) bolReturn = true;
            return bolReturn;
        }
        #endregion

        #endregion





    }
}
