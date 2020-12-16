using System;
using System.Text;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace FeiCheConsole
{
    public abstract partial class Win32Native
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(LPENUMWINDOWSPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClassName(int hWnd, StringBuilder buf, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(int hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        private delegate bool LPENUMWINDOWSPROC(int hwnd, int lParam);

        public const int NULL = 0;
        private const int MAXBYTE = 255;
    }
}
