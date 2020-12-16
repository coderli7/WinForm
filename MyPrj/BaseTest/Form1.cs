using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowHanler;

namespace BaseTest
{

    enum ErrorEnum
    {

        ERROR_1 = -1,

    }
    public partial class Form1 : Form
    {

        #region 测试历史
        private void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
            /*
             * 1.获取VPN更新目录是否存在
             */
            //String curWrkPath = Environment.CurrentDirectory;
            //string vpnPrsName = "VPNTest";
            //string curWrkFullPath = curWrkPath + string.Format(@"\artificial\VPN\{0}.exe", vpnPrsName);
            //bool isExistFile = File.Exists(curWrkFullPath);
            //if (isExistFile)
            //{
            //    /*
            //     *2.如果VPN更新程序存在，判定是否是对应路径的启动。 
            //     *如果未启动,则启动，如果其他目录启动，如历史版本已启动
            //     *则暂时也同时启动
            //     */
            //    bool isVPNPrcStart = false;
            //    var process = Process.GetProcessesByName(vpnPrsName);
            //    foreach (Process item in process)
            //    {
            //        if (!string.IsNullOrEmpty(item.MainModule.FileName))
            //        {
            //            bool isContainVpnPath = item.MainModule.FileName.Contains(curWrkPath);
            //            if (isContainVpnPath)
            //            {
            //                isVPNPrcStart = true;
            //                break;
            //            }
            //        }
            //    }
            /*
             * 3.如判定VPN重播工具未启动，则直接启动
             */
            //        if (!isVPNPrcStart)
            //        { Process.Start(curWrkFullPath); }
            //    }
            //}
            //catch (Exception ex)
            //{ throw ex; }

            //var enum1 = (int)ErrorEnum.ERROR_1;
            //Person p1 = new Person();
            //p1.name = "tom";
            //GetMd5Key(p1);
            //string tmp1 = "";
            //List<object> list = new List<object>();
            //list.Add(new Person());
            //GetValueFromDic<Person>();
            //foreach (var item in list)
            //{
            //    Type curType = item.GetType();
            //}
            //String tmp = "dsadsa  ='';";
            //tmp = tmp.Substring(tmp.IndexOf("=") + 1).Replace("'", "").Replace(";", "").Trim();
            //string date = "2019-6-7";
            //var tmpDT = DateTime.Parse(date);

            //string str1 = "ist_lt=1557998012855; login_assistant_%24%24_picc_usercode=05229705; login_assistant_%24%24_picc_password=picc1234; s_fid=4A242C9906818148-0342CD879C9EF285; vid=87d1e76d8ffb494c2ed87e70961dd277; s_getNewRepeat=1557998055086-Repeat; s_vnum=1589347561941%26vn%3D10; s_invisit=true; BIGipServerls_web=442991882.20480.0000; s_cc=true";


            //Byte[] byte1 = System.Text.Encoding.Default.GetBytes(str1);
            //String str2 = System.Text.Encoding.UTF8.GetString(byte1);
            //string tmp1 = System.Web.HttpUtility.UrlDecode(str1, System.Text.Encoding.UTF8);
            //string year = "2018-01-01";
            //year = year.Substring(0, 4);
            //string EnrollDate = "20170105";
            //DateTime enrolldt = DateTime.ParseExact(EnrollDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

            //if (!String.IsNullOrEmpty(EnrollDate) && DateTime.TryParse(EnrollDate, out enrolldt))
            //{
            //}


