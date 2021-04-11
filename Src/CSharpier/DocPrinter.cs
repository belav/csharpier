using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpier
{
    // a big chunk of the code in here is ported from prettier. The names and layout of the file were
    // kept consistent with how they looked in prettier because not everything
    // was ported over and porting over more code would be easier if this file looked basically the same
    // taken from prettier 2.2.1 or so
    public static class DocPrinter
    {
        [ThreadStatic]
        private static Dictionary<string, PrintMode>? groupModeMap;

        private static Indent RootIndent()
        {
            return new Indent(string.Empty, 0, new List<IndentType>());
        }

        private static Indent MakeIndent(Indent indent, Options options)
        {
            return GenerateIndent(
                indent,
                newPart: new IndentType("indent", 0),
                options
            );
        }

        private static Indent GenerateIndent(
            Indent indent,
            IndentType newPart,
            Options options)
        {
            var queue = new List<IndentType>(indent.Queue);
            if (newPart.Type == "dedent")
            {
                queue.RemoveAt(queue.Count - 1);
            }
            else
            {
                queue.Add(newPart);
            }

            var value = new StringBuilder();
            var length = 0;
            var lastTabs = 0;

            var lastSpaces = 0;
            foreach (var part in queue)
            {
                switch (part.Type)
                {
                    case "indent":
                        Flush();
                        if (options.UseTabs)
                        {
                            AddTabs(1);
                        }
                        else
                        {
                            AddSpaces(options.TabWidth);
                        }
                        break;
                    default:
                        throw new Exception(part.Type);
                }
            }

            FlushSpaces();

            void AddTabs(int count)
            {
                value.Append('\t', count);
                length += options.TabWidth * count;
            }

            void AddSpaces(int count)
            {
                value.Append(' ', count);
                length += count;
            }

            void Flush()
            {
                if (options.UseTabs)
                {
                    FlushTabs();
                }
                else
                {
                    FlushSpaces();
                }
            }

            void FlushTabs()
            {
                if (lastTabs > 0)
                {
                    AddTabs(lastTabs);
                }

                ResetLast();
            }

            void FlushSpaces()
            {
                if (lastSpaces > 0)
                {
                    AddSpaces(lastSpaces);
                }

                ResetLast();
            }

            void ResetLast()
            {
                lastTabs = 0;
                lastSpaces = 0;
            }

            // TODO 2 in prettier this has a ...ind
            return new Indent(value.ToString(), length, queue);
        }

        private static bool Fits(
            PrintCommand nextCommand,
            IEnumerable<PrintCommand> remainingCommands,
            int width,
            Options options,
            bool mustBeFlat = false)
        {
            var remainingCommandsAsArray = remainingCommands.Reverse()
                .ToArray();
            var remainingIndex = remainingCommandsAsArray.Length;
            var returnFalseIfMoreStringsFound = false;
            var currentStack = new Stack<PrintCommand>();
            currentStack.Push(nextCommand);

            void Push(Doc doc, PrintMode printMode, Indent indent)
            {
                currentStack.Push(new PrintCommand(indent, printMode, doc));
            }

            // `out` is only used for width counting because `trim` requires to look
            // backwards for space characters.
            var output = new StringBuilder();
            while (width >= 0)
            {
                if (currentStack.Count == 0)
                {
                    if (remainingIndex == 0)
                    {
                        return true;
                    }

                    currentStack.Push(
                        remainingCommandsAsArray[remainingIndex - 1]
                    );

                    remainingIndex--;
                    continue;
                }

                var (
                    currentIndent,
                    currentMode,
                    currentDoc
                    ) = currentStack.Pop();

                if (currentDoc is StringDoc stringDoc)
                {
                    if (stringDoc.Value == null)
                    {
                        continue;
                    }
                    if (returnFalseIfMoreStringsFound)
                    {
                        return false;
                    }
                    output.Append(stringDoc.Value);
                    width -= GetStringWidth(stringDoc.Value);
                }
                else if (currentDoc != Doc.Null)
                {
                    switch (currentDoc)
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
                                Push(
                                    concat.Parts[i],
                                    currentMode,
                                    currentIndent
                                );
                            }
                            break;
                        case IndentDoc indent:
                            Push(
                                indent.Contents,
                                currentMode,
                                MakeIndent(currentIndent, options)
                            );
                            break;
                        case Group group:
                            if (mustBeFlat && group.Break)
                            {
                                return false;
                            }

                            var groupMode = group.Break
                                ? PrintMode.MODE_BREAK
                                : currentMode;

                            Push(group.Contents, groupMode, currentIndent);

                            if (group.GroupId != null)
                            {
                                groupModeMap![group.GroupId] = groupMode;
                            }
                            break;
                        case IfBreak ifBreak:
                            var ifBreakMode = ifBreak.GroupId != null
                                && groupModeMap!.ContainsKey(ifBreak.GroupId)
                                ? groupModeMap[ifBreak.GroupId]
                                : currentMode;

                            var contents = ifBreakMode == PrintMode.MODE_BREAK
                                ? ifBreak.BreakContents
                                : ifBreak.FlatContents;

                            Push(contents, currentMode, currentIndent);
                            break;
                        case LineDoc line:
                            switch (currentMode)
                            {
                                case PrintMode.MODE_FLAT:
                                case PrintMode.MODE_FORCEFLAT:
                                    if (line.Type == LineDoc.LineType.Hard)
                                    {
                                        return true;
                                    }
                                    if (line.Type != LineDoc.LineType.Soft)
                                    {
                                        output.Append(" ");

                                        width -= 1;
                                    }
                                    break;
                                case PrintMode.MODE_BREAK:
                                    return true;
                            }
                            break;
                        case ForceFlat flat:
                            Push(flat.Contents, currentMode, currentIndent);
                            break;
                        case SpaceIfNoPreviousComment:
                            // TODO should this always be considered size one?
                            width -= 1;
                            break;
                        default:
                            throw new Exception(
                                "Can't handle " + currentDoc.GetType()
                            );
                    }
                }
            }

            return false;
        }

        public static string Print(Doc document, Options options)
        {
            groupModeMap = new Dictionary<string, PrintMode>();

            DocPrinterUtils.PropagateBreaks(document);

            var width = options.Width;
            var newLine = Environment.NewLine; // TODO 1 options
            var position = 0;

            var currentStack = new Stack<PrintCommand>();
            currentStack.Push(
                new PrintCommand(RootIndent(), PrintMode.MODE_BREAK, document)
            );

            var output = new StringBuilder();
            var shouldRemeasure = false;
            var newLineNextStringValue = false;
            var skipNextNewLine = false;

            var lineSuffix = new List<PrintCommand>();

            void Push(Doc doc, PrintMode printMode, Indent indent)
            {
                currentStack.Push(new PrintCommand(indent, printMode, doc));
            }
            while (currentStack.Count > 0)
            {
                var command = currentStack.Pop();
                if (command.Doc == Doc.Null)
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
                            case PrintMode.MODE_FORCEFLAT:
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
                                var next = new PrintCommand(
                                    command.Indent,
                                    PrintMode.MODE_FLAT,
                                    group.Contents
                                );

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

                        if (group.GroupId != null)
                        {
                            groupModeMap[
                                group.GroupId
                            ] = currentStack.Peek().Mode;
                        }
                        break;
                    case IfBreak ifBreak:
                        var groupMode = ifBreak.GroupId != null
                            && groupModeMap.ContainsKey(ifBreak.GroupId)
                            ? groupModeMap[ifBreak.GroupId]
                            : command.Mode;
                        var contents = groupMode == PrintMode.MODE_BREAK
                            ? ifBreak.BreakContents
                            : ifBreak.FlatContents;
                        Push(contents, command.Mode, command.Indent);
                        break;
                    case LineDoc line:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                            case PrintMode.MODE_FORCEFLAT:
                                if (line.Type == LineDoc.LineType.Soft)
                                {
                                    break;
                                }
                                else if (line.Type == LineDoc.LineType.Normal)
                                {
                                    output.Append(' ');
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
                            command.Indent.Value + leadingComment.Comment
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

            return string.Join(string.Empty, output);
        }

        // TODO 1 in prettier this deals with unicode characters that are double width
        private static int GetStringWidth(string value)
        {
            return value.Length;
        }

        private static void Trim(StringBuilder stringBuilder)
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
        private record IndentType(string Type, int Number);

        private record PrintCommand(Indent Indent, PrintMode Mode, Doc Doc);

        private enum PrintMode
        {
            MODE_FLAT,
            MODE_BREAK,
            MODE_FORCEFLAT
        }

        private record Indent(string Value, int Length, List<IndentType> Queue);
    }
}
