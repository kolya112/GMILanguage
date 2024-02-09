using System.Text;

namespace GMIMachine.Parser
{
    internal class Parser
    {
        internal static async Task Parse(string method, string line, int port, int lineCount = -1, string executeFilePath = "", string leftLineOfExp = "", string[]? lines = null)
        {
            switch (method)
            {
                case "SET":
                    string[] spaceInExpSET = line.Split(' ');
                    string variableNameSET = spaceInExpSET[0];
                    if (spaceInExpSET[1] != "=")
                        throw new CodeSyntaxException();

                    if (Common.Constants.Literals.Contains(variableNameSET))
                        throw new CodeSyntaxException();
                    else if (!Common.Utils.IsText(variableNameSET))
                        throw new CodeSyntaxException();

                    string variableValueSET = spaceInExpSET[2];

                    if (!int.TryParse(variableValueSET, out int variableValueInDigitSET))
                        // Если в виде значения обнаружена строка, проверяем, есть ли переменная с таким названием, если есть - приравниваем новую переменную к значению существующей
                        if (DataPool.variables.ContainsKey(variableValueSET))
                            variableValueSET = DataPool.variables[variableValueSET];
                        else
                            throw new ValueNotDefinedException();

                    // Если переменная с таким названием уже существует - заменяем её значение на новое
                    if (DataPool.variables.ContainsKey(variableNameSET))
                        DataPool.variables[variableNameSET] = variableValueSET;
                    else
                        DataPool.variables.Add(variableNameSET, variableValueSET);

                    break;

                case "COUT_VAR":
                    string variableNameCOut = line;
                    if (DataPool.variables.ContainsKey(variableNameCOut))
                        Console.WriteLine($"COUT >> {variableNameCOut}:{DataPool.variables[variableNameCOut]}", port);
                    else
                        throw new VariableNotFoundException();
                    break;

                case "RIGHT":
                    int coordRight;
                    if (int.TryParse(line, out int valueRight)) // Распознано числовое значение
                        coordRight = valueRight;
                    else 
                    // Пробуем отыскать переменную
                    {
                        if (DataPool.variables.ContainsKey(line))
                        {
                            if (int.TryParse(DataPool.variables[line], out int varValueRight))
                                coordRight = varValueRight;
                            else
                                throw new VariableNotIntegerException();
                        }
                        else
                            throw new VariableNotFoundException();
                    }

                    // Проверка ОДЗ
                    if (coordRight >= 1 && coordRight <= 20)
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if ((DataPool.coords.X + coordRight) <= 20)
                        {
                            Console.WriteLine($"X >> {DataPool.coords.X + coordRight}", port);
                            DataPool.coords.X += coordRight;
                        }
                        else
                            throw new CoordNotInRangeException();
                    }
                    else
                        throw new CoordNotInRangeException();

                    break;

                case "LEFT":
                    int coordLeft;
                    if (int.TryParse(line, out int valueLeft)) // Распознано числовое значение
                        coordLeft = valueLeft;
                    else
                    // Пробуем отыскать переменную
                    {
                        if (DataPool.variables.ContainsKey(line))
                        {
                            if (int.TryParse(DataPool.variables[line], out int varValueLeft))
                                coordLeft = varValueLeft;
                            else
                                throw new VariableNotIntegerException();
                        }
                        else
                            throw new VariableNotFoundException();
                    }

                    // Проверка ОДЗ
                    if (coordLeft >= 1 && coordLeft <= 20)
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if ((DataPool.coords.X - coordLeft) >= 0)
                        {
                            Console.WriteLine($"X >> {DataPool.coords.X - coordLeft}", port);
                            DataPool.coords.X -= coordLeft;
                        }
                        else
                            throw new CoordNotInRangeException();
                    }
                    else
                        throw new CoordNotInRangeException();

                    break;

                case "UP":
                    int coordUp;
                    if (int.TryParse(line, out int valueUp)) // Распознано числовое значение
                        coordUp = valueUp;
                    else 
                    // Пробуем отыскать переменную
                    {
                        if (DataPool.variables.ContainsKey(line))
                        {
                            if (int.TryParse(DataPool.variables[line], out int varValueUp))
                                coordUp = varValueUp;
                            else
                                throw new VariableNotIntegerException();
                        }
                        else
                            throw new VariableNotFoundException();
                    }

