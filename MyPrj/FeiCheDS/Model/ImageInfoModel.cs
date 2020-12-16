using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    public class ImageInfoModel
    {
        public ImageInfoModel()
        {

        }
        /// <summary>
        /// 抬头信息
        /// </summary>
        public Dictionary<String, String> titleDic { get; set; }


        /// <summary>
        /// 行明细
        /// </summary>
        public List<List<String>> lineList { get; set; }
    }
}
