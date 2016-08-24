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
        clsUpdate clsupd = new clsUpdate();
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

            //  load table names of source
            // cmb1.DataSource = Functions.SqlColumnNames(cmbTB.Text, sqlConnection);
            //cmb2.DataSource = Functions.SqlColumnNames(cmbTB.Text, sqlConnection);
        }

        private void cmbTB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  load column names of source
            cmb1.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTB.Text, sqlConnection));
            cmb2.DataSource = Functions.DataTableToList(Functions.SqlColumnNames(cmbTB.Text, sqlConnection));
        }


        private void btnG2P_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(1394, 12, 09);
            //تبدیل میلادی به شمسی
            command = "Update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb2.Text + "] = master.dbo.UDF_Gregorian_To_Persian([" + cmb1.Text + "]) where [" + cmb2.Text + "] is null";
            //Persia.DateType pd = new Persia.DateType();


            //ok = Functions.SqlExcutCommand(command,sqlConn );
            if (ok == true)
            {
                MessageBox.Show("انجام شد");
            }
            else MessageBox.Show("!خطا");
            //textBox2.Text = dt.ToShortDateString();//ToString(CultureInfo.InvariantCulture);


            //textBox1.Text = command;
            //MessageBox.Show("تبدیل تاریخ : " + Functions.sqlexecutecmd(command, Variables.sever),"نمایش پیغام");
            //lst1.Items.Add("اصلاح نوع و اندازه " + Fild + ": " + Functions.sqlexecutecmd(command, Variables.sever));
        }

        private void btnP2G_Click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //DataTable dtgrid = new DataTable();

            //command = "SELECT Id,[" + cmb1.Text + "] FROM dbo.[" + cmbTB.Text + "] WHERE [" + cmb1.Text + "] IS NOT NULL order by Id";
            //dt = clsFunctions.sqlDataAdapter(command, Variables.DBName);            

            //dtgrid.Columns.Add("Id");
            //dtgrid.Columns.Add("PBirthdate");
            //dtgrid.Columns.Add("Birthdate");



            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dtgrid.NewRow();
            //    dr["Id"]=dt.Rows[i][0].ToString();
            //    dr["PBirthdate"] = dt.Rows[i][1].ToString();


            //    dr["PBirthdate"] = clsstd.time(dt.Rows[i][1].ToString());


            //    dtgrid.Rows.Add(dr);
            //}

            //dataGridView1.DataSource = dtgrid;
            //تبدیل شمسی به میلادی


            //  update date after converter persion to milady


            command = "Update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb2.Text + "] = " +
                        "dbo.UDF_Julian_To_Gregorian(dbo.UDF_Persian_To_Julian" +
                        "(CONVERT(INT,RIGHT([" + cmb1.Text + "],4))," +
                        "CONVERT(INT,SUBSTRING([" + cmb1.Text + "],4,2))," +
                        "CONVERT(INT,LEFT([" + cmb1.Text + "],2)))) " +
                        "WHERE  LEN([" + cmb1.Text + "]) = 10 and [" + cmb2.Text + "] is null AND RIGHT([" + cmb1.Text + "],4)>1300";
            //textBox2.Text = command;
            //"master.dbo.UDF_Julian_To_Gregorian(master.dbo.UDF_Persian_To_Julian(SUBSTRING([" + cmb1.Text + "],0,5),SUBSTRING([" + cmb1.Text + "],6,2),SUBSTRING([" + cmb1.Text + "],9,2))) where [" + cmb2.Text + "] is null AND LEN([" + cmb1.Text + "])>9 AND [" + cmb1.Text + "] LIKE '____/__/__'";

            //ok = Functions.SqlExcutCommand(command, sqlConn);
            progressBar1.Value = (25 / 100) * progressBar1.Maximum;
            //MessageBox.Show("تبدیل تاریخ : " + Functions.sqlexecutecmd(command, Variables.sever), "نمایش پیغام");
            command = "Update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb2.Text + "] = [" + cmb1.Text + "] + 621 WHERE  LEN([" + cmb1.Text + "]) = 4 and [" + cmb2.Text + "] is null";
            //ok = ok | Functions.SqlExcutCommand(command, sqlConn);
            MessageBox.Show("تبدیل تاریخ : " + ok.ToString(), "نمایش پیغام");
            if (ok == true)
            {
                progressBar1.Value = (50 / 100) * progressBar1.Maximum;
                command = "update [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] SET [" + cmb1.Text + "] = RIGHT([" + cmb1.Text + "],4) WHERE [" + cmb1.Text + "] IS NOT NULL AND ISNUMERIC(RIGHT([" + cmb1.Text + "],4))=1";
                //textBox1.Text = command;
                //ok = Functions.SqlExcutCommand(command, sqlConn);
                if (ok == false)
                {
                    MessageBox.Show("تاریخ استاندارد نمی باشد", "!خطا");
                }
                command = "ALTER TABLE [" + sqlConnection.Database + "].dbo.[" + cmbTB.Text + "] ALTER COLUMN [" + cmb1.Text + "] Smallint";
                //textBox2.Text = command;
                //ok = ok | Functions.SqlExcutCommand(command, sqlConn);
                MessageBox.Show("ذخیره سال و تغییر نوع فیلد" + ok.ToString(), "نمایش پیغام");
                progressBar1.Value = (100 / 100) * progressBar1.Maximum;
            }
        }



        #region Functions

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
