using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowHanler
{
    public class WinFormModel
    {
        /// <summary>
        /// 窗口ID
        /// </summary>
        public IntPtr Id { get; set; }

        /// <summary>
        /// 窗口句柄key(实例唯一标识)
        /// </summary>
        public IntPtr Key { get; set; }

        /// <summary>
        /// 窗口父句柄
        /// </summary>
        public IntPtr FatherKey { get; set; }

        /// <summary>
        /// 窗口标题
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// 窗口类名
        /// </summary>
        public String ClassName { get; set; }

        /// <summary>
        /// 进程ID
        /// </summary>
        public int ProcessId { get; set; }

    }
}
