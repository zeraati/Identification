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

                #region Standard Data Column
                //  conclude final
                if (strFinal.Contains("Done"))
                {
                    //  report
                    lstReport.Items.Add(
                        //  save only year of persion date
                                        Functions.SqlUpdateCharacter(cmbTB.Text, cmb2.Text, 0, 5, sqlConnection, "SqlUpdateCharacter ")
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
            //  check column data type
            if (checkDataType(cmb1.Text, "varchar") == false)
            { MessageBox.Show("Not Standard"); }
            else
            {

            }

            //  update date after converter persion to milady

            //command = "Update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb2.Text + "] = " +
            //            "dbo.UDF_Julian_To_Gregorian(dbo.UDF_Persian_To_Julian" +
            //            "(CONVERT(INT,RIGHT([" + cmb1.Text + "],4))," +
            //            "CONVERT(INT,SUBSTRING([" + cmb1.Text + "],4,2))," +
            //            "CONVERT(INT,LEFT([" + cmb1.Text + "],2)))) " +
            //            "WHERE  LEN([" + cmb1.Text + "]) = 10 and [" + cmb2.Text + "] is null AND RIGHT([" + cmb1.Text + "],4)>1300";
            ////textBox2.Text = command;
            ////"master.dbo.UDF_Julian_To_Gregorian(master.dbo.UDF_Persian_To_Julian(SUBSTRING([" + cmb1.Text + "],0,5),SUBSTRING([" + cmb1.Text + "],6,2),SUBSTRING([" + cmb1.Text + "],9,2))) where [" + cmb2.Text + "] is null AND LEN([" + cmb1.Text + "])>9 AND [" + cmb1.Text + "] LIKE '____/__/__'";

            ////ok = Functions.SqlExcutCommand(command, sqlConn);
            //progressBar1.Value = (25 / 100) * progressBar1.Maximum;
            ////MessageBox.Show("تبدیل تاریخ : " + Functions.sqlexecutecmd(command, Variables.sever), "نمایش پیغام");
            //command = "Update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb2.Text + "] = [" + cmb1.Text + "] + 621 WHERE  LEN([" + cmb1.Text + "]) = 4 and [" + cmb2.Text + "] is null";
            ////ok = ok | Functions.SqlExcutCommand(command, sqlConn);
            //MessageBox.Show("تبدیل تاریخ : " + ok.ToString(), "نمایش پیغام");
            //if (ok == true)
            //{
            //    progressBar1.Value = (50 / 100) * progressBar1.Maximum;
            //    command = "update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb1.Text + "] = RIGHT([" + cmb1.Text + "],4) WHERE [" + cmb1.Text + "] IS NOT NULL AND ISNUMERIC(RIGHT([" + cmb1.Text + "],4))=1";
            //    //textBox1.Text = command;
            //    //ok = Functions.SqlExcutCommand(command, sqlConn);
            //    if (ok == false)
            //    {
            //        MessageBox.Show("تاریخ استاندارد نمی باشد", "!خطا");
            //    }
            //    command = "ALTER TABLE [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] ALTER COLUMN [" + cmb1.Text + "] Smallint";
            //    //textBox2.Text = command;
            //    //ok = ok | Functions.SqlExcutCommand(command, sqlConn);
            //    MessageBox.Show("ذخیره سال و تغییر نوع فیلد" + ok.ToString(), "نمایش پیغام");
            //    progressBar1.Value = (100 / 100) * progressBar1.Maximum;
            //}
        }

        private void btnTest_Click(object sender, EventArgs e)
        {


            //string strQuery = "SELECT dbo.UDF_Julian_To_Gregorian(dbo.UDF_Persian_To_Julian " +
            //                    "(CONVERT(INT,RIGHT('" + cmb1.Text + "',4))," +
            //                    "CONVERT(INT,SUBSTRING('" + cmb1.Text + "',4,2))," +
            //                    "CONVERT(INT,LEFT('" + cmb1.Text + "',2)))) ";
            //dgvTest.DataSource = Functions.SqlDataAdapter(strQuery, sqlConnection);
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


        public string func_PersianDateFormat(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(date).ToString("00"));
            return sb.ToString();
        }

        public string func_milady(DateTime date)
        {
            GregorianCalendar gc = new GregorianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(gc.GetYear(date).ToString("0000"));
            sb.Append("/");
            sb.Append(gc.GetMonth(date).ToString("00"));
            sb.Append("/");
            sb.Append(gc.GetDayOfMonth(date).ToString("00"));
            return sb.ToString();
        }

        #endregion





    }
}
