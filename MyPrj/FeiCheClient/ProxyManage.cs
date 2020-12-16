
using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace FeiCheClient
{
    /// <summary>
    /// filder 拦截处理类
    /// </summary>
    public class ProxyManage
    {
        static int _random = new Random().Next(111, 999);
        //public static LoginParamModel _request;

        //private static readonly Log4NetHelp2 Log = new Log4NetHelp2(MainForm.UserName + "Fiddler监控日志");

        //请求拦截
        internal static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            #region 人脸识别步骤一 ___ 先用已经存在的头像图片,欺骗通过人脸识别检测
            if (oSession.fullUrl.Contains("/casserver/dwr/call/plaincall/FaceCheckUser.checkUserValid.dwr"))
            {
                /*
                oSession.utilDecodeRequest();
                var request = oSession.GetRequestBodyAsString();
                var reg = new Regex("scriptSessionId=([\\s|\\S]{35})");
                var match = reg.Match(request);
                var scriptSessionId = "";
                if (match.Success)
                {
                    scriptSessionId = match.Groups[1].Value;
                }
                scriptSessionId = scriptSessionId.Replace(scriptSessionId.Substring(scriptSessionId.Length - 3, 3), _random.ToString());
                //获取cookie
                var rqHeads = oSession.RequestHeaders;
                var requestCookie = string.Empty;
                foreach (var item in rqHeads)
                {
                    if (item.Name == "Cookie")
                    {
                        requestCookie = item.Value;
                        break;
                    }
                }
                var cookieDic = HttpClientHelper.String2Dictionary(requestCookie);
                var imageBase64 = GetImage();

                string account = Form1.Account;
                #region //postData 格式不要动,否则请求出错;
                var postData = @"callCount=1
                page=/casserver/faceLoginView.jsp?service=http%3A%2F%2F" + Configure.IP_Base + @"%3A" + Configure.Url_piccclaim_index_port +
  @"%2Fpiccclaim%2Fj_acegi_security_check&uCode=" + account + @"
                httpSessionId="
  + requestCookie + @"
                scriptSessionId=" + scriptSessionId + @"
                c0-scriptName=Face
                c0-methodName=getFRscore
                c0-id=0
                c0-param0=sting:" + imageBase64 + @"
                c0-param1=string:" + account + @"
                batchId=0";
                #endregion

                #region 人脸识别请求
                var url = "https://" + Configure.IP_Base_Login + ":" + Configure.Url_piccclaim_login_port + "/casserver/dwr/call/plaincall/Face.getFRscore.dwr";
                var res = "";
                try
                {
                    res = HttpClientHelper.Post(url,
                  postData, cookieDic, 60, "text/plain");
                }
                catch (Exception ex)
                {
                    //Log.Info(String.Format("人脸识别请求发生异常:{0}", ex.Message));
                }
                #endregion
                 */

            }
            #endregion
        }


        //响应拦截
        internal static void FiddlerApplication_BeforeResponse(Session oSession)
        {

            #region 人脸识别步骤二 ___ 人脸识别成功后,拼接系统给的登录地址
            //if (oSession.fullUrl.Contains("/casserver/faceLoginView.jsp"))
            //{
            //    oSession.utilDecodeResponse();
            //    var responseBody = oSession.GetResponseBodyAsString();


            //    var reg_encryptUSER = new Regex("(var encryptUSER=')[\\s|\\S]{1,60}", RegexOptions.Singleline); // 暂时配置 60 字符长度, 可能会出错这里
            //    var reg_location_href = new Regex("(location.href=\")[\\s|\\S]{1,240}", RegexOptions.Singleline); // 暂时配置 300 字符长度, 可能会出错这里

            //    var match_encryptUSER = reg_encryptUSER.Match(responseBody);
            //    var match_location_href = reg_location_href.Match(responseBody);

            //    if (match_encryptUSER.Success && match_location_href.Success)
            //    {
            //        var encryUser = match_encryptUSER.Groups[0].Value.TrimEnd();
            //        var encryUser_1 = match_encryptUSER.Groups[1].Value.TrimEnd();
            //        var encryUser_final = encryUser.Replace(encryUser_1, "").Replace("';", "");

            //        var location_href = match_location_href.Groups[0].Value.TrimEnd();
            //        var location_href_1 = match_location_href.Groups[1].Value.TrimEnd();
            //        var location_href_final = location_href.Replace(location_href_1, "").Replace("\";", "").Replace("\"+encryptUSER+\"&\"+\"", encryUser_final + "&");


            //        Location_href_Logined = location_href_final;
            //    }
            //}
            #endregion
        }


        #region 获取头像, 优先本地,没有再从外网获取(接口组提供下载地址)
        private static string GetImage()
        {
            string _filePath_jpg = string.Format(@"D:\{0}.jpg", Form1.Account);
            return GetBase64Image(_filePath_jpg);
            try
            {

                //var dir = Configure.Instance.HeadImgsPath;


                //if (!Directory.Exists(dir))
                //{
                //    Directory.CreateDirectory(dir);
                //}




                //var _filePath_png = dir + _request.UserName + ".png"; //本地图片
                //var _filePath_jpg = dir + _request.UserName + ".jpg";//本地图片

                //if (File.Exists(_filePath_png))
                //{
                //    Log.Info(String.Format("从本机获取png文件：{0}", _filePath_png));
                //    return GetBase64Image(_filePath_png);
                //}
                //else if (File.Exists(_filePath_jpg))
                //{
                //    Log.Info(String.Format("从本机获取jpg文件：{0}", _filePath_jpg));
                //    return GetBase64Image(_filePath_jpg);
                //}
                //else
                //{
                //    Log.Info(String.Format("从云端获取文件：{0}", dir));
                //    return GetHeadImgFromCloud(dir);
                //}
            }
            catch (Exception ex)
            {
                //Log.Info(String.Format("GetImage执行异常:{0}", ex.Message + ex.StackTrace));
                //FaceLoginErrMsg = "账号:" + _request.UserName + ",查找人脸识别头像异常:" + ex.Message;
                //return "123123123123";//随便来点,造假图片
            }

        }


        private static string GetHeadImgFromCloud(string dir)
        {
            return "";
            try
            {
                //从网络中获取
                //var imgUrl = Configure.Instance.HeadImgsPathCloud + _request.UserName + ".jpg";//跟接口组约定都是jpg格式
                //var request = WebRequest.Create(imgUrl);
                //var response = request.GetResponse();
                //using (var responseStream = response.GetResponseStream())
                //{
                //    int buffersize = 1024;
                //    byte[] buffer = new byte[buffersize];
                //    int count = responseStream.Read(buffer, 0, buffersize);
                //    if (count > 0)
                //    {
                //        var headImgPath = dir + _request.UserName + ".jpg";
                //        var fs = new FileStream(headImgPath, FileMode.Create, FileAccess.Write);//1.先保存本地;
                //        while (count > 0)
                //        {
                //            fs.Write(buffer, 0, count);
                //            count = responseStream.Read(buffer, 0, buffersize);
                //        }

                //        //Log.Info("头像下载成功: " + _request.UserName + ".jpg");
                //        return GetBase64Image(headImgPath);//2.获取网络中已经下载好的头像
                //    }
                //    else
                //    {
                //        //Log.Info("外网下载头像失败!!: " + _request.UserName + ".jpg");
                //        throw new Exception("头像不存在,请先上传头像");
                //    }
                //}
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("404"))
                //{
                //    //Log.Info("外网下载头像异常!!: " + _request.UserName + ".jpg");
                //    FaceLoginErrMsg = "账号:" + _request.UserName + ",下载头像不存在,请先上传图片";
                //    throw new Exception("头像不存在,请先上传头像");
                //}
                //throw ex;
            }
        }

        private static string GetBase64Image(string _filePath)
        {
            using (var fs = new FileStream(_filePath, FileMode.Open))
            {
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);//
                var str = Convert.ToBase64String(bytes);
                var strEncoding = HttpUtility.UrlEncode(str);
                return strEncoding;
            }
        }
        #endregion



        /// <summary>
        /// 人脸识别成功后,登录地址
        /// </summary>
        public static string Location_href_Logined { get; set; }

        /// <summary>
        /// 人脸识别请求错误消息
        /// </summary>
        public static string FaceLoginErrMsg { get; set; }
    }
}



/*
人脸识别请求body:

callCount=1
page=/casserver/faceLoginView.jsp?service=http%3A%2F%2F56.1.80.185%3A8072%2Fpiccclaim%2Fj_acegi_security_check&uCode=44782825
httpSessionId=NKQFdRQWZ8fJgn7BcfxqTgtz2Dqd2lhJXbvvX22GgyZpKyJtpP7y!-1189813724
scriptSessionId=FA93BA4953D93B61A54F3DCA66EFAF7C162
c0-scriptName=Face
c0-methodName=getFRscore
c0-id=0
c0-param0=sting:sdfsdfsf
c0-param1=string:44782825
batchId=0













 * 
 */
