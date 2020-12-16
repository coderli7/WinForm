using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using mshtml;
using VerifyReader;
using System.Diagnostics;
using BiHuManBu.BxSys.CheckCode;
using System.Threading;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Globalization;
using IWshRuntimeLibrary;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Drawing.Imaging;
using WindowHanler;


namespace VPNRedial
{
    public delegate void SetTextCallBack(string text);

    public partial class VPNRedial : Form
    {

        #region timer定义
        /// <summary>
        /// 定时检测PIN码输入窗体
        /// </summary>
        System.Timers.Timer pinTimer = new System.Timers.Timer();

        /// <summary>
        /// 定时监测弹窗（如登陆失败等弹窗）
        /// </summary>
        System.Timers.Timer tipsTimer = new System.Timers.Timer();

        /// <summary>
        /// 开启监测系统是否登陆
        /// </summary>
        System.Timers.Timer pinLoginStatusTimer = new System.Timers.Timer();

        #endregion

        #region PIN码登陆句柄
        /// <summary>
        /// 验证码输入框句柄
        /// </summary>
        public IntPtr imgCodeTextHandler;

        /// <summary>
        /// 验证码图片空间-句柄
        /// </summary>
        public IntPtr imgCodeHandler;

        /// <summary>
        /// 更换验证码按钮-句柄
        /// </summary>
        public IntPtr changeImgCodeBtnHandler;

        /// <summary>
        /// 登陆按钮-句柄
        /// </summary>
        public IntPtr SubmitBtnHandler;

        public static IntPtr curEasyConnect;
        #endregion

        #region 其他字段

        /// <summary>
        /// 记录登陆标识
        /// </summary>
        private string loginFlag = "";

        /// <summary>
        /// 密码错误
        /// </summary>
        private static bool passwordError = false;

        /// <summary>
        /// 记录WebBrowserUrl历史记录
        /// </summary>
        private StringBuilder urlSb = new StringBuilder();

        /// <summary>
        /// 当次如果验证码破解成功标识（1当次已经执行，避免从持续破解，并登陆系统）
        /// </summary>
        private String curTimeLoginFlag = "";

        /// <summary>
        /// 记录当前窗口句柄，用作移动窗口使用
        /// </summary>
        public IntPtr curhWnd;

        #endregion

        public VPNRedial()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            //开启窗体关闭定时器
            CloseJsWindow.StartCloseWindow();
        }

        private void VPNRedial_Load(object sender, EventArgs e)
        {

            //GetAllInfoByPrcName("EasyConnect");

            Control.CheckForIllegalCrossThreadCalls = false;

            //创建桌面图标
            CreateShortCutIcon();

            //取消IE代理
            bool res = Proxies.UnsetProxy();

            //关闭已打开重播工具
            closeOpenedProcess();

            //开启定时登陆
            StartTimer();

            CreateVPNStart();

            //登陆
            Login();
        }

        /// <summary>
        /// 初始化并开启定时器
        /// </summary>
        private void StartTimer()
        {
            tipsTimer.Interval = 3000;
            tipsTimer.Elapsed += tipsTimer_Elapsed;
            tipsTimer.Start();
            pinTimer.Interval = 3000;
            pinTimer.Elapsed += pinTimer_Elapsed;
            pinTimer.Start();
            //尝试登陆时间间隔
            int interval = 1 * 60 * 1000;
            string TimerInterval = Config.TimerInterval;
            if (!string.IsNullOrEmpty(TimerInterval))
            {
                if (Convert.ToInt32(TimerInterval.Trim()) >= 1 && Convert.ToInt32(TimerInterval.Trim()) <= 10)
                {
                    interval = Convert.ToInt32(TimerInterval.Trim()) * 60 * 1000;
                }
            }
            pinLoginStatusTimer.Interval = interval;
            pinLoginStatusTimer.Elapsed += pinLoginStatusTimer_Elapsed;
            pinLoginStatusTimer.Start();
        }

        #region 定时任务

