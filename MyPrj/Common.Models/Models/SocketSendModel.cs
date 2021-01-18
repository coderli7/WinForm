using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChinaLifeTools.Models
{
    public class SocketSendModel
    {
        public SocketSendModel()
        {

        }

        public SocketSendModel(Socket _socketSend, String _strSend)
        {
            this.socketSend = _socketSend;
            this.strSend = _strSend;
        }

        public Socket socketSend { get; set; }
        public String strSend { get; set; }
    }
}
