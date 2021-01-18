using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateService
{
    class Program
    {
        private static System.Timers.Timer chkServerVersionTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            chkServerVersionTimer.Interval = 100;
            chkServerVersionTimer.Elapsed += ChkServerVersionTimer_Elapsed;
            chkServerVersionTimer.Start();
            Console.Title = "appname";
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, "appname");
            ShowWindow(ParenthWnd, 0);//隐藏本dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化
            Console.ReadKey();
        }

        private static void ChkServerVersionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock ("")
            {

                if (chkServerVersionTimer.Interval == 100)
                {
                    chkServerVersionTimer.Interval = 60000;
                    //MessageBox.Show(DateTime.Now.ToString() + "第一次执行任务!!!");
                }
            }
        }

        #region 隐藏窗口

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]   //找子窗体   
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]   //用于发送信息给窗体   
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]   //
        private static extern bool ShowWindow(IntPtr hWnd, int type);
        #endregion

    }
}
