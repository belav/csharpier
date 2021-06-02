using System;
using System.Collections.Generic;
using System.Text;
using CSharpier.DocTypes;
using CSharpier.Utilities;

namespace CSharpier.DocPrinter
{
    public static class DocPrinter
    {
        public static string Print(Doc document, PrinterOptions printerOptions, string endOfLine)
        {
            PropagateBreaks.RunOn(document);

            var remainingCommands = new Stack<PrintCommand>();
            remainingCommands.Push(
                new PrintCommand(IndentBuilder.MakeRoot(), PrintMode.Break, document)
            );

            var groupModeMap = new Dictionary<string, PrintMode>();
            var allowedWidth = printerOptions.Width;
            var currentWidth = 0;
            var output = new StringBuilder();
            var shouldRemeasure = false;
            var newLineNextStringValue = false;
            var skipNextNewLine = false;

            void Add(Doc doc, PrintMode printMode, Indent indent)
            {
                remainingCommands.Push(new PrintCommand(indent, printMode, doc));
            }
            while (remainingCommands.Count > 0)
            {
                var (currentIndent, currentMode, currentDoc) = remainingCommands.Pop();
                if (currentDoc == Doc.Null)
                {
                    continue;
                }
                switch (currentDoc)
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
                            output.TrimTrailingWhitespace();
                            output.Append(endOfLine).Append(currentIndent.Value);
                            currentWidth = currentIndent.Length;
                            newLineNextStringValue = false;
                        }
                        output.Append(stringDoc.Value);
                        currentWidth += stringDoc.Value.GetPrintedWidth();
                        break;
                    case Concat concat:
                        for (var x = concat.Contents.Count - 1; x >= 0; x--)
                        {
                            Add(concat.Contents[x], currentMode, currentIndent);
                        }
                        break;
                    case IndentDoc indentBuilder:
                        Add(
                            indentBuilder.Contents,
                            currentMode,
                            IndentBuilder.Make(currentIndent, printerOptions)
                        );
                        break;
                    case Trim:
                        currentWidth -= output.TrimTrailingWhitespace();
                        newLineNextStringValue = false;
                        break;
                    case Group group:
                        switch (currentMode)
                        {
                            case PrintMode.Flat:
                            case PrintMode.Forceflat:
                                if (!shouldRemeasure)
                                {
                                    Add(
                                        group.Contents,
                                        group.Break ? PrintMode.Break : PrintMode.Flat,
                                        currentIndent
                                    );
                                    break;
                                }

                                goto case PrintMode.Break;
                            case PrintMode.Break:
                                shouldRemeasure = false;
                                var next = new PrintCommand(
                                    currentIndent,
                                    PrintMode.Flat,
                                    group.Contents
                                );

                                if (
                                    !group.Break
                                    && DocFitter.Fits(
                                        next,
                                        remainingCommands,
                                        allowedWidth - currentWidth,
                                        printerOptions,
                                        groupModeMap
                                    )
                                ) {
                                    remainingCommands.Push(next);
                                }
                                else
                                {
                                    Add(group.Contents, PrintMode.Break, currentIndent);
                                }
                                break;
                        }

                        if (group.GroupId != null)
                        {
                            groupModeMap[group.GroupId] = remainingCommands.Peek().Mode;
                        }
                        break;
                    case IfBreak ifBreak:
                        var groupMode = ifBreak.GroupId != null
                        && groupModeMap.ContainsKey(ifBreak.GroupId)
                            ? groupModeMap[ifBreak.GroupId]
                            : currentMode;
                        var contents = groupMode == PrintMode.Break
                            ? ifBreak.BreakContents
                            : ifBreak.FlatContents;
                        Add(contents, currentMode, currentIndent);
                        break;
                    case LineDoc line:
                        switch (currentMode)
                        {
                            case PrintMode.Flat:
                            case PrintMode.Forceflat:
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
                                goto case PrintMode.Break;
                            case PrintMode.Break:
                                if (
                                    line.Squash
                                    && output.Length > 0
                                    && output.EndsWithNewLineAndWhitespace()
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
                                        output.TrimTrailingWhitespace();
                                        output.Append(endOfLine).Append(currentIndent.Value);
                                        currentWidth = currentIndent.Length;
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
                        output.TrimTrailingWhitespace();
                        if ((output.Length != 0 && output[^1] != '\n') || newLineNextStringValue)
                        {
                            output.Append(endOfLine);
                        }

                        output.Append(currentIndent.Value).Append(leadingComment.Comment);
                        currentWidth = currentIndent.Length;
                        newLineNextStringValue = false;
                        skipNextNewLine = false;
                        break;
                    case TrailingComment trailingComment:
                        output.TrimTrailingWhitespace();
                        output.Append(' ').Append(trailingComment.Comment);
                        currentWidth = currentIndent.Length;
                        newLineNextStringValue = true;
                        skipNextNewLine = true;
                        break;
                    case ForceFlat forceFlat:
                        Add(forceFlat.Contents, PrintMode.Flat, currentIndent);
                        break;
                    default:
                        throw new Exception("didn't handle " + currentDoc);
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
    }

    public record PrintCommand(Indent Indent, PrintMode Mode, Doc Doc);

    public enum PrintMode
    {
        Flat,
        Break,
        Forceflat
    }
}
