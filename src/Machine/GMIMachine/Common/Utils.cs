using System;

namespace GMIMachine.Common
{
    internal class Utils
    {
        /// <summary>
        /// Проверяется, содержится ли в указанном тексте цифры, либо какие-либо другие символы, отличные от обычных букв
        /// </summary>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        internal static bool IsText(string text)
        {
            foreach (char c in text)
                if (!Char.IsLetter(c))
                    return false;
            return true;
        }
    }
}