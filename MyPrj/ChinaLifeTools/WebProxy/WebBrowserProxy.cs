using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace BiHu.BaoXian.ClassCommon
{
    public static class WebProxy
    {
        static bool firstStart = true;
        static bool isStart = false;
        /// <summary>
        /// 安装证书并开启代理
        /// </summary>
        public static void Start(int port)
        {
            Fiddler.CONFIG.IgnoreServerCertErrors = true;
            X509StoreHelp.SetX509Store("CN=DO_NOT_TRUST_FiddlerRoot, O=DO_NOT_TRUST_BC, OU=Created by http://www.fiddler2.com", Application.StartupPath + "\\FiddlerCore.cer", "09D73C3774F6C4389BE29DA78C9BFC974E3EAC5C");

            Fiddler.FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.cert", "MIIDozCCAougAwIBAgIQAIDMbgmDdTMrEEx+tLukQzANBgkqhkiG9w0BAQsFADBqMSswKQYDVQQLDCJDcmVhdGVkIGJ5IGh0dHA6Ly93d3cuZmlkZGxlcjIuY29tMRgwFgYDVQQKDA9ET19OT1RfVFJVU1RfQkMxITAfBgNVBAMMGERPX05PVF9UUlVTVF9GaWRkbGVyUm9vdDAeFw0xODA0MjAwMDAwMDBaFw0yODA0MjcyMDIwNDJaMGoxKzApBgNVBAsMIkNyZWF0ZWQgYnkgaHR0cDovL3d3dy5maWRkbGVyMi5jb20xGDAWBgNVBAoMD0RPX05PVF9UUlVTVF9CQzEhMB8GA1UEAwwYRE9fTk9UX1RSVVNUX0ZpZGRsZXJSb290MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt9Yk7vM29pVd+ifuSzi/KwPsooQxlZXza5GhKSlc6f89u0pe09b+OJ2ZCn17GJsZFNqzutEtFQX0cBvE7pnPBFBhoK9RPtUUBdjorHQxeRY0qGGM9DxVKGU7Do4sBkJnK8TWHlOW+8Na49dRUi1z061bnxakOng9A+PAuxDuWkZoDx2V1cpCRnfviiEuTDK+vlCH2L4mPrTsJufKS/KoSlur07ZD9Ep86LNLJnhMvrtMgrbulNCrOtx6rcbxp27r7QCUIbAoWx6cb1POxSWsp8z32eJWk2Uqh+m0m6VIR8WVbKVJYNBF3ctz1xhe3X85cFjoiTziPxBvdo74isOcJwIDAQABo0UwQzASBgNVHRMBAf8ECDAGAQH/AgEAMA4GA1UdDwEB/wQEAwICBDAdBgNVHQ4EFgQUzz8IB6KXGUlL3Zg0ceqphdkmd0MwDQYJKoZIhvcNAQELBQADggEBAF9/yw9yoqlDZRA6utpV4sC211+zom6NpvE/dWq2nLlCOsIgCDtm4mMmvQHZf4u6QkNlCVvKW9Ngs047qXF2j+PgcDV/ZWkEeIW2zcwTjh22k99jpfV4hJJW4CTmx1yj2sdsd6QlpxmJQUUB/EaS0XteHJCEYcTVSVgglrOJIr4hQ22p62N08CWmPdinFVirukzsrk0QDXii1nsDjVdFvfeUSWDs3j/LlGFKYNTfyYuKuH0JmupFi9k6V+NQr8GV0bmkLtDhgC1JzzEL2nC/eawGt2HG/yEqbAS+vcVpAFy5QLdb4FIW4N7V0fqlWB2i0iVya9TZ3t9CJ2Ma/wLOQB0=");

            Fiddler.FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.key", "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC31iTu8zb2lV36J+5LOL8rA+yihDGVlfNrkaEpKVzp/z27Sl7T1v44nZkKfXsYmxkU2rO60S0VBfRwG8Tumc8EUGGgr1E+1RQF2OisdDF5FjSoYYz0PFUoZTsOjiwGQmcrxNYeU5b7w1rj11FSLXPTrVufFqQ6eD0D48C7EO5aRmgPHZXVykJGd++KIS5MMr6+UIfYviY+tOwm58pL8qhKW6vTtkP0Snzos0smeEy+u0yCtu6U0Ks63HqtxvGnbuvtAJQhsChbHpxvU87FJaynzPfZ4laTZSqH6bSbpUhHxZVspUlg0EXdy3PXGF7dfzlwWOiJPOI/EG92jviKw5wnAgMBAAECggEAfrYHtvvFAqlpoScsxIRUn92QmWPsmWJF6fryzmBIPrFDZ3iXbARgLb3S4rwiwI8G9qzX/Qh4vmjgVvWNA7jYS+zrncm5gkl0B6O+nD9qaj+4A+dZNG7xc2pzDlFIXvgeDLkbrMOI6fd5Clo7Hx1dHKX85ObCQ9rkk23ERiwhXe7C7J1SPvNGAWl0/uyuh7ZIgW1ZdBLV2q2jjQ40HRd4zu0Cy+P8asTlvU9FxXOrWmICsaQEMwODlfAHi/sZwpCeNf3wND4BdwG8pMKZDFopHHw/MfTN5YRCUpajY/Bm795r8rGZfcKDVAmbW9z2HbEaeOp+E8vzP1vNPNy5/0ho4QKBgQD87AMAQNvhxGV3igYEa6cHzS3q0RRrGCkhmsMlttrArk57FQa/tO8rvYQpvjMBZ567j16JYWmMAM7hvNbC692GogJf+/eabP3LUK+cf1TvUtHttfDTbB/NSHxcQJ6MGoP4EamULeU/+RZ4VNleQbPFm2EvdyYKJ30vKH1sjfkrQwKBgQC6EuTpKRYnzywSK2Trg0Iyt0A/fW0mGApz0tZn5tb+LJnRjZGo7s5+srFvxQYQ7vxNYGPa+KWJlMDsOgfLLx7eMbGSAcc2JoKinJJM5Ffzin6FZsV3q3a88JRHc99jYZGGC6IehbRz6fAspYovFmSDE557K+uY5SRrZRD8j6vzTQKBgQDXryyf+q+ISEN/PWUEQAmgzYhqxwHykhgBYEkq0FScHAkxTS0ELvgHBQa/0kMM36CtsgWcgkXP7qB+QNukPiAbv+zmjakJOAj2aUhY3P1BWg9L9+v3YC1+kmH7CeAq+jGwSt+iTcFCXNicpT748m6sBWVLZQA7iJFotc6/1BK4oQKBgFJCuhpuBJJy32vk7UfFDoX3R5sJ6zAVHsHXqtviTJevxgzDRmrlsDqI6zKbarQfw0C95UdP93bcYXHNsdZcrYvTcko3KQfCfKxBBdiidlN/nbGCHrhqACNFRHhWPKLl5nzZNcHCoGVCPeCvCmkIwETGbqDLaOt+Gdy0oI7XQVBdAoGAKuLVcNTOGHdOlYmGbBy3CrCCoOyVwllGeKFl51JT66txDsSK/5nchHiqlJBVYtpr7kRawirNYwXwO6e0qauPEWezKTEgMo2YZfEdYsURrHVy6C97s3oiIN4wMRmB1sdUgH76PvK6FlpFt502LxkT3+kcql7ark6nJHbcbpeSk8Q=");

            Fiddler.FiddlerApplication.Startup(port, true, true, false);
            SetProxy(string.Format("127.0.0.1:{0}", port));
        }

        struct Struct_INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        };

        [DllImport("wininet.dll", SetLastError = true)]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        /// <summary>
        /// 设置WebBrowser使用代理（不包括自定义连接，如HttpClient）
        /// 关闭 代理SetProxy(string.Empty);
        /// </summary>
        static bool SetProxy(string strProxy)
        {
            const int INTERNET_OPTION_PROXY = 38;
            const int INTERNET_OPEN_TYPE_PROXY = 3;
            const int INTERNET_OPEN_TYPE_DIRECT = 1;

            Struct_INTERNET_PROXY_INFO struct_IPI = new Struct_INTERNET_PROXY_INFO();

            // Filling in structure 
            if (string.IsNullOrEmpty(strProxy) || strProxy.Trim().Length == 0)
            {
                strProxy = string.Empty;
                struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
                struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
            }
            else
            {
                struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
                struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
                struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");
            }
            //struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            //struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");

            // Allocating memory 
            IntPtr intptrStruct = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI));

            // Converting structure to IntPtr 
            Marshal.StructureToPtr(struct_IPI, intptrStruct, true);

            return InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI));
        }

        /// <summary>
        /// 关闭fiddler
        /// </summary>
        public static void ShutDownFiddler()
        {
            try
            {
                SetProxy(string.Empty);
                Fiddler.FiddlerApplication.Shutdown();
                System.Threading.Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }
}