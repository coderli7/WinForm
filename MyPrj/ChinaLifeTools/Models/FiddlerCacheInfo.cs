using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools
{
    class FiddlerCacheInfo
    {
        public FiddlerCacheInfo(String cookieInfo)
        {
            this.CookieStr = cookieInfo;
            var cookieItems = cookieInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            CookieDic = new Dictionary<string, string>();
            foreach (var cookieItem in cookieItems)
            {
                var cookieItemArr = cookieItem.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (cookieItemArr != null && cookieItemArr.Count() == 2)
                {
                    this.CookieDic.Add(cookieItemArr[0], cookieItemArr[1]);
                }
            }
        }


        public String CookieStr { get; set; }

        public Dictionary<String, String> CookieDic { get; set; }

    }
}
