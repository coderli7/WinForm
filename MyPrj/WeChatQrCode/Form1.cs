using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowHanler;
using Baidu.Aip;
using Baidu.Aip.Ocr;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace WindowHanler
{
    delegate void SetTextCallBack(string text);

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //启动timer,默认5分钟
            StatusTimer.Interval = 60000;
            StatusTimer.Elapsed += StatusTimer_Elapsed;
            StatusTimer.Start();
            //打开后自动连接
            //ConnectSocketServer();
            ConnectSocketServerNew();
            //结束已打开进程
            closeOpenedProcess();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeOpenedProcess();
            e.Cancel = false;
            this.Dispose();
            this.Close();
        }

        #region Timer
        void StatusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //socket连接一直维持
            ConnectSocketServerNew();
            if (DateTime.Now.Hour == 6)
            {
                if (DateTime.Now.Minute >= 45 && DateTime.Now.Minute <= 50)
                {
                    LogInfo(string.Format("重置了GetWeChatQrCodeCount次数"));
                    GetWeChatQrCodeCount = 0;
                    richTextBox1Sb = new StringBuilder();
                }
            }

            //最小化某些窗口
            WindowFormAPI.MinOrMaxWindowFormByTitle(Config.MinFormTitles, 2);
        }

        #endregion

        #region 一些变量

        private static Socket clientSocket = null;

        private static byte[] result = new byte[1024];

        private StringBuilder richTextBox1Sb = new StringBuilder();

        /// <summary>
        /// 控制某个渠道当天最多获取验证码次数
        /// </summary>
        private static int GetWeChatQrCodeCount = 0;

        /// <summary>
        /// 定时器用来重置，如清空sb,重试次数等
        /// </summary>
        private static System.Timers.Timer StatusTimer = new System.Timers.Timer();

        /// <summary>
        /// 记录连接状态
        /// </summary>
        private static bool isConnected = false;

        /// <summary>
        /// 记录微信窗口句柄
        /// </summary>
        private static IntPtr weChatForm;

        /// <summary>
        /// 存储所有socket连接
        /// </summary>
        private List<SocketClient> clients = new List<SocketClient>();

        #endregion

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            String qrCode = GetQrCode();
            LogInfo(qrCode);
        }

        /// <summary>
        /// 发送到报价服务器端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //ConnectSocketServer();
            ConnectSocketServerNew();
        }

        /// <summary>
        /// 发送到报价服务器端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (clientSocket != null)
            {
                try
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes(this.richTextBox1.Text));
                }
                catch (Exception ex)
                {
                    LogInfo(String.Format("发送服务端消息异常:{0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 手工识别验证码，用于测试百度api识别结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= 10; i++)
            {
                byte[] data = GetScreenImg(String.Format("C:\\{0}.jpg", i));
                String str = GetStrByImg(data);
                sb.AppendLine(str);
            }
        }

        #region others

        /// <summary>
        /// 操作微信，并获取动态验证码
        /// </summary>
        /// <returns></returns>
        private string GetQrCode()
        {
            String qrCode = "";
            try
            {

                if (!String.IsNullOrEmpty(Config.FixedQrCode))
                {
                    return Config.FixedQrCode;
                }

                if (DateTime.Now.Hour >= 21 || DateTime.Now.Hour < 7)
                {
                    qrCode = "夜间不允许自动获取验证码";
                    return qrCode;
                }

                //IntPtr intptr1 = WindowFormAPI.FindWindow("WeWorkWindow", null);
                //IntPtr intptr2 = WindowFormAPI.FindWindow("WeChatLogin", null);
                //if (intptr1 == null || intptr1.ToInt32() == 0)
                //{
                //    qrCode = "当前企业微信未登录";
                //    return qrCode;
                //}

                if (GetWeChatQrCodeCount > 20)
                {
                    qrCode = "当前获取验证码次数已用完";
                    return qrCode;
                }


                //1.获取句柄并最大化，并前置，避免最小化等原因造成无法点击
                weChatForm = WindowHanler.WindowFormAPI.FindWindow(null, "企业微信");
                LogInfo("===========开始获取验证码===========");
                SetWeChatFormBefore();

                //2.点击消息菜单，切花到动态验证码，注意次步骤一定要设置动态验证码为置顶状态
                LogInfo("后台获取微信验证码");
                ClickMsgWidthHeight("L");
                Thread.Sleep(1000);
                LogInfo("点击微信消息菜单");
                ClickMenuWidthHeight("L");
                Thread.Sleep(1000);
                LogInfo("点击右面空白处");
                ClickNullSpaceClickWidthHeight("L");

                //3.点击重新获取验证码，并将获取页面关闭-OLD
                //ClickGenQrCodeWidthHeight("L");
                LogInfo("主动发送微信验证码");

                //2018.10.17 新增同步获取验证码方法
                String qrCodeNew = GetQrCodeSync();
                if (!string.IsNullOrEmpty(qrCodeNew))
                {
                    return qrCodeNew;
                }

                CommonUse.PostHtml(Config.WX_URL, string.Format("{0}={1}", "userid_weixin", Config.Userid_Weixin));
                GetWeChatQrCodeCount++;

                Thread.Sleep(3000);
                WindowFormAPI.CloseFormByTitle("动态密码认证");

                //3.1如果使用微信登陆，则先按照截图方式解析一次，如果不行，再行复制粘贴
                if (Config.USE_BD_API)
                {
                    LogInfo("API解析验证码-开始");
                    qrCode = GetQrCodeByImage();
                    LogInfo("API解析验证码-完成");
                    LogInfo("API解析验证码-结果：" + qrCode);
                }
                if (string.IsNullOrEmpty(qrCode))
                {
                    SetWeChatFormBefore();
                    //4.如果经过图片解析，仍然不能成功的，用用賦值粘貼
                    LogInfo("API未获取到验证码");
                    LogInfo("开始点击右键");
                    ClickQrCodeLeftClickWidthHeight("R");
                    Thread.Sleep(1000);
                    //鼠标移动到空白处
                    LogInfo("移动鼠标到空白处");
                    MouseFlag.SetCursorPos(Config.NullSpaceClickWidthHeight["X"], Config.NullSpaceClickWidthHeight["Y"]);
                    Thread.Sleep(1000);
                    LogInfo("移动鼠标到复制处");
                    MouseFlag.SetCursorPos(Config.QrCodeRightCopyClickWidthHeight["X"], Config.QrCodeRightCopyClickWidthHeight["Y"]);
                    Thread.Sleep(1000);
                    LogInfo("点击复制");
                    ClickQrCodeRightCopyClickWidthHeight("L");
                    Thread.Sleep(1000);
                    //5.获取剪切板验证码并粘贴
                    qrCode = getClipBrdStr();
                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        if (Regex.IsMatch(qrCode, @"\d{6}"))
                        {
                            qrCode = Regex.Match(qrCode, @"\d{6}").Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message + ex.StackTrace);
            }
            LogInfo("===========结束获取验证码===========");
            return qrCode;
        }

        /// <summary>
        /// 设置微信窗口前置-避免因为遮挡等原因造成解析不成功
        /// </summary>
        private void SetWeChatFormBefore()
        {
            if (weChatForm != null && weChatForm.ToInt32() > 0)
            {
                //最小化某些窗口
                WindowFormAPI.MinOrMaxWindowFormByTitle(new string[] { "发起会话", "TeamViewer Panel" }, 2);
                LogInfo("最大化微信窗口");
                //暂时不按照正常处理，减少因为刷屏导致卡屏，造成截屏失败
                //WindowFormAPI.ShowWindow(weChatForm, 1);
                WindowFormAPI.ShowWindow(weChatForm, 3);
                LogInfo("前置微信窗口");
                WindowFormAPI.SetForegroundWindow(weChatForm);
            }
        }

        /// <summary>
        /// 点击微信-消息按钮，为了使对话框是动态验证码（L左键单击，R右键单击）
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickMsgWidthHeight(string clickType)
        {
            MyMouseClick(Config.MsgWidthHeight, clickType);
        }

        /// <summary>
        /// 点击动态验证码微信菜单（L左键单击，R右键单击）
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickMenuWidthHeight(string clickType)
        {
            MyMouseClick(Config.MenuWidthHeight, clickType);
        }

        /// <summary>
        /// 点击获取验证码（L左键单击，R右键单击）
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickGenQrCodeWidthHeight(string clickType)
        {
            MyMouseClick(Config.GenQrCodeWidthHeight, clickType);
        }

        /// <summary>
        ///左键选中（L左键单击，R右键单击）
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickQrCodeLeftClickWidthHeight(string clickType)
        {
            MyMouseClick(Config.QrCodeLeftClickWidthHeight, clickType);
        }

        /// <summary>
        /// 右键复制（L左键单击，R右键单击）
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickQrCodeRightCopyClickWidthHeight(string clickType)
        {
            MyMouseClick(Config.QrCodeRightCopyClickWidthHeight, clickType);
        }

        /// <summary>
        /// 验证码空白处
        /// </summary>
        /// <param name="clickType"></param>
        public void ClickNullSpaceClickWidthHeight(string clickType)
        {
            MyMouseClick(Config.NullSpaceClickWidthHeight, clickType);
        }

        public void MyMouseClick(Dictionary<String, int> clickDic, string clickType)
        {
            int X = clickDic["X"];
            int Y = clickDic["Y"];
            LogInfo(string.Format("鼠标点击({2})坐标：{0},{1}", X, Y, clickType));
            if (clickType == "L")
            {
                MouseFlag.MouseLefClickEvent(X, Y, 0);
            }
            else if (clickType == "R")
            {
                MouseFlag.MouseRightClickEvent(X, Y, 0);
            }
        }

        /// <summary>
        /// 获取剪切板数据
        /// </summary>
        /// <returns></returns>
        private static String getClipBrdStr()
        {
            String clipbrdStr = "";
            IDataObject iData = Clipboard.GetDataObject();
            if (iData != null && iData.GetDataPresent(DataFormats.Text))
            {
                clipbrdStr = (string)iData.GetData(DataFormats.UnicodeText);
            }
            return clipbrdStr;
        }

        /// <summary>
        /// 把操作日志记录到页面上，每天清空一次
        /// </summary>
        private void LogInfo(String text)
        {
            try
            {
                //每次调用就将记录追加至sb中
                richTextBox1Sb.AppendLine(String.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                SetText(richTextBox1Sb.ToString());
            }
            catch (Exception ex)
            {
            }

        }

        private void closeOpenedProcess()
        {
            try
            {
                StringBuilder sb1 = new StringBuilder();
                Process curProcess = Process.GetCurrentProcess();
                Process[] curSystemProcesses = Process.GetProcesses();
                sb1.AppendLine(curProcess.ProcessName + "=======" + curProcess.Id);
                foreach (Process item in curSystemProcesses)
                {
                    sb1.AppendLine(item.ProcessName + "=======" + item.Id);
                    if (item.Id != curProcess.Id && item.ProcessName == curProcess.ProcessName)
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo(string.Format("执行closeOpenedProcess发生异常:{0}", ex.StackTrace));
            }
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
                string val = CommonUse.PostHtml(Config.WX_URL, string.Format("{0}={1}&dynamic_type=1", "userid_weixin", Config.Userid_Weixin));
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

        #endregion

        #region Socket连接
        /// <summary>
        /// 连接SocketServer
        /// </summary>
        private void ConnectSocketServer()
        {
            if (isConnected)
            {
                return;
            }
            //设定服务器IP地址 
            IPAddress ipAdress = IPAddress.Parse(Config.SocketServerIp);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ipAdress, Config.SocketServerPort)); //配置服务器IP与端口
                isConnected = true;
            }
            catch (Exception ex)
            {
                LogInfo("连接报价服务异常：" + ex.Message);
                return;
            }
            Thread clientReceiveThread = new Thread(ReceiveMessage);
            clientReceiveThread.SetApartmentState(ApartmentState.STA);//这句是关键
            clientReceiveThread.Start(clientSocket);
            LogInfo("连接报价服务成功");
        }

        /// <summary>
        /// 连接SocketServer-多个Server-包括监测掉线后重试等
        /// </summary>
        private void ConnectSocketServerNew()
        {
            try
            {
                //遍历配置文件，连接
                foreach (var item in Config.SocketServerAdresses)
                {
                    string curAdressAndPort = string.Format("{0}:{1}", item.Key, item.Value);
                    SocketClient curSocketClient = null;
                    ///是否首次增加
                    bool isFirAdd = false;
                    List<SocketClient> findClients = clients.Where(c => c.socketAdress == curAdressAndPort).ToList();
                    if (findClients == null || findClients.Count == 0)
                    {
                        isFirAdd = true;
                        //设定服务器IP地址并连接
                        curSocketClient = new SocketClient();
                        curSocketClient.socketAdress = curAdressAndPort;
                        IPAddress ipAdress = IPAddress.Parse(item.Key);
                        curSocketClient.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    }
                    else
                    {
                        curSocketClient = findClients.FirstOrDefault();
                    }
                    //开始准备连接
                    try
                    {
                        if (curSocketClient.isConnected == false)
                        {
                            //关闭，并重新初始化
                            curSocketClient.client.Close();
                            curSocketClient.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //连接
                            curSocketClient.client.Connect(item.Key, item.Value);
                            LogInfo(String.Format("连接{0}成功~", curAdressAndPort));
                            curSocketClient.isConnected = true;
                            //启动监听请求
                            Thread clientReceiveThread = new Thread(ReceiveMessageNew);
                            clientReceiveThread.SetApartmentState(ApartmentState.STA);//这句是关键
                            clientReceiveThread.Start(curSocketClient);
                        }
                    }
                    catch (Exception ex)
                    {
                        curSocketClient.isConnected = false;
                        LogInfo(String.Format("{0}连接异常：{1}", curAdressAndPort, ex.Message));
                    }
                    if (isFirAdd)
                    {
                        clients.Add(curSocketClient);
                    }
                    if (curSocketClient.isConnected)
                    {
                        //如果渠道正常，则发送消息
                        SendToServerNew(curSocketClient, "hello~");
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo(String.Format("执行ConnectSocketServerNew连接异常：{0}", ex.Message));
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
                Thread.Sleep(500);
                try
                {
                    //通过clientSocket接收数据 
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber > 0)
                    {
                        String receiveVal = Encoding.UTF8.GetString(result, 0, receiveNumber);
                        LogInfo("报价服务发来消息：" + receiveVal);
                        //如果消息等于QrCode,则启动获取验证码步骤
                        if ("QrCode" == receiveVal)
                        {
                            //获取验证码后，发动给服务端
                            String sendMessage = GetQrCode();
                            if (string.IsNullOrEmpty(sendMessage))
                            {
                                sendMessage = "没有获取到动态验证码";
                            }
                            sendMessage = "QrCode=" + sendMessage;
                            SendToServer(sendMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo(ex.Message + "ReceiveMessage");
                    //myClientSocket.Shutdown(SocketShutdown.Both);
                    //myClientSocket.Close();
                    break;
                }
            }
        }

        private void ReceiveMessageNew(Object clientSocket)
        {
            SocketClient newSocketClient = (SocketClient)clientSocket;
            Socket myClientSocket = newSocketClient.client;
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    //通过clientSocket接收数据 
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber > 0)
                    {
                        String receiveVal = Encoding.UTF8.GetString(result, 0, receiveNumber);
                        LogInfo("报价服务发来消息：" + receiveVal);
                        //如果消息等于QrCode,则启动获取验证码步骤
                        if ("QrCode" == receiveVal)
                        {
                            //获取验证码后，发动给服务端
                            String sendMessage = GetQrCode();
                            if (string.IsNullOrEmpty(sendMessage))
                            {
                                sendMessage = "没有获取到动态验证码";
                            }
                            sendMessage = "QrCode=" + sendMessage;
                            SendToServerNew(newSocketClient, sendMessage);
                            //SendToServer(sendMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo(ex.Message + "ReceiveMessage");
                    //myClientSocket.Shutdown(SocketShutdown.Both);
                    //myClientSocket.Close();
                    break;
                }
            }
        }

        private void SendToServer(String sendMessage)
        {
            try
            {
                if (clientSocket != null)
                {
                    LogInfo("发送给报价服务消息：" + sendMessage);
                    clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
                }
            }
            catch (Exception ex)
            {
                LogInfo(string.Format("给服务器发送消息异常：{0}", ex.Message));
                isConnected = false;
                LogInfo("给服务器发送消息异常,准备等服务器稳定后，重新连接吧！");
            }
        }


        private void SendToServerNew(SocketClient curClient, String sendMessage)
        {
            try
            {
                if (curClient.client != null)
                {
                    LogInfo(String.Format("发送给报价服务消息{0}消息：{1}", curClient.socketAdress, sendMessage));
                    curClient.client.Send(Encoding.UTF8.GetBytes(sendMessage));
                }
            }
            catch (Exception ex)
            {
                LogInfo(String.Format("发送给报价服务消息{0}消息异常：{1}", curClient.socketAdress, ex.Message));
                curClient.isConnected = false;
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="text"></param>
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

        #endregion

        #region 百度图片识别

        /// <summary>
        /// 验证码截屏获取-百度API识别
        /// </summary>
        /// <returns></returns>
        public String GetQrCodeByImage()
        {
            String qrCode = "";
            try
            {
                LogInfo("API获取屏幕二进制数组");
                byte[] scbyte = GetScreenImg();
                if (scbyte != null && scbyte.Length > 0)
                {
                    LogInfo("API屏幕二进制数组-解析");
                    qrCode = TransQrCodeByBaiDuOCRApi(scbyte);
                }
                else
                {
                    LogInfo("API屏幕二进制数组-为空");
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message + ex.StackTrace);
            }
            return qrCode;
        }


        /// <summary>
        /// 获取屏幕二进制字符
        /// </summary>
        /// <param name="filePath">如果指定了文件路径，则直接获取路径下文件</param>
        /// <returns></returns>
        public byte[] GetScreenImg(String filePath = "")
        {
            /*
             * 1.考虑截取全部屏幕，API解析成功率较低，
             * 2.还是通过配置截取最右侧屏幕
             */
            byte[] data = null;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    data = File.ReadAllBytes(filePath);
                    return data;
                }
                //Rectangle tScreenRect = new Rectangle(0, 0, Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                //Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                //Graphics g = Graphics.FromImage(img);
                //g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
                //g.DrawImage(img, 0, 0, tScreenRect, GraphicsUnit.Pixel);

                int copyScreenX = Screen.AllScreens[0].Bounds.Width;
                int copyScreenXStart = Convert.ToInt32(copyScreenX * Config.CopySrceenX);
                int copyScreenY = Screen.AllScreens[0].Bounds.Height;
                SetWeChatFormBefore();
                Rectangle tScreenRect = new Rectangle(0, 0, copyScreenX - copyScreenXStart, copyScreenY);
                Bitmap tSrcBmp = new Bitmap(copyScreenX - copyScreenXStart, copyScreenY); // 用于屏幕原始图片保存

                Graphics gp = Graphics.FromImage(tSrcBmp);
                LogInfo("截取图片");
                gp.CopyFromScreen(new Point(copyScreenXStart, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
                gp.DrawImage(tSrcBmp, 0, 0, tScreenRect, GraphicsUnit.Pixel);

                try
                {
                    tSrcBmp.Save(string.Format("C:\\{0}.jpg", DateTime.Now.ToString("yyy-MM-dd HH-mm-ss")));
                }
                catch (Exception ex)
                {
                    LogInfo(String.Format("保存验证码图片异常：{0}", ex.Message));
                }

                //转化image为二进制数组
                MemoryStream s = new MemoryStream();
                tSrcBmp.Save(s, System.Drawing.Imaging.ImageFormat.Jpeg);
                data = s.ToArray();

                s.Close();
                s.Dispose();
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message + ex.StackTrace);
            }
            return data;
        }


        /// <summary>
        /// 根据二进制的img获取对应验证码
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public String TransQrCodeByBaiDuOCRApi(byte[] images)
        {
            string qrCode = "";
            try
            {
                String qrCodeByImg = GetStrByImg(images);
                //根基页面解析到的返回结果，根据指定正则匹配最后一位，如果存在则返回
                string qrCodePatern = @"动态验证码[\S|\s]*?\d{6,}[\S|\s]*?\}";
                MatchCollection matches = Regex.Matches(qrCodeByImg, qrCodePatern);
                if (matches != null && matches.Count == 0)
                {
                    return qrCode;
                }
                String lastMatchVal = matches[matches.Count - 1].Value;
                if (Regex.IsMatch(lastMatchVal, @"\d{6}"))
                {
                    qrCode = Regex.Match(lastMatchVal, @"\d{6}").Value;
                }
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message + ex.StackTrace);
            }
            return qrCode;
        }

        /// <summary>
        /// 百度API-获取全部消息
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        private static string GetStrByImg(byte[] images)
        {
            String qrCodeByImg = "";
            try
            {
                // 设置APPID/AK/SK
                var APP_ID = Config.APP_ID;
                var API_KEY = Config.API_KEY;
                var SECRET_KEY = Config.SECRET_KEY;
                Ocr client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
                // 如果有可选参数
                var options = new Dictionary<string, object> { { "language_type", "CHN_ENG" } };
                // 带参数调用通用文字识别, 图片参数为本地图片
                JObject result = client.GeneralBasic(images, options);
                qrCodeByImg = result.ToString();
            }
            catch (Exception ex)
            {
                qrCodeByImg = ex.Message;
            }
            return qrCodeByImg;
        }


        #endregion

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

