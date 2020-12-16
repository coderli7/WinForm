using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowHanler;

namespace JBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IntPtr tipsForm = WindowFormAPI.FindWindow(null, "提示信息");
            if (tipsForm.ToInt32() > 0)
            {
                IntPtr sureBtn = WindowFormAPI.GetDlgItem(tipsForm, 2);
                IntPtr tipInfo = WindowFormAPI.GetDlgItem(tipsForm, 65535);



                StringBuilder s = new StringBuilder(512);
                int i = WindowFormAPI.GetWindowTextW(tipInfo, s, s.Capacity);


                if (sureBtn.ToInt32() > 0)
                {
                    WindowFormAPI.ClickByIntPtr(sureBtn);
                }
            }


        }
    }
}
