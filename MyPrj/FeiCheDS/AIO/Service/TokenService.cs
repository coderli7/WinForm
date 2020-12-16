using FeiCheDS;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    /// <summary>
    /// token服务
    /// </summary>
    public static class TokenService
    {
        static readonly ILog Log = LogManager.GetLogger("TokenService");

        public static string Token { get; set; }
        public static long TimeStamp = DateTime.Now.Ticks;
        public static string companyId = Config.companyId;
        public static string key = Config.key;
        public static string url = Config.url;

        public static void GetToken()
        {
            try
            {
                var timeStamp = TimeStamp;
                var requestData = string.Format("companyid={0}&timestamp={1}&key={2}", companyId, timeStamp, key);
                var sign = (requestData).StringToMD5Hash().ToUpper();
                var requestData2 = string.Format("companyId={0}&timeStamp={1}", companyId, timeStamp);
                var requestDataWithSign = requestData2 + "&signature=" + sign;
                var res = HttpClientHelperCommon.Get(string.Format("{0}/AIOLogin?{1}", url, requestDataWithSign), null, false, 30);
                Token = JsonConvert.DeserializeObject<TokenACResponse>(res).data;
                Log.Info("Token: " + Token);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message + Environment.NewLine + ex.StackTrace);
                throw new Exception("获取Token异常");
            }
        }

    }
}
