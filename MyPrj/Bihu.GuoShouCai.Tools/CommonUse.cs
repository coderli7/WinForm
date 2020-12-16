using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bihu.GuoShouCai.Tools
{
    class CommonUse
    {
        /// <summary>
        /// 设置IE代理
        /// </summary>
        /// <param name="strProxyUrl"></param>
        public static void SetIEProxy(string strProxyUrl)
        {
            //打开注册表键 
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            //设置代理可用 
            rk.SetValue("ProxyEnable", 1);
            //设置代理IP和端口 
            rk.SetValue("ProxyServer", strProxyUrl);
            rk.Close();
        }

        /// <summary>
        /// 取消IE代理
        /// </summary>
        public static void CancelIEProxy()
        {
            //打开注册表键 
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            //设置代理不可用 
            rk.SetValue("ProxyEnable", 0);
            rk.Close();
        }

        /// <summary>
        /// 关闭IE浏览器
        /// </summary>
        public static void CloseIexplorer()
        {

            Process[] processes = Process.GetProcessesByName("iexplore");
            foreach (Process item in processes)
            {
                item.Kill();
            }
        }
    }
}
