using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum CityEnum
    {
        [Description("北京")]
        BeiJing = 1,

        [Description("南京")]
        NanJing = 8,

        [Description("天津")]
        TianJin = 3,

        [Description("广州")]
        GuangZhou = 14,

        [Description("成都")]
        ChengDu = 4,

        [Description("杭州")]
        HangZhou = 9,

        [Description("济南")]
        JiNan = 18,

        [Description("郑州")]
        ZhengZhou = 27,

        [Description("深圳")]
        ShenZhen = 11,

        [Description("上海")]
        ShangHai = 6,

        [Description("重庆")]
        ChongQing = 2,

        [Description("石家庄")]
        ShiJiaZhuang = 12,

        [Description("滨州")]
        BinZhou = 80,

        [Description("临沂")]
        LinYi = 55,

        [Description("烟台")]
        YanTai = 22,

        [Description("淄博")]
        ZiBo = 65,

        [Description("济宁")]
        JiNing = 83,

        [Description("武汉")]
        WuHan = 19,

        [Description("合肥")]
        HeFei = 31

    }
}
