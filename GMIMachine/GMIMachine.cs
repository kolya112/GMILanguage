using GMIMachine.Lexer;

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
                string[] sourceLines = await File.ReadAllLinesAsync(_executeFilePath);

                await Lexer.Lexer.LexarySearch(sourceLines);
            }
            else
                throw new ExecutableFileNotFoundException();
        }
    }
}