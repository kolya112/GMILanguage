using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GMIMachine
{
    internal class ServerProvider
    {
        internal static async Task<int> SendPacket(string message, int port)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", port);

            if (!tcpClient.Connected)
                throw new ConnectProcessException();
            byte[] bufferOfMessage = Encoding.UTF8.GetBytes(message);
            int bytesSended = await tcpClient.Client.SendAsync(bufferOfMessage);
            tcpClient.Close();
            return bytesSended;
        }
    }
}
