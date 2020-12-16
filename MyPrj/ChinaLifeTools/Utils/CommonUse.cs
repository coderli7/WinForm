using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.Utils
{
    public class CommonUse
    {

        #region 0.post

        public static string Post(string url, string postData, Dictionary<string, string> cookies, int timeout = 60, string contentType = null, bool useProxy = true, string chartset = null)
        {
            ServicePointManager.ServerCertificateValidationCallback =
            new RemoteCertificateValidationCallback(
                      OnRemoteCertificateValidationCallback);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            var cookieContainer = new CookieContainer();
            //, Proxy = useProxy ? _webProxy : null 
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                client.Timeout = new TimeSpan(0, 0, timeout);
                HttpContent content = new StringContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue(string.IsNullOrEmpty(contentType) ? "application/x-www-form-urlencoded" : contentType);
                if (chartset != null)
                {
                    content.Headers.ContentType.CharSet = chartset;
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                }
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
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                var response = client.PostAsync(url, content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }

        private static bool OnRemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion
    }
}
