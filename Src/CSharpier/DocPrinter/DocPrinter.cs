using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpier.DocTypes;
using Newtonsoft.Json.Serialization;

namespace CSharpier.DocPrinter
{
    // a big chunk of the code in here is ported from prettier. The names and layout of the file were
    // kept consistent with how they looked in prettier because not everything
    // was ported over and porting over more code would be easier if this file looked basically the same
    // taken from prettier 2.2.1 or so
    public static class DocPrinter
    {
        [ThreadStatic]
        private static Dictionary<string, PrintMode>? groupModeMap;

        private static bool Fits(
            PrintCommand nextCommand,
            List<PrintCommand> remainingCommands,
            int remainingWidth,
            PrinterOptions printerOptions,
            bool mustBeFlat = false
        ) {
            var returnFalseIfMoreStringsFound = false;
            var commandsToTest = new List<PrintCommand>(remainingCommands) { nextCommand };

            void Push(Doc doc, PrintMode printMode, Indent indent)
            {
                commandsToTest.Add(new PrintCommand(indent, printMode, doc));
            }

            var output = new StringBuilder();
            while (remainingWidth >= 0)
            {
                if (commandsToTest.Count == 0)
                {
                    return true;
                }

                var (currentIndent, currentMode, currentDoc) = commandsToTest[^1];
                commandsToTest.RemoveAt(commandsToTest.Count - 1);

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
                    remainingWidth -= GetStringWidth(stringDoc.Value);
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
                            for (var i = concat.Contents.Count - 1; i >= 0; i--)
                            {
                                Push(concat.Contents[i], currentMode, currentIndent);
                            }
                            break;
                        case IndentDoc indent:
                            Push(
                                indent.Contents,
                                currentMode,
                                IndentBuilder.Make(currentIndent, printerOptions)
                            );
                            break;
                        case Trim:
                            remainingWidth += TrimOutput(output);
                            break;
                        case Group group:
                            if (mustBeFlat && group.Break)
                            {
                                return false;
                            }

                            var groupMode = group.Break ? PrintMode.MODE_BREAK : currentMode;

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
                                    if (currentDoc is HardLine { SkipBreakIfFirstInGroup: true })
                                    {
                                        returnFalseIfMoreStringsFound = false;
                                    }
                                    else if (line.Type == LineDoc.LineType.Hard)
                                    {
                                        return true;
                                    }

                                    if (line.Type != LineDoc.LineType.Soft)
                                    {
                                        output.Append(' ');

                                        remainingWidth -= 1;
                                    }
                                    break;
                                case PrintMode.MODE_BREAK:
                                    return true;
                            }
                            break;
                        case ForceFlat flat:
                            Push(flat.Contents, currentMode, currentIndent);
                            break;
                        case BreakParent:
                            break;
                        default:
                            throw new Exception("Can't handle " + currentDoc.GetType());
                    }
                }
            }

            return false;
        }

        public static string Print(Doc document, PrinterOptions printerOptions, string endOfLine)
        {
            groupModeMap = new Dictionary<string, PrintMode>();

            PropagateBreaks.RunOn(document);

            var allowedWidth = printerOptions.Width;
            var currentWidth = 0;

            var remainingCommands = new List<PrintCommand>();

            remainingCommands.Add(
                new PrintCommand(IndentBuilder.MakeRoot(), PrintMode.MODE_BREAK, document)
            );

            var output = new StringBuilder();
            var shouldRemeasure = false;
            var newLineNextStringValue = false;
            var skipNextNewLine = false;

            void Add(Doc doc, PrintMode printMode, Indent indent)
            {
                remainingCommands.Add(new PrintCommand(indent, printMode, doc));
            }
            while (remainingCommands.Count > 0)
            {
                var command = remainingCommands[^1];
                remainingCommands.RemoveAt(remainingCommands.Count - 1);
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

                        // this ensures we don't print extra spaces after a trailing comment
                        // newLineNextStringValue & skipNextNewLine are set to true when we print a trailing comment
                        // when they are set we new line the next string we find. If we new line and then print a " " we end up with an extra space
                        if (newLineNextStringValue && skipNextNewLine && stringDoc.Value == " ")
                        {
                            break;
                        }

                        if (newLineNextStringValue)
                        {
                            TrimOutput(output);
                            output.Append(endOfLine).Append(command.Indent.Value);
                            currentWidth = command.Indent.Length;
                            newLineNextStringValue = false;
                        }
                        output.Append(stringDoc.Value);
                        currentWidth += GetStringWidth(stringDoc.Value);
                        break;
                    case Concat concat:
                        for (var x = concat.Contents.Count - 1; x >= 0; x--)
                        {
                            Add(concat.Contents[x], command.Mode, command.Indent);
                        }
                        break;
                    case IndentDoc indentBuilder:
                        Add(
                            indentBuilder.Contents,
                            command.Mode,
                            IndentBuilder.Make(command.Indent, printerOptions)
                        );
                        break;
                    case Trim:
                        currentWidth -= TrimOutput(output);
                        newLineNextStringValue = false;
                        break;
                    case Group group:
                        switch (command.Mode)
                        {
                            case PrintMode.MODE_FLAT:
                            case PrintMode.MODE_FORCEFLAT:
                                if (!shouldRemeasure)
                                {
                                    Add(
                                        group.Contents,
                                        group.Break ? PrintMode.MODE_BREAK : PrintMode.MODE_FLAT,
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

                                if (
                                    !group.Break
                                    && Fits(
                                        next,
                                        remainingCommands,
                                        allowedWidth - currentWidth,
                                        printerOptions
                                    )
                                ) {
                                    remainingCommands.Add(next);
                                }
                                else
                                {
                                    Add(group.Contents, PrintMode.MODE_BREAK, command.Indent);
                                }
                                break;
                        }

                        if (group.GroupId != null)
                        {
                            groupModeMap[group.GroupId] = remainingCommands[^1].Mode;
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
                        Add(contents, command.Mode, command.Indent);
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
                                    currentWidth += 1;
                                    break;
                                }

                                // This line was forced into the output even if we were in flattened mode, so we need to tell the next
                                // group that no matter what, it needs to remeasure because the previous measurement didn't accurately
                                // capture the entire expression (this is necessary for nested groups)
                                shouldRemeasure = true;
                                goto case PrintMode.MODE_BREAK;
                            case PrintMode.MODE_BREAK:
                                if (
                                    line.Squash
                                    && output.Length > 0
                                    && EndsWithNewLineAndWhitespace(output)
                                ) {
                                    break;
                                }

                                if (line.IsLiteral)
                                {
                                    if (output.Length > 0)
                                    {
                                        output.Append(endOfLine);
                                        currentWidth = 0;
                                    }
                                }
                                else
                                {
                                    if (!skipNextNewLine || !newLineNextStringValue)
                                    {
                                        TrimOutput(output);
                                        output.Append(endOfLine).Append(command.Indent.Value);
                                        currentWidth = command.Indent.Length;
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
                        TrimOutput(output);
                        if ((output.Length != 0 && output[^1] != '\n') || newLineNextStringValue)
                        {
                            output.Append(endOfLine);
                        }

                        output.Append(command.Indent.Value).Append(leadingComment.Comment);
                        currentWidth = command.Indent.Length;
                        newLineNextStringValue = false;
                        skipNextNewLine = false;
                        break;
                    case TrailingComment trailingComment:
                        TrimOutput(output);
                        output.Append(' ').Append(trailingComment.Comment);
                        currentWidth = command.Indent.Length;
                        newLineNextStringValue = true;
                        skipNextNewLine = true;
                        break;
                    case ForceFlat forceFlat:
                        Add(forceFlat.Contents, PrintMode.MODE_FLAT, command.Indent);
                        break;
                    default:
                        throw new Exception("didn't handle " + command.Doc);
                }
            }

            if (output.Length == 0 || output[^1] != '\n')
            {
                output.Append(endOfLine);
            }

            var result = output.ToString();
            if (printerOptions.TrimInitialLines)
            {
                result = result.TrimStart('\n', '\r');
            }

            return result;
        }

        // TODO 1 in prettier this deals with unicode characters that are double width
        private static int GetStringWidth(string value)
        {
            return value.Length;
        }

        private static bool EndsWithNewLineAndWhitespace(StringBuilder stringBuilder)
        {
            for (var index = 1; index <= stringBuilder.Length; index++)
            {
                var next = stringBuilder[^index];
                if (next == ' ' || next == '\t')
                {
                    continue;
                }
                else if (next == '\n')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private static int TrimOutput(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length == 0)
            {
                return 0;
            }

            var trimmed = 0;
            for (; trimmed < stringBuilder.Length; trimmed++)
            {
                if (stringBuilder[^(trimmed + 1)] != ' ' && stringBuilder[^(trimmed + 1)] != '\t')
                {
                    break;
                }
            }

            stringBuilder.Length = stringBuilder.Length - trimmed;
            return trimmed;
        }

        private record PrintCommand(Indent Indent, PrintMode Mode, Doc Doc);

        private enum PrintMode
        {
            MODE_FLAT,
            MODE_BREAK,
            MODE_FORCEFLAT
        }
    }
}
