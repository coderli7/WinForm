using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.Models
{
    /// <summary>
    /// 服务端版本号
    /// </summary>
    public class VersionInfoResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public VersionInfoResponse_DataItem data { get; set; }

        /// <summary>
        /// 此字段用来标记从服务端下载后，处理状态
        /// 如下载完成之后，需要提醒客户更新
        /// 0 标记为当前版本下载完，且更新覆盖玩
        /// 1 标记为下载完，未更新，需更新处理
        /// </summary>
        public string updateSign { get; set; }

        /// <summary>
        /// 下载后，存储本地目录
        /// </summary>
        public string downloadFilePath { get; set; }

        /// <summary>
        /// 主程序目录
        /// </summary>
        public string mainProcessPath { get; set; }


        /// <summary>
        /// 主程序名称，用于更新程序关闭或启动
        /// 如（ChinaLifeTools）
        /// </summary>
        public string mainProcessName { get; set; }

    }

    public class VersionInfoResponse_DataItem
    {
        public string downLoadUrl { get; set; }
        public string fileName { get; set; }
        public string versionNumber { get; set; }
        public string versionType { get; set; }

    }
}
