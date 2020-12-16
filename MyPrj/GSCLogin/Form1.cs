using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSCLogin
{
    delegate void SetTextCallBack(string text);
    public partial class Form1 : Form
    {


        public static string ipAdress = "127.0.0.1";

        private int connectPort = 8899;

        private static byte[] result = new byte[1024];

        private static Socket clientSocket = null;

        private static String cookies = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(this.urlTxt.Text);
        }

        private void getCookieBtn_Click(object sender, EventArgs e)
        {
            getCookie();
        }

        private void conQPBtn_Click(object sender, EventArgs e)
        {
            //1.先建立客户端，连接到报价服务
            ConnectSocketServer();
        }


        private void ConnectSocketServer()
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text))
            {
                Form1.ipAdress = this.textBox1.Text.Trim();
                connectPort = Convert.ToInt32(this.textBox2.Text.Trim());
            }
            IPAddress ip = null;
            if (!"127.0.0.1".Equals(this.textBox1.Text))
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(this.textBox1.Text);
                ip = hostInfo.AddressList[0];
            }
            else
            {
                ip = IPAddress.Parse(this.textBox1.Text);
            }
            connectPort = Convert.ToInt32(this.textBox2.Text);
            //设定服务器IP地址 
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, connectPort)); //配置服务器IP与端口 
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = this.richTextBox1.Text + "\r\n" + String.Format("连接服务器异常：{0}", ex.Message);
                return;
            }
            Thread clientReceiveThread = new Thread(ReceiveMessage);
            clientReceiveThread.Start(clientSocket);
            this.richTextBox1.Text = this.richTextBox1.Text + "\r\n连接Socket服务端成功~";
        }

        private void getCookie()
        {
            try
            {
                clientSocket.Send(Encoding.UTF8.GetBytes("Cookie"));
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = ex.Message;
            }
        }


        /// <summary> 
        /// 接收消息 
        /// </summary> 
        /// <param name="clientSocket"></param> 
        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据 
                    //文本类型
                    int receiveNumber = myClientSocket.Receive(result);
                    String receiveVal = Encoding.UTF8.GetString(result, 0, receiveNumber);
                    if (receiveVal.Contains("Cookie="))
                    {
                        receiveVal = receiveVal.Replace("Cookie=", "");
                        Form1.cookies = receiveVal;
                    }
                    SetText(Form1.cookies);

                    //文件
                    //byte[] fileByte = ReceiveVarData(myClientSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }


        private void SetText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text = text;
            }
        }

        private void setCookieBtn_Click(object sender, EventArgs e)
        {
            setCookie();
        }


        private void setCookie()
        {
            SuppressWininetBehavior();
            Dictionary<string, string> dics = GetDicPair(Form1.cookies);
            foreach (var item in dics)
            {
                Form1.InternetSetCookie("http://9.0.6.69:7001/prpall/index.jsp", item.Key, item.Value);
            }
            this.richTextBox1.Text = "重新设置Cookie完成!当前登陆Cookie是：" + this.webBrowser.Document.Cookie;

        }


        public Dictionary<string, string> GetDicPair(string str)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] retArr = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in retArr)
                {
                    string[] pairArr = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pairArr.Count() == 2)
                    {
                        if (!dic.Keys.Contains(pairArr[0]))
                        {
                            dic.Add(pairArr[0], pairArr[1]);
                        }
                        else
                        {
                            dic[pairArr[0]] = pairArr[1];
                        }
                    }
                }
            }

            return dic;
        }

        private static unsafe void SuppressWininetBehavior()
        {
            /* SOURCE: http://msdn.microsoft.com/en-us/library/windows/desktop/aa385328%28v=vs.85%29.aspx
                * INTERNET_OPTION_SUPPRESS_BEHAVIOR (81):
                *      A general purpose option that is used to suppress behaviors on a process-wide basis. 
                *      The lpBuffer parameter of the function must be a pointer to a DWORD containing the specific behavior to suppress. 
                *      This option cannot be queried with InternetQueryOption. 
                *      
                * INTERNET_SUPPRESS_COOKIE_PERSIST (3):
                *      Suppresses the persistence of cookies, even if the server has specified them as persistent.
                *      Version:  Requires Internet Explorer 8.0 or later.
                */
            int option = (int)3/* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;

            bool success = InternetSetOption(0, 81/*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/, new IntPtr(optionPtr), sizeof(int));
            if (!success)
            {
                MessageBox.Show("Something went wrong ! Clear Cookie Failed!");
            }

        }

        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private void Form1_Load(object sender, EventArgs e)
        {
            this.webBrowser.Navigate("http://9.0.6.69:7001/prpall/index.jsp");
        }

        private void getQrCodeBtn_Click(object sender, EventArgs e)
        {
            this.qrCodeTxt.Text = GetQrCodeSync();

        }


        /// <summary>
        /// 同步获取验证码，不用登陆微信截屏等操作，直接同步获取，这种操作
        /// </summary>
        /// <returns></returns>
        private String GetQrCodeSync()
        {
            string qrCode = "";
            try
            {
                string val = CommonUse.PostHtml("http://wxqy.chinalife-p.com.cn:8084/dynamic_pass_web/TwoFactor/getCode", string.Format("{0}={1}&dynamic_type=1", "userid_weixin", this.weixinTxt.Text));
                if (!string.IsNullOrEmpty(val))
                {
                    QrCodeBean qrBean = JsonConvert.DeserializeObject<QrCodeBean>(val);
                    if (!string.IsNullOrEmpty(qrBean.dynamic_code))
                    {
                        return qrBean.dynamic_code;
                    }
                }
            }
            catch (Exception)
            {
            }
            return qrCode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string docNo = this.textBox4.Text;
            if (!string.IsNullOrEmpty(docNo))
            {
                String url = "";
                if (docNo.StartsWith("80511"))
                {
                    url = "http://9.0.6.69:7001/prpall/0511/tbcbpg/UIPrPoEn0511Show.jsp?BIZTYPE=POLICY&BizNo={0}&SHOWTYPE=SHOW&RiskCode=0511&UserCode=&myComCode=&BusinessNo={0}&BusinessType=P&OperatorCode=110106198412046028&riskCode=0511&sysflag=prpall";
                }
                else if (docNo.StartsWith("80507"))
                {
                    url = "http://9.0.6.69:7001/prpall/0507/tbcbpg/UIPrPoEn0507Show.jsp?BIZTYPE=POLICY&BizNo={0}&SHOWTYPE=SHOW&RiskCode=0507&UserCode=110106198412046028&myComCode=11010880&BusinessNo={0}&BusinessType=P&OperatorCode=&riskCode=0507&sysflag=prpallz";
                }
                else if (docNo.StartsWith("90511"))
                {
                    url = "http://9.0.6.69:7001/prpall/0501/tbcbpg/UIPrPoEn0501UnShow.jsp?BIZTYPE=PROPOSAL&SHOWTYPE=SHOW&BizNo={0}&RiskCode=0501&UserCode=null&myComCode=5301129001&PolicyNo=null&prpallIP=null&BusinessNo={0}&BusinessType=T&OperatorCode=&strIsUseImagePlat=null&RiskCode=0501&sysflag=prpall&isUsedRightHand=";
                }
                else if (docNo.StartsWith("90507"))
                {
                    url = "http://9.0.6.69:7001/prpall/0501/tbcbpg/UIPrPoEn0501UnShow.jsp?BIZTYPE=PROPOSAL&SHOWTYPE=SHOW&BizNo={0}&RiskCode=0507&UserCode=null&myComCode=3302646001&PolicyNo=null&prpallIP=null&BusinessNo={0}&BusinessType=T&OperatorCode=330227198709226829&strIsUseImagePlat=null&RiskCode=0507&sysflag=prpall&isUsedRightHand=null";

                }
                url = string.Format(url, docNo);
                this.webBrowser.Navigate(url);
            }
        }
    }


    public class QrCodeBean
    {
        /// <summary>
        /// 
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string flag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dynamic_code { get; set; }
    }
}
