namespace Identification
{
    partial class FindDuplicate
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.cmbDBName = new System.Windows.Forms.ComboBox();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.cmbColumnName = new System.Windows.Forms.ComboBox();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(169, 47);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "اجرا";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(31, 26);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(131, 109);
            this.checkedListBox1.TabIndex = 1;
            // 
            // cmbDBName
            // 
            this.cmbDBName.FormattingEnabled = true;
            this.cmbDBName.Location = new System.Drawing.Point(732, 37);
            this.cmbDBName.Name = "cmbDBName";
            this.cmbDBName.Size = new System.Drawing.Size(121, 21);
            this.cmbDBName.TabIndex = 2;
            this.cmbDBName.SelectedIndexChanged += new System.EventHandler(this.cmbDBName_SelectedIndexChanged);
            // 
            // cmbTableName
            // 
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(550, 49);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(121, 21);
            this.cmbTableName.TabIndex = 3;
            this.cmbTableName.SelectedIndexChanged += new System.EventHandler(this.cmbTableName_SelectedIndexChanged);
            // 
            // cmbColumnName
            // 
            this.cmbColumnName.FormattingEnabled = true;
            this.cmbColumnName.Location = new System.Drawing.Point(389, 50);
            this.cmbColumnName.Name = "cmbColumnName";
            this.cmbColumnName.Size = new System.Drawing.Size(121, 21);
            this.cmbColumnName.TabIndex = 4;
            // 
            // dgvSearch
            // 
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Location = new System.Drawing.Point(29, 182);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvSearch.Size = new System.Drawing.Size(660, 256);
            this.dgvSearch.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(695, 182);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 256);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkedListBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 230);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 230);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 31);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(180, 164);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // FindDuplicate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 480);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.dgvSearch);
            this.Controls.Add(this.cmbColumnName);
            this.Controls.Add(this.cmbTableName);
            this.Controls.Add(this.cmbDBName);
            this.Controls.Add(this.btnRun);
            this.Name = "FindDuplicate";
            this.Text = "FindDuplicate";
            this.Load += new System.EventHandler(this.FindDuplicate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ComboBox cmbDBName;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.ComboBox cmbColumnName;
        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}