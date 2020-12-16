using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiHu.BaoXian.ClassCommon
{
    /// <summary>
    /// 设置安全证书
    /// http://www.cnblogs.com/xiaofoyuan/archive/2012/12/10/2811270.html
    /// </summary>
    public class X509StoreHelp
    {
        /// <summary>
        /// 设置安全证书
        /// 默认到期前一个月重置
        /// </summary>
        /// <param name="subjectName">证书名称</param>
        /// <param name="x509StorePath">证书路径</param>
        /// <param name="thumbprint">证书指纹</param>
        public static void SetX509Store(string subjectName, string X509StorePath, string thumbprint = "")
        {

            //是否需要重置证书,包括新建
            var isResetCert = true;
            {

                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.MaxAllowed);
                //轮询存储区中的所有证书
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.Subject == subjectName && (thumbprint == "" || cert.Thumbprint == thumbprint))
                    {
                        if (DateTime.Now > cert.NotAfter) //到期前1个月
                        {
                            Task.Run(() =>
                            {
                                long starttime = DateTime.Now.Ticks;
                                while (true)
                                {
                                    Autoclicker.Click("根证书存储", "是(&Y)");
                                    Thread.Sleep(1000);

                                    //if (CommonCla.IsTimeout(starttime, 10))
                                    //    break;
                                }
                            });
                            store.Remove(cert);
                        }
                        else
                            isResetCert = false;

                        break;
                    }
                }
                store.Close();
            }

            if (isResetCert)
            {
                Task.Run(() =>
                {
                    long starttime = DateTime.Now.Ticks;
                    while (true)
                    {
                        Autoclicker.Click("安全性警告", "是(&Y)");
                        Autoclicker.Click("安全警告", "是(&Y)");
                        Thread.Sleep(1000);

                        //if (CommonCla.IsTimeout(starttime, 10))
                        //    break;
                    }
                });

                X509Certificate2 cert = new X509Certificate2(X509StorePath);
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.MaxAllowed);
                store.Add(cert);
                store.Close();
            }
        }
    }
}