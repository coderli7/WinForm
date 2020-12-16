using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketProxyServer
{
    public partial class SocketProxyServer : Form
    {
        public SocketProxyServer()
        {
            InitializeComponent();
        }

        private void SocketProxyServer_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                const int port = 8899;
                //定义端口号
                TcpListener tcplistener = new TcpListener(port);
                Console.WriteLine("侦听端口号： " + port.ToString());
                tcplistener.Start();
                //侦听端口号
                while (true)
                {
                    Socket socket = tcplistener.AcceptSocket();
                    //并获取传送和接收数据的Scoket实例
                    Proxy proxy = new Proxy(socket);
                    //proxy.Run();



                    //Proxy类实例化
                    Thread thread = new Thread(new ThreadStart(proxy.Run));
                    //创建线程
                    thread.Start();
                    //启动线程
                }

                //    const int port = 45001;
                //    //定义端口号
                //    TcpListener tcplistener = new TcpListener(port);

                //    //TcpListener tcplistener = TcpListener.Create(port);
                //    tcplistener.Start();
                //    Log("启动监听成功");



                ////并获取传送和接收数据的Scoket实例
                //Socket socket = tcplistener.AcceptSocket();
                ////Proxy类实例化
                //Proxy proxy = new Proxy(socket);
                ////创建线程
                //Thread thread = new Thread(new ThreadStart(proxy.Run));
                ////启动线程
                //thread.Start();

                //Thread td = new Thread(ListenTCP);
                //td.IsBackground = true;
                //td.Start(tcplistener);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListenTCP(object obj)
        {
            try
            {
                TcpListener tcplistener = (TcpListener)obj;
                //侦听端口号
                while (true)
                {
                    //并获取传送和接收数据的Scoket实例
                    Socket socket = tcplistener.AcceptSocket();
                    //Proxy类实例化
                    Proxy proxy = new Proxy(socket);
                    //创建线程
                    Thread thread = new Thread(new ThreadStart(proxy.Run));
                    //启动线程
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }







        private void Log(string message)
        {
            richTextBox1.AppendText(message + "\r\n");
        }
    }
}
