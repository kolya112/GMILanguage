using System;
using System.Threading.Tasks;

namespace EmeraldMachine
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Передано неверное количество аргументов");

            // Инициализация конструктора класса EmeraldMachine
            var machine = new EmeraldMachine(args[0]);
            // Запускаем машину-интерпретатор
            await machine.Init();
        }
    }
}