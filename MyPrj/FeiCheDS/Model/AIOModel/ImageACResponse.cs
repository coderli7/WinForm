using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    /// <summary>
    /// 图片上传结果返回
    /// </summary>
    public class ImageACResponse: BaseAcResponse
    {
      
        /// <summary>
        /// base64 字符串
        /// </summary>
        public string data { get; set; }
    }
}
