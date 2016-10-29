namespace Identification
{
    partial class Uniqe
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
            //Variables.dbname = "";
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbDBName = new System.Windows.Forms.ComboBox();
            this.cmbTBName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCheck = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbField = new System.Windows.Forms.CheckedListBox();
            this.btnAllSelect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lstReport = new System.Windows.Forms.ListBox();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chCreateID = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbDBName
            // 
            this.cmbDBName.FormattingEnabled = true;
            this.cmbDBName.Location = new System.Drawing.Point(109, 38);
            this.cmbDBName.Name = "cmbDBName";
            this.cmbDBName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbDBName.Size = new System.Drawing.Size(220, 21);
            this.cmbDBName.TabIndex = 0;
            this.cmbDBName.SelectedIndexChanged += new System.EventHandler(this.cmbDBName_SelectedIndexChanged);
            // 
            // cmbTBName
            // 
            this.cmbTBName.FormattingEnabled = true;
            this.cmbTBName.Location = new System.Drawing.Point(109, 65);
            this.cmbTBName.Name = "cmbTBName";
            this.cmbTBName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbTBName.Size = new System.Drawing.Size(220, 21);
            this.cmbTBName.TabIndex = 1;
            this.cmbTBName.SelectedIndexChanged += new System.EventHandler(this.cmbTBName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "بانک";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "جدول";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(378, 38);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 43);
            this.btnCheck.TabIndex = 4;
            this.btnCheck.Text = "بررسی";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbField);
            this.groupBox1.Location = new System.Drawing.Point(10, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 272);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "انتخاب فیلد";
            // 
            // clbField
            // 
            this.clbField.CheckOnClick = true;
            this.clbField.FormattingEnabled = true;
            this.clbField.HorizontalScrollbar = true;
            this.clbField.Location = new System.Drawing.Point(6, 19);
            this.clbField.Name = "clbField";
            this.clbField.Size = new System.Drawing.Size(217, 244);
            this.clbField.TabIndex = 0;
            // 
            // btnAllSelect
            // 
            this.btnAllSelect.Location = new System.Drawing.Point(78, 145);
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Size = new System.Drawing.Size(75, 23);
            this.btnAllSelect.TabIndex = 6;
            this.btnAllSelect.Text = "انتخاب همه";
            this.btnAllSelect.UseVisualStyleBackColor = true;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(586, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "یکتا کردن";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstReport
            // 
            this.lstReport.FormattingEnabled = true;
            this.lstReport.Location = new System.Drawing.Point(248, 193);
            this.lstReport.Name = "lstReport";
            this.lstReport.Size = new System.Drawing.Size(205, 251);
            this.lstReport.TabIndex = 8;
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(109, 92);
            this.cmbField.Name = "cmbField";
            this.cmbField.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbField.Size = new System.Drawing.Size(218, 21);
            this.cmbField.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "براساس فیلد";
            // 
            // chCreateID
            // 
            this.chCreateID.AutoSize = true;
            this.chCreateID.Location = new System.Drawing.Point(333, 94);
            this.chCreateID.Name = "chCreateID";
            this.chCreateID.Size = new System.Drawing.Size(89, 17);
            this.chCreateID.TabIndex = 11;
            this.chCreateID.Text = "ساخت فیلد ID";
            this.chCreateID.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(459, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "بیشتر";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(498, 204);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox1.Size = new System.Drawing.Size(195, 21);
            this.comboBox1.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(462, 284);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(238, 80);
            this.textBox1.TabIndex = 14;
            // 
            // Uniqe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 456);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chCreateID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.lstReport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAllSelect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTBName);
            this.Controls.Add(this.cmbDBName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Uniqe";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "یکتا سازی";
            this.Load += new System.EventHandler(this.Uniqe_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDBName;
        private System.Windows.Forms.ComboBox cmbTBName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbField;
        private System.Windows.Forms.Button btnAllSelect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstReport;
        private System.Windows.Forms.ComboBox cmbField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chCreateID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}