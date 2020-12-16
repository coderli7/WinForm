using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FeiCheClient
{
    static class Program
    {


        /// <summary>
        /// 端口号
        /// </summary>
        public static int Port { get; set; }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetSSLCer();
            Fiddler.FiddlerApplication.BeforeRequest += ProxyManage.FiddlerApplication_BeforeRequest;
            Fiddler.FiddlerApplication.BeforeResponse += ProxyManage.FiddlerApplication_BeforeResponse;
            WebBrowserProxy.Start(Program.Port + 1);
            Application.Run(new Form1());

            //Application.Run(new TestForm());

        }







        private static void SetSSLCer()
        {
            Fiddler.CertMaker.createRootCert();
            X509Certificate2 oRootCert = Fiddler.CertMaker.GetRootCertificate();//Returns the Root certificate that Fiddler uses to generate per-site certificates used for HTTPS interception. 
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
            Fiddler.FiddlerApplication.oDefaultClientCertificate = oRootCert;
            Fiddler.CONFIG.IgnoreServerCertErrors = true;


        }
    }
}
