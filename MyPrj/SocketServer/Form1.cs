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

namespace SocketServer
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// 存储连接
        /// </summary>
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取IP
                IPAddress ip = IPAddress.Parse(txtIp.Text);
                IPEndPoint port = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                socketWatch.Bind(port);
                ShowMsg("监听成功");
                socketWatch.Listen(100);
                //新建线程，去接收客户端发来的信息
                Thread td = new Thread(AcceptMgs);
                td.IsBackground = false;
                td.Start(socketWatch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 接收客户端发送的信息
        /// </summary>
        /// <param name="o"></param>
        private void AcceptMgs(object o)
        {
            try
            {
                Socket socketWatc = (Socket)o;
                while (true)
                {
                    ////负责跟客户端通信的Socket
                    Socket socketSend = socketWatc.Accept();
                    //将远程连接的客户端的IP地址和Socket存入集合中
                    dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
                    //将远程连接的客户端的IP地址和端口号存储下拉框中
                    cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ": 连接成功");
                    //新建线程循环接收客户端发来的信息
                    Thread td = new Thread(Recive);
                    td.IsBackground = true;
                    td.Start(socketSend);
                }
            }
            catch { }

        }

        /// <summary>
        /// 接收客户端发来的数据，并显示出来
        /// </summary>
        private void Recive(object o)
        {
            Socket socketSend = (Socket)o;
            try
            {
                while (true)
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息

                    if (socketSend == null)
                    {
                        MessageBox.Show("请选择要发送的客户端");
                        continue;
                    }
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接受到的有效字节数
                    int r = socketSend.Receive(buffer);
                    //如果客户端关闭，发送的数据就为空，然后就跳出循环
                    if (r == 0)
                    {
                        break;
                    }

                    if (buffer[0] == 0) //如果接收的字节数组的第一个字节是0，说明接收的字符串信息
                    {
                        string strMsg = Encoding.UTF8.GetString(buffer, 1, r - 1);
                        ShowMsg(socketSend.RemoteEndPoint.ToString() + ": " + strMsg);
                    }
                    else if (buffer[0] == 1) //如果接收的字节数组的第一个字节是1，说明接收的是文件
                    {
                        string filePath = "";
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
                        //保存接收的文件
                        using (FileStream fsWrite = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fsWrite.Write(buffer, 1, r - 1);
                        }
                        ShowMsg(socketSend.RemoteEndPoint + ": 接收文件成功");

                    }
                    else if (buffer[0] == 2) //如果接收的字节数组的第一个字节是2，说明接收的是震动
                    {
                        //ZD();
                    }
                }
            }
            catch { }
        }

        #region utils

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
            SendInfo("1");
            txtMsg.Text = "";
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
            SendInfo("2");
            txtPath.Text = "";
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="type">1文本, 2文件</param>
        public void SendInfo(String type)
        {

            //1.获取要发送的连接
            List<Socket> toSocketList = new List<Socket>();
            if (checkAllBox.Checked)
            {
                toSocketList.AddRange(dicSocket.Values);
            }
            else
            {
                //获得选中客户端ip对应的通信Socket      
                if (cboUsers.SelectedItem == null)
                {
                    MessageBox.Show("请选择要发送的客户端");
                    return;
                }
                Socket socketSend = dicSocket[cboUsers.SelectedItem.ToString()];
                if (socketSend == null)
                {
                    MessageBox.Show("请选择要发送的客户端");
                    return;
                }
                else
                {
                    toSocketList.Add(socketSend);
                }
            }

            //2.往客户端发送信息
            foreach (var item in toSocketList)
            {
                Socket socketSend = item;

                if (type == "1")
                {
                    string strSend = txtMsg.Text;
                    Task.Factory.StartNew(SendMsg, new SocketSendModel(socketSend, strSend));
                }
                else if (true)
                {
                    string strFilepathSend = txtPath.Text;
                    Task.Factory.StartNew(SendBigFile, new SocketSendModel(socketSend, strFilepathSend));
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="conection"></param>
        /// <param name="msg"></param>
        public void SendMsg(Object _socketModel)
        {
            try
            {
                SocketSendModel socketModel = (SocketSendModel)_socketModel;
                Socket socketSend = socketModel.socketSend;
                String strSend = socketModel.strSend;

                byte[] buffer = Encoding.UTF8.GetBytes(strSend);
                //获得发送的信息时候，在数组前面加上一个字节 0
                List<byte> list = new List<byte>();
                list.Add(0);
                list.AddRange(buffer);
                //将泛型集合转换为数组
                byte[] newBuffer = list.ToArray();
                //将了标识字符的字节数组传递给客户端
                socketSend.Send(newBuffer);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 大文件断点传送
        /// </summary>
        private void SendBigFile(Object _socketModel)
        {
            SocketSendModel socketModel = (SocketSendModel)_socketModel;
            Socket socketSend = socketModel.socketSend;
            String filePath = socketModel.strSend;
            try
            {
                //读取选择的文件
                using (FileStream fsRead = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    //1. 第一步：发送一个包，表示文件的长度，让客户端知道后续要接收几个包来重新组织成一个文件
                    long length = fsRead.Length;
                    byte[] byteLength = Encoding.UTF8.GetBytes(length.ToString());
                    //获得发送的信息时候，在数组前面加上一个字节 1
                    List<byte> list = new List<byte>();
                    list.Add(1);
                    list.AddRange(byteLength);
                    socketSend.Send(list.ToArray()); //
                    //2. 第二步：每次发送一个1MB的包，如果文件较大，则会拆分为多个包
                    byte[] buffer = new byte[1024 * 1024];
                    long send = 0; //发送的字节数                  
                    while (true)  //大文件断点多次传输
                    {
                        int r = fsRead.Read(buffer, 0, buffer.Length);
                        if (r == 0)
                        {
                            break;
                        }
                        socketSend.Send(buffer, 0, r, SocketFlags.None);
                        send += r;
                        ShowMsg(string.Format("{0}: 已发送：{1}/{2}", socketSend.RemoteEndPoint, send, length));
                    }
                    ShowMsg("发送完成");
                }
            }
            catch
            {
            }
        }
    }
}
