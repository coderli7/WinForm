using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeepVPNTools
{
    public partial class KeepVPNForm : Form
    {
        public KeepVPNForm()
        {
            InitializeComponent();
        }


        #region timer

        System.Timers.Timer chkVPNTimer = new System.Timers.Timer();

        #endregion

        private void KeepVPNForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            chkVPNTimer.Interval = 60000 * 5;
            chkVPNTimer.Elapsed += ChkVPNTimer_Elapsed;
            chkVPNTimer.Start();
        }

        private void ChkVPNTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            String info = "";
            try
            {
                string checkStatusUrl = string.Format("http://9.0.9.11/workbench/resources/nprogress/nprogress.css");
                string html = HttpUtils.Get(checkStatusUrl, new Dictionary<string, string>(), false, 5);
                if (!String.IsNullOrEmpty(html))
                {
                    if (!html.Contains("nprogress"))
                    {
                        info = String.Format("获取css异常:{0}", html);
                    }
                    else
                    {
                        info = String.Format("获取css成功!!!", html);
                    }
                }
            }
            catch (Exception ex)
            {
                info = ex.Message;
            }
            Log(info);
        }


        private void Log(string info)
        {
            if (!String.IsNullOrEmpty(this.richTextBox2.Text) && this.richTextBox2.Text.Length > 10000)
            {
                this.richTextBox2.Clear();
            }
            this.richTextBox2.AppendText(String.Format("{0}:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "), info) + "\r\n");
        }

    }
}
