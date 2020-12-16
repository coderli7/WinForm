using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest
{
    /// <summary>
    /// 案件信息
    /// </summary>
    public class CaseInfoModel
    {
        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// 流程信息
        /// </summary>
        public List<CaseflowInfoModel> FlowList { get; set; }
    }
}
