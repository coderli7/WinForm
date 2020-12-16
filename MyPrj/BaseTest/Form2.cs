using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseTest
{
    public delegate void SetTextCallBack(string text);

    public partial class Form2 : Form
    {

        System.Timers.Timer timer = new System.Timers.Timer();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            click1();
        }

        public void click1()
        {
            this.richTextBox1.Invoke(new SetTextCallBack(str => { this.richTextBox1.Text += str; }), new object[] { "sss" });
        }


        //private void SetText(string text)
        //{
        //    if (this.richTextBox1.InvokeRequired)
        //    {
        //        SetTextCallBack stcb = new SetTextCallBack(SetText);
        //        this.Invoke(stcb, new object[] { text });
        //    }
        //    else
        //    {
        //        this.richTextBox1.Text = string.Format("{0}\r\n{1}", this.richTextBox1.Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + text);
        //    }
        //}

        private void Form2_Load(object sender, EventArgs e)
        {
            timer.Interval = 5000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            click1();
        }

    }
}
