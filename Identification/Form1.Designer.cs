namespace Identification
{
    partial class Form1
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDBNames = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.پروندهToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.بازکردنفرماستانداردسازیToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.احرازمکانToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.احرازهویتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ثبتتلفنToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.یونیکToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.نمایشاطلاعاتخالیToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.پیداکردنتکراریToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.گزارشازکلبانکToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.خروجToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ابزارهاToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.اتصالToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.سرورمحلیToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.پشتیبانگیریToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.chbRemember = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(168, 209);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(106, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "سرور محلی (Ctrl+E)";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(43, 209);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(88, 23);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "اتصال (Ctrl+R)";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(142, 92);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(250, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(142, 118);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(250, 20);
            this.txtPass.TabIndex = 3;
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(142, 65);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbServer.Size = new System.Drawing.Size(250, 21);
            this.cmbServer.TabIndex = 1;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "نام سرور";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "کاربری";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "رمز";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "بانکها";
            // 
            // cmbDBNames
            // 
            this.cmbDBNames.FormattingEnabled = true;
            this.cmbDBNames.Location = new System.Drawing.Point(142, 145);
            this.cmbDBNames.Name = "cmbDBNames";
            this.cmbDBNames.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbDBNames.Size = new System.Drawing.Size(250, 21);
            this.cmbDBNames.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.پروندهToolStripMenuItem,
            this.ابزارهاToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(423, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // پروندهToolStripMenuItem
            // 
            this.پروندهToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.بازکردنفرماستانداردسازیToolStripMenuItem,
            this.احرازمکانToolStripMenuItem,
            this.احرازهویتToolStripMenuItem,
            this.ثبتتلفنToolStripMenuItem,
            this.یونیکToolStripMenuItem,
            this.نمایشاطلاعاتخالیToolStripMenuItem,
            this.پیداکردنتکراریToolStripMenuItem,
            this.گزارشازکلبانکToolStripMenuItem,
            this.خروجToolStripMenuItem});
            this.پروندهToolStripMenuItem.Name = "پروندهToolStripMenuItem";
            this.پروندهToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.پروندهToolStripMenuItem.Text = "پرونده";
            // 
            // بازکردنفرماستانداردسازیToolStripMenuItem
            // 
            this.بازکردنفرماستانداردسازیToolStripMenuItem.Name = "بازکردنفرماستانداردسازیToolStripMenuItem";
            this.بازکردنفرماستانداردسازیToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.بازکردنفرماستانداردسازیToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.بازکردنفرماستانداردسازیToolStripMenuItem.Text = "استاندارد سازی";
            this.بازکردنفرماستانداردسازیToolStripMenuItem.Click += new System.EventHandler(this.بازکردنفرماستانداردسازیToolStripMenuItem_Click);
            // 
            // احرازمکانToolStripMenuItem
            // 
            this.احرازمکانToolStripMenuItem.Name = "احرازمکانToolStripMenuItem";
            this.احرازمکانToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.احرازمکانToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.احرازمکانToolStripMenuItem.Text = "احراز موقعیت جغرافیایی";
            // 
            // احرازهویتToolStripMenuItem
            // 
            this.احرازهویتToolStripMenuItem.Name = "احرازهویتToolStripMenuItem";
            this.احرازهویتToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.احرازهویتToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.احرازهویتToolStripMenuItem.Text = "احراز هویت اشخاص";
            this.احرازهویتToolStripMenuItem.Click += new System.EventHandler(this.احرازهویتToolStripMenuItem_Click);
            // 
            // ثبتتلفنToolStripMenuItem
            // 
            this.ثبتتلفنToolStripMenuItem.Name = "ثبتتلفنToolStripMenuItem";
            this.ثبتتلفنToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.ثبتتلفنToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.ثبتتلفنToolStripMenuItem.Text = "ثبت تلفن ها";
            // 
            // یونیکToolStripMenuItem
            // 
            this.یونیکToolStripMenuItem.Name = "یونیکToolStripMenuItem";
            this.یونیکToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.یونیکToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.یونیکToolStripMenuItem.Text = "یکتا سازی";
            this.یونیکToolStripMenuItem.Click += new System.EventHandler(this.یونیکToolStripMenuItem_Click);
            // 
            // نمایشاطلاعاتخالیToolStripMenuItem
            // 
            this.نمایشاطلاعاتخالیToolStripMenuItem.Name = "نمایشاطلاعاتخالیToolStripMenuItem";
            this.نمایشاطلاعاتخالیToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.نمایشاطلاعاتخالیToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.نمایشاطلاعاتخالیToolStripMenuItem.Text = "نمایش تعداد رکوردها";
            this.نمایشاطلاعاتخالیToolStripMenuItem.Click += new System.EventHandler(this.نمایشاطلاعاتخالیToolStripMenuItem_Click);
            // 
            // پیداکردنتکراریToolStripMenuItem
            // 
            this.پیداکردنتکراریToolStripMenuItem.Name = "پیداکردنتکراریToolStripMenuItem";
            this.پیداکردنتکراریToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.پیداکردنتکراریToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.پیداکردنتکراریToolStripMenuItem.Text = "پیدا کردن تکراری";
            this.پیداکردنتکراریToolStripMenuItem.Click += new System.EventHandler(this.پیداکردنتکراریToolStripMenuItem_Click);
            // 
            // گزارشازکلبانکToolStripMenuItem
            // 
            this.گزارشازکلبانکToolStripMenuItem.Name = "گزارشازکلبانکToolStripMenuItem";
            this.گزارشازکلبانکToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.گزارشازکلبانکToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.گزارشازکلبانکToolStripMenuItem.Text = "گزارش از کل بانک";
            this.گزارشازکلبانکToolStripMenuItem.Click += new System.EventHandler(this.گزارشازکلبانکToolStripMenuItem_Click);
            // 
            // خروجToolStripMenuItem
            // 
            this.خروجToolStripMenuItem.Name = "خروجToolStripMenuItem";
            this.خروجToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.خروجToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.خروجToolStripMenuItem.Text = "خروج";
            this.خروجToolStripMenuItem.Click += new System.EventHandler(this.خروجToolStripMenuItem_Click);
            // 
            // ابزارهاToolStripMenuItem
            // 
            this.ابزارهاToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.اتصالToolStripMenuItem,
            this.سرورمحلیToolStripMenuItem,
            this.پشتیبانگیریToolStripMenuItem});
            this.ابزارهاToolStripMenuItem.Name = "ابزارهاToolStripMenuItem";
            this.ابزارهاToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ابزارهاToolStripMenuItem.Text = "ابزارها";
            // 
            // اتصالToolStripMenuItem
            // 
            this.اتصالToolStripMenuItem.Name = "اتصالToolStripMenuItem";
            this.اتصالToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.اتصالToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.اتصالToolStripMenuItem.Text = "اتصال";
            this.اتصالToolStripMenuItem.Click += new System.EventHandler(this.اتصالToolStripMenuItem_Click);
            // 
            // سرورمحلیToolStripMenuItem
            // 
            this.سرورمحلیToolStripMenuItem.Name = "سرورمحلیToolStripMenuItem";
            this.سرورمحلیToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.سرورمحلیToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.سرورمحلیToolStripMenuItem.Text = "سرور محلی";
            this.سرورمحلیToolStripMenuItem.Click += new System.EventHandler(this.سرورمحلیToolStripMenuItem_Click);
            // 
            // پشتیبانگیریToolStripMenuItem
            // 
            this.پشتیبانگیریToolStripMenuItem.Name = "پشتیبانگیریToolStripMenuItem";
            this.پشتیبانگیریToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.پشتیبانگیریToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.پشتیبانگیریToolStripMenuItem.Text = "پشتیبان گیری";
            this.پشتیبانگیریToolStripMenuItem.Click += new System.EventHandler(this.پشتیبانگیریToolStripMenuItem_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(304, 209);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "خروج";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.SystemColors.Control;
            this.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsg.Location = new System.Drawing.Point(142, 39);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(250, 13);
            this.txtMsg.TabIndex = 17;
            // 
            // chbRemember
            // 
            this.chbRemember.AutoSize = true;
            this.chbRemember.Location = new System.Drawing.Point(142, 176);
            this.chbRemember.Name = "chbRemember";
            this.chbRemember.Size = new System.Drawing.Size(112, 17);
            this.chbRemember.TabIndex = 6;
            this.chbRemember.Text = "به خاطر سپردن رمز";
            this.chbRemember.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(357, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "label4";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "label6";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(423, 253);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chbRemember);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.cmbDBNames);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "اتصال به بانک";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDBNames;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.CheckBox chbRemember;
        private System.Windows.Forms.ToolStripMenuItem پروندهToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem خروجToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem بازکردنفرماستانداردسازیToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ابزارهاToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem اتصالToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem سرورمحلیToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem احرازهویتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ثبتتلفنToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem احرازمکانToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem یونیکToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem نمایشاطلاعاتخالیToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem پشتیبانگیریToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem پیداکردنتکراریToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem گزارشازکلبانکToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
    }
}

