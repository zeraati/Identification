namespace Identification
{
    partial class DataBasesReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRun = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.lstDatabase = new System.Windows.Forms.ListBox();
            this.lstTableName = new System.Windows.Forms.ListBox();
            this.lstColumnName = new System.Windows.Forms.ListBox();
            this.lstbxMain = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(23, 14);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "نمایش";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(133, 14);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 2;
            this.btnExcel.Text = "اکسپورت";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // lstDatabase
            // 
            this.lstDatabase.FormattingEnabled = true;
            this.lstDatabase.Location = new System.Drawing.Point(12, 49);
            this.lstDatabase.Name = "lstDatabase";
            this.lstDatabase.Size = new System.Drawing.Size(184, 173);
            this.lstDatabase.TabIndex = 14;
            this.lstDatabase.SelectedIndexChanged += new System.EventHandler(this.lstDatabase_SelectedIndexChanged);
            this.lstDatabase.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstDatabase_MouseDoubleClick);
            // 
            // lstTableName
            // 
            this.lstTableName.FormattingEnabled = true;
            this.lstTableName.Location = new System.Drawing.Point(220, 49);
            this.lstTableName.Name = "lstTableName";
            this.lstTableName.Size = new System.Drawing.Size(184, 82);
            this.lstTableName.TabIndex = 15;
            this.lstTableName.SelectedIndexChanged += new System.EventHandler(this.lstTableName_SelectedIndexChanged);
            // 
            // lstColumnName
            // 
            this.lstColumnName.FormattingEnabled = true;
            this.lstColumnName.Location = new System.Drawing.Point(221, 137);
            this.lstColumnName.Name = "lstColumnName";
            this.lstColumnName.Size = new System.Drawing.Size(184, 82);
            this.lstColumnName.TabIndex = 16;
            // 
            // lstbxMain
            // 
            this.lstbxMain.FormattingEnabled = true;
            this.lstbxMain.Location = new System.Drawing.Point(428, 49);
            this.lstbxMain.Name = "lstbxMain";
            this.lstbxMain.Size = new System.Drawing.Size(184, 173);
            this.lstbxMain.TabIndex = 17;
            this.lstbxMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstbxMain_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(12, 228);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 260);
            this.tabControl1.TabIndex = 22;
            // 
            // DataBasesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 500);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lstbxMain);
            this.Controls.Add(this.lstColumnName);
            this.Controls.Add(this.lstTableName);
            this.Controls.Add(this.lstDatabase);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DataBasesReport";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "DataBasesReport";
            this.Load += new System.EventHandler(this.DataBasesReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.ListBox lstDatabase;
        private System.Windows.Forms.ListBox lstTableName;
        private System.Windows.Forms.ListBox lstColumnName;
        private System.Windows.Forms.ListBox lstbxMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.FolderBrowserDialog FBD;
    }
}