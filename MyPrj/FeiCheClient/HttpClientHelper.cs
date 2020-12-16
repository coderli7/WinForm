using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FeiCheClient
{
    public class HttpClientHelper
    {

        //private static readonly Log4NetHelp2 Log = new Log4NetHelp2(MainForm.UserName + "HttpClientHelper");

        public static string Post(string url, string postData, Dictionary<string, string> cookies, int timeout = 30, string contentType = null, Dictionary<string, string> headers = null)
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

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

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

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteCertificateValidate);

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = IsRedirect, Proxy = null })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                client.Timeout = new TimeSpan(0, 0, timeout);

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
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            //为了通过证书验证，总是返回true
            return true;
        }


        /// <summary>
        /// 获取医审cookie信息
        /// </summary>
        //public static string GetCookie(string url, string cookie, string TaskId)
        //{
        //    string personId = string.Empty;
        //    try
        //    {
        //        Regex reg = new Regex("name=\"prpLperson\\.id\"\\s*value=\"([0-9]+)\"");
        //        var personTraceHtml = Get(url, GetDirCookies(cookie));
        //        Match match = reg.Match(personTraceHtml);
        //        personId = match.Groups[1].Value;//获取prpLperson.id
        //        //Log.Info("获取personId:" + personId);
        //        var url2 = Configure.Instance.GetUrlBusinessServlet(TaskId, personId);
        //        var cookieOfMedical = GetResponseHeaderString(url2, GetDirCookies(cookie), "Set-Cookie");
        //        if (!string.IsNullOrWhiteSpace(cookieOfMedical))
        //            return cookieOfMedical;
        //        else
        //            return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}


        /// <summary>
        /// 获取响应头中的信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="IsRedirect"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string GetResponseHeaderString(string url, Dictionary<string, string> cookies, string key, bool IsRedirect = true, int timeout = 30)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = IsRedirect, Proxy = null })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                client.Timeout = new TimeSpan(0, 0, timeout);

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
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                var response = client.GetAsync(url).Result;
                //Log.Info("GetResponseHeaderString\r\n" + JsonConvert.SerializeObject(response));
                var result = string.Empty;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.RequestMessage.RequestUri.ToString().Contains("/casserver/login"))//根目录cookie文件下的cookie如果失效，会跳转到登录页
                    {
                        return result;
                    }
                    var resultList = response.Headers.GetValues(key).ToList();
                    var list = new List<string>();

                    if (!resultList.Any()) return result;
                    foreach (var item in resultList)
                    {
                        var cookieStr = item.Split(new[] { ';' })[0];
                        if (!string.IsNullOrWhiteSpace(cookieStr))
                            list.Add(cookieStr);
                    }
                    if (list.Count > 0)
                        result = string.Join(";", list);
                }
                return result;
            }
        }

        /// <summary>
        /// 获取医审页面cookie, 不通过缓存
        /// </summary>
        /// <param name="cookie">主页面的cookie</param>
        /// <param name="registNo">报案号</param>
        /// <param name="taskId">任务号</param>
        /// <returns></returns>
        public static string CookieOfMedical(string cookie, string registNo, string taskId)
        {
            return "";
            //判断缓存里面是否有值
            //if (CommonCla.CacheIsHave(cookie))
            //{
            //    return CommonCla.GetCache(cookie) != null ? CommonCla.GetCache(cookie).ToString() : null;
            //}
            ////#region 获取医审cookie字符串

            //var personIdUrl = Configure.Instance.GetPersonIdUrl(registNo, taskId);
            //var requestCookie = GetCookie(personIdUrl, cookie, taskId);

            //if (!string.IsNullOrWhiteSpace(requestCookie))
            //{
            //    //写入缓存
            //    CommonCla.AddCacheAbsolute(cookie, requestCookie, 3000);//缓存50分钟；todo: 缓存时间最好小于人保系统登录时候的cookie过期时间；
            //    return requestCookie;
            //}
            //return requestCookie;
        }

        /// <summary>
        /// 获取医审cookie信息
        /// </summary>
        /// <param name="cookie">非车首页cookie信息</param>
        /// <param name="request">请求模型</param>
        /// <param name="paramModel">人工模拟请求模型</param>
        /// <param name="result"></param>
        /// <returns></returns>
        //public static string GetMediaCookie(string cookie, MediaPageBase request, ArtificialParamModel paramModel, string result, string _filePath)
        //{
        //    return "";
        //    //var requestCookie = CookieOfMedical(cookie, request.RegistNo, request.TaskId);
        //    //if (requestCookie == null)
        //    //{
        //    //    Login.LoginLoseEfficacy(request.LoginParam);
        //    //    if (Login.TryLogin2(paramModel, request.LoginParam, ref result))
        //    //    {
        //    //        cookie = GetCookieToFile(_filePath);//重新获取cookie信息
        //    //        requestCookie = CookieOfMedical(cookie, request.RegistNo, request.TaskId);
        //    //    }
        //    //    else
        //    //        return result;
        //    //}
        //    //return requestCookie;
        //}


        /// <summary>
        /// Cookies转换
        /// </summary>
        public static Dictionary<string, string> GetDirCookies(string cookies)
        {
            Dictionary<string, string> dirCookies = new Dictionary<string, string>();

            string[] cookieArr = cookies.Split(';');
            if (cookieArr == null) return dirCookies;
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
        /// 从指定路径文件获取cookie信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetCookieToFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("获取cookie信息失败");
            }
            return null;
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

        /// <summary>
        /// Cookies转换
        /// </summary>
        public static Dictionary<string, string> String2Dictionary(string cookies)
        {
            Dictionary<string, string> dirCookies = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(cookies)) return dirCookies;

            string[] cookieArr = cookies.Split(';');
            try
            {
                if (cookieArr.Length > 0)
                {
                    foreach (var item in cookieArr)
                    {
                        if (item.IndexOf('=') != -1)
                        {
                            string[] itemArr = item.Split('=');
                            if (itemArr.Length > 0)
                            {
                                if (!dirCookies.ContainsKey(itemArr[0].Trim()))
                                {
                                    dirCookies.Add(itemArr[0].Trim(), itemArr[1]);
                                }
                            }
                        }
                        else
                        {
                            if (!dirCookies.ContainsKey(item.Trim()))
                            {
                                dirCookies.Add(item.Trim(), string.Empty);
                            }
                        }

                    }
                }
                return dirCookies;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
