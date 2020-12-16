using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MutiThreadWinform
{
    public partial class Form1 : Form
    {
        public delegate void doSomethingDelegate();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate("http://sso.cic.cn/cas/login?service=http%3A%2F%2Fjscd.cic.cn%3A8897%2Fipartner%2Fj_spring_cas_security_check");
        }

        private void doSomething()
        {
            try
            {
                //while (true)
                //{
                //    Thread.Sleep(1000);
                //    richTextBox1.AppendText(DateTime.UtcNow.ToString() + "\r\n");

                //    Thread.Sleep(1000);
                //    richTextBox2.AppendText(DateTime.UtcNow.ToString() + "\r\n");
                //}



                Thread.Sleep(1000);
                richTextBox1.AppendText(DateTime.UtcNow.ToString() + "\r\n");

                Thread.Sleep(1000);
                richTextBox2.AppendText(DateTime.UtcNow.ToString() + "\r\n");

                var tmp = webBrowser1.Document;
                webBrowser1.Navigate("https://www.baidu.com/");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { this.Invoke(new doSomethingDelegate(doSomething)); });
        }
    }
}
