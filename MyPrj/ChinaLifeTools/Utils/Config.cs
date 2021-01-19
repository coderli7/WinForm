using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.Utils
{
    public class Config
    {
        private static string _userName;

        public static String UserName
        {
            get
            {
                String confDefault = _userName;
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["username"]))
                {
                    confDefault = ConfigurationManager.AppSettings["username"];
                }
                return confDefault;
            }
            set { _userName = value; }
        }

        public static String CookieUrl
        {
            get
            {
                String confDefault = "http://111.198.29.209:35001/webtool/bhChannelinfo/getChannelCookie.do";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CookieUrl"]))
                {
                    confDefault = ConfigurationManager.AppSettings["CookieUrl"];
                }
                return confDefault;
            }
        }

        public static String ServerVersionUrl
        {
            get
            {
                String confDefault = "http://111.198.29.209:35001/webtool/file/getLatestVersion.do?versionType=GSCProxyTool";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ServerVersionUrl"]))
                {
                    confDefault = ConfigurationManager.AppSettings["ServerVersionUrl"];
                }
                return confDefault;
            }
        }

    }
}
