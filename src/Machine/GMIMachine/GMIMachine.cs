using System.Net.Sockets;
using System.Text;

namespace GMIMachine
{
    public class GMIMachine
    {
        internal string _executeFilePath;
        internal int _tcpServerPort;
        internal TcpClient? _tcpClient;

        public GMIMachine(string executeFilePath, int tcpServerPort)
        {
            _executeFilePath = executeFilePath;
            _tcpServerPort = tcpServerPort;
        }

        public async Task Init()
        {
            if (File.Exists(_executeFilePath))
            {
                string[] sourceLines = await File.ReadAllLinesAsync(_executeFilePath, Encoding.UTF8);

                await Lexer.Lexer.LexarySearch(sourceLines, _tcpServerPort, _executeFilePath, firstStart: true);
            }
            else
                throw new ExecutableFileNotFoundException();
        }
    }
}