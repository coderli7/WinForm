namespace ChinaLifeTools
{
    partial class ChinaLifeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChinaLifeForm));
            this.startProxyButton = new System.Windows.Forms.Button();
            this.closeProxyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameTxt = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkPwd = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.logTxt = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startProxyButton
            // 
            this.startProxyButton.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startProxyButton.Location = new System.Drawing.Point(80, 101);
            this.startProxyButton.Name = "startProxyButton";
            this.startProxyButton.Size = new System.Drawing.Size(162, 66);
            this.startProxyButton.TabIndex = 0;
            this.startProxyButton.Text = "启动代理";
            this.startProxyButton.UseVisualStyleBackColor = true;
            this.startProxyButton.Click += new System.EventHandler(this.startProxyButton_Click);
            // 
            // closeProxyButton
            // 
            this.closeProxyButton.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.closeProxyButton.Location = new System.Drawing.Point(249, 101);
            this.closeProxyButton.Name = "closeProxyButton";
            this.closeProxyButton.Size = new System.Drawing.Size(162, 66);
            this.closeProxyButton.TabIndex = 1;
            this.closeProxyButton.Text = "关闭代理";
            this.closeProxyButton.UseVisualStyleBackColor = true;
            this.closeProxyButton.Click += new System.EventHandler(this.closeProxyButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "工号";
            // 
            // usernameTxt
            // 
            this.usernameTxt.Location = new System.Drawing.Point(84, 46);
            this.usernameTxt.Name = "usernameTxt";
            this.usernameTxt.Size = new System.Drawing.Size(327, 21);
            this.usernameTxt.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(452, 240);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkPwd);
            this.tabPage1.Controls.Add(this.startProxyButton);
            this.tabPage1.Controls.Add(this.usernameTxt);
            this.tabPage1.Controls.Add(this.closeProxyButton);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(444, 214);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "代理工具";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkPwd
            // 
            this.chkPwd.AutoSize = true;
            this.chkPwd.Location = new System.Drawing.Point(84, 73);
            this.chkPwd.Name = "chkPwd";
            this.chkPwd.Size = new System.Drawing.Size(72, 16);
            this.chkPwd.TabIndex = 4;
            this.chkPwd.Text = "记住工号";
            this.chkPwd.UseVisualStyleBackColor = true;
            this.chkPwd.CheckedChanged += new System.EventHandler(this.chkPwd_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.logTxt);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(444, 214);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "操作日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // logTxt
            // 
            this.logTxt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.logTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTxt.Location = new System.Drawing.Point(3, 3);
            this.logTxt.Name = "logTxt";
            this.logTxt.Size = new System.Drawing.Size(438, 208);
            this.logTxt.TabIndex = 0;
            this.logTxt.Text = "";
            // 
            // ChinaLifeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 240);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ChinaLifeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "核心系统代理工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChinaLifeForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChinaLifeForm_FormClosed);
            this.Load += new System.EventHandler(this.ChinaLifeForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startProxyButton;
        private System.Windows.Forms.Button closeProxyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usernameTxt;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox logTxt;
        private System.Windows.Forms.CheckBox chkPwd;
    }
}