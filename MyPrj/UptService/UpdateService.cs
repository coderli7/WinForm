using ChinaLifeTools.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UptService
{
    public class UpdateService
    {
        private System.Timers.Timer chkServerVersionTimer = new System.Timers.Timer();

        private Socket socketSend;

        public void Start()
        {
            //启动连接socket
            ConectSocketServer();

            //定时监测，防止关闭
            chkServerVersionTimer.Interval = 60000;
            chkServerVersionTimer.Elapsed += ChkServerVersionTimer_Elapsed;
            chkServerVersionTimer.Start();
        }

        private void ChkServerVersionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock ("")
                {
                    if (socketSend == null)
                    {
                        ConectSocketServer();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 连接柱程序
        /// </summary>
        private void ConectSocketServer()
        {
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取服务端的IP
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                //获取服务端的端口号
                IPEndPoint port = new IPEndPoint(ip, Convert.ToInt32("10086"));
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(port);
                //新建线程，去接收客户端发来的信息
                Thread td = new Thread(ConectSocketServer_AcceptMgs);
                td.IsBackground = true;
                td.Start();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ConectSocketServer_AcceptMgs()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socketSend.Receive(buffer);
                    if (r == 0) { break; }
                    string receiveMsg = Encoding.UTF8.GetString(buffer, 0, r);
                    UpdateFile(receiveMsg);
                }
            }
            catch { }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        private void ConectSocketServer_SendInfo(String info)
        {
            try
            {
                if (socketSend != null && socketSend.Connected)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(info);
                    socketSend.Send(buffer);
                }
            }
            catch (Exception)
            {
            }
        }

        #region 更新程序

        /// <summary>
        /// 1.更新程序
        /// </summary>
        public void UpdateFile(String receiveMsg)
        {
            try
            {
                if (!String.IsNullOrEmpty(receiveMsg))
                {
                    //1.反序列化
                    VersionInfoResponse updateInfo = JsonConvert.DeserializeObject<VersionInfoResponse>(receiveMsg);
                    if (updateInfo != null && "1".Equals(updateInfo.updateSign) && File.Exists(updateInfo.downloadFilePath))
                    {
                        //1.关闭进程
                        UpdateFile_CloseSameProcess(updateInfo.mainProcessName);

                        //2.直接解压直新
                        //String depressPath = updateInfo.downloadFilePath.Substring(0, updateInfo.downloadFilePath.LastIndexOf('.'));
                        bool depressStatus = ZipUtils.Decompression(updateInfo.downloadFilePath, updateInfo.mainProcessPath, true);

                        if (depressStatus)
                        {
                            File.Delete(updateInfo.downloadFilePath);
                            //4.更新本地最新版本号
                            updateInfo.updateSign = "0";
                            String updateVersionFilePath = String.Format("{0}\\update\\version.txt", updateInfo.mainProcessPath);
                            File.WriteAllText(updateVersionFilePath, JsonConvert.SerializeObject(updateInfo));
                        }


                        //5.启动文件
                        String mainProcess = String.Format("{0}\\{1}", updateInfo.mainProcessPath, updateInfo.mainProcessName);
                        Process.Start(mainProcess);

                        //6.可退出更新程序,也可不退出(考虑socket连接不生效)
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        void UpdateFile_CloseSameProcess(String processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                if (processes != null)
                {
                    foreach (var item in processes)
                    {
                        if (item.Id != Process.GetCurrentProcess().Id)
                        {
                            item.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