                    // Проверка ОДЗ
                    if (coordUp >= 1 && coordUp <= 20)
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if ((DataPool.coords.Y + coordUp) <= 20)
                        {
                            Console.WriteLine($"Y >> {DataPool.coords.Y + coordUp}", port);
                            DataPool.coords.Y += coordUp;
                        }
                        else
                            throw new CoordNotInRangeException();
                    }
                    else
                        throw new CoordNotInRangeException();

                    break;

                case "DOWN":
                    int coordDown;
                    if (int.TryParse(line, out int valueDown)) // Распознано числовое значение
                        coordDown = valueDown;
                    else
                    // Пробуем отыскать переменную
                    {
                        if (DataPool.variables.ContainsKey(line))
                        {
                            if (int.TryParse(DataPool.variables[line], out int varValueDown))
                                coordDown = varValueDown;
                            else
                                throw new VariableNotIntegerException();
                        }
                        else
                            throw new VariableNotFoundException();
                    }

                    // Проверка ОДЗ
                    if (coordDown >= 1 && coordDown <= 20)
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if ((DataPool.coords.Y - coordDown) >= 0)
                        {
                            Console.WriteLine($"Y >> {DataPool.coords.Y - coordDown}", port);
                            DataPool.coords.Y -= coordDown;
                        }
                        else
                            throw new CoordNotInRangeException();
                    }
                    else
                        throw new CoordNotInRangeException();

                    break;

                case "PROCEDURE":
                    string procName = line;

                    if (Common.Constants.Literals.Contains(procName))
                        throw new CodeSyntaxException();
                    else if (!Common.Utils.IsText(procName))
                        throw new CodeSyntaxException();
                    else if (DataPool.procedures.ContainsKey(procName))
                        throw new CodeSyntaxException();

                    int endProcLineNumber = -1;
                    string[] executableFileLines = await File.ReadAllLinesAsync(executeFilePath, Encoding.UTF8);
                    List<string> executableFileLinesInList = executableFileLines.ToList();
                    executableFileLinesInList.RemoveRange(0, lineCount);

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

                    break;

                case "CALL":
                    string calledProcName = line;

                    if (DataPool.procedures.ContainsKey(calledProcName))
                    {
                        DataPool.startedProcedures.Add(calledProcName);
                        DataPool.procedureIsStarted = true;
                        DataPool.CodeLevel += 1;
                        if (DataPool.CodeLevel > 3)
                            throw new CodeLevelException();
                        await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, DataPool.procedures[calledProcName][0] + 1, DataPool.procedures[calledProcName][1] - 1, false, calledProcName);
                        DataPool.startedProcedures.Remove(calledProcName);
                        DataPool.CodeLevel -= 1;

                        if (DataPool.startedProcedures.Count == 0)
                            DataPool.procedureIsStarted = false;
                    }
                    else
                        throw new ProcedureNotFoundException();

                    break;

                case "IFBLOCK":
                    if (line != Common.Constants.Literals[1] &&
                        line != Common.Constants.Literals[2] &&
                        line != Common.Constants.Literals[3] &&
                        line != Common.Constants.Literals[4])
                            throw new CodeSyntaxException();

                    int spaceSymbolsInExp = leftLineOfExp.Length; // Получаем количество пустых символов

                    // Ищем выражение ENDIF
                    int endIfBlockLineNumber = -1;
                    List<string> executableFileLinesInListForIfBlock = lines.ToList();
                    executableFileLinesInListForIfBlock.RemoveRange(0, lineCount);

                    int lineCounterIfBlock = 0;

                    foreach (var executableFileLine in executableFileLinesInListForIfBlock)
                    {
                        lineCounterIfBlock++;
                        if (executableFileLine.Contains("ENDIF"))
                        {
                            int spaceSymbolsInCurrentLine = executableFileLine.Split("ENDIF")[0].Length;

                            if (spaceSymbolsInExp == spaceSymbolsInCurrentLine)
                                endIfBlockLineNumber = lineCounterIfBlock + lineCount;
                            else
                                continue;

                            break;
                        }
                    }

                    if (endIfBlockLineNumber == -1)
                        throw new EndIfNotFoundException();

                    if (line == Common.Constants.Literals[1])
                    {
                        // Если ОДЗ нарушено: клетка занята
                        if (DataPool.coords.X + 1 > 20)
                        {
                            DataPool.CodeLevel += 1;
                            if (DataPool.CodeLevel > 3)
                                throw new CodeLevelException();
                            await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1);
                            DataPool.CodeLevel -= 1;
                        }
                    }
                    else if (line == Common.Constants.Literals[2])
                    {
                        // Если ОДЗ нарушено: клетка занята
                        if (DataPool.coords.X - 1 < 0)
                        {
                            DataPool.CodeLevel += 1;
                            if (DataPool.CodeLevel > 3)
                                throw new CodeLevelException();
                            await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1);
                            DataPool.CodeLevel -= 1;
                        }
                    }
                    else if (line == Common.Constants.Literals[3])
                    {
                        // Если ОДЗ нарушено: клетка занята
                        if (DataPool.coords.Y + 1 > 20)
                        {
                            DataPool.CodeLevel += 1;
                            if (DataPool.CodeLevel > 3)
                                throw new CodeLevelException();
                            await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1);
                            DataPool.CodeLevel -= 1;
                        }
                    }
                    else if (line == Common.Constants.Literals[4])
                    {
                        // Если ОДЗ нарушено: клетка занята
                        if (DataPool.coords.Y - 1 < 0)
                        {
                            DataPool.CodeLevel += 1;
                            if (DataPool.CodeLevel > 3)
                                throw new CodeLevelException();
                            await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1);
                            DataPool.CodeLevel -= 1;
                        }
                    }

                    for (int i = lineCount + 1; i <= endIfBlockLineNumber; i++)
                        DataPool.linesRangeBlackList.Add(i);

                    break;

                case "REPEAT":
                    int spaceSymbolsInRepeatExp = leftLineOfExp.Length; // Получаем количество пустых символов

                    // Ищем выражение ENDREPEAT
                    int repeatLineNumber = -1;
                    List<string> executableFileLinesInListForRepeat = lines.ToList();
                    executableFileLinesInListForRepeat.RemoveRange(0, lineCount);

                    int lineCounterRepeat = 0;

                    foreach (var executableFileLine in executableFileLinesInListForRepeat)
                    {
                        lineCounterRepeat++;
                        if (executableFileLine.Contains("ENDREPEAT"))
                        {
                            int spaceSymbolsInCurrentLine = executableFileLine.Split("ENDREPEAT")[0].Length;

                            if (spaceSymbolsInRepeatExp == spaceSymbolsInCurrentLine)
                                repeatLineNumber = lineCounterRepeat + lineCount;
                            else
                                continue;

                            break;
                        }
                    }

                    if (repeatLineNumber == -1)
                        throw new EndRepeatNotFoundException();

                    int countRepeat;
                    if (int.TryParse(line, out int repeatLineBuffer)) // Распознано числовое значение
                        countRepeat = repeatLineBuffer;
                    else
                    // Пробуем отыскать переменную
                    {
                        if (DataPool.variables.ContainsKey(line))
                        {
                            if (int.TryParse(DataPool.variables[line], out int repeatVarValueBuffer))
                                countRepeat = repeatVarValueBuffer;
                            else
                                throw new VariableNotIntegerException();
                        }
                        else
                            throw new VariableNotFoundException();
                    }

                    // Если обнаружен бесконечный цикл
                    if (countRepeat == 0)
                        throw new RepeatCountFoundEndlessCycle();

                    if (countRepeat < 1)
                        throw new RepeatCountNotInRangeException();

                    DataPool.CodeLevel += 1;
                    if (DataPool.CodeLevel > 3)
                        throw new CodeLevelException();

                    int countRepeatTmp = 0;
                    while (countRepeatTmp < countRepeat)
                    {
                        countRepeatTmp++;
                        await Lexer.Lexer.LexarySearch(lines, port, executeFilePath, lineCount + 1, repeatLineNumber - 1);
                    }

                    DataPool.CodeLevel -= 1;
                    for (int i = lineCount + 1; i <= repeatLineNumber; i++)
                        DataPool.linesRangeBlackList.Add(i);

                    break;
            }
        }
    }
}