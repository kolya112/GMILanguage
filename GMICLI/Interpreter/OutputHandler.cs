using System;

namespace GMICLI.Interpreter
{
    internal class OutputHandler
    {
        internal static void ReceivedDataHandler(string data)
        {
            switch (data)
            {
                case string when data.Contains("COUT >>"):
                    string cOutContent = data.Split("COUT >> ")[1];
                    Console.WriteLine(cOutContent);
                    break;

                case string when data.Contains("X >>"):
                    string xChangeContent = data.Split("X >> ")[1];
                    Console.WriteLine(xChangeContent);
                    break;

                case string when data.Contains("Y >>"):
                    string yChangeContent = data.Split("Y >> ")[1];
                    Console.WriteLine(yChangeContent);
                    break;

                default:
                    Console.WriteLine(data);
                    break;
            }
        }
    }
}
