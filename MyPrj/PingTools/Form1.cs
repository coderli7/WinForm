using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingTools
{
    public partial class Form1 : Form
    {

        System.Timers.Timer pingTimer = new System.Timers.Timer();

        public String pingFlag = "";

        public String pingAdress = " 127.0.0.1";

        public StringBuilder pingResultSb = new StringBuilder();
        public Form1()
        {
            InitializeComponent();
            pingTimer.Interval = 1000;
            pingTimer.Elapsed += pingTimer_Elapsed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBox1.Text))
            {
                pingAdress = textBox1.Text;
            }
            pingTimer.Start();
            this.richTextBox1.Text = "";
            pingResultSb = new StringBuilder();
        }

        private void pingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (pingFlag == "1")
            { return; }
            pingFlag = "1";
            try
            {
                string strRst = CmdPing(pingAdress);
                pingResultSb.AppendLine(System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                pingResultSb.AppendLine(strRst);
                SetText(pingResultSb.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { pingFlag = ""; }
        }

        private static string CmdPing(string strIp)
        {
            Process p = new Process();
            string pingrst = "", strRst = "";
            try
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("ping -n 1" + strIp);
                p.StandardInput.WriteLine("exit");
                strRst = p.StandardOutput.ReadToEnd();
                if (strRst.IndexOf("(0% loss)") != -1)
                    pingrst = "连接";
                else if (strRst.IndexOf("Destination host unreachable.") != -1)
                    pingrst = "无法到达目的主机";
                else if (strRst.IndexOf("Request timed out.") != -1)
                    pingrst = "超时";
                else if (strRst.IndexOf("Unknown host") != -1)
                    pingrst = "无法解析主机";
                else
                    pingrst = strRst;
            }
            catch (Exception ex)
            {
                pingrst = "ping 出错了:" + strRst + ex.Message;
            }
            p.Close();

            //正则匹配结果
            String patern = @"正在[\s|\S]*?\r\n\r\n";
            bool matchResult = Regex.IsMatch(pingrst, patern);
            if (matchResult)
            {
                pingrst = Regex.Match(pingrst, patern).Value;
            }
            return pingrst;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pingTimer.Enabled = false;
        }

        #region 控件赋值
        public void SetText(string val)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { val });
            }
            else
            {
                this.richTextBox1.Text = val;
            }
        }

        #endregion

    }

    delegate void SetTextCallBack(string text);

}
