using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;


namespace WindowHanler
{
    public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

    public delegate bool ChildCallBack(int hwnd, int lParam);

    public class WindowFormAPI
    {
        /// <summary>
        /// 为进程创造一个新的控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        /// <summary>
        /// 分离控制台和进程
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        /// <summary>
        /// 根据标题名获取窗口
        /// </summary>
        /// <param name="lpClassName">类名(可填null)</param>
        /// <param name="lpWindowName">标题(可填null)</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 获取所有桌面窗体
        /// </summary>
        /// <returns></returns>
        public static WindowInfo[] GetAllDesktopWindows()
        {
            //用来保存窗口对象 列表
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                GetWindowThreadProcessId(hWnd.ToInt32(), out wnd.ProcessId);
                //控件id
                wnd.ItemId = WindowFormAPI.GetDlgCtrlID(hWnd);
                //父句柄
                wnd.FatherhWnd = WindowFormAPI.GetParent(hWnd);
                wndList.Add(wnd);
                return true;
            }, 0);
            return wndList.ToArray();
        }

        public static List<WindowInfo> GetAllDesktopWindows(IntPtr pHWnd)
        {

            List<WindowInfo> wndParam = new List<WindowInfo>();


            List<int> child = WindowFormAPI.GetChildWindows(pHWnd.ToInt32());
            foreach (var item in child)
            {
                IntPtr hWnd = new IntPtr(item);
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                //标题
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();


                //类名
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();

                //进程ID
                GetWindowThreadProcessId(hWnd.ToInt32(), out wnd.ProcessId);
                //控件id
                wnd.ItemId = WindowFormAPI.GetDlgCtrlID(hWnd);
                //父句柄
                wnd.FatherhWnd = WindowFormAPI.GetParent(hWnd);
                wndParam.Add(wnd);
                wndParam.AddRange(GetAllDesktopWindows(hWnd));
            }

            return wndParam;
        }

        /// <summary>
        /// 回调函数(枚举当前所有窗体句柄)
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        /// <summary>
        /// 查找窗体某个ID的句柄（窗体句柄已知，ID已知）
        /// </summary>
        /// <param name="hParent">窗体句柄</param>
        /// <param name="nIDParentItem">ID</param>
        /// <returns></returns>
        [DllImport("user32.dll ", EntryPoint = "GetDlgItem")]
        public static extern IntPtr GetDlgItem(IntPtr hParent, int nIDParentItem);

        /// <summary>
        /// 已知句柄，获取对应进程Id
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(int hwnd, out int ID);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, ref int lParam);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 根据句柄关闭窗口
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public static void CloseForm(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_CLOSE, 0, 0);
        }

        /// <summary>
        /// 根据窗口标题关闭
        /// </summary>
        /// <param name="hWndTitle"></param>
        public static void CloseFormByTitle(String hWndTitle)
        {

            IntPtr hWnd = WindowHanler.WindowFormAPI.FindWindow(null, hWndTitle);
            if (hWnd.ToInt32() > 0)
            {
                SendMessage(hWnd, WM_CLOSE, 0, 0);
            }
        }

        [DllImport("user32", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("OLEACC.DLL", EntryPoint = "ObjectFromLresult")]
        public static extern int ObjectFromLresult(
            int lResult,
            ref System.Guid riid,
            int wParam,
            [MarshalAs(UnmanagedType.Interface), System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out]ref System.Object ppvObject
            //注意这个函数ObjectFromLresult的声明
        );

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("User32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        /// <summary>
        /// 鼠标移动到某个坐标位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// 获取某个矩形框
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hwnd, out  Rect lpRect);

        /// <summary>
        /// 获取标题文本
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextA(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out]StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 根据句柄，获取对应ID
        /// </summary>
        /// <param name="hWndCtl"></param>
        /// <returns></returns>
        [DllImport("User32.Dll")]
        public static extern int GetDlgCtrlID(IntPtr hWndCtl);

        [DllImport("User32.dll")]
        public static extern int GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 从父窗体中查找子窗体
        /// </summary>
        /// <param name="hWnd1">父窗体句柄值</param>
        /// <param name="hWnd2">可为空</param>
        /// <param name="lpsz1">窗体类名</param>
        /// <param name="lpsz2">窗体标题</param>
        /// <returns></returns>

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern int FindWindowEx(int hWnd1, int hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hwnd, int wMsg, string wParam, string lParam);

        /// <summary>
        /// 给控件发送消息，如给文本框赋值，点击，等
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);

        /// <summary>
        /// 修改位置、大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// 显示窗体（设置最大化，最小化）
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow">1，正常  2，最小化  3，最大化 </param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary> 
        /// 该函数设置由不同线程产生的窗口的显示状态。 (没用)
        /// </summary> 
        /// <param name="hWnd">窗口句柄</param> 
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param> 
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns> 
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary> 
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。
        /// 键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// (没用)
        /// </summary> 
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param> 
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns> 
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 窗体焦点
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        [DllImport("user32.dll ", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// 转化窗口信息为字符串
        /// </summary>
        /// <param name="allwindows"></param>
        /// <returns></returns>
        public static string convertStr(List<WindowInfo> allwindows)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in allwindows)
            {
                sb.Append(item.ProcessId + "   " + item.hWnd + "   " + item.szClassName + "   " + item.szWindowName + "\r\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 点击某个位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void ClickByLocation(int x, int y)
        {
            MouseFlag.MouseLefClickEvent(x, y, 0);

        }

        /// <summary>
        /// 通过窗体句柄发送点击消息
        /// </summary>
        /// <param name="hWnd"></param>
        public static void ClickByIntPtr(IntPtr hWnd)
        {
            WindowFormAPI.SendMessage(hWnd, BM_CLICK, IntPtr.Zero, "");
            WindowFormAPI.SendMessage(hWnd, BM_CLICK, IntPtr.Zero, "");
        }

        /// <summary>
        /// 通过窗体句柄获取标题值
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static string GetTitleValueByIntPtr(IntPtr hWnd)
        {
            string titleVal = "";
            StringBuilder titleValSb = new StringBuilder(1024);
            WindowFormAPI.GetWindowTextW(hWnd, titleValSb, titleValSb.Capacity);
            titleVal = titleValSb.ToString();
            return titleVal;
        }

        public const int WM_SETTEXT = 0x000C;

        public const int BM_CLICK = 0xF5;

        public const int WM_CLOSE = 0x10;

        #region ShowWindow 方法窗体状态的参数枚举
        /// <summary>
        /// 隐藏窗口并激活其他窗口
        /// </summary>
        public const int SW_HIDE = 0;

        /// <summary>
        /// 激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志
        /// </summary>
        public const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// 激活窗口并将其最小化
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;
        /// <summary>
        /// 激活窗口并将其最大化
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;

        /// <summary>
        /// 以窗口最近一次的大小和状态显示窗口。此值与SW_SHOWNORMAL相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

        /// <summary>
        /// 在窗口原来的位置以原来的尺寸激活和显示窗口
        /// </summary>
        public const int SW_SHOW = 5;

        /// <summary>
        /// 最小化指定的窗口并且激活在Z序中的下一个顶层窗口
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// 最小化的方式显示窗口，此值与SW_SHOWMINIMIZED相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;

        /// <summary>
        /// 以窗口原来的状态显示窗口。此值与SW_SHOW相似，只是窗口没有被激活
        /// </summary>
        public const int SW_SHOWNA = 8;

        /// <summary>
        /// 激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志
        /// </summary>
        public const int SW_RESTORE = 9;

        /// <summary>
        /// 依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的
        /// </summary>
        public const int SW_SHOWDEFAULT = 10;

        /// <summary>
        /// 最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;

        #endregion

        /// <summary>
        /// 关闭数组中的title的窗口
        /// </summary>
        /// <param name="titleArr"></param>
        public static void CloseWindowByTitle(string[] titleArr)
        {
            foreach (var item in titleArr)
            {
                IntPtr tipsForm = WindowFormAPI.FindWindow(null, item);
                if (tipsForm.ToInt32() > 0)
                {
                    IntPtr sureBtn = WindowFormAPI.GetDlgItem(tipsForm, 2);
                    if (sureBtn.ToInt32() > 0)
                    {
                        WindowFormAPI.ClickByIntPtr(sureBtn);
                    }
                }
            }
        }

        /// <summary>
        /// 最大化最小化，指定标题窗口
        /// </summary>
        /// <param name="titleArr">标题组</param>
        /// <param name="handleType">操作类型 1，正常  2，最小化  3，最大化</param>
        public static void MinOrMaxWindowFormByTitle(string[] titleArr, int handleType)
        {
            foreach (var item in titleArr)
            {
                IntPtr curForm = FindWindow(null, item);
                if (curForm != null && curForm.ToInt32() > 0)
                {
                    WindowFormAPI.ShowWindow(curForm, handleType);
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, ChildCallBack lpfn, int lParam);

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
            EnumChildWindows(parentWindow, delegate(int a, int b)
            {
                wndList.Add(a);
                sb.AppendLine(a + "===" + b);
                return true;
            }, 0);
            return wndList;
        }

        private static bool GetChildWindows(int a, int b)
        {
            return true;
        }

        /// <summary>
        /// 移动窗体的函数
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="x">窗体新位置x轴坐标</param>
        /// <param name="y">窗体新位置y轴坐标</param>
        /// <param name="nWidth">窗体新位置宽度</param>
        /// <param name="nHeight">窗体新位置高度</param>
        /// <param name="BRePaint">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        public const int KEYEVENTF_KEYUP = 2;

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void PostMessage(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    }  

    public struct WindowInfo
    {
        /// <summary>
        /// 窗体句柄
        /// </summary>
        public IntPtr hWnd;

        /// <summary>
        /// 窗体标题名
        /// </summary>
        public string szWindowName;

        /// <summary>
        /// 窗体类名
        /// </summary>
        public string szClassName;

        /// <summary>
        /// 当前进程ID
        /// </summary>
        public int ProcessId;

        /// <summary>
        /// 父句柄
        /// </summary>
        public int FatherhWnd;

        /// <summary>
        /// 控件id
        /// </summary>
        public int ItemId;
    }

    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    #region 暂无引用

    public static class Shell
    {
        /// <summary>  
        /// 往控制台输出文本信息
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLine(string output)
        {
            ConsoleColor curConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss:fff"), output);
            Console.ForegroundColor = curConsoleColor;
        }
    }

    public enum MouseEventFlags
    {
        Move = 0x0001,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        Wheel = 0x0800,
        Absolute = 0x8000
    }
  
    #endregion

}
