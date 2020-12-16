using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class CookieMsg
    {
        private static Dictionary<string, string> cookieDic = new Dictionary<string, string>();

        public static void AddOrSetCookie(string cookie)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                foreach (var item in cookie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int index = item.IndexOf('=');
                    if (index > 0)
                    {
                        AddOrSetCookie(item.Substring(0, index), item.Substring(index + 1));
                    }
                }
            }
        }

        public static void AddOrSetCookie(string cookieKey, string cookieValue)
        {
            cookieKey = cookieKey.Trim();
            cookieValue = cookieValue.Trim();
            //if (cookieKey != "prpallJSESSIONID" && cookieKey != "JSESSIONID")
            //{
            //    return;
            //}
            if (cookieDic.Keys.Contains(cookieKey))
            {
                cookieDic[cookieKey] = cookieValue;
            }
            else
            {
                cookieDic.Add(cookieKey, cookieValue);
            }
        }

        public static Dictionary<string, string> GetCookie()
        {
            return cookieDic;
        }
    }
}
