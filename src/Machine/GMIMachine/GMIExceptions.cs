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
        public ExecutableFileNotFoundException() : base("Файл исходного кода не найден") { }
    }

    // Ошибка синтаксиса кода
    internal class CodeSyntaxException : Exception
    {
        public CodeSyntaxException() : base("Ошибка синтаксиса") { }
    }

    // Ошибка подключения к серверу
    internal class ConnectProcessException : Exception
    {
        public ConnectProcessException() : base() { }
    }

    // Указанная переменная не найдена
    internal class VariableNotFoundException : Exception
    {
        public VariableNotFoundException() : base("Использумая переменная не найдена") { }
    }

    // Указанная переменная не является числовой
    internal class VariableNotIntegerException : Exception
    {
        public VariableNotIntegerException() : base("Переменной присваивается значение, отличное от числового") { }
    }

    // Указанная координата не в ОДЗ
    internal class CoordNotInRangeException : Exception
    {
        public CoordNotInRangeException() : base("Полученная координата нарушает определённую область допустимых значений") { }
    }

    // Выражение ENDPROC не найдено
    internal class EndProcNotFoundException : Exception
    {
        public EndProcNotFoundException() : base("Выражение ENDPROC, обозначающая конец определения процедуры, не найдено") { }
    }

    // Процедура не найдена
    internal class ProcedureNotFoundException : Exception
    {
        public ProcedureNotFoundException() : base("Процедура с указанным названием не найдена") { }
    }

    // Превышена максимальная вложенность конструкций
    internal class CodeLevelException : Exception
    {
        public CodeLevelException() : base("Превышена максимальная вложенность условных конструкций") { }
    }

    // Инициализация новой процедуры в блоке другой процедуры
    internal class ProcedureIsStartedException : Exception
    {
        public ProcedureIsStartedException() : base("Создание новой процедуры невозможна в блоке другой процедуры") { }
    }

    // Значение переменной является ни числовым, ни названием другой переменной
    internal class ValueNotDefinedException : Exception
    {
        public ValueNotDefinedException() : base("Указанное значение является ни числовым, ни названием другой переменной") { }
    }

    // Выражение ENDIF не найдено
    internal class EndIfNotFoundException : Exception
    {
        public EndIfNotFoundException() : base("Выражение ENDIF, обозначающее конец блока IFBLOCK, не найдено") { }
    }

    // Выражение ENDREPEAT не найдено

    internal class EndRepeatNotFoundException : Exception
    {
        public EndRepeatNotFoundException() : base("Выражение ENDREPEAT, обозначающее конец цикла REPEAT, не найдено") { }
    }

    // Если число в цикле REPEAT не входит в ОДЗ

    internal class RepeatCountNotInRangeException : Exception
    {
        public RepeatCountNotInRangeException() : base("Значение в цикле REPEAT не входит в область допустимых значений") { }
    }

    // Если в цикле REPEAT обнаружен бесконечный цикл

    internal class RepeatCountFoundEndlessCycle : Exception
    {
        public RepeatCountFoundEndlessCycle() : base("Обнаружен бесконечный цикл") { }
    }
}