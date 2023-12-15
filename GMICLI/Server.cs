using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GMICLI
{
    internal class Server
    {
        internal static async Task Start()
        {
            TcpListener tcpServer = new TcpListener(IPAddress.Any, 0);
            tcpServer.Start();
            await Receive(tcpServer);

        }

        internal static async Task Receive(TcpListener tcp)
        {
            while (true)
            {
                using var tcpClient = await tcp.AcceptTcpClientAsync();
                Console.WriteLine($"Входящее подключение: {tcpClient.Client.RemoteEndPoint}");
            }
        }
    }
}
