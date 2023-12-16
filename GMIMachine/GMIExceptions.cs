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

    // Ошибка подключения к серверу
    internal class ConnectProcessException : Exception
    {
        public ConnectProcessException() : base() { }
    }

    // Указанная переменная не найдена
    internal class VariableNotFoundException : Exception
    {
        public VariableNotFoundException() : base() { }
    }

    // Указанная переменная не является числовой
    internal class VariableNotIntegerException : Exception
    {
        public VariableNotIntegerException() : base() { }
    }

    // Указанная координата не в ОДЗ
    internal class CoordNotInRangeException : Exception
    {
        public CoordNotInRangeException() : base() { }
    }
}