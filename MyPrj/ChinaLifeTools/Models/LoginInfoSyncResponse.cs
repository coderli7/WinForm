using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.models
{
    /// <summary>
    /// model主要用于，将本地cookie同步到服务器
    /// </summary>
    public class LoginInfoSyncResponse
    {
        public String code { get; set; }

        public String data { get; set; }

        public Object message { get; set; }

    }
}
