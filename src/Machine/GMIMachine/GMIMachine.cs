using System.Text;

namespace GMIMachine
{
    public class GMIMachine
    {
        internal string _executeFilePath;

        public GMIMachine(string executeFilePath)
        {
            _executeFilePath = executeFilePath;
        }

        public async Task Init()
        {
            if (File.Exists(_executeFilePath))
            {
                string[] sourceLines = await File.ReadAllLinesAsync(_executeFilePath, Encoding.UTF8);

                await Lexer.Lexer.LexarySearch(sourceLines, firstStart: true);
            }
            else
                throw new ExecutableFileNotFoundException();
        }
    }
}