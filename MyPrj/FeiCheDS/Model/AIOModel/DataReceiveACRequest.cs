using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    /// <summary>
    /// 
    /// </summary>
    public class DataReceiveACRequest
    {
        /// <summary>
        /// 赔案 ID
        /// </summary>
        public string caseId { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        /// <summary>
        /// 结果数据
        /// </summary>
        public object resultData { get; set; }
        public long timeStamp { get; set; }
        public string signature { get; set; }
    }
}
