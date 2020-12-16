using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace VPNRedial
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class Config
    {

        /// <summary>
        /// PIN码登陆（如果VPN登陆需要PIN码，此处节点配置,如果只配置了1则认为虽然不是pin码登陆的VPN，但仍然采用抓句柄的方式登陆）
        /// </summary>
        public static string PIN
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["PIN"]) ? ConfigurationManager.AppSettings["PIN"] : "";
            }
        }

        /// <summary>
        /// VPN账号）
        /// </summary>
        public static string VPNUser
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["VPNUser"]) ? ConfigurationManager.AppSettings["VPNUser"] : "";
            }
        }

        /// <summary>
        ///VPN密码
        /// </summary>
        public static string VPNPassword
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["VPNPassword"]) ? ConfigurationManager.AppSettings["VPNPassword"] : "";
            }
        }

        /// <summary>
        ///VPN安装包路径（如VPN登陆软件未安装在C盘目录下，则配置）
        /// </summary>
        public static string SangforCSClientPath
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["SangforCSClientPath"]) ? ConfigurationManager.AppSettings["SangforCSClientPath"] : @"C:\Program Files (x86)\Sangfor\SSL\SangforCSClient\SangforCSClient.exe";
            }
        }

        /// <summary>
        /// VPN登录地址
        /// </summary>
        public static string VPNAdress
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["VPNAdress"]) ? ConfigurationManager.AppSettings["VPNAdress"] : "";
            }
        }

        /// <summary>
        /// 监测VPN地址
        /// </summary>
        public static string LoginAdress
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoginAdress"]) ? ConfigurationManager.AppSettings["LoginAdress"] : "http://9.0.6.69:7001/prpall/index.jsp";
            }
        }

        /// <summary>
        /// 监测间隔时间
        /// </summary>
        public static string TimerInterval
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimerInterval"]) ? ConfigurationManager.AppSettings["TimerInterval"] : "2";
            }
        }

        /// <summary>
        /// 重播工具需要结束的进程
        /// </summary>
        public static List<String> ClosePrcesses
        {
            get
            {
                List<String> prcList = new List<string>();
                prcList.Add("SangforCSClient");
                prcList.Add("SangforPromoteService");
                prcList.Add("LogoutTimeOut");
                prcList.Add("iexplore");
                prcList.Add("firefox");
                prcList.Add("chrome");
                prcList.Add("360chrome");
                prcList.Add("360se");

                String ClosePrcNamesTmp = ConfigurationManager.AppSettings["ClosePrcesses"];
                if (!string.IsNullOrEmpty(ClosePrcNamesTmp))
                {
                    String[] arrTmp = ClosePrcNamesTmp.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrTmp != null && arrTmp.Count() > 0)
                    {
                        prcList.AddRange(arrTmp);
                    }
                }
                return prcList;
            }
        }


        /// <summary>
        /// 是否不需要验证码
        /// </summary>
        public static string UnNeedQrCode
        {
            get
            {
                return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["UnNeedQrCode"]) ? ConfigurationManager.AppSettings["UnNeedQrCode"] : "";
            }
        }



    }
}