            //string id = "411082199002111218";
            //string birthDayTmp = id.Substring(6, 8);
            //DateTime birthDay = DateTime.ParseExact(birthDayTmp, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            //int age = DateTime.Now.Year - birthDay.Year;
            //string genderTmp = id.Substring(16, 1);
            //int gender = Convert.ToInt32(genderTmp);
            //if (Convert.ToInt32(genderTmp) >= 2)
            //{
            //    gender = Convert.ToInt32(genderTmp) % 2;
            //}
            //string tm = "";
        }
        private static System.Timers.Timer timer1 = new System.Timers.Timer();
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 60000;
            timer1.Elapsed += timer1_Elapsed;
            timer1.Start();
        }


        private void GetMd5Key(Person p)
        {

            string jsonValP = JsonConvert.SerializeObject(p);
            Person pTmp = JsonConvert.DeserializeObject<Person>(jsonValP);
            pTmp.name = "jack";



            dynamic cloneRequest = new ExpandoObject();

            cloneRequest = p;
            cloneRequest.name = "lucy";

            String jsonVal = JsonConvert.SerializeObject(pTmp);
            string md5 = StringToMD5Hash(jsonVal);
        }


        private string StringToMD5Hash(string str)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (MD5 md5 = MD5.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    byte[] md5Bytes = md5.ComputeHash(bytes);

                    foreach (byte item in md5Bytes)
                    {
                        stringBuilder.Append(item.ToString("x2"));
                    }
                }
                return stringBuilder.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }




        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                GetScreen1();
                //GetFullScreenNew();
            }
            catch (Exception)
            {
            }
        }

        class Person
        {
            public string name { get; set; }
        }

        private void GetValueFromDic<T>()
        {
            Type t1 = typeof(T);

            var p1 = t1.GetProperties();

            Person person = (Person)System.Activator.CreateInstance(t1);
        }



        private static Image GetFullScreenNew()
        {
            //获得当前屏幕的大小 
            Rectangle rect = new Rectangle();
            rect = Screen.AllScreens[0].WorkingArea;

            //计算图片的大小，因为图片的长和宽有可能超过目前屏幕的大小 
            //创建一个以当前屏幕为模板的图象 
            Control ctl = new Control();
            Graphics g1 = ctl.CreateGraphics();
            //创建以屏幕大小为标准的位图 
            Image MyImage = new Bitmap(rect.Width, rect.Height, g1);

            Graphics g2 = Graphics.FromImage(MyImage);
            //得到屏幕的DC 
            IntPtr dc1 = g1.GetHdc();
            //得到Bitmap的DC 
            IntPtr dc2 = g2.GetHdc();
            //调用此API函数，实现屏幕捕获 

            BitBlt(dc2, 0, 0, rect.Width, rect.Height, dc1, 0, 0, 13369376);
            //释放掉屏幕的DC 
            g1.ReleaseHdc(dc1);
            //释放掉Bitmap的DC 
            g2.ReleaseHdc(dc2);
            //以JPG文件格式来保存 
            //Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            //return MyImage.GetThumbnailImage(control.Width, control.Height, myCallback, IntPtr.Zero);
            MyImage.Save(string.Format("C:\\aa-{0}.jpg", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
            return MyImage;
        }


        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
         IntPtr hdcDest,  //目标设备的句柄
         int nXDest,    //目标对象的左上角的X坐标
         int nYDest,    //目标对象的左上角的Y坐标
         int nWidth,    //目标对象的矩形的宽度
         int nHeight,    //目标对象的矩形的长度
         IntPtr hdcSrc,   //源设备的句柄
         int nXSrc,     //源对象的左上角的X坐标
         int nYSrc,     //源对象的左上角的X坐标
         System.Int32 dwRop //光栅的操作值
        );

        private void button1_Click(object sender, EventArgs e)
        {
            string firPid = "0000001";
            String step3UrlPostData = "{" + String.Format("'collectionNumber':['{0}']", firPid) + "}";

            IPAddress ip = IPAddress.Any;
            //bool isOk;

            //String value = "411082199002111218";
            //string birth = value.Substring(6, 8);
            //string gender = value.Substring(16, 1);
            //String val = "商业险保费计算出错：您录入的行业车型编码 BYQKSUUB0016与平台返回的行业车型编码不匹配,平台返回的行业车型编码有： BYQKSUUD0024";

            //string patern2 = @"平台返回的行业车型编码有：[\s]*[0-9|a-z|A-Z]*";
            //String val1 = Regex.Match(val, patern2).Value;


            //string imgStr = "data:image/png;base64,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";

        }

        private static void GetScreen1()
        {
            Rectangle tScreenRect = new Rectangle(0, 0, Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
            g.DrawImage(img, 0, 0, tScreenRect, GraphicsUnit.Pixel);

            img.Save(string.Format("C:\\aa-{0}.jpg", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //WindowFormAPI.CloseFormByTitle("发起会话");

            //WindowFormAPI.MinOrMaxWindowFormByTitle(new string[] { "发起会话" }, 2);

            WindowFormAPI.MinOrMaxWindowFormByTitle(new string[] { "TeamViewer Panel", "发起会话" }, 2);


        }
        #endregion

        #region MyRegion

        private void btn1_Click(object sender, EventArgs e)
        {
            CloseVPN("VPNTest");
            StartVPN("VPNTest");
        }


        /// <summary>
        /// 根据进程名，
        /// 0.获取对应目录的配置文件后，必须非当前目录，
        /// 1.获取对应配置文件详细信息
        /// 2.先备份对应目录
        /// 3.删除对应目录文件（防止自动重启）
        /// 4.将配置文件，覆盖到本机配置文件，
        /// 5.启动当前目录下的VPN重播工具
        /// 6.
        /// </summary>
        /// <param name="prcName"></param>
        /// <returns></returns>
        private void StartVPN(string prcName)
        {

            if (DateTime.Now.Hour < 7 || DateTime.Now.Hour >= 22) { return; };
            try
            {
                //1.获取配置信息
                String oldPrcConfigStr = "";
                String curPath = Environment.CurrentDirectory;
                String curVPNPath = Environment.CurrentDirectory + @"\artificial\VPN重播";
                Process[] processes = Process.GetProcessesByName(prcName);
                foreach (Process prc in processes)
                {
                    if (prc != null)
                    {
                        String oldPrcPath = prc.MainModule.FileName;
                        string oldPrcConfigPath = oldPrcPath + ".config";
                        if (!oldPrcPath.Contains(curVPNPath) && File.Exists(oldPrcConfigPath))
                        {

                            prc.Kill();
                            string fromPath = oldPrcPath;
                            string oldPrcPathTmp = oldPrcPath.Substring(0, oldPrcPath.LastIndexOf('\\'));
                            string backUpPath = oldPrcPathTmp + @"\backup";
                            if (!Directory.Exists(backUpPath))
                            {
                                Directory.CreateDirectory(backUpPath);
                            }
                            string toPath = String.Format(@"{0}\{1}", backUpPath, prcName + ".exe");
                            File.Move(fromPath, toPath);
                            oldPrcConfigStr = File.ReadAllText(oldPrcConfigPath);
                            break;
                        }
                    }
                }

                //2.更新现有配置信息
                if (!String.IsNullOrEmpty(oldPrcConfigStr))
                {
                    String curVPNConfigPath = string.Format(@"{0}\{1}", curVPNPath, prcName + ".exe.config");
                    if (File.Exists(curVPNConfigPath))
                    {
                        File.WriteAllText(curVPNConfigPath, oldPrcConfigStr, Encoding.UTF8);

                    }
                }

                //3.无论如何都要保证VPN是打开的状态
                List<Process> curPathVpnList = Process.GetProcessesByName(prcName).Where(c => c.MainModule.FileName.Contains(curVPNPath)).ToList();
                if (curPathVpnList != null && curPathVpnList.Count <= 0)
                {
                    String curVPNExePath = String.Format(@"{0}\{1}", curVPNPath, prcName + ".exe");
                    Process.Start(curVPNExePath);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CloseVPN(String prcName)
        {
            try
            {
                /*
                 * 1.关闭对应报价服务下的VPN工具进程
                 */
                string qpVpnPath = "";
                List<Process> prcList = Process.GetProcessesByName(prcName).ToList();
                foreach (Process curPrc in prcList)
                {
                    if (!string.IsNullOrEmpty(curPrc.MainModule.FileName) && curPrc.MainModule.FileName.Contains(qpVpnPath))
                    {
                        curPrc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {

            /*
             * 1.先判断是否安装了，EasyConnect这个软件，此条件，可以拦截一大部分重复开启的问题
             * 
             */
            if (Directory.Exists(@"C:\Program Files (x86)\Sangfor\SSL\SangforCSClient") || Directory.Exists(@"C:\Program Files\Sangfor\SSL\SangforCSClient"))
            {
                String curPath = Environment.CurrentDirectory;
                string easyVPNPath = curPath + @"\VPN-EasyConnect.rar";
                string easyVPNWinRarPath = curPath + @"\WinRAR.exe";
                string easyVPNUnZipPath = curPath + @"\Unzip.bat";
                String easyVPNexePath = curPath + @"\VPN-EasyConnect\VPNRedial.bat";

                //2.只有更新成功，存在对应压缩包，才去解压缩
                if (File.Exists(easyVPNPath))
                {
                    //2.1如果存在，需要先关闭，包含当前目录下的对应VPN工具，
                    Process[] VpnRedialPrcs = Process.GetProcessesByName("VPNRedial");
                    foreach (var item in VpnRedialPrcs)
                    {
                        if (!String.IsNullOrEmpty(item.MainModule.FileName) && item.MainModule.FileName.Contains(curPath))
                        {
                            item.Kill();
                        }
                    }
                    //2.3关闭后，在解压缩当前的这个目录，注意，解压前，也要判断，rar包是否存在
                    if (File.Exists(easyVPNWinRarPath) && File.Exists(easyVPNUnZipPath))
                    {
                        Process.Start(easyVPNUnZipPath);
                    }
                }

                //3.开启对应目录下文件即可
                if (File.Exists(easyVPNexePath))
                {
                    Process.Start(easyVPNexePath);
                }

            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(StartVPN);
        }

        private void StartVPN()
        {
            try
            {

                /*
                 * 1.判断是否需要更新旧版本的VPN重播工具配置，如果存在，则读取到对应配置文件。
                 *     1.1遍历过程中，依据是否包含当前VPN文件目录判断。如果存在，则读取。
                 *     1.2对于不存在情况，也需要关闭重播工具，方便下一步解压缩。
                 * 2.先判断对应目录，解压缩更新VPN重播工具，并删除压缩包。
                 * 3.如果读取到旧版VPN配置文件，同步到新的工具。
                 * 4.启动VPN重播工具
                 */


                if (DateTime.Now.Hour < 7 || DateTime.Now.Hour >= 22) { return; };
                string basePath = Environment.CurrentDirectory;
                string baseVpnPath = string.Format(@"{0}\{1}", basePath, @"lib\artificial\VPN");
                string oldPrcConfigStr = "";
                string prcName = "VPNTest";
                //1.获取配置信息
                Process[] processes = Process.GetProcessesByName(prcName);
                foreach (Process prc in processes)
                {
                    if (prc != null)
                    {
                        String oldPrcPath = prc.MainModule.FileName;
                        string oldPrcConfigPath = oldPrcPath + ".config";
                        if (!oldPrcPath.Contains(baseVpnPath) && File.Exists(oldPrcConfigPath))
                        {
                            prc.Kill();
                            string fromPath = oldPrcPath;
                            string oldPrcPathTmp = oldPrcPath.Substring(0, oldPrcPath.LastIndexOf('\\'));
                            string backUpPath = oldPrcPathTmp + @"\backup";
                            if (!Directory.Exists(backUpPath))
                            {
                                Directory.CreateDirectory(backUpPath);
                            }
                            string toPath = String.Format(@"{0}\{1}", backUpPath, prcName + ".exe");
                            File.Move(fromPath, toPath);
                            oldPrcConfigStr = File.ReadAllText(oldPrcConfigPath);
                        }
                    }
                }


                //2.解压缩压缩包
                string unZipFilePath = string.Format(@"{0}\{1}", baseVpnPath, "Unzip.bat");
                string vpnRarFilePath = string.Format(@"{0}\{1}", baseVpnPath, "VPN重播.rar");
                if (File.Exists(unZipFilePath) && File.Exists(vpnRarFilePath))
                {
                    Process.Start(unZipFilePath);
                    Thread.Sleep(2000);
                    if (File.Exists(vpnRarFilePath))
                    { File.Delete(vpnRarFilePath); }
                }

                //3.更新配置文件
                string unZipVpnExePath = string.Format(@"{0}\{1}", baseVpnPath, "VPN重播");
                if (!String.IsNullOrEmpty(oldPrcConfigStr))
                {
                    String curVPNConfigPath = string.Format(@"{0}\{1}", unZipVpnExePath, prcName + ".exe.config");
                    if (File.Exists(curVPNConfigPath))
                    {
                        File.WriteAllText(curVPNConfigPath, oldPrcConfigStr, Encoding.UTF8);
                    }
                }



                //4.无论如何都要保证VPN是打开的状态
                List<Process> curPathVpnList = (new List<Process>(Process.GetProcessesByName(prcName))).Where(c => c.MainModule.FileName.Contains(baseVpnPath)).ToList();
                if (curPathVpnList != null && curPathVpnList.Count <= 0)
                {
                    String curVPNExePath = String.Format(@"{0}\{1}", unZipVpnExePath, prcName + ".exe");
                    if (File.Exists(curVPNExePath))
                    {
                        Process.Start(curVPNExePath);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Process[] prc = Process.GetProcessesByName("VPNTest");

            foreach (Process curPrc in prc)
            {

                try
                {
                    String prcName = curPrc.MainModule.FileName;
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            StringBuilder sb1 = new StringBuilder();
            Process[] prc = Process.GetProcessesByName("VPNTest");
            foreach (Process curPrc in prc)
            {
                try
                {
                    String prcName = curPrc.MainModule.FileName;
                    sb1.AppendLine(prcName);
                }
                catch (Exception ex)
                {
                    sb1.AppendLine(ex.Message + ex.StackTrace);
                }
            }
            this.richTextBox1.Text = sb1.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {


            string str = "[{fName:'潮州市潮州医院法医临床司法鉴定所',name:'潮州市潮州医院法医临床司法鉴定所',disabilityIdentifyOrgId:'CE0B1770DC20416FB88C53D7A6A85EAD',remarkInfo:'联系方式:0768-2262612   许可证号:445101-001    业务范围:法医临床鉴定  地址:广东省潮州市城新路8号潮州医院内',organizationCode:'34440000MD8019379K'},{fName:'广东潮汕司法鉴定所',name:'广东潮汕司法鉴定所',disabilityIdentifyOrgId:'4028d00168163c1c01682794a54a002a',remarkInfo:'联系方式:0768-2973013   许可证号:445102-001    业务范围:法医临床司法鉴定  地址:湘桥区新洋路11-12号商铺',organizationCode:'34440000MD80192135'}]";

            //var obj1 = JsonConvert.DeserializeObject(str);
            //List<Root> list = (List<Root>)obj1;

            List<Root> root = JsonConvert.DeserializeObject<List<Root>>(str);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            List<String> list = new List<string>();
            list.AddRange(ReadTxt("C:\\1.txt"));
            list.AddRange(ReadTxt("C:\\2.txt"));
            list.AddRange(ReadTxt("C:\\3.txt"));
            list.AddRange(ReadTxt("C:\\4.txt"));
            Dictionary<String, DateTime> dic = new Dictionary<string, DateTime>();
            foreach (String item in list)
            {
                String[] curArr = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (curArr.Length == 2)
                {
                    String curDate = curArr[0].Trim();
                    String key = curArr[1].Replace("\"", "").Trim();
                    DateTime dt;
                    if (DateTime.TryParse(curDate, out dt))
                    {
                        if (!dic.Keys.Contains(key))
                        {
                            dic.Add(key, dt);
                        }
                        else
                        {

                        }
                    }
                }
            }

            StringBuilder sb1 = new StringBuilder();


            var dicSort = from objDic in dic orderby objDic.Value ascending select objDic;

            foreach (var item in dicSort)
            {
                string curInfo = item.Value.ToString("yyyy-MM-dd HH:mm") + "        " + item.Key;
                sb1.AppendLine(curInfo);
            }
            File.WriteAllText("C:\\tmp2.txt", sb1.ToString(), Encoding.Default);






            //String str = File.ReadAllText("C:\\1.txt");
            //var arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //List<String> list = new List<string>();
            //foreach (var item in arr)
            //{

            //    String tmp = item.Replace("RegistNo", "").Replace(":", "").Replace("\"", "").Trim();
            //    if (!list.Contains(tmp))
            //    {
            //        list.Add(tmp);
            //    }
            //}

            //StringBuilder sb = new StringBuilder();

            //foreach (var item in list)
            //{
            //    sb.AppendLine(item);
            //}
            //File.WriteAllLines("C:\\tmp.txt", list.ToArray(), Encoding.Default);
        }

        static List<String> ReadTxt(string path)
        {
            String str = File.ReadAllText(path);
            var arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<String> list = new List<string>();
            foreach (var item in arr)
            {
                //String tmp = item.Replace("RegistNo", "").Replace(":", "").Replace("\"", "").Trim();
                String tmp = item.Trim();
                if (!list.Contains(tmp))
                {
                    list.Add(tmp);
                }
            }
            return list;
        }

        static List<String> ReadTxt1(string path)
        {
            String str = File.ReadAllText(path);

            var arr = File.ReadAllLines(path);

            //var arr = str.Split(new String[] { @"\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //var arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<String> list = new List<string>();
            foreach (var item in arr)
            {
                //String tmp = item.Replace("RegistNo", "").Replace(":", "").Replace("\"", "").Trim();
                String tmp = item.Trim();
                if (!list.Contains(tmp))
                {
                    list.Add(tmp);
                }
            }
            return list;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();

            list1.AddRange(ReadTxt1("D:\\tmp\\1.txt"));
            list2.AddRange(ReadTxt1("D:\\tmp\\2.txt"));

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            foreach (var item in list1)
            {
                if (!list2.Contains(item))
                {
                    sb1.AppendLine(item);
                }
            }


            foreach (var item in list2)
            {
                if (!list1.Contains(item))
                {
                    sb2.AppendLine(item);
                }
            }

            File.WriteAllText("D:\\tmp\\sb1.txt", sb1.ToString(), Encoding.Default);
            File.WriteAllText("D:\\tmp\\sb2.txt", sb2.ToString(), Encoding.Default);



        }

        #endregion

        /// <summary>
        /// 测试解析非车信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {

            string html1 = File.ReadAllText(@"D:\tmp\1.html", Encoding.Default);
            string html2 = File.ReadAllText(@"D:\tmp\2.html", Encoding.Default);
            string html3 = File.ReadAllText(@"D:\tmp\3.html", Encoding.Default);
            CaseInfoModel case1 = GetCaseInfo(html1);
            CaseInfoModel case2 = GetCaseInfo(html2);
            CaseInfoModel case3 = GetCaseInfo(html3);


        }

        private CaseInfoModel GetCaseInfo(String caseInfohtml)
        {
            CaseInfoModel caseInfoModel = new CaseInfoModel();
            try
            {
                HtmlAgilityPack.HtmlDocument curHtmlDoc = new HtmlAgilityPack.HtmlDocument();
                curHtmlDoc.LoadHtml(caseInfohtml);

                #region 1.获取头信息
                String regexNo = @"报案号：[\s|\S]*?[&|<]";
                if (Regex.IsMatch(caseInfohtml, regexNo))
                {
                    caseInfoModel.CaseNo = Regex.Match(caseInfohtml, regexNo).Value.Replace("报案号", "").Replace("：", "").Replace("<", "").Replace("&", "").Trim();
                }
                #endregion

                #region 2.获取流程信息
                HtmlAgilityPack.HtmlNodeCollection allTr = curHtmlDoc.DocumentNode.SelectNodes("//*[@id='tableSrc']/tr");
                caseInfoModel.FlowList = new List<CaseflowInfoModel>();
                foreach (HtmlAgilityPack.HtmlNode item in allTr)
                {
                    if (!String.IsNullOrEmpty(item.InnerText) && !item.InnerText.Contains("业务环节"))
                    {
                        List<HtmlAgilityPack.HtmlNode> curTd = item.SelectNodes("td").ToList();
                        if (curTd.Count == 8)
                        {
                            CaseflowInfoModel curInfo = new CaseflowInfoModel();
                            curInfo.CaseStep = curTd[0].InnerText;
                            curInfo.CaseNo = curTd[1].InnerText;
                            curInfo.CaseOperateName = curTd[2].InnerText;
                            curInfo.CaseSubmitName = curTd[3].InnerText;
                            curInfo.CaseInDateTime = curTd[4].InnerText;
                            curInfo.CaseOutDateTime = curTd[5].InnerText;
                            curInfo.CaseStatus = curTd[6].InnerText;
                            curInfo.CaseCancelSign = curTd[7].InnerText;
                            caseInfoModel.FlowList.Add(curInfo);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return caseInfoModel;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            Person1 p = new Person1();
            p.Name = "小明";
            p.Age = "18";

            String str = EnumHelper.getProperties<Person1>(p);
            var rmp = ((DescriptionAttribute)Attribute.GetCustomAttribute(p.GetType(), typeof(DescriptionAttribute))).Description;

        }

        private String imgBytes = "";


        private void button8_Click(object sender, EventArgs e)
        {
            Byte[] bytes = File.ReadAllBytes(@"D:\B1.JPG");
            imgBytes = Convert.ToBase64String(bytes);
            File.WriteAllText("C:\\1.txt", imgBytes);

        }

        private static void NewMethod(String imgBytes)
        {
            Byte[] imgByte = Convert.FromBase64String(imgBytes);
            MemoryStream memStory = new MemoryStream(imgByte);
            Image img = Image.FromStream(memStory);
            img.Save("C:\\1.jpg");
            MessageBox.Show("保存成功！");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NewMethod(imgBytes);
        }
    }

    public class EnumHelper
    {
        public static string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name; //名称
                object value = item.GetValue(t, null);  //值
                string des = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute))).Description;// 属性值
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1}:{2},", name, value, des);
                }
                else
                {
                    getProperties(value);
                }
            }
            return tStr;
        }
    }

    [Description("人")]
    public class Person1
    {

        [Description("名称")]
        public String Name { get; set; }

        [Description("年龄")]
        public String Age { get; set; }


    }
}
