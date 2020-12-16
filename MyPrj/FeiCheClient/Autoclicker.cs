using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FeiCheClient
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

        /// <summary>
        /// 发送按键命令
        /// </summary>
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

        /// <summary>
        /// 返回指定窗口的标题文本（如果存在）的字符长度
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        /// <summary>
        /// 将指定窗口的标题条文本（如果存在）拷贝到一个缓存区内
        /// </summary>
        /// <param name="hWnd">带文本的窗口或控件的句柄</param>
        /// <param name="text">保存文本的对象</param>
        /// <param name="nMaxCount">指定要保存在缓冲区内的字符的最大个数</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        const int BM_CLICK = 0xF5; //对于各种消息的数值,得去API手册 http://www.cnblogs.com/xiaogelove/archive/2013/01/06/2847412.html

        public static bool Click(string title, string buttonName)
        {
            return Click(title, null, buttonName, null);
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
            return Click2(title, null, buttonName, null);
        }

        public static bool Click2(string title, string lpClassName, string buttonName, string lpszClass)
        {
            IntPtr hwndCalc = FindWindow(lpClassName, title);
            if (hwndCalc != IntPtr.Zero)
            {
                IntPtr iResult = IntPtr.Zero;
                EnumChildWindows(hwndCalc, (h, l) =>
                {
                    IntPtr hwnd = FindWindowEx(h, IntPtr.Zero, lpszClass, buttonName);
                    if (hwnd != IntPtr.Zero)
                    {
                        iResult = hwnd;
                        return false;
                    }
                    return true;
                }, 0);

                if (iResult != IntPtr.Zero)
                {
                    SendMessage(iResult, BM_CLICK, 0, 0);
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


        #region 程序产生未知异常崩溃时
        /// <summary>
        /// 获取崩溃异常信息并点击退出
        /// </summary>
        /// <param name="title"></param>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool ClickNetFramework(string title, string buttonName, ref string errorMsg)
        {
            IntPtr hwndCalc = FindWindow(null, title);//获取窗体句柄
            if (hwndCalc == IntPtr.Zero) return false;

            //1.获取按钮的句柄,点击详细信息
            // "详细信息(&D)" : "Details";
            errorMsg = string.Empty;
            IntPtr hwndThree = FindWindowEx(hwndCalc, IntPtr.Zero, null, buttonName);
            if (hwndThree != IntPtr.Zero)
            {
                SendMessage(hwndThree, BM_CLICK, 0, 0); //鼠标点击详细信息(&D)
                //2.获取详细信息               
                errorMsg = GetNetFrameworkErrorInfo(hwndCalc);
            }

            //3.点击退出(&Q)
            buttonName = buttonName == "详细信息(&D)" ? "退出(&Q)" : "&Quit";
            hwndThree = FindWindowEx(hwndCalc, IntPtr.Zero, null, buttonName);
            if (hwndThree == IntPtr.Zero) return false;
            SendMessage(hwndThree, BM_CLICK, 0, 0); //鼠标点击退出
            return true;
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="hwndParent">父窗体句柄</param>
        /// <returns>异常信息</returns>
        public static string GetNetFrameworkErrorInfo(IntPtr hwndParent)
        {
            string errorMsg = string.Empty;
            IntPtr hwndText = IntPtr.Zero;
            EnumChildWindows(hwndParent, (h, l) =>
            {
                hwndText = FindWindowEx(hwndParent, hwndText, null, null);
                if (hwndText != IntPtr.Zero)
                {
                    int length = GetWindowTextLength(hwndText);
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(hwndText, sb, sb.Capacity);
                    if (sb.ToString().Contains("异常文本"))
                    {
                        errorMsg = sb.ToString();
                        return false;
                    }
                }
                return true;
            }, 0);
            return errorMsg;
        }
        #endregion
    }
}