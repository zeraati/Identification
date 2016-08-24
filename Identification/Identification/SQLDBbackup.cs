using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persia;
using PersiaSL;
using System.Data.SqlClient;

namespace Identification
{
    public partial class SQLDBbackup : Form
    {
        Functions func = new Functions();
        FunctionsSQL sqldal = new FunctionsSQL();
        Dictionary<int, string> dicDBName = new Dictionary<int, string>();
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        SqlConnection sqlConn = new SqlConnection();
        
        string query;
        string filename;
        string filepath;
        bool bit_check;
        int Checked_Total;
        int prog;
        public SQLDBbackup(SqlConnection sqlConnection)
        {
            InitializeComponent();
            sqlConn = sqlConnection;
        }

        private void SQLDBbackup_Load(object sender, EventArgs e)
        {
            lblConnect.Text = "نام سرور : " + sqlConn.DataSource + " - نام بانک : " + sqlConn.Database;
            Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(DateTime.Now);
            lblDate.Text = solarDate.ToString("yyyyMMdd");
            func.ChlbDB(sqlConn, chlbDB);
            txtName.Enabled = false;
            lblFinish.Visible = false;
            if (txtFilePath.Text == "")
            {
                chlbDB.Enabled = false;
                groupBox1.Enabled = false;
                btnAllSelect.Enabled = false;
                btnBackup.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            //sfd.Filter = "Text files (*.bak)|*.bak|All files (*.*)|*.*";
            void_filepath();
            if (fbd.SelectedPath != "")
            {
                dataGridView1.DataSource = DBfiles(fbd.SelectedPath);
            }
        }

        public DataTable DBfiles(string selectpath)
        {
            DataTable dtfiles = new DataTable();
            dtfiles.Columns.Add("files");
            string[] files = Directory.GetFiles(selectpath, "*.bak");
            foreach (string file in files)
            {
                DataRow dr = dtfiles.NewRow();
                dr["files"] = file.Replace(selectpath + @"\", "");  // fbd.SelectedPath + ";" + file;
                dtfiles.Rows.Add(dr);
            }
            return dtfiles;
        }
        void void_filepath()
        {
            chlbDB.Enabled = true;
            groupBox1.Enabled = true;
            btnAllSelect.Enabled = true;
            btnBackup.Enabled = true;
            //string Path;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                filename = @"\DBName.bak";
                filepath = fbd.SelectedPath;
                txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\");
            }
            else
            {
                if (txtFilePath.Text == "")
                {
                    chlbDB.Enabled = false;
                    groupBox1.Enabled = false;
                    btnAllSelect.Enabled = false;
                    btnBackup.Enabled = false;
                }
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;// this.Enabled = false;
            lstReporting.Items.Clear();
            lblFinish.Visible = false;
            lblPersent.Text = "شروع";
            fun_backup();
            dataGridView1.DataSource = DBfiles(fbd.SelectedPath);
            Cursor.Current = Cursors.Default;// this.Enabled = true;
        }

        void fun_backup()
        {
            progressBar1.Value = 0;
            lblPersent.Text = progressBar1.Value.ToString() +" %";
            Checked_Total = 0;
            for (int y = 0; y < chlbDB.Items.Count; y++)
            {
                if (chlbDB.GetItemCheckState(y) == CheckState.Checked)
                {
                    Checked_Total++;
                }
            }
            int i = 0;
            DataTable dtfiles = new DataTable();
            DataTable dtDBUpdate = new DataTable();
            //**********************************************************
            //  پیدا کردن فایلهای پشتیبانی و ذخیره در جدول
            dtfiles.Clear();
            dtfiles.Columns.Add("files");
            //textBox1.Text = fbd.SelectedPath;
            string[] files = Directory.GetFiles(fbd.SelectedPath, "*.bak");
            foreach (string file in files)
            {
                DataRow dr = dtfiles.NewRow();
                dr["files"] = file.Replace(fbd.SelectedPath + @"\", "");  // fbd.SelectedPath + ";" + file;
                dtfiles.Rows.Add(dr);
            }
            //dataGridView1.DataSource = dtfiles;
            //*******************************************************
            for (int l = 0; l < chlbDB.Items.Count; l++)
            {
                if (chlbDB.GetItemCheckState(l) == CheckState.Checked)
                {
                    i++;
                }
            }
            if (i == 0)
            {
                MessageBox.Show("هیچ بانکی را انتخاب نکرده اید", "خطا");
            }
            else
            {
                i = 0;
                dtDBUpdate.Clear();
                dtDBUpdate.Columns.Add("DB");
                for (int k = 0; k < dtfiles.Rows.Count; k++)
                {
                    for (int j = 0; j < chlbDB.Items.Count; j++)
                    {
                        if (chlbDB.GetItemCheckState(j) == CheckState.Checked)
                        {
                            if (chbDate.CheckState == CheckState.Checked) { filename = chlbDB.Items[j].ToString() + "_" + lblDate.Text.Replace("/", "") + ".bak"; }
                            else if (chbName.CheckState == CheckState.Checked) { filename = chlbDB.Items[j].ToString() + "_" + txtName.Text + ".bak"; }
                            else { filename = chlbDB.Items[j].ToString() + ".bak"; }
                            if (filename == dtfiles.Rows[k]["files"].ToString())
                            {
                                i++;
                                DataRow drdb = dtDBUpdate.NewRow();
                                drdb["DB"] = filename;
                                dtDBUpdate.Rows.Add(drdb);
                                //dataGridView1.DataSource = dtDBUpdate;
                            }
                        }
                    }
                }
            }
            if (dtDBUpdate.Rows.Count == 0)
            {
                BackupTotal();
            }
            else
            {

                DialogResult dr = new DialogResult();
                dr = MessageBox.Show(i.ToString() + "بانک پشتیبانی همنام وجود دارد ، آیا می خواهید" + "بروزرسانی شود؟", "!هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    BackupTotal();
                    dataGridView1.DataSource = DBfiles(fbd.SelectedPath);
                }
                else if (dr == DialogResult.No)
                {
                    prog = 0;
                    DataTable dttest = new DataTable();
                    dttest.Clear();
                    dttest.Columns.Add("DB");
                    for (int r = 0; r < chlbDB.Items.Count; r++)
                    {
                        if (chlbDB.GetItemCheckState(r) == CheckState.Checked)
                        {
                            if (chbDate.CheckState == CheckState.Checked) { filename = chlbDB.Items[r].ToString() + "_" + lblDate.Text.Replace("/", "") + ".bak"; }
                            else if (chbName.CheckState == CheckState.Checked) { filename = chlbDB.Items[r].ToString() + "_" + txtName.Text + ".bak"; }
                            else { filename = chlbDB.Items[r].ToString() + ".bak"; }
                            i = 0;
                            for (int t = 0; t < dtDBUpdate.Rows.Count; t++)
                            {
                                if (filename != dtDBUpdate.Rows[t][0].ToString())
                                {
                                    i++;
                                }
                            }
                            if (i == (dtDBUpdate.Rows.Count))
                            {
                                prog++;
                                query = "BACKUP DATABASE [" + chlbDB.Items[r].ToString() + "] TO DISK='" + (fbd.SelectedPath + @"\" + filename).Replace(@"\\", @"\") + "'";
                                //textBox1.Text = query;
                                DataRow drtest = dttest.NewRow();
                                drtest["DB"] = filename;
                                dttest.Rows.Add(drtest);
                                //if (Functions.SqlExecuteCmd(query, sqlConn) == true)
                                //{
                                //    lstReporting.Items.Add(chlbDB.Items[r].ToString() + " : true");
                                //    progressBar1.Value = (i * 100) / (Checked_Total + 1);
                                //    lblPersent.Text = progressBar1.Value.ToString()+" %";
                                //}
                                //else lstReporting.Items.Add(chlbDB.Items[r].ToString() + " : false");
                            }
                        }
                    }
                    if (progressBar1.Value != 100)
                    {
                        progressBar1.Value = 100;
                        lblPersent.Text = "100 %";
                        lblFinish.Visible = true;
                    }
                    //dataGridView2.DataSource = dttest;
                    //textBox1.Text = dttest.Rows.Count.ToString();
                }
            }
        }
        void BackupTotal()
        {
            int i = 0;
            for (int w = 0; w < chlbDB.Items.Count; w++)
            {
                if (chlbDB.GetItemCheckState(w) == CheckState.Checked)
                {
                    i++;
                    if (chbDate.CheckState == CheckState.Checked) { filename = chlbDB.Items[w].ToString() + "_" + lblDate.Text.Replace("/", ""); }
                    else if (chbName.CheckState == CheckState.Checked) { filename = chlbDB.Items[w].ToString() + "_" + txtName.Text; }
                    else { filename = filename = chlbDB.Items[w].ToString(); }
                    query = "BACKUP DATABASE [" + chlbDB.Items[w].ToString() + "] TO DISK='" + (fbd.SelectedPath + @"\" + filename).Replace(@"\\", @"\") + ".bak'";
                    //textBox1.Text = query;
                    //if (Functions.SqlExecuteCmd(query, sqlConn) == true)
                    //{
                    //    lstReporting.Items.Add(chlbDB.Items[w].ToString() + " : true");
                    //    progressBar1.Value = (i * 100) / (Checked_Total + 1);
                    //    lblPersent.Text = progressBar1.Value.ToString()+" %";
                    //}
                    //else lstReporting.Items.Add(chlbDB.Items[w].ToString() + " : false");
                }
            }
            if (progressBar1.Value != 100)
            {
                progressBar1.Value = 100;
                lblPersent.Text = "100 %";
                lblFinish.Visible = true;
            }
        }
        private void btnAllSelect_Click(object sender, EventArgs e)
        {

            func.SelectUnselect(chlbDB, btnAllSelect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDate.CheckState == CheckState.Checked)
            {
                chbName.CheckState = CheckState.Unchecked;
                filename = @"\DBName_" + lblDate.Text.Replace(@"\", "") + ".bak";
                txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\"); ;
                txtName.Enabled = false;
            }
            else
            {
                filename = @"\DBName.bak";
                txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\"); ;
            }
        }

        private void chbName_CheckedChanged(object sender, EventArgs e)
        {
            if (chbName.CheckState == CheckState.Checked)
            {
                chbDate.CheckState = CheckState.Unchecked;
                txtName.Enabled = true;
                if (txtName.Text != "")
                {
                    filename = @"\DBName_" + txtName.Text + ".bak";
                    txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\"); ;
                }
            }
            else
            {
                filename = @"\DBName.bak";
                txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\"); ;
                txtName.Enabled = false;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                filename = @"\DBName_" + txtName.Text + ".bak";
                txtFilePath.Text = (filepath + filename).Replace(@"\\", @"\"); ;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(DateTime.Now);
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss ");
        }

        private void محلذخیرهفایلToolStripMenuItem_Click(object sender, EventArgs e)
        {
            void_filepath();
            if (fbd.SelectedPath != "")
            {
                dataGridView1.DataSource = DBfiles(fbd.SelectedPath);
            }
        }

        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void بستنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void پشتیبانگیریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fun_backup();
        }

        private void lstReporting_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
