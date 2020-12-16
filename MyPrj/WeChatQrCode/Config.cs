using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WindowHanler
{
    public class Config
    {
        static Config()
        {
            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            Y = rect.Height; //高（像素）
            X = rect.Width; //宽（像素）
        }

        private static int X;

        private static int Y;

        /// <summary>
        /// 当前屏幕分辨率宽度
        /// </summary>
        public static int WindowX
        {
            get
            {
                return X;
            }
        }

        /// <summary>
        /// 当前屏幕分辨率高度
        /// </summary>
        public static int WindowY
        {
            get
            {
                return Y;
            }
        }

        /// <summary>
        /// 消息按钮坐标百分比（X,Y）
        /// </summary>
        public static Dictionary<String, int> MsgWidthHeight
        {
            get
            {
                return GetXAndY("MsgWidthHeight");
            }
        }

        /// <summary>
        /// 点击消息后，需要动态验证码置顶
        /// </summary>
        public static Dictionary<String, int> MenuWidthHeight
        {
            get
            {
                return GetXAndY("MenuWidthHeight");
            }
        }

        /// <summary>
        /// 生成验证码按钮
        /// </summary>
        public static Dictionary<String, int> GenQrCodeWidthHeight
        {
            get
            {
                return GetXAndY("GenQrCodeWidthHeight");
            }
        }

        /// <summary>
        /// 左键触发单击按钮
        /// </summary>
        public static Dictionary<String, int> QrCodeLeftClickWidthHeight
        {
            get
            {
                return GetXAndY("QrCodeLeftClickWidthHeight");
            }
        }

        /// <summary>
        /// 右键复制按钮坐标
        /// </summary>
        public static Dictionary<String, int> QrCodeRightCopyClickWidthHeight
        {
            get
            {
                return GetXAndY("QrCodeRightCopyClickWidthHeight");
            }
        }

        /// <summary>
        /// 点击验证码空白处（一般用来清除上次选中内容）
        /// </summary>
        public static Dictionary<String, int> NullSpaceClickWidthHeight
        {
            get
            {
                return GetXAndY("NullSpaceClickWidthHeight");
            }
        }

        /// <summary>
        /// 消息按钮坐标百分比（X,Y）
        /// </summary>
        /// <param name="confKey"></param>
        /// <returns></returns>
        private static Dictionary<String, int> GetXAndY(String confKey)
        {
            Dictionary<String, int> dic = new Dictionary<string, int>();
            try
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[confKey]))
                {
                    String[] arr = ConfigurationManager.AppSettings[confKey].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 2)
                    {
                        dic.Add("X", Convert.ToInt32(WindowX * Convert.ToDouble(arr[0])));
                        dic.Add("Y", Convert.ToInt32(WindowY * Convert.ToDouble(arr[1])));
                    }
                }
            }
            catch (Exception)
            {
            }
            return dic;
        }

        /// <summary>
        /// Socket服务端IP地址（用来和报价服务相互通信）
        /// </summary>
        public static String SocketServerIp
        {
            get
            {
                return ConfigurationManager.AppSettings["SocketServerIp"];
            }
        }


        /// <summary>
        /// Socket服务端端口号
        /// </summary>
        public static int SocketServerPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SocketServerPort"]);
            }
        }

        /// <summary>
        /// 百度api相关配置
        /// </summary>
        public static String APP_ID
        {
            get
            {
                string app_id = "14220143";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["APP_ID"]))
                {
                    app_id = ConfigurationManager.AppSettings["APP_ID"];
                }
                return app_id;

            }
        }

        /// <summary>
        /// 百度api相关配置
        /// </summary>
        public static String API_KEY
        {
            get
            {
                string api_key = "ZF31ZkVGPBzl03jBGjTtic3E";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_KEY"]))
                {
                    api_key = ConfigurationManager.AppSettings["API_KEY"];
                }

                return api_key;

            }
        }

        /// <summary>
        /// 百度api相关配置
        /// </summary>
        public static String SECRET_KEY
        {
            get
            {
                string secret_key = "PvDTdq1L5ZhSx3lyIfiLjqATMHwKLSRw";
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SECRET_KEY"]))
                {
                    secret_key = ConfigurationManager.AppSettings["SECRET_KEY"];
                }
                return secret_key;

            }
        }

        /// <summary>
        /// 是否启用百度API(默认启用：1，如果清空，则按照复制粘贴操作)
        /// </summary>
        public static bool USE_BD_API
        {
            get
            {
                bool use = false;
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["USE_BD_API"]) && ConfigurationManager.AppSettings["USE_BD_API"] == "1")
                {
                    use = true;
                }
                return use;
            }
        }

        /// <summary>
        /// 微信id,用于请求验证码
        /// </summary>
        public static String Userid_Weixin
        {
            get
            {
                string wxId = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["userid_weixin"]))
                {
                    wxId = ConfigurationManager.AppSettings["userid_weixin"];
                }
                return wxId;
            }
        }

        /// <summary>
        /// 微信url,用于请求验证码
        /// </summary>
        public static String WX_URL
        {
            get
            {
                string wx_url = "http://wxqy.chinalife-p.com.cn:8084/dynamic_pass_web/TwoFactor/getCode";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["wx_url"]))
                {
                    wx_url = ConfigurationManager.AppSettings["wx_url"];
                }
                return wx_url;
            }
        }

        /// <summary>
        /// 截取屏幕验证码时,X轴比例
        /// </summary>
        public static Double CopySrceenX
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings["CopySrceenX"]);
            }
        }

        /// <summary>
        /// 配置多个socket服务端，格式如：（192.168.1.1：8899,192.168.1.2:8890）
        /// 默认配置为 127.0.0.1:8899 本机地址
        /// </summary>
        public static Dictionary<String, int> SocketServerAdresses
        {
            get
            {
                Dictionary<string, int> serverArr = new Dictionary<string, int>();
                try
                {
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SocketServerAdresses"]))
                    {
                        String socketServerAdressesConf = ConfigurationManager.AppSettings["SocketServerAdresses"];
                        String[] ipAndPortArr = socketServerAdressesConf.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in ipAndPortArr)
                        {
                            String[] ipAndPort = item.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (ipAndPort != null && ipAndPort.Count() == 2 && !serverArr.Keys.Contains("ipAndPort[0]"))
                            {
                                serverArr.Add(ipAndPort[0], Convert.ToInt32(ipAndPort[1]));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    serverArr.Add("127.0.0.1", 8899);
                }
                return serverArr;
            }
        }

        /// <summary>
        /// 固定的验证码（比如有的渠道验证码是固定的，如果配置，则每次都返回固定的验证码，默认为空）
        /// </summary>
        public static string FixedQrCode
        {
            get
            {
                return ConfigurationManager.AppSettings["FixedQrCode"];
            }
        }

        /// <summary>
        /// 配置窗口标题，根据标题名称，自动最小化,格式如：发起会话，远程等
        /// </summary>
        public static String[] MinFormTitles
        {
            get
            {
                List<String> titles = new List<string>();
                titles.Add("发起会话");
                titles.Add("TeamViewer Panel");
                //WindowFormAPI.MinOrMaxWindowFormByTitle(new string[] { "发起会话", "TeamViewer Panel" }, 2);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MinFormTitles"]))
                {
                    string titlesConf = ConfigurationManager.AppSettings["MinFormTitles"];
                    String[] titlesConfArr = titlesConf.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    titles.AddRange(titlesConfArr);
                }
                return titles.ToArray();

            }
        }

    }
}
