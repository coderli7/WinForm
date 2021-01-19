using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class SocketUtils
    {

        // 检查一个Socket是否可连接
        public static bool IsSocketConnected(Socket client)
        {
            if (client == null) { return false; }
            bool blockingState = client.Blocking;
            try
            {
                byte[] tmp = new byte[1];
                client.Blocking = false;
                client.Send(tmp, 0, 0);
                return true;
            }
            catch (SocketException e)
            {
                // 产生 10035 == WSAEWOULDBLOCK 错误，说明被阻止了，但是还是连接的
                if (e.NativeErrorCode.Equals(10035))
                    return false;
                else
                    return true;
            }
            finally
            {
                client.Blocking = blockingState;    // 恢复状态
            }
        }
    }
}
