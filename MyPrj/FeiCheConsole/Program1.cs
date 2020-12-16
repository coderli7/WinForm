using System;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FeiCheConsole
{
    public static partial class Program
    {

        private static void FindUserMessage(IntPtr hwnd)
        {
            if (!Win32Native.IsWindow(hwnd))
                Console.WriteLine("QQ session window has been closed.");
            else
            {
                AutomationElement element = AutomationElement.FromHandle(hwnd);
                element = element.FindFirst(TreeScope.Descendants, PropertyCondition.TrueCondition);
                if (element != null)
                {
                    // && element.Current.IsEnabled
                    ValuePattern vpTextEdit = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                    if (vpTextEdit != null)
                    {
                        string value = vpTextEdit.Current.Value;
                        Console.WriteLine(value);
                        MessageBox.Show(value);
                    }
                }
            }
        }


    }
}
