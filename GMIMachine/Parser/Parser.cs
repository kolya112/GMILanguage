﻿using System.Text;

namespace GMIMachine.Parser
{
    internal class Parser
    {
        internal static async Task Parse(string method, string line, int port, int lineCount = -1, string executeFilePath = "", string leftLineOfExp = "")
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
                    else if (!IsText(variableNameSET))
                        throw new CodeSyntaxException();

                    string variableValueSET = spaceInExpSET[2];

                    if (!int.TryParse(variableValueSET, out int variableValueInDigitSET))
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
                        await ServerProvider.SendPacket($"COUT >> {variableNameCOut}:{DataPool.variables[variableNameCOut]}", port);
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
                            await ServerProvider.SendPacket($"X >> {DataPool.coords.X + coordRight}", port);
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
                            await ServerProvider.SendPacket($"X >> {DataPool.coords.X - coordLeft}", port);
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
                            await ServerProvider.SendPacket($"Y >> {DataPool.coords.Y + coordUp}", port);
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
                            await ServerProvider.SendPacket($"Y >> {DataPool.coords.Y - coordDown}", port);
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
                    else if (!IsText(procName))
                        throw new CodeSyntaxException();

                    if (DataPool.procedures.ContainsKey(procName))
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
                        string[] executableFileLinesCall = await File.ReadAllLinesAsync(executeFilePath, Encoding.UTF8);
                        await Lexer.Lexer.LexarySearch(executableFileLinesCall, port, executeFilePath);
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

                    //int spaceSymbolsInExp = Lexer.Lexer.GetSpaceSymbolsCount(leftLineOfExp); // Получаем количество пустых символов
                    int spaceSymbolsInExp = leftLineOfExp.Length; // Получаем количество пустых символов

                    //await ServerProvider.SendPacket("leftLineOfExp: '" + leftLineOfExp.ToCharArray().Length + "'", port);

                    // Ищем выражение ENDIF
                    int endIfBlockLineNumber = -1;
                    string[] executableFileLinesForIfBlock = await File.ReadAllLinesAsync(executeFilePath, Encoding.UTF8);
                    List<string> executableFileLinesInListForIfBlock = executableFileLinesForIfBlock.ToList();
                    executableFileLinesInListForIfBlock.RemoveRange(0, lineCount);

                    int lineCounterIfBlock = 0;

                    foreach (var executableFileLine in executableFileLinesInListForIfBlock)
                    {
                        lineCounterIfBlock++;
                        if (executableFileLine.Contains("ENDIF"))
                        {
                            //int spaceSymbolsInCurrentLine = Lexer.Lexer.GetSpaceSymbolsCount(executableFileLine.Split("ENDIF")[0]);
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
                            await Lexer.Lexer.LexarySearch(executableFileLinesForIfBlock, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1, "IFBLOCK");
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
                            await Lexer.Lexer.LexarySearch(executableFileLinesForIfBlock, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1, "IFBLOCK");
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
                            await Lexer.Lexer.LexarySearch(executableFileLinesForIfBlock, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1, "IFBLOCK");
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
                            await Lexer.Lexer.LexarySearch(executableFileLinesForIfBlock, port, executeFilePath, lineCount + 1, endIfBlockLineNumber - 1, "IFBLOCK");
                            DataPool.CodeLevel -= 1;
                        }
                    }

                    for (int i = lineCount + 1; i <= endIfBlockLineNumber; i++)
                        DataPool.linesRangeBlackList.Add(i);

                    break;
            }
        }

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