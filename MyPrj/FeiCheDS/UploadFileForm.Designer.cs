namespace FeiCheDS
{
    partial class UploadFileForm
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
            this.OpenDataViewFormBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.fileRichTxt = new System.Windows.Forms.RichTextBox();
            this.docNoTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uptBtn = new System.Windows.Forms.Button();
            this.logRichTxt = new System.Windows.Forms.RichTextBox();
            this.fileSltedBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenDataViewFormBtn
            // 
            this.OpenDataViewFormBtn.Location = new System.Drawing.Point(752, 565);
            this.OpenDataViewFormBtn.Name = "OpenDataViewFormBtn";
            this.OpenDataViewFormBtn.Size = new System.Drawing.Size(89, 23);
            this.OpenDataViewFormBtn.TabIndex = 0;
            this.OpenDataViewFormBtn.Text = "查看结果";
            this.OpenDataViewFormBtn.UseVisualStyleBackColor = true;
            this.OpenDataViewFormBtn.Click += new System.EventHandler(this.OpenDataViewFormBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(47, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(123, 21);
            this.textBox1.TabIndex = 2;
            // 
            // fileRichTxt
            // 
            this.fileRichTxt.Location = new System.Drawing.Point(49, 75);
            this.fileRichTxt.Name = "fileRichTxt";
            this.fileRichTxt.Size = new System.Drawing.Size(703, 320);
            this.fileRichTxt.TabIndex = 3;
            this.fileRichTxt.Text = "";
            // 
            // docNoTxt
            // 
            this.docNoTxt.Location = new System.Drawing.Point(330, 31);
            this.docNoTxt.Name = "docNoTxt";
            this.docNoTxt.Size = new System.Drawing.Size(188, 21);
            this.docNoTxt.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(273, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "报案号:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.uptBtn);
            this.panel1.Controls.Add(this.logRichTxt);
            this.panel1.Controls.Add(this.fileSltedBtn);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.docNoTxt);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.fileRichTxt);
            this.panel1.Location = new System.Drawing.Point(12, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(829, 537);
            this.panel1.TabIndex = 6;
            // 
            // uptBtn
            // 
            this.uptBtn.Location = new System.Drawing.Point(648, 31);
            this.uptBtn.Name = "uptBtn";
            this.uptBtn.Size = new System.Drawing.Size(104, 23);
            this.uptBtn.TabIndex = 9;
            this.uptBtn.Text = "上传";
            this.uptBtn.UseVisualStyleBackColor = true;
            this.uptBtn.Click += new System.EventHandler(this.uptBtn_Click);
            // 
            // logRichTxt
            // 
            this.logRichTxt.Enabled = false;
            this.logRichTxt.Location = new System.Drawing.Point(49, 401);
            this.logRichTxt.Name = "logRichTxt";
            this.logRichTxt.Size = new System.Drawing.Size(703, 109);
            this.logRichTxt.TabIndex = 8;
            this.logRichTxt.Text = "";
            // 
            // fileSltedBtn
            // 
            this.fileSltedBtn.Location = new System.Drawing.Point(524, 31);
            this.fileSltedBtn.Name = "fileSltedBtn";
            this.fileSltedBtn.Size = new System.Drawing.Size(108, 23);
            this.fileSltedBtn.TabIndex = 7;
            this.fileSltedBtn.Text = "选择图片";
            this.fileSltedBtn.UseVisualStyleBackColor = true;
            this.fileSltedBtn.Click += new System.EventHandler(this.fileSltedBtn_Click);
            // 
            // UploadFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 607);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OpenDataViewFormBtn);
            this.MaximizeBox = false;
            this.Name = "UploadFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "资料上传";
            this.Load += new System.EventHandler(this.UploadFileForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenDataViewFormBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox docNoTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button fileSltedBtn;
        private System.Windows.Forms.RichTextBox fileRichTxt;
        private System.Windows.Forms.RichTextBox logRichTxt;
        private System.Windows.Forms.Button uptBtn;
    }
}