using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class HttpHelper
    {
        public static string SimpleGetOrPostUrlData(string GetOrPostUrl, string TransData, string TransType, string EncodeType = "UTF-8", bool allowAutoRedirect = true)
        {
            string Result = "";
            byte[] bs = Encoding.ASCII.GetBytes(TransData);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(GetOrPostUrl);
            req.AllowAutoRedirect = allowAutoRedirect;
            try
            {
                #region 解析cookies
                Uri uri = new Uri(GetOrPostUrl);
                CookieContainer cookieContainer = new CookieContainer();
                Dictionary<string, string> cookieDic = CookieMsg.GetCookie();
                foreach (var item in cookieDic)
                {
                    cookieContainer.Add(new Uri(GetOrPostUrl), new Cookie(item.Key, item.Value));
                }
                if (cookieContainer.Count > 0)
                {
                    req.CookieContainer = cookieContainer;

                }

                #endregion

                req.Method = TransType;
                if (TransType != "GET")
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = bs.Length;
                    Stream reqstream = req.GetRequestStream();
                    reqstream.Write(bs, 0, bs.Length);
                }
                req.Accept = "*/*";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
                var res = req.GetResponse();
                using (Stream ReceiveStream = req.GetResponse().GetResponseStream())
                {
                    Encoding encode = System.Text.Encoding.GetEncoding(EncodeType);
                    StreamReader readStream = new StreamReader(ReceiveStream, encode);
                    Char[] read = new Char[1024];
                    int count = readStream.Read(read, 0, 1024);
                    StringBuilder ReceiveStreamSb = new StringBuilder();
                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        ReceiveStreamSb.Append(str);
                        count = readStream.Read(read, 0, 1024);
                    }

                    #region 设置返回Cookie
                    if (req.GetResponse().Headers != null && req.GetResponse().Headers.AllKeys.Contains("Set-Cookie"))
                    {
                        string setCookieStr = req.GetResponse().Headers["Set-Cookie"];
                        string[] setCookieArr = setCookieStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        foreach (string item in setCookieArr)
                        {
                            if (!item.Contains("path="))
                            {
                                string[] keyValueArr = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                                if (keyValueArr.Count() == 2)
                                {
                                    CookieMsg.AddOrSetCookie(keyValueArr[0], keyValueArr[1]);
                                }
                            }
                        }
                    }
                    #endregion

                    Result = Result + ReceiveStreamSb.ToString();
                    readStream.Close();
                    ReceiveStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                req.Abort();
            }
            return Result;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
