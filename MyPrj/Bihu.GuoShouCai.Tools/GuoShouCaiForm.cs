using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using mshtml;
using System.Diagnostics;

namespace Bihu.GuoShouCai.Tools
{
    public partial class GuoShouCaiForm : Form
    {
        public GuoShouCaiForm()
        {

            InitializeComponent();
            //this.ShowInTaskbar = false;
            //this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*去除IE代理*/
            CommonUse.CancelIEProxy();

            this.webBrowser1.Navigate("http://9.0.6.69:7001/prpall/index.jsp");

        }

        private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            CommonUse.CloseIexplorer();

            WebBrowser websr = (WebBrowser)sender;

            if (e.Url.ToString().Contains("http://9.0.6.69:7001/prpall/index.jsp") && websr.ReadyState == WebBrowserReadyState.Complete)
            {

                #region /*登录*/
                HTMLDocument doc = (HTMLDocument)webBrowser1.Document.DomDocument;
                HtmlDocument docFrame = webBrowser1.Document.Window.Frames["fraInterface"].Document;
                foreach (var item in docFrame.Forms)
                {
                    HtmlElement formele = (HtmlElement)item;
                    if (formele.Name == "fm")
                    {
                        foreach (var curelement in formele.All)
                        {
                            HtmlElement htmlelement = (HtmlElement)curelement;
                            if (htmlelement.GetAttribute("name") == "UserCode")
                            {
                                htmlelement.SetAttribute("value", Config.UserCode);
                            }
                            if (htmlelement.GetAttribute("name") == "Password")
                            {
                                htmlelement.SetAttribute("value", Config.Password);
                            }
                            if (htmlelement.GetAttribute("name") == "ComCode")
                            {
                                htmlelement.SetAttribute("value", Config.ComCode);
                            }
                            if (htmlelement.GetAttribute("name") == "RiskCode")
                            {
                                htmlelement.SetAttribute("value", "0511");
                            }
                        }
                    }
                }
                docFrame.Forms[0].InvokeMember("submit");
                #endregion

            }
            else if (e.Url.ToString().Contains("/prpall/commonship/pub/UIMenuShip.jsp") && websr.ReadyState == WebBrowserReadyState.Complete)
            {

                /*登陆国寿财系统*/
                string ProxyUrl = "127.0.0.1:1001";
                string configProxyUrl = ConfigurationManager.AppSettings["ProxyUrl"];
                if (!string.IsNullOrEmpty(configProxyUrl))
                {
                    ProxyUrl = configProxyUrl;
                }
                CommonUse.CloseIexplorer();
                CommonUse.SetIEProxy(ProxyUrl);
                MessageBox.Show("处理完成，重新登录试试吧~");


                System.Diagnostics.Process.Start("iexplore.exe", "http://9.0.6.69:7001/prpall/index.jsp");  


                System.Environment.Exit(0);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
