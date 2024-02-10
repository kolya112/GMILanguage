using System;

namespace GMIMachine
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Передано неверное количество аргументов");

            // Инициализация конструктора класса GMIMachine
            var machine = new GMIMachine(args[0]);
            // Запускаем машину-интерпретатор
            await machine.Init();
        }
    }
}