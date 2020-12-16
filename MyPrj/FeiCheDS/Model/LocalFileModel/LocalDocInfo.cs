using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS.Model.LocalFileModel
{
    public class LocalDocInfo
    {
        /// <summary>
        /// 报案号
        /// </summary>
        public String DocNo { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        public List<String> ImageList { get; set; }

        /// <summary>
        /// 图片解析数据列表
        /// </summary>
        public List<String> ImageDataList { get; set; }

    }
}
