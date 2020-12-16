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
    public class LoginInfoSyncRequest
    {

        public String channelKey { get; set; }

        public String cookie { get; set; }

        public Object data { get; set; }

    }
}
