using BiHu.BaoXian.ClassCommon;
using ChinaLifeTools.models;
using ChinaLifeTools.Models;
using ChinaLifeTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChinaLifeTools
{
    public partial class ChinaLifeForm : Form
    {
        #region 0.初始化

        public ChinaLifeForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void ChinaLifeForm_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                UpdateVersion();
            });
            CloseSameProcess();
            MessageBox.Show("请注意!!! 使用代理工具期间,尽量关闭安全软件哦！（如360安全卫士，防火墙等）", "提醒");
            this.usernameTxt.Text = Config.UserName;
            if (String.IsNullOrEmpty(Config.UserName))
            {
                try
                {
                    String filePath = GetUserCodeFilePath();
                    if (File.Exists(filePath))
                    {
                        this.usernameTxt.Text = File.ReadAllText(filePath);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            StartSyncCookieTimer();
            StartUpdateVersionTimer();
            StartUpdateProcessAndStartSocketConnect();
        }

        #endregion

        #region 1.字段

        FiddlerCacheInfo CookieModel { get; set; }

        bool SaveCookieSign { get; set; }

        /// <summary>
        /// 代理地址
        /// </summary>
        public String ProxyUrl { get; set; }

        /// <summary>
        /// cookie同步timer
        /// </summary>
        System.Timers.Timer cookieSyncTimer = new System.Timers.Timer();

        /// <summary>
        /// 定时获取服务端代理程序timer
        /// </summary>
        System.Timers.Timer uptVersionTimer = new System.Timers.Timer();

        StringBuilder localSetCookieSb = new StringBuilder();

        string fileName = "usrcode.txt";

        Object lockObj = new object();

        static String basePath = System.Environment.CurrentDirectory;

        static String updateDic = String.Format("{0}\\update", basePath);

        static String localVersionFilePath = String.Format("{0}\\version.txt", updateDic);

        static string uptTips = "有新版本哦,点我更新!";

        /// <summary>
        /// 更新程序是否存在并启动着
        /// </summary>
        //static String updateProcessPath = String.Format("{0}\\UptService.exe", basePath);

        //D:\01.GitHub\WinForm\MyPrj\UptService\bin\Debug

        static String updateProcessPath = String.Format("{0}\\UptService.exe", @"D:\01.GitHub\WinForm\MyPrj\UptService\bin\Debug");


        Socket clientSocket;

        #endregion

        #region 2.操作事件

        #region 2.1启动代理

        private void startProxyButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.startProxyButton.Enabled = false;
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
                if (rk.GetValue("ProxyEnable").ToString() == "1")
                {
                    /**
                     * 1.如果系统代理已经是勾选状态，且判定为代理工具设置的代理，则直接从服务器获取最新cookie，提示已更新即可
                     * 此场景应该较多
                     * 2.否则提醒用户关闭后在开启
                     */
                    String proxyServer = rk.GetValue("ProxyServer").ToString();
                    if (!String.IsNullOrEmpty(proxyServer) && proxyServer.Contains(":8899"))
                    {
                        Config.UserName = this.usernameTxt.Text.Trim();
                        bool syncCookieStatus = SyncCookie();
                        if (syncCookieStatus)
                        {
                            MessageBox.Show("代理已更新,请 CTRL+F5 刷新页面哦!", "提醒");
                        }
                        else
                        {
                            MessageBox.Show("代理更新失败,请关闭代理后重试，或联系管理员处理!", "提醒");
                        }
                    }
                    else
                    {
                        MessageBox.Show("代理已经是启动状态哦!,如需重新启动代理,请先点击关闭代理按钮!", "提醒");
                    }
                }
                else
                {
                    try
                    {
                        Config.UserName = this.usernameTxt.Text.Trim();
                        if (!String.IsNullOrEmpty(Config.UserName))
                        {
                            string result = GetServerCookie();
                            if (!String.IsNullOrEmpty(result))
                            {
                                LoginInfoSyncResponse infoSyncResponse = JsonConvert.DeserializeObject<LoginInfoSyncResponse>(result);
                                if (infoSyncResponse.data != null && !String.IsNullOrEmpty(infoSyncResponse.data))
                                {
                                    try
                                    {
                                        CookieModel = new FiddlerCacheInfo(infoSyncResponse.data);
                                        InitProxyUrl(infoSyncResponse.proxyUrl);
                                        SaveCookieSign = true;
                                        FiddlerUtils.SetSSLCer();
                                        Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
                                        Fiddler.FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
                                        BiHu.BaoXian.ClassCommon.WebProxy.Start(8899);
                                        MessageBox.Show("代理已启动!!! \r\n\r\n请执行以下步骤 ( 建议使用Chrome浏览器 ^_^ ) ：\r\n\r\n\r\n1.打开浏览器。\r\n\r\n2.执行 Ctrl + Shift + Delete  清空缓存。\r\n\r\n3.重启浏览器,直接进入核心系统首页即可。", "提醒");
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(String.Format("启动代理发生异常，请与管理员联系!\r\n\r\n({0})\r\n\r\n注:可点击【关闭代理】按钮,再试一次哦!", ex.Message));
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("工号信息失效,请与管理员联系!", "提醒");
                                }
                            }
                            else
                            {
                                MessageBox.Show("工号信息为空,请与管理员联系!", "提醒");
                            }
                        }
                        else
                        {
                            MessageBox.Show("请输入核心系统工号哦!", "提醒");
                            this.usernameTxt.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("启动代理发生异常:{0}\r\n\r\n注:可点击【关闭代理】按钮,再试一次哦!", ex.Message), "提醒");
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this.startProxyButton.Enabled = true;
            }
        }

        private string GetServerCookie()
        {
            if (String.IsNullOrEmpty(Config.UserName)) { return ""; }
            String postUrl = Config.CookieUrl;
            LoginInfoSyncRequest infoSyncRequest = new LoginInfoSyncRequest();
            infoSyncRequest.channelKey = Config.UserName;
            String postData = JsonConvert.SerializeObject(infoSyncRequest);
            String result = "";
            try
            {
                result = CommonUse.Post(postUrl, postData, null, 60, "application/json", false);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("获取工号登录信息失败，请与管理员联系!\r\n\r\n({0})", ex.Message));
            }

            return result;
        }

        #endregion

        #region 2.2关闭代理

        private void closeProxyButton_Click(object sender, EventArgs e)
        {
            FiddlerUtils.CloseProxy();
            MessageBox.Show("代理已关闭!\r\n\r\n如代理仍未关闭，请手动取消系统代理即可!", "提醒");
            System.Environment.Exit(0);
        }

        private void ChinaLifeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FiddlerUtils.CloseProxy();
        }

        private void ChinaLifeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FiddlerUtils.CloseProxy();
        }
        #endregion

        #region 3.3记住我
        private void chkPwd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                String filetxt = this.chkPwd.Checked ? this.usernameTxt.Text.Trim() : "";
                File.WriteAllText(GetUserCodeFilePath(), filetxt);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 3.3更新
        private void versionLabel_Click(object sender, EventArgs e)
        {
            SendInfo(File.ReadAllText(localVersionFilePath));
        }
        #endregion

        #endregion

        #region 3.BeforeRequest&BeforeResponse

        public void FiddlerApplication_BeforeRequest(Fiddler.Session oSession)
        {
            lock (lockObj)
            {
                try
                {
                    if (!SaveCookieSign) { return; }
                    if (!oSession.fullUrl.StartsWith(String.Format("https://{0}", ProxyUrl)) && !oSession.fullUrl.StartsWith(String.Format("http://{0}", ProxyUrl))) { return; }
                    //#region 测试人保
                    //if (!oSession.fullUrl.Contains("157.122.153.67")) { return; }
                    //#endregion

                    if (oSession.fullUrl.Equals("https://ins.chinalife-p.com.cn/workbench/workbench/login.html"))
                    {
                        oSession.fullUrl = "https://ins.chinalife-p.com.cn/workbench/workbench/index.html";
                    }
                    String cookieRequestStr = oSession.oRequest.headers["Cookie"].ToString();
                    if (String.IsNullOrEmpty(cookieRequestStr)) { return; }
                    FiddlerCacheInfo cookieRequestModel = new FiddlerCacheInfo(cookieRequestStr);
                    if (CookieModel != null)
                    {
                        if (CookieModel.CookieStr.Equals(cookieRequestModel.CookieStr)) { return; }
                        //if (oSession.oRequest.headers["Cookie"] != null) { oSession.oRequest.headers.Remove("Cookie"); }

                        #region 更新cookie,不能完全替换
                        int changeCookieSign = 0;
                        foreach (var requestItem in cookieRequestModel.CookieDic)
                        {
                            if (CookieModel.CookieDic.ContainsKey(requestItem.Key) && !String.IsNullOrEmpty(requestItem.Value))
                            {
                                if (ProxyUrl.Contains("ins.chinalife-p.com.cn"))
                                {
                                    if (requestItem.Value.Contains("AlteonP73workbench"))
                                    {
                                        CookieModel.CookieDic[requestItem.Key] = requestItem.Value;
                                        SetRichText(String.Format("fiddler 键值对需要更新:{0}={1}", requestItem.Key, requestItem.Value));
                                        changeCookieSign++;
                                    }
                                }
                                else
                                {
                                    //CookieModel.CookieDic[requestItem.Key] = requestItem.Value;
                                    //SetRichText(String.Format("fiddler 键值对需要更新:{0}={1}", requestItem.Key, requestItem.Value));
                                    //标记需要更新
                                    changeCookieSign++;
                                }
                            }
                            else
                            {
                                CookieModel.CookieDic.Add(requestItem.Key, requestItem.Value);
                                SetRichText(String.Format("fiddler 键值对需要新增:{0}={1}", requestItem.Key, requestItem.Value));
                                changeCookieSign++;
                            }
                        }
                        if (changeCookieSign > 0)
                        {
                            //生成新的cookieSb
                            StringBuilder newCookieSb = new StringBuilder();
                            foreach (var item in CookieModel.CookieDic)
                            {
                                newCookieSb.Append(String.Format(";{0}={1}", item.Key, item.Value));
                            }
                            String newCookieStr = newCookieSb.ToString().Trim(new char[] { ';' }).Trim();
                            if (!String.IsNullOrEmpty(newCookieStr))
                            {
                                CookieModel = new FiddlerCacheInfo(newCookieStr);
                                SetRichText(String.Format("fiddler更新Cookie为:{0}", newCookieStr));
                            }
                        }
                        #endregion

                        oSession.oRequest.headers["Cookie"] = CookieModel.CookieStr;
                        SetRichText(String.Format("fiddler修改了请求:{0}", oSession.fullUrl.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("BeforeRequest异常:" + ex.Message));
                }
            }
        }

        public void FiddlerApplication_BeforeResponse(Fiddler.Session oSession)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("BeforeResponse异常:" + ex.Message));
            }
        }

        #endregion

        #region 4.定时器,定时与服务器通信更新本地cookie

        void StartSyncCookieTimer()
        {
            try
            {
                cookieSyncTimer.Interval = 60000 * 3;
                cookieSyncTimer.Elapsed += CookieSyncTimer_Elapsed;
                cookieSyncTimer.Start();
            }
            catch (Exception ex)
            { }
        }

        private void CookieSyncTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SyncCookie();
        }

        #endregion

        #region 5.定时器,定时查询服务器,是否更新

        void StartUpdateVersionTimer()
        {
            try
            {
                uptVersionTimer.Interval = 60000;
                uptVersionTimer.Elapsed += UptVersionTimer_Elapsed; ;
                uptVersionTimer.Start();
            }
            catch (Exception ex)
            { }
        }

        private void UptVersionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateVersion();
        }

        #endregion

        #region 6.启动与更新程序socket通信

        void StartUpdateProcessAndStartSocketConnect()
        {
            try
            {
                if (File.Exists(updateProcessPath))
                {
                    /*
                     * 1.更新程序存在
                     */

                    //1.启动socket监听
                    StartUpdateProcessAndStartSocketConnect_StartSocketServer();

                    //2.开启客户端
                    //Process.Start(updateProcessPath);

                }
            }
            catch (Exception ex)
            {
            }
        }

        void StartUpdateProcessAndStartSocketConnect_StartSocketServer()
        {
            try
            {
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取IP
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint port = new IPEndPoint(ip, Convert.ToInt32("10086"));
                socketWatch.Bind(port);
                socketWatch.Listen(100);
                //新建线程，去接收客户端发来的信息
                Thread td = new Thread(StartUpdateProcessAndStartSocketConnect_AcceptMgs);
                td.IsBackground = false;
                td.Start(socketWatch);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 接收客户端发送的信息
        /// </summary>
        /// <param name="o"></param>
        private void StartUpdateProcessAndStartSocketConnect_AcceptMgs(object o)
        {
            try
            {
                Socket socketWatc = (Socket)o;
                while (true)
                {
                    ////负责跟客户端通信的Socket
                    Socket socketSend = socketWatc.Accept();
                    clientSocket = socketSend;
                    //新建线程循环接收客户端发来的信息
                    Thread td = new Thread(StartUpdateProcessAndStartSocketConnect_Recive);
                    td.IsBackground = true;
                    td.Start(clientSocket);
                }
            }
            catch { }
        }

        private void StartUpdateProcessAndStartSocketConnect_Recive(object o)
        {
            Socket socketSend = (Socket)o;
            try
            {
                while (true)
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接受到的有效字节数
                    int r = socketSend.Receive(buffer);
                    string strMsg = Encoding.UTF8.GetString(buffer, 0, r);
                }
            }
            catch { }
        }

        public void SendInfo(String info)
        {
            if (clientSocket != null && !String.IsNullOrEmpty(info))
            {
                Task.Factory.StartNew(SendMsg, new SocketSendModel(clientSocket, info));
            }
        }

        public void SendMsg(Object _socketModel)
        {
            try
            {
                SocketSendModel socketModel = (SocketSendModel)_socketModel;
                Socket socketSend = socketModel.socketSend;
                String strSend = socketModel.strSend;
                byte[] buffer = Encoding.UTF8.GetBytes(strSend);
                //获得发送的信息时候，在数组前面加上一个字节 0
                List<byte> list = new List<byte>();
                list.AddRange(buffer);
                //将泛型集合转换为数组
                byte[] newBuffer = list.ToArray();
                //将了标识字符的字节数组传递给客户端
                socketSend.Send(newBuffer);
            }
            catch
            {
            }
        }

        #endregion

        #region 7.Others

        void SetRichText(String logInfo)
        {

            String curLogTxt = this.logTxt.Text.ToString();
            if (!String.IsNullOrEmpty(curLogTxt) && curLogTxt.Length > 5000)
            {
                this.logTxt.Text = "";
                curLogTxt = "";
            }

            if (!String.IsNullOrEmpty(logInfo))
            {
                this.logTxt.Text = String.Format("{0}\r\n{1}", curLogTxt, logInfo);
            }
        }

        string GetUserCodeFilePath()
        {
            String filePath = String.Format("{0}\\{1}", basePath, fileName);
            return filePath;
        }

        void InitProxyUrl(String fullProxyUrl)
        {
            try
            {

                if (String.IsNullOrEmpty(fullProxyUrl))
                {
                    this.ProxyUrl = "ins.chinalife-p.com.cn";
                    return;
                }
                //解析全部url如：https://ins.chinalife-p.com.cn/workbench/workbench/login.html  ==》 ins.chinalife-p.com.cn
                //解析全部url如：http://157.122.153.67:9000/khyx/login.jsp  ==》 157.122.153.67
                string regPatern = @"://[\S|\s]*?/";
                if (Regex.IsMatch(fullProxyUrl, regPatern))
                {
                    String newUrl = Regex.Match(fullProxyUrl, regPatern).Value.Replace("://", "").Replace("/", "");
                    if (newUrl.Contains(":"))
                    {
                        var spliturlArr = newUrl.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (spliturlArr.Length == 2)
                        {
                            this.ProxyUrl = spliturlArr[0];
                        }
                        else
                        {
                            this.ProxyUrl = newUrl;
                        }
                    }
                    else
                    {
                        this.ProxyUrl = newUrl;
                    }
                }
                else
                {
                    //默认为人寿
                    this.ProxyUrl = "ins.chinalife-p.com.cn";
                }

            }
            catch (Exception ex)
            {
                SetRichText(ex.Message);
            }
        }

        /// <summary>
        /// 此流程为从服务器更新cookie
        /// </summary>
        /// <returns></returns>
        bool SyncCookie()
        {
            bool syncStatus = false;
            try
            {
                String result = GetServerCookie();
                if (!String.IsNullOrEmpty(result))
                {
                    LoginInfoSyncResponse infoSyncResponse = JsonConvert.DeserializeObject<LoginInfoSyncResponse>(result);
                    if (infoSyncResponse.data != null && !String.IsNullOrEmpty(infoSyncResponse.data))
                    {
                        CookieModel = new FiddlerCacheInfo(infoSyncResponse.data);
                        InitProxyUrl(infoSyncResponse.proxyUrl);
                        SaveCookieSign = true;
                        syncStatus = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return syncStatus;
        }

        /// <summary>
        /// 关闭已启动代理工具
        /// </summary>
        void CloseSameProcess()
        {
            try
            {
                var processes = Process.GetProcessesByName("ChinaLifeTools");
                if (processes != null)
                {
                    foreach (var item in processes)
                    {
                        if (item.Id != Process.GetCurrentProcess().Id)
                        {
                            item.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        void UpdateVersion()
        {
            try
            {
                /**
                 * 1.获取服务器版本号
                 * 2.所有文件存储至本地目录
                 */
                //如果目录不存在,则重新创建目录
                if (!Directory.Exists(updateDic))
                { Directory.CreateDirectory(updateDic); }
                VersionInfoResponse localVersion = UpdateVersion_GetLocalVersion();
                VersionInfoResponse serverVersion = UpdateVersion_GetServerVersion();
                double serverV = UpdateVersion_ConvertVersionVal(serverVersion);
                double localV = UpdateVersion_ConvertVersionVal(localVersion);
                if (serverV > localV)
                {
                    //如服务器端版本信息大于本地，则从服务端下载最新版本，并更新本地信息
                    bool downLoadSign = UpdateVersion_DownloadFromServer(serverVersion);
                    if (downLoadSign)
                    {
                        //下载成功后，将最新版本信息更新至本地,并设置为未处理
                        serverVersion.updateSign = "1";
                        serverVersion.mainProcessPath = basePath;
                        serverVersion.mainProcessName = "ChinaLifeTools";
                        //覆盖到本地，提醒客户更新
                        File.WriteAllText(localVersionFilePath, JsonConvert.SerializeObject(serverVersion));
                        this.versionLabel.Text = uptTips;
                    }
                }
                else
                {
                    if (localV > 0 && "1".Equals(localVersion.updateSign))
                    {
                        this.versionLabel.Text = uptTips;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取本地版本号
        /// </summary>
        /// <returns></returns>
        VersionInfoResponse UpdateVersion_GetLocalVersion()
        {
            VersionInfoResponse localVersionInfo = new VersionInfoResponse();
            try
            {
                if (!File.Exists(localVersionFilePath))
                {
                    //写入空文件即可
                    File.WriteAllText(localVersionFilePath, "");
                    return localVersionInfo;
                }
                else
                {
                    String versionTxt = File.ReadAllText(localVersionFilePath, Encoding.UTF8);
                    if (!String.IsNullOrEmpty(versionTxt))
                    {
                        localVersionInfo = JsonConvert.DeserializeObject<VersionInfoResponse>(versionTxt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return localVersionInfo;
        }

        /// <summary>
        /// 获取服务端版本号
        /// </summary>
        /// <returns></returns>
        VersionInfoResponse UpdateVersion_GetServerVersion()
        {
            VersionInfoResponse serverVersionInfo = new VersionInfoResponse();
            try
            {
                /*
                 * 1.从服务端查询出版本号信息
                 */
                String serverVersionUrl = String.Format("http://localhost:45001/WebTool_Web_war_exploded/file/getLatestVersion.do?versionType=GSCProxyTool");
                String serverResult = HttpUtils.Get(serverVersionUrl, null);
                if (!String.IsNullOrEmpty(serverResult))
                {
                    serverVersionInfo = JsonConvert.DeserializeObject<VersionInfoResponse>(serverResult);
                }
            }
            catch (Exception ex)
            { }
            return serverVersionInfo;
        }

        /// <summary>
        /// 转换版本大小
        /// </summary>
        /// <param name="versionInfo"></param>
        /// <returns></returns>
        double UpdateVersion_ConvertVersionVal(VersionInfoResponse versionInfo)
        {
            double versionDouble = 0;
            try
            {
                if (versionInfo != null && versionInfo.data != null && !String.IsNullOrEmpty(versionInfo.data.versionNumber))
                {
                    double tmp;
                    if (double.TryParse(versionInfo.data.versionNumber, out tmp))
                    {
                        versionDouble = tmp;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return versionDouble;
        }

        /// <summary>
        /// 从服务端下载更新库
        /// </summary>
        /// <param name="serverVersionInfo"></param>
        /// <returns></returns>
        bool UpdateVersion_DownloadFromServer(VersionInfoResponse serverVersionInfo)
        {
            bool downloadSign = false;
            try
            {
                if (serverVersionInfo != null && serverVersionInfo.data != null && serverVersionInfo.data.downLoadUrl != null)
                {
                    //从服务端下载
                    String downloadFilePath = String.Format("{0}\\{1}", updateDic, serverVersionInfo.data.fileName);
                    HttpUtils.Get_SaveToLocalPath(serverVersionInfo.data.downLoadUrl, null, downloadFilePath);
                    if (File.Exists(downloadFilePath))
                    {
                        serverVersionInfo.downloadFilePath = downloadFilePath;
                        downloadSign = true;
                    }
                }
            }
            catch (Exception) { }
            return downloadSign;
        }

        #endregion

    }
}
