using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest
{
    /// <summary>
    /// 非车流程信息
    /// </summary>
    public class CaseflowInfoModel
    {
        /// <summary>
        /// 案件-业务环节
        /// </summary>
        public string CaseStep { get; set; }

        /// <summary>
        /// 案件-业务号
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// 案件-操作人员
        /// </summary>
        public string CaseOperateName { get; set; }

        /// <summary>
        /// 案件-提交人员
        /// </summary>
        public string CaseSubmitName { get; set; }

        /// <summary>
        /// 案件-流入时间
        /// </summary>
        public string CaseInDateTime { get; set; }

        /// <summary>
        /// 案件-流出时间
        /// </summary>
        public string CaseOutDateTime { get; set; }

        /// <summary>
        /// 案件-任务状态
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 案件-注销标志
        /// </summary>
        public string CaseCancelSign { get; set; }


    }
}
