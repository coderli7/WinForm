using FeiCheDS;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    /// <summary>
    /// FileUpload服务
    /// </summary>
    public static class FileUploadService
    {
        public static void UploadFile()
        {
            try
            {
                var token_req = TokenService.Token;
                var companyId_req = Config.companyId;
                var key_req = Config.key;
                var caseId = UploadFileForm.docNo;
                var caseInfo = "";
                var priorityNo = "0";
                var timeStamp = DateTime.Now.Ticks;
                var signature_before_cal = "token=" + token_req + "&" +
                                "companyId=" + companyId_req + "&" +
                                "caseId=" + caseId + "&" +
                                "caseInfo=" + caseInfo + "&" +
                                "priorityNo=" + priorityNo + "&" +
                                 "timeStamp=" + timeStamp + "&key=" + key_req;
                var signature = signature_before_cal.StringToMD5Hash().ToUpper();
                //暂时不处理
                string _caseImagesPath = "";
                DirectoryInfo di = new DirectoryInfo(_caseImagesPath);
                var images = new List<string>();
                foreach (var item in di.GetFiles())
                {
                    images.Add(item.Name);
                }
                var postDataModel = new
                {
                    token = token_req,
                    companyId = companyId_req,
                    caseId = caseId,
                    caseInfo = caseInfo,
                    priorityNo = priorityNo,
                    images = images,
                    timeStamp = timeStamp,
                    signature = signature
                };

                var postData = postDataModel.SerializeObject();

                var res = HttpClientHelperCommon.Post(Config.Url_upload, postData);

                //Log.Info("请求地址:" + url_upload + Environment.NewLine + JsonConvert.SerializeObject(postData));
                JObject obj = JObject.Parse(res);

                //Log.Info(obj["message"].ToString());

                if (obj["code"].ToString() == "0")
                {
                    // label2.Text = "赔案信息上传到安诚成功!";
                }
                else
                {
                    //label2.Text = obj["message"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Log.Fatal(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
