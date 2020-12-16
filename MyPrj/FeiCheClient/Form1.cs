using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FeiCheClient
{
    public partial class Form1 : Form
    {

        public List<String> UrlList = new List<string>();

        public int UrlListFlag = -1;

        System.Timers.Timer loginUrlChkTimer = new System.Timers.Timer();

        public static String Account = "44003505";

        public StringBuilder UrlSbHis = new StringBuilder();

        public String HomeUrl = "";

        public Form1()
        {
            InitializeComponent();
        }

        void loginUrlChkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ProxyManage.Location_href_Logined))
            {
                var url = "https://" + Configure.IP_Base_Login + ":" + Configure.Url_piccclaim_login_port
                           + ProxyManage.Location_href_Logined;
                this.webBrowser1.Navigate(url);
                ProxyManage.Location_href_Logined = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActInfo();
            this.webBrowser1.Document.GetElementById("username").SetAttribute("value", this.actTxt.Text);
            this.webBrowser1.Document.GetElementById("queId").InvokeMember("click");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = string.Format("http://56.1.80.185:8072/piccclaim/insbpm/viewFlowChart.do?registNo={0}", this.textBox2.Text);
            this.webBrowser1.Navigate(url);
            UrlList.Add(url);
            UrlListFlag++;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetActInfo();
            this.webBrowser1.Navigate(this.textBox1.Text);
            UrlList.Add(this.textBox1.Text);
            UrlListFlag++;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (e.Url != null && !"about:blank".Equals(e.Url.ToString()))
            {
                String curUrl = e.Url.ToString();
                UrlSbHis.AppendLine(curUrl);
                this.textBox1.Text = curUrl;
                this.richTextBox1.Text = UrlSbHis.ToString();

                if (curUrl.Contains("piccclaim/index.jsp;jsessionpncallid"))
                {
                    this.HomeUrl = curUrl;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.textBox1.Text);
            //Control.CheckForIllegalCrossThreadCalls = false;
            //loginUrlChkTimer.Interval = 1000;
            //loginUrlChkTimer.Elapsed += loginUrlChkTimer_Elapsed;
            //loginUrlChkTimer.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (UrlListFlag > 0 && UrlList.Count > 0)
            {
                this.webBrowser1.Navigate(UrlList[UrlListFlag - 1]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (UrlListFlag > 0 && UrlListFlag + 1 < UrlList.Count)
            {
                this.webBrowser1.Navigate(UrlList[UrlListFlag + 1]);
            }
        }


        public void SetActInfo()
        {
            Form1.Account = String.IsNullOrEmpty(this.actTxt.Text) ? Form1.Account : this.actTxt.Text;
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            //e.Cancel = true;
            //string url = this.webBrowser1.StatusText;
            //this.webBrowser1.Url = new Uri(url);
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(HomeUrl);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.webBrowser1.Document.GetElementById("username").SetAttribute("value", "18262526");
            this.webBrowser1.Document.GetElementById("submit").InvokeMember("click");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            String img = File.ReadAllText("D:\\1.txt");
            HtmlElement faceElement = this.webBrowser1.Document.GetElementById("imgStr");
            faceElement.SetAttribute("value", img);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //webBrowser.Document.Forms["loginForm"];
            //HtmlElement formLogin = webBrowser.Document.Forms["loginForm"];
            HtmlElement faceElement1 = this.webBrowser1.Document.GetElementById("imgStr");
            String curImgVal = faceElement1.GetAttribute("value");
            HtmlElement faceForm = this.webBrowser1.Document.Forms["fm"];
            faceForm.InvokeMember("submit");
        }
    }
}
