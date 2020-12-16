using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Test1
{
    delegate void SetTextCallBack(string text);
    public partial class Form1 : Form
    {
        private static string ipadress = "127.0.0.1";
        private static byte[] result = new byte[1024];
        private static int myProt = 8899;
        static Socket serverSocket;

        /// <summary>
        /// 保存已連接客戶
        /// </summary>
        private Dictionary<String, Socket> connectedClientList = new Dictionary<string, Socket>();

        private static List<Socket> connectList = new List<Socket>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            //ServerStart();
        }

        /// <summary>
        /// 启动服务端
        /// </summary>
        private void ServerStart()
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text) && !string.IsNullOrEmpty(this.textBox2.Text))
            {
                ipadress = this.textBox1.Text.Trim();
                myProt = Convert.ToInt32(this.textBox2.Text.Trim());
            }
            IPAddress ip = IPAddress.Parse(ipadress);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口 
            //设定最多10个排队连接请求 
            serverSocket.Listen(10);
            //通过Clientsoket发送数据 
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            this.richTextBox1.Text = "socket服务端启动了~";
        }

        /// <summary>
        /// 监听客户端连接 
        /// </summary>
        private void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.UTF8.GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
                string conectedKey = clientSocket.RemoteEndPoint.ToString();
                connectedClientList.Add(conectedKey, clientSocket);
                SetText(conectedKey + "上线了~");
                String curKeys = string.Join("|", connectedClientList.Keys.ToArray());
                clientSocket.Send(Encoding.UTF8.GetBytes(curKeys));
                connectList.Add(clientSocket);
            }
        }

        /// <summary> 
        /// 接收消息 
        /// </summary> 
        /// <param name="clientSocket"></param> 
        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    //通过clientSocket接收数据 
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber > 0)
                    {
                        string serverReceive = String.Format("来自{0}的消息：{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(result, 0, receiveNumber));
                        SetText(serverReceive);
                    }
                }
                catch (Exception ex)
                {
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        public void SetText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServerStart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Socket clientConnet in connectedClientList.Values)
            {
                //clientConnet.Send(Encoding.UTF8.GetBytes(this.richTextBox1.Text));
                SendBytes(clientConnet);
            }
        }


        #region Utils



        private void SendBytes(Socket client)
        {
            //client.SendFile(@"D:\01.SocketFile\Client.rar");
        }




        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
