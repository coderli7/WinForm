using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowHanler
{
    /// <summary>
    /// 用来记录socketclient
    /// </summary>
    public class SocketClient
    {
        public Socket client = null;

        public string socketAdress;

        public bool isConnected = false;

        public SocketClient()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="clientVal"></param>
        /// <param name="socketAdressVal"></param>
        /// <param name="isConnectedVal"></param>
        public SocketClient(Socket clientVal, string socketAdressVal, bool isConnectedVal)
        {
            this.client = clientVal;
            this.socketAdress = socketAdressVal;
            this.isConnected = isConnectedVal;
        }
    }
}
