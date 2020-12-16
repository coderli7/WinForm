using BiHu.BaoXian.ClassCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChinaLifeTools
{
    public class FiddlerUtils
    {
        #region 1.证书相关

        public static void SetSSLCer()
        {
            X509Certificate2 oRootCert = GetCert();

            if (oRootCert == null)
            {
                Fiddler.CertMaker.createRootCert();
                oRootCert = Fiddler.CertMaker.GetRootCertificate();//Returns the Root certificate that Fiddler uses to generate per-site certificates used for HTTPS interception. 
                System.Security.Cryptography.X509Certificates.X509Store certStore = new System.Security.Cryptography.X509Certificates.X509Store(StoreName.Root, StoreLocation.LocalMachine);
                certStore.Open(OpenFlags.ReadWrite);
                try
                {
                    certStore.Add(oRootCert);
                }
                finally
                {
                    certStore.Close();
                }
            }
            Fiddler.FiddlerApplication.oDefaultClientCertificate = oRootCert;
            Fiddler.CONFIG.IgnoreServerCertErrors = true;
        }

        static X509Certificate2 GetCert(string serch = null)
        {
            X509Certificate2 certificate2 = null;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.MaxAllowed);

            //轮询存储区中的所有证书
            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.Subject.Contains(serch ?? "DO_NOT_TRUST_FiddlerRoot"))
                {
                    certificate2 = cert;
                }
            }
            store.Close();
            return certificate2;
        }

        #endregion

        #region 2.关闭代理

        private const int INTERNET_OPTION_REFRESH = 0x000025;
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 0x000027;

        public static void CloseProxy()
        {
            try
            {

                Fiddler.FiddlerApplication.Shutdown();
                String proxyRegPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
                //打开注册表键
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(proxyRegPath, true);
                SetRegProxyClose(rk);

                //循环检测3次关闭状态是否生效如未生效，在执行3次关闭
                int researchCount = 0;
                Microsoft.Win32.RegistryKey rsrch_rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(proxyRegPath, true);
                while ("1".Equals(rsrch_rk.GetValue("ProxyEnable").ToString()) && researchCount < 3)
                {
                    SetRegProxyClose(rsrch_rk);
                    researchCount++;
                    rsrch_rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(proxyRegPath, true);
                }

                Fiddler.FiddlerApplication.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("关闭代理异常!\r\n\r\n{0}", ex.Message), "提醒");
            }
        }

        private static void SetRegProxyClose(Microsoft.Win32.RegistryKey rsrch_rk)
        {
            rsrch_rk.SetValue("ProxyEnable", 0);
            rsrch_rk.Flush();
            rsrch_rk.Close();
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            FiddlerUtils.ChangeReg();
        }
        #endregion

        #region 3.Win32

        [DllImport("wininet.dll", SetLastError = true)]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        #endregion

        #region 4.通知所有打开程序，注册表已修改

        const int WM_SETTINGCHANGE = 0x001A;
        const int HWND_BROADCAST = 0xffff;
        static IntPtr result1;
        private enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
        IntPtr windowHandle,
        uint Msg,
        IntPtr wParam,
        IntPtr lParam,
        SendMessageTimeoutFlags flags,
        uint timeout,
        out IntPtr result
        );

        private static void ChangeReg()
        {
            //通知所有打开的程序注册表已修改
            SendMessageTimeout(new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out result1);
        }

        #endregion
    }
}
