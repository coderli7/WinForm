namespace GSCLogin
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.urlTxt = new System.Windows.Forms.TextBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.getCookieBtn = new System.Windows.Forms.Button();
            this.conQPBtn = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.setCookieBtn = new System.Windows.Forms.Button();
            this.weixinTxt = new System.Windows.Forms.TextBox();
            this.getQrCodeBtn = new System.Windows.Forms.Button();
            this.qrCodeTxt = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 61);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(1055, 470);
            this.webBrowser.TabIndex = 0;
            // 
            // urlTxt
            // 
            this.urlTxt.Location = new System.Drawing.Point(12, 12);
            this.urlTxt.Name = "urlTxt";
            this.urlTxt.Size = new System.Drawing.Size(1055, 21);
            this.urlTxt.TabIndex = 1;
            // 
            // loginBtn
            // 
            this.loginBtn.Location = new System.Drawing.Point(1096, 14);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(220, 23);
            this.loginBtn.TabIndex = 2;
            this.loginBtn.Text = "进入";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1096, 227);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(376, 120);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // getCookieBtn
            // 
            this.getCookieBtn.Location = new System.Drawing.Point(1096, 143);
            this.getCookieBtn.Name = "getCookieBtn";
            this.getCookieBtn.Size = new System.Drawing.Size(220, 23);
            this.getCookieBtn.TabIndex = 4;
            this.getCookieBtn.Text = "2.获取缓存";
            this.getCookieBtn.UseVisualStyleBackColor = true;
            this.getCookieBtn.Click += new System.EventHandler(this.getCookieBtn_Click);
            // 
            // conQPBtn
            // 
            this.conQPBtn.Location = new System.Drawing.Point(1096, 102);
            this.conQPBtn.Name = "conQPBtn";
            this.conQPBtn.Size = new System.Drawing.Size(220, 23);
            this.conQPBtn.TabIndex = 5;
            this.conQPBtn.Text = "1.连接到报价服务";
            this.conQPBtn.UseVisualStyleBackColor = true;
            this.conQPBtn.Click += new System.EventHandler(this.conQPBtn_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1241, 65);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(75, 21);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "8899";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1096, 65);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 21);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "127.0.0.1";
            // 
            // setCookieBtn
            // 
            this.setCookieBtn.Location = new System.Drawing.Point(1096, 183);
            this.setCookieBtn.Name = "setCookieBtn";
            this.setCookieBtn.Size = new System.Drawing.Size(220, 23);
            this.setCookieBtn.TabIndex = 8;
            this.setCookieBtn.Text = "3.设置Cookie";
            this.setCookieBtn.UseVisualStyleBackColor = true;
            this.setCookieBtn.Click += new System.EventHandler(this.setCookieBtn_Click);
            // 
            // weixinTxt
            // 
            this.weixinTxt.Location = new System.Drawing.Point(1094, 368);
            this.weixinTxt.Name = "weixinTxt";
            this.weixinTxt.Size = new System.Drawing.Size(126, 21);
            this.weixinTxt.TabIndex = 9;
            // 
            // getQrCodeBtn
            // 
            this.getQrCodeBtn.Location = new System.Drawing.Point(1241, 366);
            this.getQrCodeBtn.Name = "getQrCodeBtn";
            this.getQrCodeBtn.Size = new System.Drawing.Size(87, 23);
            this.getQrCodeBtn.TabIndex = 10;
            this.getQrCodeBtn.Text = "获取验证码";
            this.getQrCodeBtn.UseVisualStyleBackColor = true;
            this.getQrCodeBtn.Click += new System.EventHandler(this.getQrCodeBtn_Click);
            // 
            // qrCodeTxt
            // 
            this.qrCodeTxt.Location = new System.Drawing.Point(1346, 368);
            this.qrCodeTxt.Name = "qrCodeTxt";
            this.qrCodeTxt.Size = new System.Drawing.Size(126, 21);
            this.qrCodeTxt.TabIndex = 11;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1346, 397);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(126, 21);
            this.textBox3.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1241, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "打开保单";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(1094, 397);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(126, 21);
            this.textBox4.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1487, 613);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.qrCodeTxt);
            this.Controls.Add(this.getQrCodeBtn);
            this.Controls.Add(this.weixinTxt);
            this.Controls.Add(this.setCookieBtn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.conQPBtn);
            this.Controls.Add(this.getCookieBtn);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.urlTxt);
            this.Controls.Add(this.webBrowser);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TextBox urlTxt;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button getCookieBtn;
        private System.Windows.Forms.Button conQPBtn;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button setCookieBtn;
        private System.Windows.Forms.TextBox weixinTxt;
        private System.Windows.Forms.Button getQrCodeBtn;
        private System.Windows.Forms.TextBox qrCodeTxt;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox4;
    }
}

