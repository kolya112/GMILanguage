using GMIMachine.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GMIMachine.Lexer
{
    internal class Lexer
    {
        internal static async Task LexarySearch(string[] lines, int port)
        {
            foreach (var line in lines)
            {
                switch (line)
                {
                    case string when line.Contains("SET"):
                        string rightOfExpSET = line.Split("SET ")[1];
                        if (GetSpaceSymbolsCount(rightOfExpSET) > 2)
                            throw new CodeSyntaxException();
                        if (rightOfExpSET.ToCharArray()[0] == ' ')
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("SET", rightOfExpSET, port);

                        break;

                    case string when line.Contains("COUT VAR >>"):
                        string rightOfCOut = line.Split("COUT VAR >> ")[1];
                        if (GetSpaceSymbolsCount(rightOfCOut) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("COUT_VAR", rightOfCOut, port);

                        break;

                    case string when line.Contains("RIGHT"):
                        string rightOfRight = line.Split("RIGHT ")[1];
                        if (GetSpaceSymbolsCount(rightOfRight) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("RIGHT", rightOfRight, port);

                        break;

                    case string when line.Contains("LEFT"):
                        string rightOfLeft = line.Split("LEFT ")[1];
                        if (GetSpaceSymbolsCount(rightOfLeft) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("LEFT", rightOfLeft, port);

                        break;

                    case string when line.Contains("UP"):
                        string rightOfUp = line.Split("UP ")[1];
                        if (GetSpaceSymbolsCount(rightOfUp) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("UP", rightOfUp, port);

                        break;

                    case string when line.Contains("DOWN"):
                        string rightOfDown = line.Split("DOWN ")[1];
                        if (GetSpaceSymbolsCount(rightOfDown) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("DOWN", rightOfDown, port);

                        break;

                    default:
                        break;
                }
            }
        }

        internal static int GetSpaceSymbolsCount(string line)
        {
            int spaceSymbolsCount = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line.ToCharArray()[i] == ' ')
                    spaceSymbolsCount++;
            }
            return spaceSymbolsCount;
        }
    }
}