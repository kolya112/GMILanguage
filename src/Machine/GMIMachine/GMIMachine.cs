using System.Net.Sockets;

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
                string[] sourceLines = await File.ReadAllLinesAsync(_executeFilePath);

                await Lexer.Lexer.LexarySearch(sourceLines, _tcpServerPort, _executeFilePath);
            }
            else
                throw new ExecutableFileNotFoundException();
        }
    }
}