using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SocketProxyServer
{
    public class Proxy
    {
        #region 0.构造函数

        Socket clientSocket;
        Byte[] read = new byte[4096 * 2];
        //定义一个空间，存储来自客户端请求数据包
        Byte[] Buffer = null;
        Encoding ASCII = Encoding.ASCII;
        //设定编码
        Byte[] RecvBytes = new Byte[4096 * 2];
        //定义一个空间，存储Web服务器返回的数据

        public Proxy(Socket socket)
        {
            this.clientSocket = socket;
        }

        #endregion


        public void Run1()
        {
            string clientmessage = " ";
            //存放来自客户端的HTTP请求字符串
            string URL = " ";
            //存放解析出地址请求信息
            int bytes = ReadMessage(read, ref clientSocket, ref clientmessage);
            if (bytes == 0)
            {
                return;
            }

            int index1 = clientmessage.IndexOf(' ');
            int index2 = clientmessage.IndexOf(' ', index1 + 1);
            if ((index1 == -1) || (index2 == -1))
            {
                throw new IOException();
            }
            string part1 = clientmessage.Substring(index1 + 1, index2 - index1);
            int index3 = part1.IndexOf('/', index1 + 8);
            int index4 = part1.IndexOf(' ', index1 + 8);
            int index5 = index4 - index3;
            URL = part1.Substring(index1 + 4, (part1.Length - index5) - 8);

            try
            {
                IPHostEntry IPHost = Dns.Resolve(URL);
                Console.WriteLine("远程主机名： " + IPHost.HostName);
                string[] aliases = IPHost.Aliases;
                IPAddress[] address = IPHost.AddressList;
                Console.WriteLine("Web服务器IP地址：" + address[0]);
                //解析出要访问的服务器地址
                IPEndPoint ipEndpoint = new IPEndPoint(address[0], 80);
                Socket IPsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建连接Web服务器端的Socket对象
                IPsocket.Connect(ipEndpoint);
                //Socket连Web接服务器
                if (IPsocket.Connected)
                    Console.WriteLine("Socket 正确连接！");
                string GET = clientmessage;
                Byte[] ByteGet = ASCII.GetBytes(GET);
                IPsocket.Send(ByteGet, ByteGet.Length, 0);
                //代理访问软件对服务器端传送HTTP请求命令
                Int32 rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);
                //代理访问软件接收来自Web服务器端的反馈信息
                Console.WriteLine("接收字节数：" + rBytes.ToString());
                String strRetPage = null;
                strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, rBytes);
                while (rBytes > 0)
                {
                    rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);
                    strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, rBytes);
                }
                IPsocket.Shutdown(SocketShutdown.Both);
                IPsocket.Close();
                SendMessage(clientSocket, strRetPage);
                //代理服务软件往客户端传送接收到的信息
            }
            catch (Exception exc2)
            {
                Console.WriteLine(exc2.ToString());
            }
        }

        public void Run()
        {
            Socket IPsocket = null;
            try
            {
                string clientmessage = " ";
                //存放来自客户端的HTTP请求字符串
                string URL = " ";
                //存放解析出地址请求信息
                int bytes = ReadMessage(read, ref clientSocket, ref clientmessage);
                if (bytes == 0) { return; }

                if (clientmessage.ToLower().StartsWith("connect"))
                {
                    SendMessage(clientSocket, "HTTP/1.0 200 Connection established");//\r\n\r\n
                    return;
                }


                int index1 = clientmessage.IndexOf(' ');
                int index2 = clientmessage.IndexOf(' ', index1 + 1);
                if ((index1 == -1) || (index2 == -1))
                {
                    throw new IOException();
                }
                string part1 = clientmessage.Substring(index1 + 1, index2 - index1);
                int index3 = part1.IndexOf('/', index1 + 8);
                int index4 = part1.IndexOf(' ', index1 + 8);
                int index5 = index4 - index3;
                //URL = part1.Substring(index1 + 4, (part1.Length - index5) - 8);

                String regx = @"//[\s|\S]*?/";
                if (Regex.IsMatch(part1, regx))
                {
                    URL = Regex.Match(part1, regx).Value.Replace("/", "").Trim();
                }
                else
                {
                    URL = part1.Trim();
                }

                if (String.IsNullOrEmpty(URL.Trim()))
                {
                    return;
                }


                //ins.chinalife - p.com.cn:443

                int port = 80;
                String[] arr = URL.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr != null && arr.Length == 2)
                {
                    int urlPort;
                    if (int.TryParse(arr[1], out urlPort) && urlPort > 0)
                    {
                        URL = arr[0];
                        port = urlPort;
                    }
                }

                try
                {
                    IPHostEntry IPHost = Dns.Resolve(URL);
                    IPAddress[] address = IPHost.AddressList;

                    //解析出要访问的服务器地址
                    IPEndPoint ipEndpoint = new IPEndPoint(address[0], port);

                    IPsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //创建连接Web服务器端的Socket对象
                    IPsocket.Connect(ipEndpoint);



                    //Socket连Web接服务器
                    if (IPsocket.Connected)
                        Console.WriteLine("Socket 正确连接！");

                    Encoding.UTF8.GetBytes(clientmessage);


                    Byte[] ByteGet = Encoding.UTF8.GetBytes(clientmessage);

                    IPsocket.Send(ByteGet, ByteGet.Length, 0);


                    //代理访问软件对服务器端传送HTTP请求命令
                    Int32 rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);

                    //代理访问软件接收来自Web服务器端的反馈信息
                    String strRetPage = null;
                    strRetPage = strRetPage + Encoding.UTF8.GetString(RecvBytes, 0, rBytes);
                    while (rBytes > 0)
                    {
                        rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);
                        strRetPage = strRetPage + Encoding.UTF8.GetString(RecvBytes, 0, rBytes);
                    }

                    //IPsocket.Shutdown(SocketShutdown.Both);
                    //IPsocket.Close();
                    SendMessage(clientSocket, strRetPage);
                    //代理服务软件往客户端传送接收到的信息

                }
                catch (Exception exc2)
                {
                    //if (IPsocket != null)
                    //{
                    //    IPsocket.Shutdown(SocketShutdown.Both);
                    //    IPsocket.Close();
                    //}
                    SendMessage(clientSocket, exc2.ToString());
                    //Console.WriteLine(exc2.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //接收客户端的HTTP请求数据
        private int ReadMessage(byte[] ByteArray, ref Socket s, ref String clientmessage)
        {
            try
            {
                int bytes = s.Receive(ByteArray, 1024, 0);
                string messagefromclient = Encoding.UTF8.GetString(ByteArray);
                clientmessage = (String)messagefromclient;
                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //传送从Web服务器反馈的数据到客户端
        private void SendMessage(Socket s, string message)
        {
            try
            {




                byte[] buffer = Encoding.UTF8.GetBytes(message);
                //获得发送的信息时候，在数组前面加上一个字节 0
                List<byte> list = new List<byte>();
                //list.Add(0);
                list.AddRange(buffer);
                //将泛型集合转换为数组
                byte[] newBuffer = list.ToArray();
                //将了标识字符的字节数组传递给客户端
                s.Send(newBuffer);




                //var remotAddress = s.RemoteEndPoint.ToString();
                //Buffer = new Byte[message.Length + 1];
                //int length = Encoding.UTF8.GetBytes(message, 0, message.Length, Buffer, 0);
                ////int length = ASCII.GetBytes(message, 0, message.Length, Buffer, 0);
                ////Console.WriteLine("传送字节数：" + length.ToString());
                //s.Send(Buffer, length, 0);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
