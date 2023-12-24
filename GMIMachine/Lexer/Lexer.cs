using System;

namespace GMIMachine.Lexer
{
    internal class Lexer
    {
        internal static async Task LexarySearch(string[] lines, int port, string executableFilePath, int startLineIndex = -1, int endLineIndex = -1, string internalMethod = "")
        {
            int lineCount = 0;
            foreach (var line in lines)
            {
                lineCount++;

                if (startLineIndex != -1 && endLineIndex != -1)
                {
                    if (lineCount < startLineIndex)
                        continue;

                    if (lineCount > endLineIndex)
                        break;
                }

                if (DataPool.linesRangeBlackList.Count > 0)
                    if (DataPool.linesRangeBlackList.Contains(lineCount))
                    {
                        DataPool.linesRangeBlackList.Remove(lineCount);
                        continue;
                    }

                if (DataPool.procedures.Count > 0)
                {
                    bool alert = false;
                    foreach (var procedure in DataPool.procedures)
                    {
                        if (lineCount < procedure.Value[1] && lineCount > procedure.Value[0])
                        {
                            if (!DataPool.procedureIsStarted)
                            {
                                alert = true;
                                break;
                            }
                            else
                            {
                                if (DataPool.startedProcedures.Contains(procedure.Key))
                                    if (DataPool.startedProcedures.IndexOf(procedure.Key) != 0)
                                    {
                                        alert = true;
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            if (DataPool.procedureIsStarted)
                                if (DataPool.startedProcedures.Contains(procedure.Key))
                                {
                                    alert = true;
                                    break;
                                }
                        }
                    }

                    if (alert)
                        continue;

                    if (DataPool.procedureIsStarted)
                        if (line == "ENDPROC")
                            break;
                }

                // Проверка на пустую строку
                if (string.IsNullOrWhiteSpace(line))
                    continue;

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

                    case string when line.Contains("IFBLOCK"):
                        string rightOfIfBlock = line.Split("IFBLOCK ")[1];
                        string leftOfIfBlock = line.Split("IFBLOCK ")[0];
                        if (GetSpaceSymbolsCount(rightOfIfBlock) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("IFBLOCK", rightOfIfBlock, port, lineCount, executableFilePath, leftOfIfBlock);

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

                    case string when line.Contains("PROCEDURE"):
                        if (DataPool.procedureIsStarted)
                            throw new ProcedureIsStartedException();

                        string rightOfExpProc = line.Split("PROCEDURE ")[1];
                        if (GetSpaceSymbolsCount(rightOfExpProc) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("PROCEDURE", rightOfExpProc, port, lineCount, executableFilePath);

                        break;

                    case string when line.Contains("CALL"):
                        string rightOfCall = line.Split("CALL ")[1];
                        if (GetSpaceSymbolsCount(rightOfCall) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("CALL", rightOfCall, port, lineCount, executableFilePath);

                        break;

                    case string when line.Contains("ENDPROC"):
                        break;

                    default:
                        throw new CodeSyntaxException();
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