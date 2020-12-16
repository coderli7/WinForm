using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FeiCheClient
{
    public class Configure
    {
        public static String Url_piccclaim_index_port { get { return GetValue("Url_piccclaim_index_port"); } }

        public static String IP_Base { get { return GetValue("IP_Base"); } }

        public static String IP_Base_Login { get { return GetValue("IP_Base_Login"); } }

        public static String Url_piccclaim_login_port { get { return GetValue("Url_piccclaim_login_port"); } }


        static String GetValue(string key)
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? "" : ConfigurationManager.AppSettings[key];
        }
    }
}
