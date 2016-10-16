using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace Identification
{
    public partial class Form1 : Form
    {
        Functions Functions = new Functions();

        Boolean EnableFrm;
        SqlConnection sqlConnection = new SqlConnection();

        string strUser, strPass, str, Alltext;
        string strPathLoginFolder = @"../Login.pos";


        public Form1()
        {
            InitializeComponent();

            //  Load Server Name
            cmbServer.DataSource = Functions.LoadSvrName(strPathLoginFolder);

        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {

            //  change database
            //if (cmbDBNames.Text != "") sqlConnection = Functions.SqlConnectionChangeDB(cmbDBNames.Text, sqlConnection);
            sqlConnection = (cmbDBNames.Text != "") ? Functions.SqlConnectionChangeDB(cmbDBNames.Text, sqlConnection) : sqlConnection;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConnect_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) { btnConnect_Click(null, null); }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //  desable open other forms
            EnableFrm = false;

            // get server info ==> server , user , pass
            string strServer = cmbServer.Text, strUser = txtUser.Text, strPass = txtPass.Text;


            //  create sql connection            
            sqlConnection = Functions.SqlConnect(strServer, strUser, strPass);

            //  test sql connection
            if (Functions.SqlConnectionTest(sqlConnection))
            {

                #region save login info with encrypt pass

                //  save login info with encrypt pass
                if (chbRemember.CheckState == CheckState.Checked)
                {
                    //  encrypt pass
                    string strPassEncrypt = CryptorEngine.Encrypt(strPass, true);

                    List<string> lst = Functions.ReadTxt(strPathLoginFolder);


                    //  save login when login file info is empty
                    if (File.ReadAllText(strPathLoginFolder) == "")
                    { File.WriteAllText(strPathLoginFolder, strServer + "," + strUser + "," + strPassEncrypt); }


                    //  save or replace login when login file info is not empty
                    if (File.ReadAllText(strPathLoginFolder) != "")
                    {
                        int intFind = Functions.listFind(lst, strServer);

                        if (intFind != -1)
                        {
                            lst[intFind] = strServer + "," + strUser + "," + strPassEncrypt;
                            Functions.saveList(lst, strPathLoginFolder);
                        }

                        else File.WriteAllText(strPathLoginFolder, File.ReadAllText(strPathLoginFolder) + "\r\n" + strServer + "," + strUser + "," + strPassEncrypt);
                    }

                    //  load server name
                    cmbServer.DataSource = lstSvrName(strPathLoginFolder);
                }

                #endregion


                //  load database name  // set source cmbDBNames 
                cmbDBNames.DataSource = Functions.SqlGetDBName(sqlConnection);

                // set btnConnect text
                btnConnect.Text = "اتصال مجدد";

                //  enable open other forms
                EnableFrm = true;

            }

            else   //   if problem in sql connection
            {
                MessageBox.Show("عدم برقراری ارتباط", "!هشدار");
                btnConnect.Text = "اتصال";

                //  desable open other forms
                EnableFrm = false;
            }

            Cursor.Current = Cursors.Default;
            //Connect();
        }

        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            //  read login file
            FileStream fileStream = new FileStream(strPathLoginFolder, FileMode.Open, FileAccess.Read);

            #region Server Info Substring

            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {

                    str = line.Substring(0, line.IndexOf(","));
                    Alltext = line.Replace(str + ",", "").Trim();
                    if (cmbServer.Text == str)
                    {
                        strUser = Alltext.Substring(0, Alltext.IndexOf(","));
                        strPass = Alltext.Replace(strUser + ",", "").Trim();
                    }
                }
            }

            #endregion

            #region User & Pass to textbox


            if (cmbServer.Text == ".")
            {
                txtUser.Text = "";
                txtPass.Text = "";
            }
            else
            {
                txtUser.Text = strUser;
                txtPass.Text = strPass;
            }

            #endregion

        }

        private void بازکردنفرماستانداردسازیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EnableFrm == true)
            {
                //  form estandard & show
                Estandard frmEst = new Estandard(sqlConnection);
                frmEst.ShowDialog();
            }
        }

        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void اتصالToolStripMenuItem_Click(object sender, EventArgs e)
        { btnConnect_Click(null, null); }

        private void احرازهویتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EnableFrm == true)
            {
                //  form identification & show
                SetPerson SPFrm = new SetPerson(sqlConnection);
                SPFrm.ShowDialog();
            }
        }

        private void یونیکToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uniqe unFrm = new Uniqe(sqlConnection);
            unFrm.ShowDialog();
        }

        private void نمایشاطلاعاتخالیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoColumns frmInfoClm = new InfoColumns(sqlConnection);
            frmInfoClm.ShowDialog();
        }

        private void پیداکردنتکراریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDuplicate frmfd = new FindDuplicate(sqlConnection);
            frmfd.ShowDialog();
        }

        private void پشتیبانگیریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  form backup from sql
            SQLDBbackup sqlbackup = new SQLDBbackup(sqlConnection);
            sqlbackup.ShowDialog();
        }

        private void سرورمحلیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }


        //************      Functions


        #region lstSvrName
        /// <summary>
        /// get server name from file
        /// </summary>
        /// <param name="strPathLogin">login file path</param>
        /// <returns>List</returns>
        private List<string> lstSvrName(string strPathLogin)
        {
            List<string> lstSvrName = new List<string>();

            //  load server names into cmbServer
            if (File.Exists(strPathLogin))
            {
                lstSvrName = Functions.ReadTxt(strPathLogin);

                lstSvrName.Insert(0, "., ,");

                //  get server from read line
                for (int i = 0; i < lstSvrName.Count; i++)
                { lstSvrName[i] = lstSvrName[i].Substring(0, lstSvrName[i].IndexOf(",")); }

            }

            return lstSvrName;
        }
        #endregion


    }
}
