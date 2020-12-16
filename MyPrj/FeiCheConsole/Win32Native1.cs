using System;
using System.Text;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace FeiCheConsole
{
    public abstract partial class Win32Native
    {
        private static volatile List<int> hWndCollect;


        private static bool ScanSessionWindow(int hwnd, int lParam)
        {


            //StringBuilder buf = new StringBuilder(MAXBYTE);
            //if (GetClassName(hwnd, buf, MAXBYTE) && buf.ToString() == "TXGuiFoundation")
            //    if (GetWindowTextLength(hwnd) > 0 && GetWindowText(hwnd, buf, MAXBYTE))
            //    {
            //        string str = buf.ToString();
            //        if (str != "TXMenuWindow" && str != "QQ" && str != "增加时长")
            //        {
            //            Console.WriteLine("\t" + (hWndCollect.Count + 1) + ": " + str);
            //            hWndCollect.Add(hwnd);
            //        }
            //    }
            //return true;



            //StringBuilder buf = new StringBuilder(MAXBYTE);
            //GetClassName(hwnd, buf, MAXBYTE);
            //if (GetWindowTextLength(hwnd) > 0 && GetWindowText(hwnd, buf, MAXBYTE))
            //{
            //    String str = buf.ToString();
            //    if (str != null && str.Contains("上传文件"))
            //    {
            //        hWndCollect.Add(hwnd);
            //    }
            //}
            //return true;


            StringBuilder buf = new StringBuilder(MAXBYTE);
            GetClassName(hwnd, buf, MAXBYTE);
            if (GetWindowTextLength(hwnd) > 0 && GetWindowText(hwnd, buf, MAXBYTE))
            {
                String str = buf.ToString();
                if (str != null && str.Contains("微信"))
                {
                    hWndCollect.Add(hwnd);
                }
            }
            return true;



        }

        public static int[] GetSessionWindow()
        {
            hWndCollect = new List<int>();
            EnumWindows(ScanSessionWindow, NULL);
            return hWndCollect.ToArray();
        }
    }
}
