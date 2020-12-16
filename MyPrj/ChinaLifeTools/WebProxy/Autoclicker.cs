using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BiHu.BaoXian.ClassCommon
{
    /// <summary>
    /// 自动点击Windows窗口
    /// http://blog.csdn.net/dream_dt/article/details/46356333
    /// http://www.360doc.com/content/15/0709/07/26582694_483701864.shtml
    /// </summary>
    public class Autoclicker
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        /// <summary>
        /// 发送字符串消息
        /// </summary>

        [DllImport("User32 ")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow); //0关闭窗口 1正常大小显示窗口 2最小化窗口 3最大化窗口

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        public delegate bool CallBack(IntPtr hwnd, int lParam);

        const int BM_CLICK = 0xF5; //对于各种消息的数值,得去API手册 http://www.cnblogs.com/xiaogelove/archive/2013/01/06/2847412.html

        public static bool Click(string title, string buttonName)
        {
            IntPtr hwndCalc = FindWindow(null, title);
            if (hwndCalc != IntPtr.Zero)
            {
                IntPtr hwndThree = FindWindowEx(hwndCalc, IntPtr.Zero, null, buttonName); //获取按钮的句柄
                if (hwndThree != IntPtr.Zero)
                {
                    SendMessage(hwndThree, BM_CLICK, 0, 0); //鼠标点击的消息
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public static bool Click(string title, string lpClassName, string buttonName, string lpszClass)
        {
            IntPtr hwndCalc = FindWindow(lpClassName, title);
            if (hwndCalc != IntPtr.Zero)
            {
                IntPtr hwndThree = FindWindowEx(hwndCalc, IntPtr.Zero, lpszClass, buttonName); //获取按钮的句柄
                if (hwndThree != IntPtr.Zero)
                {
                    SendMessage(hwndThree, BM_CLICK, 0, 0); //鼠标点击的消息
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        /// <summary>
        /// 由于层级太多,需要遍历子窗口
        /// </summary>
        public static bool Click2(string title, string buttonName)
        {
            IntPtr hwndCalc = FindWindow(null, title);

            if (hwndCalc != IntPtr.Zero)
            {
                IntPtr iResult = IntPtr.Zero;
                EnumChildWindows(hwndCalc, (h, l) =>
                {
                    IntPtr hwnd = FindWindowEx(h, IntPtr.Zero, null, buttonName);

                    if (hwnd != IntPtr.Zero)
                    {
                        iResult = hwnd;
                        return false;
                    }
                    return true;
                }, 0);

                if (iResult != IntPtr.Zero)
                {
                    SendMessage(iResult, 0xF5, 0, 0); //鼠标点击的消息
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 发送字符串消息
        /// </summary>
        public static bool Send(string title, string lpClassName, string buttonName, string lpszClass, string message)
        {
            IntPtr hwndCalc = FindWindow(lpClassName, title);
            if (hwndCalc != IntPtr.Zero)
            {
                IntPtr hwndThree = FindWindowEx(hwndCalc, IntPtr.Zero, lpszClass, buttonName);
                if (hwndThree != IntPtr.Zero)
                {
                    SendMessage(hwndThree, 0xC, 100, message);
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public static long closeErrTickes = 0;
        //const int BM_CLICK = 0xF5; //对于各种消息的数值,得去API手册 
        const int WM_CLOSE = 0x0010;
        //        public static void CloseErrJsAlert()
        //        {
        //#if DEBUG
        //            TimerHelp.Start((timer, param, isCancel) =>
        //            {
        //                int times = 30;
        //                while (times > 0)
        //                {

        //                    IntPtr mainHandle = FindWindow(null, "安全警报");
        //                    if (mainHandle == IntPtr.Zero)
        //                    {
        //                        mainHandle = FindWindow(null, "脚本错误");
        //                    }
        //                    if (mainHandle != IntPtr.Zero)
        //                    {
        //                        //IntPtr hwnd_button = FindWindowEx(mainHandle, new IntPtr(0), null, "Test");
        //                        IntPtr hwndThree = FindWindowEx(mainHandle, IntPtr.Zero, "Shell DocObject View", "是(&Y)"); //获取按钮的句柄
        //                        if (hwndThree == IntPtr.Zero)
        //                        {
        //                            hwndThree = FindWindowEx(mainHandle, IntPtr.Zero, null, "是(&Y)"); //获取按钮的句柄
        //                        }
        //                        if (hwndThree != IntPtr.Zero)
        //                        {
        //                            SendMessage(hwndThree, BM_CLICK, 0, 0); //鼠标点击的消息
        //                        }
        //                        else
        //                        {
        //                            try
        //                            {
        //                                List<int> li = GetChildWindows((int)mainHandle);
        //                                mshtml.IHTMLDocument2 doc = GetHtmlDocument((int)li[0]);
        //                                mshtml.IHTMLElement ele = doc.all.item("btnYes");
        //                                ele.click();
        //                            }
        //                            catch
        //                            {
        //                                //SendMessage(mainHandle, WM_CLOSE, 0, 0);
        //                            }
        //                        }
        //                        //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
        //                        //bool result = ShowWindowAsync(mainHandle, 3);
        //                    }
        //                    times--;
        //                    Thread.Sleep(1000);
        //                }
        //            }, null);
        //#else
        //#endif
        //        }

        /// <summary>
        /// 获取窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns>返回句柄</returns>
        //[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 通过句柄，窗体显示函数
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="cmdShow">显示方式</param>
        /// <returns>返工成功与否</returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindowAsync", SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);


        //[DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        //extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        //[DllImport("user32.dll", EntryPoint = "SendMessage")]
        //public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #region gwei
        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, ref int lParam);
        [DllImport("OLEACC.DLL", EntryPoint = "ObjectFromLresult")]
        public static extern int ObjectFromLresult(
            int lResult,
            ref System.Guid riid,
            int wParam,
            [MarshalAs(UnmanagedType.Interface), System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out]ref System.Object ppvObject
        //注意这个函数ObjectFromLresult的声明
        );
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, ChildCallBack lpfn, int lParam);
        public delegate bool ChildCallBack(int hwnd, int lParam);
        [DllImport("user32", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);

        /// <summary>
        /// 根据某个窗口句柄获取全部下级句柄
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <returns></returns>
        public static List<int> GetChildWindows(int parentWindow)
        {
            //用来保存窗口对象 列表
            List<int> wndList = new List<int>();
            StringBuilder sb = new StringBuilder();
            EnumChildWindows(parentWindow, delegate (int a, int b)
            {
                wndList.Add(a);
                sb.AppendLine(a + "===" + b);
                return true;
            }, 0);
            return wndList;
        }
        //public static mshtml.IHTMLDocument2 GetHtmlDocument(int hwnd)
        //{
        //    System.Object domObject = new System.Object();
        //    int tempInt = 0;
        //    System.Guid guidIEDocument2 = new Guid();
        //    int WM_Html_GETOBJECT = RegisterWindowMessage("WM_Html_GETOBJECT");//定义一个新的窗口消息
        //    int W = SendMessage(hwnd, WM_Html_GETOBJECT, 0, ref tempInt);//注:第二个参数是RegisterWindowMessage函数的返回值
        //    int lreturn = ObjectFromLresult(W, ref guidIEDocument2, 0, ref domObject);
        //    mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)domObject;

        //    return doc;
        //}
        #endregion

        /// <summary>
        /// 通过句柄设置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWindowHander_Click(object sender, EventArgs e)
        {
            // 获取查找窗体句柄(通过窗体标题名)
            IntPtr mainHandle = FindWindow(null, "演示窗体");
            if (mainHandle != IntPtr.Zero)
            {
                IntPtr hwnd_button = FindWindowEx(mainHandle, new IntPtr(0), null, "Test");

                //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
                bool result = ShowWindowAsync(mainHandle, 3);
            }
        }

    }
}