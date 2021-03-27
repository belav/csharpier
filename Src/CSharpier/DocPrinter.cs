using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return this.GenerateIndent(
                indent,
                new IndentType { Type = "indent" },
                options
            );
        }

        // TODO 2 there is more going on here with dedent and number/string align
        private Indent GenerateIndent(
            Indent ind,
            IndentType newPart,
            Options options)
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
                        // TODO 2 huh? length += part.n.length;
                        break;
                    case "numberAlign":
                        lastTabs += 1;
                        // TODO 2 huh? lastSpaces += part.n;
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
                // TODO 2 in prettier this has a ...ind
                Value = value,
                Length = length,
                Queue = queue
            };
        }

        private bool Fits(
            PrintCommand next,
            Stack<PrintCommand> restCommands,
            int width,
            Options options,
            bool mustBeFlat = false)
        {
            var commandsAsArray = restCommands.Reverse().ToArray();
            var restIdx = commandsAsArray.Length;
            var returnFalseIfMoreStringsFound = false;
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
                    if (stringDoc.Value != null)
                    {
                        if (returnFalseIfMoreStringsFound)
                        {
                            return false;
                        }
                        output.Append(stringDoc.Value);
                        width -= GetStringWidth(stringDoc.Value);
                    }
                }
                else if (doc != null)
                {
                    switch (doc)
                    {
                        case LeadingComment:
                        case TrailingComment:
                            if (output.Length > 0)
                            {
                                returnFalseIfMoreStringsFound = true;
                            }
                            break;
                        case Concat concat:
                            for (var i = concat.Parts.Count - 1; i >= 0; i--)
                            {
                                cmds.Push(
                                    new PrintCommand
                                    {
                                        Indent = ind,
                                        Mode = mode,
                                        Doc = concat.Parts[i]
                                    }
                                );
                            }
                            break;
                        case IndentDoc indent:
                            cmds.Push(
                                new PrintCommand
                                {
                                    Indent = MakeIndent(ind, options),
                                    Mode = mode,
                                    Doc = indent.Contents
                                }
                            );
                            break;
                        case Group group:
                            if (mustBeFlat && group.Break)
                            {
                                return false;
                            }

                            cmds.Push(
                                new PrintCommand
                                {
                                    Indent = ind,
                                    Mode = group.Break
                                        ? PrintMode.MODE_BREAK
                                        : mode,
                                    Doc = group.Contents,

                                }
                            );
                            break;
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
                        case ForceFlat flat:
                            cmds.Push(
                                new PrintCommand
                                {
                                    Indent = ind,
                                    Mode = mode,
                                    Doc = flat.Contents
                                }
                            );
                            break;
                        case SpaceIfNoPreviousComment:
                            // TODO should this always be considered size one?
                            width -= 1;
                            break;
                        default:
                            throw new Exception(
                                "Can't handle " + doc.GetType()
                            );
                    }
                }
            }

            return false;
        }

        public string Print(Doc document, Options options)
        {
            DocPrinterUtils.PropagateBreaks(document);

            var width = options.Width;
            var newLine = Environment.NewLine; // TODO 1 options
            var position = 0;

            var currentStack = new Stack<PrintCommand>();
            currentStack.Push(
                new PrintCommand
                {
                    Doc = document,
                    Indent = this.RootIndent(),
                    Mode = PrintMode.MODE_BREAK,

                }
            );

            var output = new StringBuilder();
            var shouldRemeasure = false;
            var newLineNextStringValue = false;
            var skipNextNewLine = false;

            var lineSuffix = new List<PrintCommand>();

            void Push(Doc doc, PrintMode printMode, Indent indent)
            {
                currentStack.Push(
                    new PrintCommand
                    {
                        Doc = doc,
                        Mode = printMode,
                        Indent = indent
                    }
                );
            }
            while (currentStack.Count > 0)
            {
                var command = currentStack.Pop();
                if (command.Doc == null)
                {
                    continue;
                }
                switch (command.Doc)
                {
                    case StringDoc stringDoc:
                        if (string.IsNullOrEmpty(stringDoc.Value))
                        {
                            break;
                        }
                        if (newLineNextStringValue)
                        {
                            // TODO 1 new line stuff
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
                            Push(concat.Parts[x], command.Mode, command.Indent);
                        }
                        break;
                    case IndentDoc indentBuilder:
                        Push(
                            indentBuilder.Contents,
                            command.Mode,
                            MakeIndent(command.Indent, options)
                        );
                        break;
                    case Group group:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                                if (!shouldRemeasure)
                                {
                                    Push(
                                        group.Contents,
                                        group.Break
                                            ? PrintMode.MODE_BREAK
                                            : PrintMode.MODE_FLAT,
                                        command.Indent
                                    );
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

                                if (
                                    !group.Break
                                    && Fits(next, currentStack, rem, options)
                                )
                                {
                                    currentStack.Push(next);
                                }
                                else
                                {
                                    Push(
                                        group.Contents,
                                        PrintMode.MODE_BREAK,
                                        command.Indent
                                    );
                                }
                                break;
                        }
                        break;
                    case LineDoc line:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                                if (line.Type == LineDoc.LineType.Soft)
                                {
                                    break;
                                }
                                else if (line.Type == LineDoc.LineType.Normal)
                                {
                                    output.Append(" ");
                                    position += 1;
                                    break;
                                }

                                // This line was forced into the output even if we were in flattened mode, so we need to tell the next
                                // group that no matter what, it needs to remeasure  because the previous measurement didn't accurately
                                // capture the entire expression (this is necessary for nested groups)
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
                                    if (output.Length > 0)
                                    {
                                        // if we have to undo this, another option I was considering was modifying NamespaceDeclarationSyntax
                                        // when joining things with HardLines there, if the first item in each thing was a LiteralLine, skip the HardLine
                                        // the problem with that approach is we'd have to make that same change in other places, but maybe
                                        // there aren't a ton of those places.
                                        Trim(output);
                                        if (newLine.Length == 2)
                                        {
                                            if (output[^2] == '\r')
                                            {
                                                output.Length -= 2;
                                            }
                                        }
                                        else
                                        {
                                            if (output[^1] == '\n')
                                            {
                                                output.Length -= 1;
                                            }
                                        }

                                        output.Append(newLine);
                                        position = 0;
                                    }
                                }
                                else
                                {
                                    if (
                                        (!newLineNextStringValue
                                        || !skipNextNewLine)
                                        && output.Length > 0
                                    )
                                    {
                                        Trim(output);
                                        output.Append(
                                            newLine + command.Indent.Value
                                        );
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
                    case BreakParent:
                        break;
                    case LeadingComment leadingComment:
                        Trim(output);
                        if (
                            (output.Length != 0
                            && output[^1] != '\n')
                            || newLineNextStringValue
                        )
                        {
                            output.Append(newLine);
                        }

                        output.Append(
                            command.Indent.Value + leadingComment.Comment + newLine + command.Indent.Value
                        );
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
                    case SpaceIfNoPreviousComment:
                        if (!newLineNextStringValue)
                        {
                            Push(" ", command.Mode, command.Indent);
                        }
                        break;
                    case ForceFlat forceFlat:
                        Push(
                            forceFlat.Contents,
                            PrintMode.MODE_FLAT,
                            command.Indent
                        );
                        break;
                    default:
                        throw new Exception("didn't handle " + command.Doc);
                }
            }

            if (output.Length == 0 || output[^1] != '\n')
            {
                output.Append(newLine);
            }

            return string.Join("", output);
        }

        // TODO 1 in prettier this deals with unicode characters that are double width
        private int GetStringWidth(string value)
        {
            return value.Length;
        }

        private void Trim(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length == 0)
            {
                return;
            }

            var i = stringBuilder.Length - 1;
            for (; i >= 0; i--)
            {
                if (stringBuilder[i] != ' ' && stringBuilder[i] != '\t')
                {
                    break;
                }
            }

            stringBuilder.Length = i + 1;
        }

        // // TODO 2 does the above method do the same thing as this method?
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
            MODE_FORCEFLAT
        }

        private class Indent
        {
            public string Value { get; set; }
            public int Length { get; set; }
            public List<IndentType> Queue { get; set; }
        }
    }
}
