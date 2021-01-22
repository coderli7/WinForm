using ChinaLifeTools.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools
{
    public class FiddlerCacheInfo
    {
        public FiddlerCacheInfo(String cookieInfo)
        {
            #region 1.初始化cookie信息
            this.CookieStr = cookieInfo;
            var cookieItems = cookieInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            CookieDic = new Dictionary<string, string>();
            foreach (var cookieItem in cookieItems)
            {
                //需考虑cookie Value中包含等号的情况
                //var cookieItemArr = cookieItem.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                var cookieItemArr = ConvertCookieToArr(cookieItem);
                if (cookieItemArr != null && cookieItemArr.Count() == 2)
                {
                    String newCookieKey = String.IsNullOrEmpty(cookieItemArr[0]) ? "" : cookieItemArr[0].Trim();
                    String newCookieVal = String.IsNullOrEmpty(cookieItemArr[1]) ? "" : cookieItemArr[1].Trim();
                    if (!String.IsNullOrEmpty(newCookieVal))
                    {
                        this.CookieDic.Add(newCookieKey, newCookieVal);
                    }
                }
            }
            #endregion
        }

        public String CookieStr { get; set; }

        public Dictionary<String, String> CookieDic { get; set; }

        public String[] ConvertCookieToArr(String cookieStr)
        {
            String[] cookieArr = new string[2];
            if (!String.IsNullOrEmpty(cookieStr))
            {
                int firIdx = cookieStr.IndexOf('=');
                if (firIdx > 0)
                {
                    cookieArr[0] = cookieStr.Substring(0, firIdx);
                    cookieArr[1] = cookieStr.Substring(firIdx + 1);
                }
            }
            return cookieArr;
        }

    }
}
