using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowHanler;

namespace WeChatSendMessage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region 1.获取到微信窗口，最大化，前置
            IntPtr weChatForm = WindowFormAPI.FindWindow("WeChatMainWndForPC", null);
            //注意，最大化，要在设置前置之前
            WindowFormAPI.ShowWindow(weChatForm, 3);
            WindowFormAPI.SetForegroundWindow(weChatForm);
            #endregion

            #region 2.点击搜索栏，粘贴搜索内容，点击最新的一条结果
            MouseFlag.MouseLefClickEvent(150, 35, 0);
            SetClipBordText(this.richTextBox1.Text);
            ControlKeyBoardClick(Keys.V);
            MouseFlag.MouseLefClickEvent(150, 100, 0);
            #endregion

            #region 3.点击内容框，ctrl+v,回车发送
            WindowFormAPI.ClickByLocation(700, 800);
            SetClipBordText("您的报价是，总计1000元！");
            ControlKeyBoardClick(Keys.V);
            WindowFormAPI.PostMessage(Keys.Enter, 0, 0, 0);
            WindowFormAPI.PostMessage(Keys.Enter, 0, WindowFormAPI.KEYEVENTF_KEYUP, 0);
            #endregion
        }

        /// <summary>
        /// 触发Ctrl键盘+某个键盘字符，（如ctrl+c 或者 ctrl+v）
        /// </summary>
        /// <param name="key"></param>
        private void ControlKeyBoardClick(Keys key)
        {
            WindowFormAPI.PostMessage(Keys.ControlKey, 0, 0, 0);
            WindowFormAPI.PostMessage(key, 0, 0, 0);
            WindowFormAPI.PostMessage(Keys.ControlKey, 0, WindowFormAPI.KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 设置剪切板内容
        /// </summary>
        /// <param name="val"></param>
        public void SetClipBordText(String val)
        {
            Clipboard.SetDataObject(val, true);
        }





    }
}
