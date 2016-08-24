namespace Identification
{
    partial class NullSearch
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.DGVSearch = new System.Windows.Forms.DataGridView();
            this.cmbDBName = new System.Windows.Forms.ComboBox();
            this.cmbTBName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelField = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(534, 33);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "جستجو";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // DGVSearch
            // 
            this.DGVSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.DGVSearch.Location = new System.Drawing.Point(12, 120);
            this.DGVSearch.Name = "DGVSearch";
            this.DGVSearch.Size = new System.Drawing.Size(735, 307);
            this.DGVSearch.TabIndex = 1;
            this.DGVSearch.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVSearch_CellContentClick);
            // 
            // cmbDBName
            // 
            this.cmbDBName.FormattingEnabled = true;
            this.cmbDBName.Location = new System.Drawing.Point(119, 35);
            this.cmbDBName.Name = "cmbDBName";
            this.cmbDBName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbDBName.Size = new System.Drawing.Size(230, 21);
            this.cmbDBName.TabIndex = 2;
            this.cmbDBName.SelectedIndexChanged += new System.EventHandler(this.cmbDBName_SelectedIndexChanged);
            // 
            // cmbTBName
            // 
            this.cmbTBName.FormattingEnabled = true;
            this.cmbTBName.Location = new System.Drawing.Point(119, 75);
            this.cmbTBName.Name = "cmbTBName";
            this.cmbTBName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbTBName.Size = new System.Drawing.Size(230, 21);
            this.cmbTBName.TabIndex = 3;
            this.cmbTBName.SelectedIndexChanged += new System.EventHandler(this.cmbTBName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "بانک";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "جدول";
            // 
            // btnDelField
            // 
            this.btnDelField.Location = new System.Drawing.Point(517, 73);
            this.btnDelField.Name = "btnDelField";
            this.btnDelField.Size = new System.Drawing.Size(121, 23);
            this.btnDelField.TabIndex = 6;
            this.btnDelField.Text = "حذف فیلدهای خالی";
            this.btnDelField.UseVisualStyleBackColor = true;
            this.btnDelField.Click += new System.EventHandler(this.btnDelField_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(400, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "نمایش";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Text = "نمایش";
            this.Column1.ToolTipText = "نمایش";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "فیلد";
            this.Column2.Name = "Column2";
            // 
            // NullSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 439);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnDelField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTBName);
            this.Controls.Add(this.cmbDBName);
            this.Controls.Add(this.DGVSearch);
            this.Controls.Add(this.btnSearch);
            this.Name = "NullSearch";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "جستجوی اطلاعات خالی";
            this.Load += new System.EventHandler(this.NullSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView DGVSearch;
        private System.Windows.Forms.ComboBox cmbDBName;
        private System.Windows.Forms.ComboBox cmbTBName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelField;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column2;
    }
}