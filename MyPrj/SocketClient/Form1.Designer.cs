namespace SocketClient
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(350, 393);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(125, 23);
            this.btnSend.TabIndex = 19;
            this.btnSend.Text = "发送消息";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(29, 211);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(587, 118);
            this.txtMsg.TabIndex = 18;
            this.txtMsg.Text = "";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(29, 73);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(587, 118);
            this.txtLog.TabIndex = 17;
            this.txtLog.Text = "";
            // 
            // txtPath
            // 
            this.txtPath.Enabled = false;
            this.txtPath.Location = new System.Drawing.Point(29, 356);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(297, 21);
            this.txtPath.TabIndex = 16;
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(481, 354);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSendFile.Size = new System.Drawing.Size(135, 23);
            this.btnSendFile.TabIndex = 15;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(350, 354);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(125, 23);
            this.btnSelect.TabIndex = 14;
            this.btnSelect.Text = "选择文件";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cboUsers
            // 
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(495, 31);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(121, 20);
            this.cboUsers.TabIndex = 13;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(350, 29);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "连接";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(199, 29);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 11;
            this.txtPort.Text = "8899";
            // 
            // txtServerIp
            // 
            this.txtServerIp.Location = new System.Drawing.Point(29, 29);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(153, 21);
            this.txtServerIp.TabIndex = 10;
            this.txtServerIp.Text = "127.0.0.1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(659, 458);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServerIp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Socket客户端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtServerIp;
    }
}