        void pinTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pinTimer.Stop();
            try
            {
                PinCheck();
            }
            catch (Exception ex)
            {
                SetText(string.Format("执行pinTimer_Elapsed发生异常:{0}", ex.Message));
            }
            pinTimer.Start();
        }

        void tipsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tipsTimer.Stop();
            try
            {
                TipsCheck();
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = string.Format("执行tipsTimer_Elapsed发生异常:{0}", ex.Message);
            }
            tipsTimer.Start();
        }

        void pinLoginStatusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.curTimeLoginFlag = "";
            WindowFormAPI.MinOrMaxWindowFormByTitle(new string[] { "TeamViewer Panel", "发起会话" }, 2);
            try
            {
                if (this.loginFlag == "1")
                {
                    return;
                }
                else
                {
                    this.loginFlag = "1";
                }
                Login();
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = this.richTextBox1.Text + string.Format("执行函数pinLoginStatusTimer_Elapsed发生异常：{0}", ex.Message);
            }
            finally
            {
                this.loginFlag = "";
            }
        }

        #endregion

        #region Webbrowser 登陆


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            urlSb.AppendLine(webBrowser1.Document.Body.OuterHtml.ToString());
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                try
                {
                    WebBrowserLogin();
                }
                catch (Exception ex)
                {
                    this.richTextBox1.AppendText(string.Format("执行函数发生异常：{0}", ex.StackTrace));
                }
            }
        }

        private void WebBrowserLogin()
        {
            try
            {
                if (webBrowser1.Document != null && this.curTimeLoginFlag != "1")
                {
                    //&& !webBrowser1.Document.GetElementById("DkeyTips").OuterHtml.Contains("hide")
                    #region 判断页面是否就绪
                    HtmlElement name = webBrowser1.Document.GetElementById("svpn_name");
                    HtmlElement pwd = webBrowser1.Document.GetElementById("svpn_password");
                    HtmlElement code = webBrowser1.Document.GetElementById("randcode");
                    if (name == null | pwd == null | code == null)
                    {
                        return;
                    }
                    #endregion

                    #region 登陆属性赋值

                    name.InnerText = Config.VPNUser.Trim();
                    pwd.InnerText = Config.VPNPassword.Trim();
                    /*获取页面验证码文件*/
                    string imgcode = GetImageCode();
                    if (!string.IsNullOrEmpty(imgcode) && imgcode.Length >= 4)
                    {
                        code.InnerText = imgcode;
                        this.webBrowser1.Document.Forms[0].InvokeMember("submit");
                        this.curTimeLoginFlag = "1";
                        return;
                    }
                    #endregion
                }
                if (webBrowser1.Document.Body.InnerHtml.Contains("您本机登录的用户还未注销,请点击“注销”按钮注销并重新登录!"))
                {
                    #region 注销
                    var imput = webBrowser1.Document.GetElementsByTagName("input");
                    foreach (var item in imput)
                    {
                        HtmlElement btnelement = item as HtmlElement;
                        if (btnelement.OuterHtml.Contains("注销"))
                        {
                            btnelement.InvokeMember("onclick");
                            break;
                        }
                        if (btnelement.OuterHtml.Contains("重新登录"))
                        {
                            btnelement.InvokeMember("onclick");
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = string.Format("执行函数WebBrowserLogin出现异常:{0}", ex.Message);
            }
        }

        #endregion


        #region 点击事件
        private void button4_Click(object sender, EventArgs e)
        {
            Login();
        }
        #endregion

        private static void PinCheck()
        {
            IntPtr pinForm = WindowFormAPI.FindWindow(null, "用户 PIN 码验证");
            if (pinForm.ToInt32() > 0)
            {
                IntPtr pinText = WindowFormAPI.GetDlgItem(pinForm, 1000);
                IntPtr sureBtn = WindowFormAPI.GetDlgItem(pinForm, 1);
                if (pinText.ToInt32() > 0 && sureBtn.ToInt32() > 0)
                {
                    WindowFormAPI.SendMessage(pinText, WindowFormAPI.WM_SETTEXT, IntPtr.Zero, Config.PIN);
                    WindowFormAPI.ClickByIntPtr(sureBtn);
                }
            }
        }

        /// <summary>
        /// 弹窗提示，自动操作，如弹出硬件特征码校验，自动点击提交申请。
        /// </summary>
        private void TipsCheck()
        {
            IntPtr tipsForm = WindowFormAPI.FindWindow(null, "提示信息");
            if (tipsForm.ToInt32() > 0)
            {
                IntPtr sureBtn = WindowFormAPI.GetDlgItem(tipsForm, 2);
                IntPtr tipInfo = WindowFormAPI.GetDlgItem(tipsForm, 65535);
                if (tipInfo.ToInt32() > 0)
                {
                    StringBuilder s = new StringBuilder(512);
                    int i = WindowFormAPI.GetWindowTextW(tipInfo, s, s.Capacity);
                    if (s.ToString().Contains("密码错误"))
                    {
                        passwordError = true;
                        SetText("用户名密码错误");
                    }
                }
                if (sureBtn.ToInt32() > 0)
                {
                    WindowFormAPI.ClickByIntPtr(sureBtn);
                }
            }

            IntPtr hardwareTipsForm = WindowFormAPI.FindWindow(null, "EasyConnect");
            if (hardwareTipsForm.ToInt32() > 0)
            {
                //硬件特征码校验
                IntPtr yingjianBtn = WindowFormAPI.GetDlgItem(hardwareTipsForm, 1201);
                //提交申请ID
                IntPtr submitBtn = WindowFormAPI.GetDlgItem(hardwareTipsForm, 1274);
                if (yingjianBtn.ToInt32() > 0 && submitBtn.ToInt32() > 0)
                {
                    WindowFormAPI.ClickByIntPtr(submitBtn);
                }
            }

            IntPtr jsTips = WindowFormAPI.FindWindow(null, "脚本错误");
            if (jsTips.ToInt32() > 0)
            {
                int tmp = 0;
                WindowFormAPI.SendMessage(jsTips.ToInt32(), WindowFormAPI.WM_CLOSE, 0, ref tmp);
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
                this.richTextBox1.Text = string.Format("{0}\r\n{1}", this.richTextBox1.Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + text);
            }
        }

        private void Login()
        {
            if (DateTime.Now.Hour > 7 && DateTime.Now.Hour < 22)
            {
                if (passwordError) { return; };
                bool vpnStatus = GetVPNStatus();
                if (vpnStatus == false)
                {
                    SetText("掉线啦");
                    DoLogin();
                }
                else
                {
                    SetText("未掉线");
                }
            }
        }

        /// <summary>
        /// pin码登陆
        /// </summary>
        public void DoLogin()
        {
            try
            {
                CloseProcess();

                Process.Start(Config.SangforCSClientPath);
                Thread.Sleep(15000);
                List<WindowInfo> list = WindowFormAPI.GetAllDesktopWindows().ToList();
                List<WindowInfo> newwindows = list.Where(c => c.szWindowName.Contains("EasyConnect")).ToList();
                int i = 0;
                foreach (var item in newwindows)
                {
                    i++;
                    IntPtr curtab1 = WindowFormAPI.GetDlgItem(item.hWnd, 1002);
                    IntPtr curtab2 = WindowFormAPI.GetDlgItem(item.hWnd, 1086);
                    IntPtr curtab3 = WindowFormAPI.GetDlgItem(item.hWnd, 1091);
                    if (curtab1 != IntPtr.Zero && curtab2 != IntPtr.Zero && curtab3 != IntPtr.Zero)
                    {
                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                        int tab1 = WindowFormAPI.FindWindowEx(item.hWnd.ToInt32(), 0, "SysTabControl32", null);
                        int tab2 = WindowFormAPI.FindWindowEx(item.hWnd.ToInt32(), 1, "SysTabControl32", null);
                        int tab3 = WindowFormAPI.FindWindowEx(item.hWnd.ToInt32(), 2, "SysTabControl32", null);
                        if (tab1 != 0)
                        {
                            IntPtr sureBtn = WindowFormAPI.GetDlgItem(item.hWnd, 1086);
                            this.SubmitBtnHandler = sureBtn;
                            if (!string.IsNullOrEmpty(Config.VPNUser) && !string.IsNullOrEmpty(Config.VPNPassword))
                            {
                                #region 解析账号密码
                                IntPtr fatherInt = new IntPtr(tab1);
                                List<int> childWindowList = WindowFormAPI.GetChildWindows(tab1);
                                foreach (var curitem in childWindowList)
                                {
                                    IntPtr curIntPtr = new IntPtr(curitem);
                                    int itemId = WindowFormAPI.GetDlgCtrlID(curIntPtr);
                                    //用户名
                                    if (itemId == 1009)
                                    {
                                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                                        WindowFormAPI.SendMessage(curIntPtr, WindowFormAPI.WM_SETTEXT, IntPtr.Zero, Config.VPNUser);
                                        curEasyConnect = curIntPtr;
                                        WindowFormAPI.SetForegroundWindow(curIntPtr);
                                        curhWnd = item.hWnd;
                                        MoveLeftTopSide();
                                        continue;
                                    }
                                    //密码
                                    if (itemId == 1010)
                                    {
                                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                                        WindowFormAPI.SendMessage(curIntPtr, WindowFormAPI.WM_SETTEXT, IntPtr.Zero, Config.VPNPassword);
                                        MoveLeftTopSide();
                                        continue;
                                    }
                                    //验证码-输入文本框
                                    if (itemId == 1011)
                                    {
                                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                                        MoveLeftTopSide();
                                        this.imgCodeTextHandler = curIntPtr;
                                        continue;
                                    }
                                    //验证码-图片
                                    if (itemId == 1022)
                                    {
                                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                                        this.imgCodeHandler = curIntPtr;
                                        MoveLeftTopSide();
                                        continue;
                                    }
                                    //验证码切换
                                    if (itemId == 1023)
                                    {
                                        WindowFormAPI.SetForegroundWindow(item.hWnd);
                                        this.changeImgCodeBtnHandler = curIntPtr;
                                        MoveLeftTopSide();
                                        continue;
                                    }
                                }
                                #endregion
                            }
                            Submit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetText(ex.Message);
            }
        }

        /// <summary>
        /// PIN码登陆
        /// </summary>
        private void Submit()
        {
            try
            {
                /*
                            * 1.获取到验证码图片。
                            * 2.破解
                            * 3.赋值
                            * 4.提价
                            * 5.重试
                            */
                #region 解析验证码并填入
                MoveLeftTopSide();
                string imgCode = "";
                Bitmap bitmap = GetImage();
                int tryCount = 0;
                while (!"1".Equals(Config.UnNeedQrCode) && tryCount < 2 && imgCode.Length < 4)
                {
                    try
                    {
                        tryCount++;
                        if (tryCount > 1)
                        {
                            WindowFormAPI.SendMessage(this.changeImgCodeBtnHandler, WindowFormAPI.BM_CLICK, IntPtr.Zero, null);
                            Thread.Sleep(1000);
                            bitmap = GetImage();
                        }
                        if (bitmap == null)
                        {
                            break;
                        }
                        BreakCodeServer unCheckobj = new BreakCodeServer(bitmap);
                        Thread.Sleep(1000);
                        imgCode = unCheckobj.getPicnum();
                    }
                    catch (Exception ex)
                    {
                        SetText(string.Format("解析验证码异常：{0}", "未成功解析验证码"));
                    }
                }
                if ((!string.IsNullOrEmpty(imgCode) && imgCode.Length == 4))
                {
                    WindowFormAPI.SendMessage(this.imgCodeTextHandler, WindowFormAPI.WM_SETTEXT, IntPtr.Zero, imgCode);
                }
                #endregion

                #region 点击登录
                Thread.Sleep(2000);
                WindowFormAPI.SendMessage(this.SubmitBtnHandler, WindowFormAPI.BM_CLICK, IntPtr.Zero, null);
                #endregion
            }
            catch (Exception ex)
            {
                SetText(String.Format("Submit提交异常:{0}", ex.Message));
            }
        }

        #region Tools

        /// <summary>
        /// 关闭已经打开的重播工具
        /// </summary>
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
                SetText(string.Format("执行closeOpenedProcess发生异常:{0}", ex.StackTrace));
            }
        }

        /// <summary>
        /// 获取页面验证码图片转换为字符串
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <returns></returns>
        public string ConImageCodeToString(System.Windows.Forms.WebBrowser webbrowser, out HtmlElement imagecodeelement)
        {
            string imagecode = default(string);
            try
            {
                #region  解析验证码
                HtmlElementCollection imageresult = webbrowser.Document.Images;
                imagecodeelement = null;
                foreach (var item in imageresult)
                {
                    HtmlElement curimg = item as HtmlElement;
                    if (curimg.OuterHtml.Contains("rand_code.csp"))
                    {
                        /*获取页面图片*/
                        imagecodeelement = curimg;
                        HTMLDocument hdoc = (HTMLDocument)webbrowser.Document.DomDocument;
                        HTMLBody hbody = (HTMLBody)hdoc.body;
                        IHTMLControlRange hcr = (IHTMLControlRange)hbody.createControlRange();
                        IHTMLControlElement hImg = (IHTMLControlElement)curimg.DomElement;
                        hcr.add(hImg);
                        hcr.execCommand("Copy", false, null);
                        Image CodeImage = Clipboard.GetImage();
                        Bitmap bitmap = new Bitmap(CodeImage);
                        /*******解析验证码********/
                        BreakCodeServer unCheckobj = new BreakCodeServer(bitmap);
                        Thread.Sleep(2000);
                        imagecode = unCheckobj.getPicnum();
                        break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                SetText(System.DateTime.Now.ToString() + "\r\n解析验证码失败\r\n");
                imagecodeelement = null;
                imagecode = "";
            }
            return imagecode;
        }

        /// <summary>
        /// 通过访问主页，判断VPN是否掉线 
        /// </summary>
        /// <returns>True未掉线（默认），False掉线</returns>
        public bool GetVPNStatus()
        {
            bool vpnStatus = (OpenUrlStatus(Config.LoginAdress)) || (PingIpStatus(Config.LoginAdress)) ? true : false;
            return vpnStatus;
        }

        private Bitmap GetImage()
        {
            try
            {
                WindowFormAPI.SetForegroundWindow(curEasyConnect);
                Rect rect = new Rect();
                WindowFormAPI.GetWindowRect(this.imgCodeHandler, out rect);
                int startX = rect.Left;
                int startY = rect.Top;
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                Bitmap bitmap = GetPartOfImageRec(startX, startY, width, height);
                return bitmap;
            }
            catch (Exception ex)
            {
                SetText(String.Format("\r\n获取验证码失败:{0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// 从屏幕中截取图片
        /// </summary>
        /// <param name="startX">起始坐标X</param>
        /// <param name="startY">起始坐标Y</param>
        /// <param name="width">截取宽度</param>
        /// <param name="height">截取高度</param>
        /// <returns></returns>
        public Bitmap GetPartOfImageRec(int startX, int startY, int width, int height)
        {
            Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //设置图像的大小
            Bitmap bitMap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitMap);
            //从指定的区域中复制图形
            g.CopyFromScreen(startX, startY, 0, 0, new Size(width, height));
            //bitMap.Save("C:\\1.jpg");
            return bitMap;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        private string GetImageCode()
        {
            #region 获取验证码
            string imagecode = default(string);
            HtmlElement imagecodeelement;
            imagecode = ConImageCodeToString(this.webBrowser1, out imagecodeelement);
            int getimagecount = 0;
            while ((string.IsNullOrEmpty(imagecode) || imagecode.Length < 4) & (getimagecount <= 10))
            {
                getimagecount++;
                this.webBrowser1.Navigate(Config.VPNAdress);
                break;
            }
            return imagecode;
            #endregion
        }

        private void VPNRedial_MinimumSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal && this.Visible == true)
            {
                this.notifyIcon1.Visible = true;//在通知区显示Form的Icon
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;//使Form不在任务栏上显示
            }
        }

        private void VPNRedial_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {

                this.ShowInTaskbar = false;
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;  //显示在系统任务栏
                this.WindowState = FormWindowState.Normal;  //还原窗体
                this.notifyIcon1.Visible = false;
            }
        }

        /// <summary>
        /// 将窗口移动到左上角（避免遮挡）
        /// </summary>
        private void MoveLeftTopSide()
        {
            try
            {
                if (curhWnd != null && curhWnd.ToInt32() > 0)
                {
                    WindowFormAPI.SetForegroundWindow(curhWnd);
                    WindowFormAPI.MoveWindow(curhWnd, 0, 0, 410, 300, true);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// ping某个地址，判定是否稳定
        /// 默认为是通的
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool PingIpStatus(string ip)
        {
            bool result = true;
            try
            {
                result = NetCommon.TestNetConnected(ip, 2, 5);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private static bool OpenUrlStatus(String url)
        {
            bool vpnStatus = true;
            try
            {
                #region 新版，直接访问页面
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Automatic;

                    using (HttpClient client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Add("Accept", "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, */*");
                        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                        client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN");
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
                        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                        client.DefaultRequestHeaders.ExpectContinue = false;
                        client.Timeout = new TimeSpan(0, 0, 0, 5000);
                        handler.AutomaticDecompression = DecompressionMethods.GZip;
                        client.BaseAddress = new Uri(url);
                        var response = client.GetAsync(url).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            vpnStatus = false;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                vpnStatus = false;
            }
            return vpnStatus;
        }

        private void CreateShortCutIcon()
        {
            try
            {
                string desktoppath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                string curworkpath = System.Environment.CurrentDirectory;
                if (!System.IO.File.Exists(desktoppath + @"\VPN重播工具.lnk"))
                {
                    WshShell quicklink = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)quicklink.CreateShortcut(desktoppath + @"\VPN重播工具.lnk");
                    shortcut.TargetPath = curworkpath + @"\VPNRedial.exe";
                    shortcut.Description = "VPN重播工具";
                    shortcut.WorkingDirectory = curworkpath;
                    shortcut.IconLocation = curworkpath + @"\VPNReplay.ico";
                    shortcut.WindowStyle = 1;
                    shortcut.Save();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void CloseProcess()
        {
            if (Config.ClosePrcesses.Count > 0)
            {
                var Processes = Process.GetProcesses();
                foreach (Process item in Processes)
                {
                    var prcList = Config.ClosePrcesses.Where(c => c.Equals(item.ProcessName)).ToList();
                    if (prcList.Count > 0)
                    {
                        item.Kill();
                    }
                }
            }
        }

        /// <summary>
        /// 执行批量名，开机自动启动VPN
        /// </summary>
        private static void CreateVPNStart()
        {
            try
            {
                String curPath = Environment.CurrentDirectory;
                String createVPNBatPath = curPath + @"\CreateTaskChd.bat";
                if (System.IO.File.Exists(createVPNBatPath))
                {
                    //Process.Start(createVPNBatPath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region TEST


        private void GetAllInfoByPrcName(String prcName)
        {
            List<WinFormModel> formInfos = new List<WinFormModel>();
            try
            {
                #region 根据进程名，获取所有对应窗口下的子句柄
                List<WindowInfo> list = WindowFormAPI.GetAllDesktopWindows().ToList();
                //List<WindowInfo> newwindows = list.Where(c => c.szWindowName.Contains(prcName)).ToList();
                List<WindowInfo> curFormList = new List<WindowInfo>();
                foreach (WindowInfo item in list)
                {
                    curFormList.AddRange(WindowFormAPI.GetAllDesktopWindows(item.hWnd));
                }

                //代理设置
                List<WindowInfo> newList = new List<WindowInfo>();
                foreach (var item in curFormList)
                {

                    List<WindowInfo> curList = newList.Where(c =>
                    {
                        bool retVal = false;
                        if (c.hWnd == item.hWnd && c.ItemId == item.ItemId)
                        {
                            retVal = true;
                        }
                        return retVal;
                    }).ToList();

                    if (curList != null && curList.Count <= 0)
                    {
                        newList.Add(item);
                    }
                }


                //List<WindowInfo> curEditList = newList.Where(c => !string.IsNullOrEmpty(c.szWindowName) && c.szWindowName.Contains("代理设置")).ToList();


                List<WindowInfo> curEditList1 = list.Where(c => !string.IsNullOrEmpty(c.szWindowName) && c.szWindowName.Contains("EasyConnect")).ToList();

                List<WindowInfo> curEditList2 = curFormList.Where(c => !string.IsNullOrEmpty(c.szWindowName) && c.szWindowName.Contains("EasyConnect")).ToList();


                List<WindowInfo> curEditList3 = list.Where(c => !string.IsNullOrEmpty(c.szWindowName) && c.szWindowName.Contains("EasyConnect")).ToList();


                WindowInfo easyConnect = curEditList3.Where(c =>
                {
                    bool retVal = false;
                    List<WindowInfo> allConnect = WindowFormAPI.GetAllDesktopWindows(c.hWnd);
                    List<WindowInfo> existList = allConnect.Where(d => d.ItemId == 1001).ToList();
                    if (existList.Count > 0)
                    {
                        retVal = true;
                    }
                    return retVal;
                }).FirstOrDefault();

                List<WindowInfo> curConnectList = WindowFormAPI.GetAllDesktopWindows(easyConnect.hWnd);


                WindowInfo editHandler = curConnectList.Where(d => d.ItemId == 1001).FirstOrDefault();

                StringBuilder s = new StringBuilder(512);
                //标题
                //WindowFormAPI.GetWindowText(editHandler.hWnd, s, s.Capacity);

                WindowFormAPI.GetDlgItemText(editHandler.hWnd, editHandler.ItemId, s, s.Capacity);

                editHandler.szWindowName = s.ToString();


                //List<WindowInfo> curEditList2 = newList.Where(c => !string.IsNullOrEmpty(c.szClassName) && c.szClassName.Contains("ComboBox")).ToList();
                #endregion
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}


