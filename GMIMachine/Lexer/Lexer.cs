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
                        string rightOfExp = line.Split("SET")[1];
                        int spaceSymbolCount = 0;
                        for (int i = 0; i < rightOfExp.Length; i++)
                        {
                            if (rightOfExp.ToCharArray()[i] == ' ')
                                spaceSymbolCount++;
                        }
                        if (spaceSymbolCount > 3)
                            throw new CodeSyntaxException();
                        if (rightOfExp.ToCharArray()[0] != ' ')
                            throw new CodeSyntaxException();
                        rightOfExp = rightOfExp.Remove(0, 1);
                        if (rightOfExp.ToCharArray()[0] == ' ')
                            throw new CodeSyntaxException();

                        string[] spaceInExp = rightOfExp.Split(' ');
                        string variableName = spaceInExp[0];
                        if (spaceInExp[1] != "=")
                            throw new CodeSyntaxException();
                        string variableValue = spaceInExp[2];

                        DataPool.variables.Add(variableName, variableValue);
                        break;

                    case string when line.Contains("COUT VAR >>"):
                        string rightOfCOut = line.Split("COUT VAR >>")[1];
                        int spaceSymbolCountCOut = 0;
                        for (int i = 0; i < rightOfCOut.Length; i++)
                        {
                            if (rightOfCOut.ToCharArray()[i] == ' ')
                                spaceSymbolCountCOut++;
                        }
                        if (spaceSymbolCountCOut > 1)
                            throw new CodeSyntaxException();
                        if (rightOfCOut.ToCharArray()[0] != ' ')
                            throw new CodeSyntaxException();
                        rightOfCOut = rightOfCOut.Remove(0, 1);
                        if (rightOfCOut.ToCharArray()[0] == ' ')
                            throw new CodeSyntaxException();
                        string variableNameCOut = rightOfCOut;
                        await ServerProvider.SendPacket($"COUT >> {variableNameCOut}:{DataPool.variables[variableNameCOut]}", port);
                        break;
                }
            }
        }
    }
}
