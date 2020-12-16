using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// 用来存放连接服务的IP地址和端口号，对应的Socket (这个为了以后的扩展用，现在暂时没用)
        /// </summary>
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        /// <summary>
        /// 存储保存文件的路径
        /// </summary>
        string filePath = "";

        /// <summary>
        /// 负责通信的Socket
        /// </summary>
        Socket socketSend;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取服务端的IP
                IPAddress ip = IPAddress.Parse(txtServerIp.Text.Trim());
                //获取服务端的端口号
                IPEndPoint port = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(port);
                ShowMsg("连接成功");
                //新建线程，去接收客户端发来的信息
                Thread td = new Thread(AcceptMgs);
                td.IsBackground = true;
                td.Start();
            }
            catch (Exception ex)
            {
                ShowMsg("连接失败：" + ex.Message);
            }

        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void AcceptMgs()
        {
            try
            {
                /// <summary>
                /// 存储大文件的大小
                /// </summary>
                long length = 0;
                long recive = 0; //接收的大文件总的字节数
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    if (length > 0)  //判断大文件是否已经保存完
                    {
                        //保存接收的文件
                        using (FileStream fsWrite = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                        {
                            fsWrite.Write(buffer, 0, r);
                            length -= r; //减去每次保存的字节数
                            ShowMsg(string.Format("{0}: 已接收：{1}/{2}", socketSend.RemoteEndPoint, recive - length, recive));
                            if (length <= 0)
                            {
                                ShowMsg(socketSend.RemoteEndPoint + ": 接收文件成功");
                            }
                            continue;
                        }
                    }
                    if (buffer[0] == 0) //如果接收的字节数组的第一个字节是0，说明接收的字符串信息
                    {
                        string strMsg = Encoding.UTF8.GetString(buffer, 1, r - 1);
                        ShowMsg(socketSend.RemoteEndPoint.ToString() + ": " + strMsg);
                    }
                    else if (buffer[0] == 1) //如果接收的字节数组的第一个字节是1，说明接收的是文件
                    {
                        length = int.Parse(Encoding.UTF8.GetString(buffer, 1, r - 1));
                        recive = length;
                        filePath = "";
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Title = "保存文件";
                        sfd.InitialDirectory = @"C:\Users\Administrator\Desktop";
                        sfd.Filter = "所有文件|*.*|文本文件|*.txt|图片文件|*.jpg|视频文件|*.avi";
                        //如果没有选择保存文件路径就一直打开保存框
                        while (true)
                        {
                            sfd.ShowDialog(this);
                            filePath = sfd.FileName;
                            if (string.IsNullOrEmpty(filePath))
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (buffer[0] == 2) //如果接收的字节数组的第一个字节是2，说明接收的是震动
                    {
                        //ZD();
                    }
                }
            }
            catch { }

        }

        #region Utils


        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMsg(string message)
        {
            txtLog.AppendText(message + "\r\n");
        }
        #endregion

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(txtMsg.Text);
                //获得发送的信息时候，在数组前面加上一个字节 0
                List<byte> list = new List<byte>();
                list.Add(0);
                list.AddRange(buffer);
                //将泛型集合转换为数组
                byte[] newBuffer = list.ToArray();
                //将了标识字符的字节数组传递给客户端
                socketSend.Send(newBuffer);
                txtMsg.Text = "";
            }
            catch { }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //打开文件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要传的文件";
            ofd.InitialDirectory = @"C:\Users\Administrator\Desktop";
            ofd.Filter = "所有文件|*.*|文本文件|*.txt|图片文件|*.jpg|视频文件|*.avi";
            ofd.ShowDialog();
            //得到选择文件的路径
            txtPath.Text = ofd.FileName;
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = txtPath.Text;
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("请选择文件");
                    return;
                }
                //读取选择的文件
                using (FileStream fsRead = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = fsRead.Read(buffer, 0, buffer.Length);
                    //获得发送的信息时候，在数组前面加上一个字节 1
                    List<byte> list = new List<byte>();
                    list.Add(1);
                    list.AddRange(buffer);
                    byte[] newBuffer = list.ToArray();
                    //将了标识字符的字节数组传递给客户端
                    socketSend.Send(newBuffer, 0, r + 1, SocketFlags.None);
                    txtPath.Text = "";
                }
            }
            catch { }
        }
    }
}
