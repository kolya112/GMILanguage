using System.Numerics;

namespace GMIMachine
{
    internal class DataPool
    {
        internal static Dictionary<string, string> variables = new Dictionary<string, string>();
        internal static Dictionary<string, List<int>> procedures = new Dictionary<string, List<int>>();
        internal static List<int> linesRangeBlackList = new List<int>(); // То, какие строки лексер будет пропускать код
        internal static List<string> startedProcedures = new List<string>();
        internal static Vector2 coords = new Vector2() { X = 0, Y = 0 } ;
        internal static int CodeLevel = 0; // Уровень вложенности конструкций
        internal static bool procedureIsStarted = false;
    }
}