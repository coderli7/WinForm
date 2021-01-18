using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.Utils
{
    public class HttpUtils
    {
        public static string Get(string url, Dictionary<string, string> cookies, bool IsRedirect = true, int timeout = 30)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
        new RemoteCertificateValidationCallback(
                  OnRemoteCertificateValidationCallback);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = IsRedirect })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                    if (cookies != null)
                    {
                        foreach (var item in cookies)
                        {
                            try { cookieContainer.Add(new Uri(url), new Cookie(item.Key, item.Value)); } catch { }
                        }
                    }
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                    client.DefaultRequestHeaders.Add("Accept", "*/*");
                    var response = client.GetAsync(url).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void Get_SaveToLocalPath(string url, Dictionary<string, string> cookies, String localFilePath, bool IsRedirect = true, int timeout = 30)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
        new RemoteCertificateValidationCallback(
                  OnRemoteCertificateValidationCallback);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = IsRedirect })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                    if (cookies != null)
                    {
                        foreach (var item in cookies)
                        {
                            try { cookieContainer.Add(new Uri(url), new Cookie(item.Key, item.Value)); } catch { }
                        }
                    }
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)");
                    client.DefaultRequestHeaders.Add("Accept", "*/*");

                    Stream outStream = new FileStream(localFilePath, FileMode.Create);
                    byte[] arr = new byte[1024];
                    var responseStream = client.GetStreamAsync(url).Result;
                    int size = responseStream.Read(arr, 0, arr.Length);
                    while (size > 0)
                    {
                        outStream.Write(arr, 0, size);
                        size = responseStream.Read(arr, 0, arr.Length);
                    }
                    outStream.Close();
                    responseStream.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        private static bool OnRemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }

}
