using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPNTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reLoadTimer.Interval = 120000;
            reLoadTimer.Elapsed += ReLoadTimer_Elapsed;
            reLoadTimer.Start();

        }

        private void ReLoadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.webBrowser1.Navigate("https://www.baidu.com/");
                Thread.Sleep(10000);
                string checkUrl = "http://9.0.9.11/workbench/workbench/login.html";
                String confUrl = ConfigurationManager.AppSettings["url"];
                if (!String.IsNullOrEmpty(confUrl)) { checkUrl = confUrl; }
                this.webBrowser1.Navigate(checkUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region timer

        /// <summary>
        /// 重新加载定时器
        /// </summary>
        System.Timers.Timer reLoadTimer = new System.Timers.Timer();


        #endregion

        /// <summary>
        /// 手动进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.urlTxt.Text) && !String.IsNullOrEmpty(this.urlTxt.Text.Trim()))
            {
                this.webBrowser1.Navigate(this.urlTxt.Text.Trim());
            }
        }
    }
}
