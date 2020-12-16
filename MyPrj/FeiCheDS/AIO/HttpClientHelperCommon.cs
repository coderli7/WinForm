using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FeiCheDS
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientHelperCommon
    {

        public static string Post(string url, string postData, Dictionary<string, string> cookies, int timeout = 30, string contentType = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, Proxy = null })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                client.Timeout = new TimeSpan(0, 0, timeout);

                HttpContent content = new StringContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue(string.IsNullOrEmpty(contentType) ? "application/x-www-form-urlencoded" : contentType);
                if (cookies != null)
                {
                    foreach (var item in cookies)
                    {
                        try
                        {
                            cookieContainer.Add(new Uri(url), new Cookie(item.Key, item.Value));
                        }
                        catch
                        {

                        }

                    }
                }
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                var response = client.PostAsync(url, content).Result;

                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }

        public static string Post(string url, string postData, string cookieStr, int timeout = 30, string contentType = null)
        {
            return Post(url, postData, GetDirCookies(cookieStr), timeout, contentType);
        }
        /// <summary>
        /// 普通post请求，请求人保系统，不能使用这个
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string postData, int timeout = 30, string contentType = "application/json")
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeout);
                HttpContent content = new StringContent(postData);
                MediaTypeHeaderValue typeHeader = new MediaTypeHeaderValue(contentType);
                typeHeader.CharSet = "utf-8";
                content.Headers.ContentType = typeHeader;
                HttpResponseMessage result = client.PostAsync(url, content).Result;
                if (result.IsSuccessStatusCode)
                {
                    return result.Content.ReadAsStringAsync().Result;
                }
                return null;
            }
        }

        public static string Get(string url, Dictionary<string, string> cookies, bool IsRedirect = true, int timeout = 30)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = IsRedirect, Proxy = null })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                client.Timeout = new TimeSpan(0, 0, timeout);

                if (cookies != null && cookies.Count > 0)
                {
                    foreach (var item in cookies)
                    {
                        try
                        {
                            cookieContainer.Add(new Uri(url), new Cookie(item.Key, item.Value));
                        }
                        catch
                        {

                        }
                    }
                }
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        /// <summary>
        /// Cookies转换
        /// </summary>
        public static Dictionary<string, string> GetDirCookies(string cookies)
        {
            Dictionary<string, string> dirCookies = new Dictionary<string, string>();

            string[] cookieArr = cookies.Split(';');
            foreach (var item in cookieArr)
            {
                string[] itemArr = item.Split('=');
                if (!dirCookies.ContainsKey(itemArr[0].Trim()))
                    dirCookies.Add(itemArr[0].Trim(), itemArr[1]);
            }
            return dirCookies;
        }

        public static int Post(string url, string strData, out string ret, int timeout = 30)
        {
            byte[] data = Encoding.UTF8.GetBytes(strData);
            ret = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1000 * timeout;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            var stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
            var response = (HttpWebResponse)request.GetResponse();
            if (request.HaveResponse)
            {
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    ret = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                }
                response.Close();
            }
            return 0;
        }

        /// <summary>
        /// Url编码,GB2312
        /// </summary>
        public static string UrlEncode(string txt, string code = "GB2312")
        {
            return HttpUtility.UrlEncode(txt, Encoding.GetEncoding(code));
        }
        /// <summary>
        /// Url解码
        /// </summary>
        protected static string UrlDecode(string txt, string code = "GB2312")
        {
            return HttpUtility.UrlDecode(txt, Encoding.GetEncoding(code));
        }
    }
}
