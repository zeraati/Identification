namespace Identification
{
    partial class SQLDBbackup
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
            this.components = new System.ComponentModel.Container();
            this.chlbDB = new System.Windows.Forms.CheckedListBox();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblConnect = new System.Windows.Forms.Label();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnAllSelect = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chbName = new System.Windows.Forms.CheckBox();
            this.chbDate = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.پروندهToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.بستنToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.خروجToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ابزارهاToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.محلذخیرهفایلToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.پشتیبانگیریToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstReporting = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFinish = new System.Windows.Forms.Label();
            this.lblPersent = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chlbDB
            // 
            this.chlbDB.CheckOnClick = true;
            this.chlbDB.Enabled = false;
            this.chlbDB.FormattingEnabled = true;
            this.chlbDB.Location = new System.Drawing.Point(599, 100);
            this.chlbDB.Name = "chlbDB";
            this.chlbDB.Size = new System.Drawing.Size(341, 274);
            this.chlbDB.TabIndex = 0;
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(241, 38);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(116, 31);
            this.btnFilePath.TabIndex = 1;
            this.btnFilePath.Text = "محل ذخیره فایل";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(22, 392);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "خروج";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(380, 44);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFilePath.Size = new System.Drawing.Size(466, 20);
            this.txtFilePath.TabIndex = 2;
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Location = new System.Drawing.Point(474, 11);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(108, 13);
            this.lblConnect.TabIndex = 3;
            this.lblConnect.Text = "نمایش مشخصات اتصال";
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(117, 392);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(84, 23);
            this.btnBackup.TabIndex = 4;
            this.btnBackup.Text = "پشتیبان گیری";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnAllSelect
            // 
            this.btnAllSelect.Location = new System.Drawing.Point(599, 71);
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Size = new System.Drawing.Size(75, 23);
            this.btnAllSelect.TabIndex = 6;
            this.btnAllSelect.Text = "انتخاب همه";
            this.btnAllSelect.UseVisualStyleBackColor = true;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(32, 63);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 7;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // chbName
            // 
            this.chbName.AutoSize = true;
            this.chbName.Location = new System.Drawing.Point(138, 66);
            this.chbName.Name = "chbName";
            this.chbName.Size = new System.Drawing.Size(15, 14);
            this.chbName.TabIndex = 8;
            this.chbName.UseVisualStyleBackColor = true;
            this.chbName.CheckedChanged += new System.EventHandler(this.chbName_CheckedChanged);
            // 
            // chbDate
            // 
            this.chbDate.AutoSize = true;
            this.chbDate.Location = new System.Drawing.Point(102, 29);
            this.chbDate.Name = "chbDate";
            this.chbDate.Size = new System.Drawing.Size(51, 17);
            this.chbDate.TabIndex = 9;
            this.chbDate.Text = "تاریخ";
            this.chbDate.UseVisualStyleBackColor = true;
            this.chbDate.CheckedChanged += new System.EventHandler(this.chbDate_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbDate);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.chbName);
            this.groupBox1.Location = new System.Drawing.Point(27, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "قسمت آخر نام فایل پشتیبان";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 392);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "بستن";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Enabled = false;
            this.progressBar1.Location = new System.Drawing.Point(467, 392);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(473, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "ساعت ";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(171, 81);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(40, 13);
            this.lblTime.TabIndex = 14;
            this.lblTime.Text = "lblTime";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "تاریخ امروز";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(169, 110);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(40, 13);
            this.lblDate.TabIndex = 16;
            this.lblDate.Text = "lblDate";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(241, 100);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(352, 274);
            this.dataGridView1.TabIndex = 17;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.پروندهToolStripMenuItem,
            this.ابزارهاToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // پروندهToolStripMenuItem
            // 
            this.پروندهToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.بستنToolStripMenuItem,
            this.خروجToolStripMenuItem});
            this.پروندهToolStripMenuItem.Name = "پروندهToolStripMenuItem";
            this.پروندهToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.پروندهToolStripMenuItem.Text = "پرونده";
            // 
            // بستنToolStripMenuItem
            // 
            this.بستنToolStripMenuItem.Name = "بستنToolStripMenuItem";
            this.بستنToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.بستنToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.بستنToolStripMenuItem.Text = "بستن";
            this.بستنToolStripMenuItem.Click += new System.EventHandler(this.بستنToolStripMenuItem_Click);
            // 
            // خروجToolStripMenuItem
            // 
            this.خروجToolStripMenuItem.Name = "خروجToolStripMenuItem";
            this.خروجToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.خروجToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.خروجToolStripMenuItem.Text = "خروج";
            this.خروجToolStripMenuItem.Click += new System.EventHandler(this.خروجToolStripMenuItem_Click);
            // 
            // ابزارهاToolStripMenuItem
            // 
            this.ابزارهاToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.محلذخیرهفایلToolStripMenuItem,
            this.پشتیبانگیریToolStripMenuItem});
            this.ابزارهاToolStripMenuItem.Name = "ابزارهاToolStripMenuItem";
            this.ابزارهاToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.ابزارهاToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.ابزارهاToolStripMenuItem.Text = "ابزارها";
            // 
            // محلذخیرهفایلToolStripMenuItem
            // 
            this.محلذخیرهفایلToolStripMenuItem.Name = "محلذخیرهفایلToolStripMenuItem";
            this.محلذخیرهفایلToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.محلذخیرهفایلToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.محلذخیرهفایلToolStripMenuItem.Text = "محل ذخیره فایل";
            this.محلذخیرهفایلToolStripMenuItem.Click += new System.EventHandler(this.محلذخیرهفایلToolStripMenuItem_Click);
            // 
            // پشتیبانگیریToolStripMenuItem
            // 
            this.پشتیبانگیریToolStripMenuItem.Name = "پشتیبانگیریToolStripMenuItem";
            this.پشتیبانگیریToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.پشتیبانگیریToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.پشتیبانگیریToolStripMenuItem.Text = "پشتیبان گیری";
            this.پشتیبانگیریToolStripMenuItem.Click += new System.EventHandler(this.پشتیبانگیریToolStripMenuItem_Click);
            // 
            // lstReporting
            // 
            this.lstReporting.FormattingEnabled = true;
            this.lstReporting.HorizontalScrollbar = true;
            this.lstReporting.Location = new System.Drawing.Point(12, 266);
            this.lstReporting.Name = "lstReporting";
            this.lstReporting.Size = new System.Drawing.Size(225, 108);
            this.lstReporting.TabIndex = 19;
            this.lstReporting.SelectedIndexChanged += new System.EventHandler(this.lstReporting_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "بانک های موجود در پوشه انتخابی";
            // 
            // lblFinish
            // 
            this.lblFinish.AutoSize = true;
            this.lblFinish.Location = new System.Drawing.Point(302, 397);
            this.lblFinish.Name = "lblFinish";
            this.lblFinish.Size = new System.Drawing.Size(55, 13);
            this.lblFinish.TabIndex = 21;
            this.lblFinish.Text = "پایان یافت";
            // 
            // lblPersent
            // 
            this.lblPersent.AutoSize = true;
            this.lblPersent.Location = new System.Drawing.Point(408, 397);
            this.lblPersent.Name = "lblPersent";
            this.lblPersent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPersent.Size = new System.Drawing.Size(0, 13);
            this.lblPersent.TabIndex = 22;
            // 
            // SQLDBbackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 424);
            this.Controls.Add(this.lblPersent);
            this.Controls.Add(this.lblFinish);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstReporting);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAllSelect);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.lblConnect);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnFilePath);
            this.Controls.Add(this.chlbDB);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SQLDBbackup";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "SQLDBbackup";
            this.Load += new System.EventHandler(this.SQLDBbackup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chlbDB;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnAllSelect;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chbName;
        private System.Windows.Forms.CheckBox chbDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem پروندهToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem بستنToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem خروجToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ابزارهاToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem محلذخیرهفایلToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem پشتیبانگیریToolStripMenuItem;
        private System.Windows.Forms.ListBox lstReporting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFinish;
        private System.Windows.Forms.Label lblPersent;
    }
}