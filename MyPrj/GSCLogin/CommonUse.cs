using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;


namespace GSCLogin
{
    public class CommonUse
    {
        public static String PostHtml(string url, string postData)
        {
            string retVal = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                retVal = responseString.ToString();
            }
            catch (Exception ex)
            {
                retVal = String.Format("请求发生异常：{0}", ex.Message);
            }
            return retVal;
        }
    }
}
