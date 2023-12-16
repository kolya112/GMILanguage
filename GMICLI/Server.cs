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
        internal static void Start()
        {
            TcpListener tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 0);
            tcpServer.Start();
            Global.Buffers.tcpServerPort = ((IPEndPoint)tcpServer.LocalEndpoint).Port;
            Thread receiveThread = new Thread(() => Receive(tcpServer));
            receiveThread.Start();
        }

        internal static void Receive(TcpListener tcp)
        {
            while (true)
            {
                using var tcpClient = tcp.AcceptTcpClient();
                //Console.WriteLine($"Входящее подключение: {tcpClient.Client.RemoteEndPoint}");

                if (tcpClient.Client.ReceiveBufferSize > 0)
                {
                    byte[] testMessageBuffer = new byte[512];
                    int bytesReceivedCount = tcpClient.Client.Receive(testMessageBuffer);
                    if (bytesReceivedCount > 0)
                    {
                        //Console.WriteLine($"Получены данные от клиента: {Encoding.UTF8.GetString(testMessageBuffer)}");
                        string message = Encoding.UTF8.GetString(testMessageBuffer, 0, bytesReceivedCount);
                        Thread receivedDataHandler = new Thread(() => Interpreter.OutputHandler.ReceivedDataHandler(message));
                        receivedDataHandler.Start();
                    }
                }
            }
        }
    }
}