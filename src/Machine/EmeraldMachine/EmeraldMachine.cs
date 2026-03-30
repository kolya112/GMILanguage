using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EmeraldMachine
{
    public class EmeraldMachine
    {
        internal string _executeFilePath;

        public EmeraldMachine(string executeFilePath)
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