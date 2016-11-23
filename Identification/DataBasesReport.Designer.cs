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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataBasesReport));
            this.btnRun = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.lstbxMain = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.trvDatabase = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnAddToList = new System.Windows.Forms.Button();
            this.btnDelFromList = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // lstbxMain
            // 
            this.lstbxMain.FormattingEnabled = true;
            this.lstbxMain.Location = new System.Drawing.Point(6, 8);
            this.lstbxMain.Name = "lstbxMain";
            this.lstbxMain.Size = new System.Drawing.Size(256, 173);
            this.lstbxMain.TabIndex = 17;
            this.lstbxMain.DragOver += new System.Windows.Forms.DragEventHandler(this.lstbxMain_DragOver);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(12, 233);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 255);
            this.tabControl1.TabIndex = 22;
            // 
            // trvDatabase
            // 
            this.trvDatabase.ImageIndex = 0;
            this.trvDatabase.ImageList = this.imageListTreeView;
            this.trvDatabase.Location = new System.Drawing.Point(5, 11);
            this.trvDatabase.Name = "trvDatabase";
            this.trvDatabase.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trvDatabase.RightToLeftLayout = true;
            this.trvDatabase.SelectedImageIndex = 0;
            this.trvDatabase.Size = new System.Drawing.Size(262, 173);
            this.trvDatabase.TabIndex = 23;
            this.trvDatabase.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvDatabase_AfterSelect);
            this.trvDatabase.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvDatabase_DragDrop);
            this.trvDatabase.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvDatabase_DragEnter);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "server.png");
            this.imageList1.Images.SetKeyName(1, "database.png");
            this.imageList1.Images.SetKeyName(2, "table.png");
            this.imageList1.Images.SetKeyName(3, "column.png");
            // 
            // btnAddToList
            // 
            this.btnAddToList.Location = new System.Drawing.Point(268, 32);
            this.btnAddToList.Name = "btnAddToList";
            this.btnAddToList.Size = new System.Drawing.Size(50, 53);
            this.btnAddToList.TabIndex = 26;
            this.btnAddToList.Text = "اضافه کردن (alt+a)";
            this.btnAddToList.UseVisualStyleBackColor = true;
            this.btnAddToList.Click += new System.EventHandler(this.btnAddToList_Click);
            this.btnAddToList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnAddToList_KeyDown);
            // 
            // btnDelFromList
            // 
            this.btnDelFromList.Location = new System.Drawing.Point(268, 100);
            this.btnDelFromList.Name = "btnDelFromList";
            this.btnDelFromList.Size = new System.Drawing.Size(50, 49);
            this.btnDelFromList.TabIndex = 27;
            this.btnDelFromList.Text = "حذف از لیست (alt+d)";
            this.btnDelFromList.UseVisualStyleBackColor = true;
            this.btnDelFromList.Click += new System.EventHandler(this.btnDelFromList_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 43);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvDatabase);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(604, 187);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 28;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstbxMain);
            this.groupBox1.Controls.Add(this.btnDelFromList);
            this.groupBox1.Controls.Add(this.btnAddToList);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 181);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "database.png");
            this.imageListTreeView.Images.SetKeyName(1, "");
            this.imageListTreeView.Images.SetKeyName(2, "table.png");
            this.imageListTreeView.Images.SetKeyName(3, "column.png");
            // 
            // DataBasesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 500);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DataBasesReport";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "DataBasesReport";
            this.Load += new System.EventHandler(this.DataBasesReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataBasesReport_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.ListBox lstbxMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TreeView trvDatabase;
        private System.Windows.Forms.Button btnAddToList;
        private System.Windows.Forms.Button btnDelFromList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.ImageList imageList1;
    }
}