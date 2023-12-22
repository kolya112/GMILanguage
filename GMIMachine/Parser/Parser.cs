using GMIMachine.Lexer;
using System;
using System.Text;

namespace GMIMachine.Parser
{
    internal class Parser
    {
        internal static async Task Parse(string method, string line, int port, int lineCount = -1, string executeFilePath = "")
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
                    if (!(coordRight < 1) || !(coordRight > 21))
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if (!((DataPool.coords.X + coordRight) > 21))
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
                    if (!(coordLeft < 1) || !(coordLeft > 21))
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if (!((DataPool.coords.X - coordLeft) < 0))
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
                    if (!(coordUp < 1) || !(coordUp > 21))
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if (!((DataPool.coords.Y + coordUp) > 21))
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
                    if (!(coordDown < 1) || !(coordDown > 21))
                    {
                        // Проверяем, не выйдет ли исполнитель за рамки сетки
                        if (!((DataPool.coords.Y - coordDown) < 0))
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
                    foreach (var executableFileLine in executableFileLines)
                    {
                        lineCounter++;
                        if (executableFileLine.Contains("ENDPROC"))
                        {
                            endProcLineNumber = lineCounter;
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