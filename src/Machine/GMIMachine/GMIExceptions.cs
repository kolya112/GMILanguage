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

    // Выражение ENDPROC не найдено
    internal class EndProcNotFoundException : Exception
    {
        public EndProcNotFoundException() : base() { }
    }

    // Процедура не найдена
    internal class ProcedureNotFoundException : Exception
    {
        public ProcedureNotFoundException() : base() { }
    }

    // Превышена максимальная вложенность конструкций
    internal class CodeLevelException : Exception
    {
        public CodeLevelException() : base() { }
    }

    // Инициализация новой процедуры в блоке другой процедуры
    internal class ProcedureIsStartedException : Exception
    {
        public ProcedureIsStartedException() : base() { }
    }

    // Значение переменной является ни числовым, ни названием другой переменной
    internal class ValueNotDefinedException : Exception
    {
        public ValueNotDefinedException() : base() { }
    }

    // Выражение ENDIF не найдено
    internal class EndIfNotFoundException : Exception
    {
        public EndIfNotFoundException() : base() { }
    }

    // Выражение ENDREPEAT не найдено

    internal class EndRepeatNotFoundException : Exception
    {
        public EndRepeatNotFoundException() : base() { }
    }

    // Если число в цикле REPEAT не входит в ОДЗ

    internal class RepeatCountNotInRangeException : Exception
    {
        public RepeatCountNotInRangeException() : base() { }
    }

    // Если в цикле REPEAT обнаружен бесконечный цикл

    internal class RepeatCountFoundEndlessCycle : Exception
    {
        public RepeatCountFoundEndlessCycle() : base() { }
    }
}