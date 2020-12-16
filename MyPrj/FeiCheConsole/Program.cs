using System;
using System.Text;
using System.Threading;
using System.Windows.Automation;

namespace FeiCheConsole
{
    public static partial class Program
    {
        public static StringBuilder allSb = new StringBuilder();


        [STAThread]
        private static void Main(string[] args)
        {
            IntPtr hwnd = new IntPtr(1248584);
            AutomationElement element = AutomationElement.FromHandle(hwnd);
            AutomationElement deskTop = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "回收站"));
            AutomationElement deskTop1 = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "计算机"));
            AutomationElement deskTop2 = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "Administrator"));
            AutomationElement deskTop3 = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "aliPdf"));

            AutomationElement deskTop4 = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "Dlls"));

            //AutomationElement deskTop4 = element.FindFirst(TreeScope.Ancestors, new PropertyCondition(AutomationElement.NameProperty, "全选"));
            //AutomationElement deskTop5 = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "上传"));
            StringBuilder suportSb = new StringBuilder();
            AutomationPattern[] allPaternTmp = deskTop3.GetSupportedPatterns();
            foreach (AutomationPattern item in allPaternTmp)
            {
                var clickPattern = deskTop.GetCurrentPattern(item);
                String curPatern = clickPattern.ToString();
                suportSb.AppendLine(curPatern);
            }
            ExpandCollapsePattern tmpPatrn1 = (ExpandCollapsePattern)deskTop3.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            SelectionItemPattern tmpPatrn2 = (SelectionItemPattern)deskTop3.GetCurrentPattern(SelectionItemPattern.Pattern);
            ScrollItemPattern tmpPatrn3 = (ScrollItemPattern)deskTop3.GetCurrentPattern(ScrollItemPattern.Pattern);
            String tmp = suportSb.ToString();

            SelectionItemPattern tmpPatrn42 = (SelectionItemPattern)deskTop4.GetCurrentPattern(SelectionItemPattern.Pattern);

            Thread.Sleep(1000);

            tmpPatrn2.Select();

            Thread.Sleep(1000);

            tmpPatrn42.Select();


            Thread.Sleep(1000);

            tmpPatrn2.Select();

            Thread.Sleep(1000);

            tmpPatrn42.Select();



            //StringBuilder suportSb4 = new StringBuilder();
            //AutomationPattern[] allPaternTmp4 = deskTop4.GetSupportedPatterns();
            //foreach (AutomationPattern item in allPaternTmp4)
            //{
            //    var clickPattern = deskTop4.GetCurrentPattern(item);
            //    String curPatern = clickPattern.ToString();
            //    suportSb.AppendLine(curPatern);
            //}
            ////ExpandCollapsePattern tmpPatrn14 = (ExpandCollapsePattern)deskTop3.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            ////SelectionItemPattern tmpPatrn24 = (SelectionItemPattern)deskTop3.GetCurrentPattern(SelectionItemPattern.Pattern);
            ////ScrollItemPattern tmpPatrn34 = (ScrollItemPattern)deskTop3.GetCurrentPattern(ScrollItemPattern.Pattern);
            //String tmp4 = suportSb.ToString();




            //AutomationPattern[] allPatern = deskTop.GetSupportedPatterns();

            //foreach (AutomationPattern item in allPatern)
            //{
            //    var clickPattern = deskTop.GetCurrentPattern(item);
            //    String curPatern = clickPattern.ToString();
            //}

            //ExpandCollapsePattern tmpPatrn = (ExpandCollapsePattern)deskTop.GetCurrentPattern(ExpandCollapsePattern.Pattern);

            //Thread.Sleep(1000);
            //tmpPatrn.Expand();
            //Thread.Sleep(1000);
            //tmpPatrn.Collapse();


            //Thread.Sleep(1000);
            //tmpPatrn.Expand();
            //Thread.Sleep(1000);
            //tmpPatrn.Collapse();



            //Thread.Sleep(1000);
            //tmpPatrn.Expand();
            //Thread.Sleep(1000);
            //tmpPatrn.Collapse
            //clickPattern.Invoke();
            //System.Windows.Point point = deskTop.GetClickablePoint();

            //var obj = deskTop.GetClickablePoint();
            //WindowFormAPI.SendMessage(pc, WindowFormAPI.BM_CLICK, IntPtr.Zero, "");
            //int[] ids1 = deskTop1.GetRuntimeId();
            //element = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "桌面"));
            //if (element != null && element.Current.IsEnabled)
            //{
            //    ValuePattern vpTextEdit = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            //    if (vpTextEdit != null)
            //    {
            //        string value = vpTextEdit.Current.Value;
            //        Console.WriteLine(value);
            //        MessageBox.Show(value);
            //    }
            //}

            //Console.Title = string.Empty;
            //Console.WriteLine("scan the QQ user's session window.");
            //int[] hWndOfSession = Win32Native.GetSessionWindow();
            //if (hWndOfSession.Length <= 0)
            //    Console.WriteLine("No scan to any available QQ message window.");
            //else
            //{
            //    Console.WriteLine("Look for the session record interface.");
            //    foreach (int hWnd in hWndOfSession)
            //        FindUserMessage((IntPtr)hWnd);
            //}
            //Console.WriteLine("Thank you for using, QQ conversations have been scanned.");
            //Console.WriteLine("OK!");
            //Console.ReadKey(false);
        }

    }
}
