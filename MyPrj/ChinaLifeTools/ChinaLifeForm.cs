using BiHu.BaoXian.ClassCommon;
using ChinaLifeTools.models;
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
using System.Text;
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
        }

        #endregion

        #region 1.字段

        FiddlerCacheInfo CookieModel { get; set; }

        bool SaveCookieSign { get; set; }

        System.Timers.Timer cookieSyncTimer = new System.Timers.Timer();

        StringBuilder localSetCookieSb = new StringBuilder();

        string fileName = "usrcode.txt";

        #endregion

        #region 2.操作事件

        #region 2.1启动代理

        private void startProxyButton_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            if (rk.GetValue("ProxyEnable").ToString() == "1")
            {
                MessageBox.Show("系统代理已经是启动状态哦!,如需重新启动代理,请先点击关闭代理按钮!", "提醒");
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
                            if (!String.IsNullOrEmpty(infoSyncResponse.data))
                            {
                                try
                                {
                                    CookieModel = new FiddlerCacheInfo(infoSyncResponse.data.ToString());
                                    SaveCookieSign = true;
                                    FiddlerUtils.SetSSLCer();
                                    Fiddler.FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
                                    Fiddler.FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
                                    WebProxy.Start(8899);
                                    MessageBox.Show("代理已启动!!! 请执行以下步骤(建议使用Chrome浏览器哦)：\r\n\r\n\r\n1.打开浏览器\r\n\r\n2.执行 Ctrl + Shift + Delete 清空缓存\r\n\r\n3.重启浏览器,直接打开地址:\r\n https://ins.chinalife-p.com.cn/workbench/workbench/index.html", "提醒");
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(String.Format("启动代理发生异常，请与管理员联系!\r\n\r\n({0})\r\n\r\n注:可点击【关闭代理】按钮,再试一次哦!", ex.Message));
                                }
                            }
                            else
                            {
                                MessageBox.Show("服务器工号信息失效,请与管理员联系!", "提醒");
                            }
                        }
                        else
                        {
                            MessageBox.Show("服务器工号信息为空,请与管理员联系!", "提醒");
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
            MessageBox.Show("系统代理已关闭!\r\n\r\n如代理仍未关闭，请手动取消系统代理即可!", "提醒");
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

        #endregion

        #region 3.BeforeRequest&BeforeResponse

        public void FiddlerApplication_BeforeRequest(Fiddler.Session oSession)
        {
            try
            {
                if (!SaveCookieSign) { return; }
                if (!oSession.fullUrl.Contains("ins.chinalife-p.com.cn")) { return; }

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
                    if (oSession.oRequest.headers["Cookie"] != null) { oSession.oRequest.headers.Remove("Cookie"); }

                    #region 更新cookie,不能完全替换
                    int changeCookieSign = 0;
                    foreach (var requestItem in cookieRequestModel.CookieDic)
                    {
                        if (CookieModel.CookieDic.ContainsKey(requestItem.Key) && !String.IsNullOrEmpty(requestItem.Value))
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

                //if (!SaveCookieSign) { return; }
                //if (!oSession.fullUrl.Contains("ins.chinalife-p.com.cn")) { return; }
                //String cookieRequestStr = oSession.oRequest.headers["Cookie"].ToString();
                //if (String.IsNullOrEmpty(cookieRequestStr)) { return; }
                //FiddlerCacheInfo cookieRequestModel = new FiddlerCacheInfo(cookieRequestStr);
                //if (CookieModel != null)
                //{
                //    if (CookieModel.CookieStr.Equals(cookieRequestModel.CookieStr))
                //    {
                //        return;
                //    }

                //    var headerItem = oSession.oRequest.headers["Cookie"];
                //    if (headerItem != null)
                //    {
                //        oSession.oRequest.headers.Remove("Cookie");
                //    }

                //    #region 更新cookie,不能完全替换
                //    //foreach (var requestItem in cookieRequestModel.CookieDic)
                //    //{
                //    //    if (CookieModel.CookieDic.ContainsKey(requestItem.Key) && !String.IsNullOrEmpty(requestItem.Value))
                //    //    {
                //    //        CookieModel.CookieDic[requestItem.Key] = requestItem.Value;
                //    //    }
                //    //    else
                //    //    {
                //    //        CookieModel.CookieDic.Add(requestItem.Key, requestItem.Value);
                //    //    }
                //    //}
                //    #endregion

                //    oSession.oRequest.headers["Cookie"] = CookieModel.CookieStr;
                //    SetRichText(String.Format("fiddler修改了请求{0}Cookie", oSession.fullUrl.ToString()));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("BeforeRequest异常:" + ex.Message));
            }
        }

        public void FiddlerApplication_BeforeResponse(Fiddler.Session oSession)
        {
            try
            {

                //if (!oSession.fullUrl.Contains("ins.chinalife-p.com.cn")) { return; }
                //if (!SaveCookieSign) { return; }
                //String cookieResponseStr = oSession.oRequest.headers["Set-Cookie"].ToString();
                //FiddlerCacheInfo cookieResponseModel = new FiddlerCacheInfo(cookieResponseStr);
                //if (cookieResponseModel != null && cookieResponseModel.CookieDic.Count > 0)
                //{
                //StringBuilder setCookieSb = new StringBuilder();
                //foreach (var item in cookieResponseModel.CookieDic)
                //{
                //    if (!String.IsNullOrEmpty(item.Key) && !String.IsNullOrEmpty(item.Value))
                //    {
                //        if (!CookieModel.CookieDic.ContainsKey(item.Key))
                //        {
                //            setCookieSb.Append(String.Format(";{0}", cookieResponseStr));
                //        }
                //    }
                //}
                //if (!String.IsNullOrEmpty(setCookieSb.ToString()))
                //{
                //    String newLocalCookie = String.Format("{0}{1}", CookieModel.CookieStr, setCookieSb.ToString());
                //    CookieModel = new FiddlerCacheInfo(newLocalCookie);
                //}
                // && cookieResponseStr.Contains("_loginInfo_userId") && cookieResponseStr.Contains("_loginInfo_loginStructure")
                //if (!String.IsNullOrEmpty(cookieResponseModel.CookieStr) && cookieResponseModel.CookieDic.Count > 2)
                //    {
                //        SetRichText(String.Format("fiddler拦截到可用cookie为:{0}", cookieResponseStr));
                //        CookieModel = cookieResponseModel;
                //        SaveCookieSign = true;
                //    }
                //}
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
            {
            }
        }

        private void CookieSyncTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                String result = GetServerCookie();
                if (!String.IsNullOrEmpty(result))
                {
                    LoginInfoSyncResponse infoSyncResponse = JsonConvert.DeserializeObject<LoginInfoSyncResponse>(result);
                    if (!String.IsNullOrEmpty(infoSyncResponse.data))
                    {
                        CookieModel = new FiddlerCacheInfo(infoSyncResponse.data.ToString());
                        SaveCookieSign = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 5.Others

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
            String basePath = System.Environment.CurrentDirectory;
            String filePath = String.Format("{0}\\{1}", basePath, fileName);
            return filePath;
        }

        #endregion


    }

}
