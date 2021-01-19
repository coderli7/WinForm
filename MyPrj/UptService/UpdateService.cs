using ChinaLifeTools.Models;
using Common.Utils;
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
            chkServerVersionTimer.Interval = 10000;
            chkServerVersionTimer.Elapsed += ChkServerVersionTimer_Elapsed;
            chkServerVersionTimer.Start();
        }

        private void ChkServerVersionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock ("")
                {
                    bool socketStatus = SocketUtils.IsSocketConnected(socketSend);
                    if (!socketStatus)
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
                    //MessageBox.Show(String.Format("更新程序收到消息：{0}", receiveMsg), "提醒");
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
                        //bool depressStatus = ZipUtils.Decompression(updateInfo.downloadFilePath, updateInfo.mainProcessPath, true);
                        bool depressStatus = UpdateFile_CopyFile(updateInfo);
                        if (depressStatus)
                        {
                            File.Delete(updateInfo.downloadFilePath);
                            //4.更新本地最新版本号
                            updateInfo.updateSign = "0";
                            String updateVersionFilePath = String.Format("{0}\\update\\version.txt", updateInfo.mainProcessPath);
                            File.WriteAllText(updateVersionFilePath, JsonConvert.SerializeObject(updateInfo));
                        }
                        else
                        {
                            MessageBox.Show("解压缩失败,请稍后重试");
                        }


                        //5.启动文件
                        String mainProcess = String.Format("{0}\\{1}.exe", updateInfo.mainProcessPath, updateInfo.mainProcessName);
                        Process.Start(mainProcess);

                        //6.可退出更新程序,也可不退出(考虑socket连接不生效)
                        System.Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("更新失败,请联系管理员或者重新下载即可!!!:{0}", ex.Message));
                System.Environment.Exit(0);
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

        /// <summary>
        /// 解压文件,并处理
        /// </summary>
        /// <returns></returns>
        bool UpdateFile_CopyFile(VersionInfoResponse updateInfo)
        {
            bool status = false;
            try
            {
                /*
                 * 1.先解压缩文件到tmp路径
                 * 2.从tmp路径下读取出非update目录下的文件，拷贝到代理工具执行目录
                 * 3.删除tmp目录，
                 */
                String unRarTmpDic = String.Format(@"{0}\update\tmp", updateInfo.mainProcessPath);
                if (!Directory.Exists(unRarTmpDic))
                { Directory.CreateDirectory(unRarTmpDic); }
                RarOrZipUtils.UnpackFileRarOrZip(updateInfo.downloadFilePath, unRarTmpDic);

                var files = Directory.GetFiles(unRarTmpDic);
                foreach (var item in files)
                {
                    FileInfo fInfo = new FileInfo(item);
                    String desFilePath = String.Format(@"{0}\{1}", updateInfo.mainProcessPath, fInfo.Name);
                    fInfo.CopyTo(desFilePath, true);
                    fInfo.Delete();
                }
                Directory.Delete(unRarTmpDic, true);
                File.Delete(updateInfo.downloadFilePath);

                status = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        #endregion
    }
}
