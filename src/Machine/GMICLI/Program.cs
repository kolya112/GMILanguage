using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GMICLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /*Thread testConnectThread = new Thread(TestConnect);
            testConnectThread.Start();*/
            Server.Start();
            Console.WriteLine("Сервер запущен");
            GMIMachine.GMIMachine gmi = new GMIMachine.GMIMachine("C:\\Users\\Gomosapiens\\source\\repos\\GMILanguage\\GMICLI\\bin\\Debug\\net7.0\\test.gmi", Global.Buffers.tcpServerPort);
            await gmi.Init();
        }
    }
}