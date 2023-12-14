using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMIMachine
{
    // Запускаемый файл не найден
    internal class ExecutableFileNotFoundException : Exception
    {
        public ExecutableFileNotFoundException() : base() { }
    }

    // Ошибка синтаксиса кода
    internal class CodeSyntaxException : Exception
    {
        public CodeSyntaxException() : base() { }
    }
}
