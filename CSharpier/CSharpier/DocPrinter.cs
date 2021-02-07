using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpier
{
    public class DocPrinter
    {
        private Indent RootIndent()
        {
            return new Indent
            {
                Value = "",
                Length = 0,
                Queue = new List<IndentType>()
            };
        }

        private Indent MakeIndent(Indent indent, Options options)
        {
            return this.GenerateIndent(indent, new IndentType { Type = "indent" }, options);
        }

        // TODO there is more going on here with dedent and number/string align
        private Indent GenerateIndent(Indent ind, IndentType newPart, Options options)
        {
            var queue = new List<IndentType>(ind.Queue);
            if (newPart.Type == "dedent")
            {
                queue.RemoveAt(queue.Count - 1);
            }
            else
            {
                queue.Add(newPart);
            }

            var value = "";
            var length = 0;
            var lastTabs = 0;

            var lastSpaces = 0;
            foreach (var part in queue)
            {
                switch (part.Type)
                {
                    case "indent":
                        flush();
                        if (options.UseTabs)
                        {
                            addTabs(1);
                        }
                        else
                        {
                            addSpaces(options.TabWidth);
                        }

                        break;
                    case "stringAlign":
                        flush();
                        value += part.Number;
                        // TODO huh? length += part.n.length;
                        break;
                    case "numberAlign":
                        lastTabs += 1;
                        // TODO huh? lastSpaces += part.n;
                        break;
                    default:
                        throw new Exception(part.Type);
                }
            }

            flushSpaces();

            void addTabs(int count)
            {
                value += new string('\t', count);
                length += options.TabWidth * count;
            }

            void addSpaces(int count)
            {
                value += new string(' ', count);
                length += count;
            }

            void flush()
            {
                if (options.UseTabs)
                {
                    flushTabs();
                }
                else
                {
                    flushSpaces();
                }
            }

            void flushTabs()
            {
                if (lastTabs > 0)
                {
                    addTabs(lastTabs);
                }

                resetLast();
            }

            void flushSpaces()
            {
                if (lastSpaces > 0)
                {
                    addSpaces(lastSpaces);
                }

                resetLast();
            }

            ;

            void resetLast()
            {
                lastTabs = 0;
                lastSpaces = 0;
            }

            ;
            return new Indent
            {
                // TODO in prettier this has a ...ind
                Value = value,
                Length = length,
                Queue = queue
            };
        }

        private bool Fits(PrintCommand next, Stack<PrintCommand> restCommands, int width, Options options, bool mustBeFlat = false)
        {
            var commandsAsArray = restCommands.ToArray();
            var restIdx = commandsAsArray.Length;
            var cmds = new Stack<PrintCommand>();
            cmds.Push(next);
            // `out` is only used for width counting because `trim` requires to look
            // backwards for space characters.
            var output = new StringBuilder();
            while (width >= 0)
            {
                if (cmds.Count == 0)
                {
                    if (restIdx == 0)
                    {
                        return true;
                    }

                    cmds.Push(commandsAsArray[restIdx - 1]);

                    restIdx--;

                    continue;
                }

                var command = cmds.Pop();
                var ind = command.Indent;
                var mode = command.Mode;
                var doc = command.Doc;

                if (doc is StringDoc stringDoc)
                {
                    output.Append(stringDoc.Value);

                    width -= GetStringWidth(stringDoc.Value);
                }
                else
                {
                    switch (doc)
                    {
                        case Concat concat:
                            for (var i = concat.Parts.Count - 1; i >= 0; i--)
                            {
                                cmds.Push(
                                    new PrintCommand
                                    {
                                        Indent = ind,
                                        Mode = mode,
                                        Doc = concat.Parts[i]
                                    });
                            }

                            break;
                        case IndentDoc indent:
                            cmds.Push(new PrintCommand
                            {
                                Indent = MakeIndent(ind, options),
                                Mode = mode,
                                Doc = indent.Contents
                            });

                            break;
                        // TODO align/trim?
                        // case "align":
                        //     cmds.Add([makeAlign(ind, doc.n, options), mode, doc.contents]);
                        //
                        //     break;
                        // case "trim":
                        //     width += trim(out);
                        //
                        //     break;
                        case Group group:
                            if (mustBeFlat && group.Break)
                            {
                                return false;
                            }

                            cmds.Push(new PrintCommand
                            {
                                Indent = ind,
                                Mode = group.Break ? PrintMode.MODE_BREAK : mode,
                                Doc = group.Contents,
                            });

                            // TODO groupId
                            // if (doc.id) {
                            //     groupModeMap[doc.id] = cmds[cmds.length - 1][1];
                            // }
                            break;
                        // TODO fill/if-break
                        // case "fill":
                        //     for (let i = doc.parts.length - 1; i >= 0; i--) {
                        //         cmds.Add([ind, mode, doc.parts[i]]);
                        //     }
                        //
                        //     break;
                        // case "if-break": {
                        //     const groupMode = doc.groupId ? groupModeMap[doc.groupId] : mode;
                        //     if (groupMode === MODE_BREAK) {
                        //         if (doc.breakContents) {
                        //             cmds.Add([ind, mode, doc.breakContents]);
                        //         }
                        //     }
                        //     if (groupMode === MODE_FLAT) {
                        //         if (doc.flatContents) {
                        //             cmds.Add([ind, mode, doc.flatContents]);
                        //         }
                        //     }
                        //
                        //     break;
                        // }
                        case LineDoc line:
                            switch (mode)
                            {
                                case PrintMode.MODE_FLAT:
                                    if (line.Type != LineDoc.LineType.Hard)
                                    {
                                        if (line.Type != LineDoc.LineType.Soft)
                                        {
                                            output.Append(" ");

                                            width -= 1;
                                        }

                                        break;
                                    }

                                    return true;
                                case PrintMode.MODE_BREAK:
                                    return true;
                            }

                            break;
                    }
                }
            }

            return false;
        }

        public string Print(Doc document, Options options)
        {
            DocPrinterUtils.PropagateBreaks(document);

            var width = options.Width;
            var newLine = Environment.NewLine; // TODO options
            var position = 0;

            var currentStack = new Stack<PrintCommand>();
            currentStack.Push(new PrintCommand
            {
                Doc = document,
                Indent = this.RootIndent(),
                Mode = PrintMode.MODE_BREAK,
            });

            var output = new StringBuilder();
            var shouldRemeasure = false;
            var newLineNextStringValue = false;
            var skipNextNewLine = false;

            var lineSuffix = new List<PrintCommand>();

            while (currentStack.Count > 0)
            {
                var command = currentStack.Pop();
                switch (command.Doc)
                {
                    case StringDoc stringDoc:
                        if (stringDoc.Value == "")
                        {
                            break;
                        }
                        if (newLineNextStringValue)
                        {
                            // TODO new line stuff
                            Trim(output);
                            output.Append(newLine + command.Indent.Value);
                            position = command.Indent.Length;
                            newLineNextStringValue = false;
                        }
                        output.Append(stringDoc.Value);
                        position += GetStringWidth(stringDoc.Value);
                        break;
                    case Concat concat:
                        for (var x = concat.Parts.Count - 1; x >= 0; x--)
                        {
                            currentStack.Push(new PrintCommand
                            {
                                Doc = concat.Parts[x],
                                Mode = command.Mode,
                                Indent = command.Indent
                            });
                        }

                        break;
                    case IndentDoc indentBuilder:
                        currentStack.Push(new PrintCommand
                        {
                            Indent = MakeIndent(command.Indent, options),
                            Mode = command.Mode,
                            Doc = indentBuilder.Contents
                        });

                        break;
                    case Group group:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                                if (!shouldRemeasure)
                                {
                                    currentStack.Push(new PrintCommand
                                    {
                                        Indent = command.Indent,
                                        Mode = group.Break ? PrintMode.MODE_BREAK : PrintMode.MODE_FLAT,
                                        Doc = group.Contents,
                                    });
                                    break;
                                }

                                goto case PrintMode.MODE_BREAK;
                            case PrintMode.MODE_BREAK:
                                shouldRemeasure = false;

                                var next = new PrintCommand
                                {
                                    Indent = command.Indent,
                                    Mode = PrintMode.MODE_FLAT,
                                    Doc = group.Contents
                                };

                                var rem = width - position;

                                if (!group.Break && Fits(next, currentStack, rem, options))
                                {
                                    currentStack.Push(next);
                                }
                                else
                                {
                                    // TODO expandedStates is a big complicated thing here, but I don't think I'll use it?
                                    currentStack.Push(new PrintCommand
                                    {
                                        Indent = command.Indent,
                                        Mode = PrintMode.MODE_BREAK,
                                        Doc = group.Contents
                                    });
                                }

                                break;
                        }

                        // TODO group ids
                        // if (doc.id)
                        // {
                        //     groupModeMap[doc.id] = cmds[cmds.length - 1][1];
                        // }

                        break;

                    case LineDoc line:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                                if (line.Type != LineDoc.LineType.Hard)
                                {
                                    if (line.Type != LineDoc.LineType.Soft)
                                    {
                                        output.Append(" ");

                                        position += 1;
                                    }

                                    break;
                                }

                                // This line was forced into the output even if we
                                // were in flattened mode, so we need to tell the next
                                // group that no matter what, it needs to remeasure
                                // because the previous measurement didn't accurately
                                // capture the entire expression (this is necessary
                                // for nested groups)
                                shouldRemeasure = true;
                                goto case PrintMode.MODE_BREAK;
                            case PrintMode.MODE_BREAK:
                                if (lineSuffix.Any())
                                {
                                    currentStack.Push(command);
                                    lineSuffix.Reverse();
                                    foreach (var otherCommand in lineSuffix)
                                    {
                                        currentStack.Push(otherCommand);
                                    }

                                    lineSuffix.Clear();
                                    break;
                                }

                                if (line.IsLiteral)
                                {
                                    // TODO indent.root perhaps?
                                    // if (ind.root) {
                                    //     out.Add(newLine, ind.root.value);
                                    //     pos = ind.root.length;
                                    // } else {
                                    output.Append(newLine);
                                    position = 0;
                                }
                                else
                                {
                                    if (!newLineNextStringValue || !skipNextNewLine)
                                    {
                                        Trim(output);
                                        output.Append(newLine + command.Indent.Value);
                                        position = command.Indent.Length;
                                    }

                                    if (skipNextNewLine)
                                    {
                                        skipNextNewLine = false;
                                    }
                                }

                                break;
                        }

                        break;
                    case BreakParent breakParent: // this doesn't seem to be used in here
                        break;
                    case LeadingComment leadingComment:
                        Trim(output);
                        if ((output.Length != 0 && output[^1] != '\n') || newLineNextStringValue)
                        {
                            output.Append(newLine);
                        }

                        output.Append(command.Indent.Value + leadingComment.Comment + newLine + command.Indent.Value);
                        position = command.Indent.Length;
                        newLineNextStringValue = false;
                        skipNextNewLine = false;
                        break;
                    case TrailingComment trailingComment:
                        Trim(output);
                        output.Append(" " + trailingComment.Comment);
                        position = command.Indent.Length;
                        newLineNextStringValue = true;
                        skipNextNewLine = true;
                        break;

                    default:
                        throw new Exception("didn't handle " + command.Doc);
                }
            }

            return string.Join("", output);
        }

        // TODO in prettier this deals with unicode characters that are double width
        private int GetStringWidth(string value)
        {
            return value.Length;
        }

        private int Trim(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length == 0)
            {
                return 0;
            }

            var trimCount = 0;
            var i = stringBuilder.Length - 1;
            for (; i >= 0; i--)
            {
                if (stringBuilder[i] != ' ' && stringBuilder[i] != '\t')
                {
                    break;
                }

                trimCount++;
            }

            stringBuilder.Length = i + 1;

            return trimCount;
        }

        // // TODO does the above method do the same thing as this method?
        // private int Trim(List<string> output)
        // {
        //     if (output.Count == 0)
        //     {
        //         return 0;
        //     }
        //
        //     var trimCount = 0;
        //
        //     // Trim whitespace at the end of line
        //     while (output.Count > 0 && Regex.IsMatch(output[^1], "^[\\t ]*$"))
        //     {
        //         trimCount += output[^1].Length;
        //         output.RemoveAt(output.Count - 1);
        //     }
        //
        //     if (output.Count > 0)
        //     {
        //         var trimmed = output[^1];
        //         trimmed = Regex.Replace(trimmed, "[\\t ]*$", "");
        //         trimCount += output[^1].Length - trimmed.Length;
        //         output[^1] = trimmed;
        //     }
        //
        //     return trimCount;
        // }

        private class IndentType
        {
            public string Type { get; set; }
            public int Number { get; set; }
        }

        private class PrintCommand
        {
            public Indent Indent { get; set; }
            public PrintMode Mode { get; set; }
            public Doc Doc { get; set; }
        }

        private enum PrintMode
        {
            MODE_FLAT,
            MODE_BREAK,
        }

        private class Indent
        {
            public string Value { get; set; }
            public int Length { get; set; }
            public List<IndentType> Queue { get; set; }
        }
    }
}