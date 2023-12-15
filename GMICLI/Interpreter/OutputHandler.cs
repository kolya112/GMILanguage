using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
        }
    }
}
