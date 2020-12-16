using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    public class Config
    {

        public static String companyId { get { return GetValue("companyId"); } }

        public static String key { get { return GetValue("key"); } }

        public static String url { get { return GetValue("url"); } }

        public static String Url_upload { get { return String.Format("{0}{1}", GetValue("url"), "/AIOCaseUpload"); } }

        public static String Url_getCaseStatus { get { return String.Format("{0}{1}", GetValue("url"), "/AIOGetCaseStatus"); } }

        public static String ImgLocation
        {
            get
            {

                return String.IsNullOrEmpty(GetValue("imgLocation")) ? String.Format("{0}\\{1}", System.Environment.CurrentDirectory, "image") : GetValue("imgLocation");
                //return GetValue("imgLocation");
            }
        }

        public static String ReceiveDataLocation { get { return GetValue("receiveDataLocation"); } }


        static String GetValue(string key)
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? "" : ConfigurationManager.AppSettings[key];
        }




    }
}
