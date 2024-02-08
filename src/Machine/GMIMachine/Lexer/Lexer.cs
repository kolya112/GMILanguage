using System;

namespace GMIMachine.Lexer
{
    internal class Lexer
    {
        internal static async Task LexarySearch(string[] lines, int port, string executableFilePath, int startLineIndex = -1, int endLineIndex = -1, bool firstStart = false, string fromProcedure = "")
        {
            int lineCount = 0; // Счётчик текущей строки
            
            // Если лексер запускается впервые
            if (firstStart)
            {
                // Поиск процедур в коде
                foreach (var line in lines)
                {
                    lineCount++;

                    if (line.Contains("PROCEDURE "))
                    {
                        string procName = line.Split("PROCEDURE ")[1];

                        // Проверки на запрещённые названия процедуры
                        if (Common.Constants.Literals.Contains(procName))
                            throw new CodeSyntaxException();
                        else if (!Common.Utils.IsText(procName))
                            throw new CodeSyntaxException();
                        else if (DataPool.procedures.ContainsKey(procName))
                            throw new CodeSyntaxException();

                        int endProcLineNumber = -1; // Номер строки с ENDPROC
                        List<string> executableFileLinesInList = lines.ToList();
                        executableFileLinesInList.RemoveRange(0, lineCount); // Удаляем строки от 0 индекса до строки, на которой находимся сейчас

                        int lineCounter = 0;
                        foreach (var executableFileLine in executableFileLinesInList)
                        {
                            lineCounter++;
                            if (executableFileLine.Contains("ENDPROC"))
                            {
                                endProcLineNumber = lineCounter + lineCount;
                                break;
                            }
                        }

                        if (endProcLineNumber == -1)
                            throw new EndProcNotFoundException();

                        DataPool.procedures.Add(procName, new List<int> { lineCount, endProcLineNumber });
                    }
                }

                lineCount = 0; // Обнуляем счётчик текущей строки после завершения цикла нахождения процедур
            }

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

                // Применяется для блока IFBLOCK
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
                            if (fromProcedure != procedure.Key)
                            {
                                alert = true;
                                break;
                            }
                        }
                    }

                    if (alert)
                        continue;
                }

                // Проверка на пустую строку
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                switch (line)
                {
                    case string when line.Contains("SET "):
                        string rightOfExpSET = line.Split("SET ")[1];
                        if (GetSpaceSymbolsCount(rightOfExpSET) > 2)
                            throw new CodeSyntaxException();
                        if (rightOfExpSET.ToCharArray()[0] == ' ')
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("SET", rightOfExpSET, port);

                        break;

                    case string when line.Contains("COUT VAR >> "):
                        string rightOfCOut = line.Split("COUT VAR >> ")[1];
                        if (GetSpaceSymbolsCount(rightOfCOut) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("COUT_VAR", rightOfCOut, port);

                        break;

                    case string when line.Contains("IFBLOCK "):
                        string rightOfIfBlock = line.Split("IFBLOCK ")[1];
                        string leftOfIfBlock = line.Split("IFBLOCK ")[0];
                        if (GetSpaceSymbolsCount(rightOfIfBlock) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("IFBLOCK", rightOfIfBlock, port, lineCount, executableFilePath, leftOfIfBlock, lines);

                        break;

                    case string when line.Contains("RIGHT "):
                        string rightOfRight = line.Split("RIGHT ")[1];
                        if (GetSpaceSymbolsCount(rightOfRight) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("RIGHT", rightOfRight, port);

                        break;

                    case string when line.Contains("LEFT "):
                        string rightOfLeft = line.Split("LEFT ")[1];
                        if (GetSpaceSymbolsCount(rightOfLeft) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("LEFT", rightOfLeft, port);

                        break;

                    case string when line.Contains("UP "):
                        string rightOfUp = line.Split("UP ")[1];
                        if (GetSpaceSymbolsCount(rightOfUp) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("UP", rightOfUp, port);

                        break;

                    case string when line.Contains("DOWN "):
                        string rightOfDown = line.Split("DOWN ")[1];
                        if (GetSpaceSymbolsCount(rightOfDown) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("DOWN", rightOfDown, port);

                        break;

                    case string when line.Contains("PROCEDURE "):
                        if (DataPool.procedureIsStarted)
                            throw new ProcedureIsStartedException();

                        string rightOfExpProc = line.Split("PROCEDURE ")[1];
                        if (GetSpaceSymbolsCount(rightOfExpProc) > 0)
                            throw new CodeSyntaxException();

                        break;

                    case string when line.Contains("CALL "):
                        string rightOfCall = line.Split("CALL ")[1];
                        if (GetSpaceSymbolsCount(rightOfCall) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("CALL", rightOfCall, port, lineCount, executableFilePath, lines: lines);

                        break;

                    case string when line.Contains("ENDPROC"):
                        break;

                    case string when line.Contains("REPEAT "):
                        string rightOfRepeat = line.Split("REPEAT ")[1];
                        string leftOfRepeat = line.Split("REPEAT ")[0];
                        if (GetSpaceSymbolsCount(rightOfRepeat) > 0)
                            throw new CodeSyntaxException();

                        await Parser.Parser.Parse("REPEAT", rightOfRepeat, port, lineCount, executableFilePath, leftOfRepeat, lines);
                        break;

                    //case string when line.Contains("ENDREPEAT"):
                      //  break;

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