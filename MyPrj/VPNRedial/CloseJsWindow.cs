﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VPNRedial
{
    public class CloseJsWindow
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        private static extern void SetForegroundWindow(IntPtr hwnd);
        private static string windowTitle = "安全警报";
        private static string windowTitle2 = "来自网页的消息";
        private static string windowTitle3 = "脚本错误";
        private const uint BM_CLICK = 0xF5; 
        //鼠标点击的消息，对于各种消息的数值，大家还是得去API手册
        private const uint MS_CLOSE = 0x0010;//
        private const uint WM_QUIT = 0x0012;
        private const uint WM_DESTROY = 0x0002;
        static System.Threading.Timer findTimer;
        static AutoResetEvent autoEvent;

        /// <summary>
        /// 开始监视js弹出的窗口
        /// </summary>
        public static void StartCloseWindow()
        {
            autoEvent = new AutoResetEvent(false); //初始状态设置为非终止
            TimerCallback timerDelegate = new TimerCallback(CloseWindow);
            findTimer = new System.Threading.Timer(timerDelegate, autoEvent, 1000, 800); //每0.8秒钟查找一次
        }
        /// <summary>
        /// 停止监视js弹出的窗口
        /// </summary>
        public static void StopCloseWindow()
        {
            autoEvent.WaitOne(10, false);  //设置10毫秒，是让程序有一个等待，如果设为0，会一直弹窗口
            findTimer.Dispose();
        }
        public static void CloseVPNClient(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_DESTROY, (IntPtr)0, 0);
            SendMessage(hwnd, WM_QUIT, (IntPtr)0, 0);
        }
        private static void CloseWindow(object state)
        {
            IntPtr hwnd = FindWindow(null, windowTitle); //查找窗口的句柄
            if (hwnd != IntPtr.Zero)
            {
                IntPtr hwndSure = FindWindowEx(hwnd, 0, "Button", "是(&Y)"); //获取确定按钮的句柄
                SendMessage(hwndSure, BM_CLICK, (IntPtr)0, 0); //发送单击消息
            }
            IntPtr hwnd2 = FindWindow(null, windowTitle2);
            if (hwnd2 != IntPtr.Zero)
            {
                IntPtr hwndSure = FindWindowEx(hwnd2, 0, null, "确定"); //获取确定按钮的句柄
                SendMessage(hwndSure, BM_CLICK, (IntPtr)0, 0); //发送单击消息
            }

            IntPtr hwnd3 = FindWindow(null, windowTitle2);
            if (hwnd3 != IntPtr.Zero)
            {
                IntPtr hwndSure = FindWindowEx(hwnd3, 0, "Button", "是(&Y)"); //获取确定按钮的句柄
                SendMessage(hwndSure, BM_CLICK, (IntPtr)0, 0); //发送单击消息
            }

            IntPtr hwndWin = FindWindow(null, "");
            if (hwnd != IntPtr.Zero)
            {
                SendMessage(hwnd, MS_CLOSE, (IntPtr)0, 0);
            }
        }
    }

}
