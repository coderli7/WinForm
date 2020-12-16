using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowHanler;

namespace OperateTools
{
    class BdTask : TaskBase
    {
        public override void excute()
        {
            try
            {
                /*
                 * 1.获取网盘窗口句柄
                 * 2.前置，并最大化
                 * 3.计算暂停位置，和开启位置
                 * 4.先点击暂停，休眠5s，后点击启动
                 * 
                 */
                IntPtr intPtr = WindowFormAPI.FindWindow(null, "PanDownload");
                WindowFormAPI.SetForegroundWindow(intPtr);
                WindowFormAPI.ShowWindow(intPtr, 1);
                Rect rect;
                WindowFormAPI.GetWindowRect(intPtr, out rect);
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                WindowFormAPI.MoveWindow(intPtr, 0, 0, width, height, false);
                int startX = 500, startY = 100;
                int stopX = 600, stopY = 100;
                WindowFormAPI.ClickByLocation(stopX, stopY);
                WindowFormAPI.ClickByLocation(stopX, stopY);
                Thread.Sleep(5000);
                WindowFormAPI.ClickByLocation(startX, startY);
                WindowFormAPI.ClickByLocation(startX, startY);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
