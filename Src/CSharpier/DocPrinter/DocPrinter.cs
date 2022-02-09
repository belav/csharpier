using System.Text;

namespace CSharpier.DocPrinter;

internal class DocPrinter
{
    protected readonly Stack<PrintCommand> RemainingCommands = new();
    protected readonly Dictionary<string, PrintMode> GroupModeMap = new();
    protected int CurrentWidth;
    protected readonly StringBuilder Output = new();
    protected bool ShouldRemeasure;
    protected bool NewLineNextStringValue;
    protected bool SkipNextNewLine;
    protected readonly string EndOfLine;
    protected readonly PrinterOptions PrinterOptions;
    protected readonly Indenter Indenter;

    protected DocPrinter(Doc doc, PrinterOptions printerOptions, string endOfLine)
    {
        this.EndOfLine = endOfLine;
        this.PrinterOptions = printerOptions;
        this.Indenter = new Indenter(printerOptions);
        this.RemainingCommands.Push(
            new PrintCommand(Indenter.GenerateRoot(), PrintMode.Break, doc)
        );
    }

    public static string Print(Doc document, PrinterOptions printerOptions, string endOfLine)
    {
        PropagateBreaks.RunOn(document);

        return new DocPrinter(document, printerOptions, endOfLine).Print();
    }

    public string Print()
    {
        while (this.RemainingCommands.Count > 0)
        {
            this.ProcessNextCommand();
        }

        this.EnsureOutputEndsWithSingleNewLine();

        var result = this.Output.ToString();
        if (this.PrinterOptions.TrimInitialLines)
        {
            result = result.TrimStart('\n', '\r');
        }

        return result;
    }

    private void EnsureOutputEndsWithSingleNewLine()
    {
        var trimmed = 0;
        for (; trimmed < this.Output.Length; trimmed++)
        {
            if (this.Output[^(trimmed + 1)] != '\r' && this.Output[^(trimmed + 1)] != '\n')
            {
                break;
            }
        }

        this.Output.Length -= trimmed;

        this.Output.Append(this.EndOfLine);
    }

    private void ProcessNextCommand()
    {
        var (indent, mode, doc) = this.RemainingCommands.Pop();
        if (doc == Doc.Null)
        {
            return;
        }

        switch (doc)
        {
            case StringDoc stringDoc:
                this.ProcessString(stringDoc, indent);
                break;
            case Concat concat:
            {
                for (var x = concat.Contents.Count - 1; x >= 0; x--)
                {
                    this.Push(concat.Contents[x], mode, indent);
                }
                break;
            }
            case IndentDoc indentDoc:
                this.Push(indentDoc.Contents, mode, this.Indenter.IncreaseIndent(indent));
                break;
            case Trim:
                this.CurrentWidth -= this.Output.TrimTrailingWhitespace();
                this.NewLineNextStringValue = false;
                break;
            case Group group:
                this.ProcessGroup(@group, mode, indent);
                break;
            case IfBreak ifBreak:
            {
                var groupMode = mode;
                if (ifBreak.GroupId != null)
                {
                    if (!this.GroupModeMap.TryGetValue(ifBreak.GroupId, out groupMode))
                    {
                        throw new Exception(
                            "You cannot use an ifBreak before the group it targets."
                        );
                    }
                }

                var contents =
                    groupMode == PrintMode.Break ? ifBreak.BreakContents : ifBreak.FlatContents;
                this.Push(contents, mode, indent);
                break;
            }
            case LineDoc line:
                this.ProcessLine(line, mode, indent);
                break;
            case BreakParent:
                break;
            case LeadingComment leadingComment:
            {
                this.Output.TrimTrailingWhitespace();
                if (
                    (this.Output.Length != 0 && this.Output[^1] != '\n')
                    || this.NewLineNextStringValue
                )
                {
                    this.Output.Append(this.EndOfLine);
                }

                this.AppendComment(leadingComment, indent);

                this.CurrentWidth = indent.Length;
                this.NewLineNextStringValue = false;
                this.SkipNextNewLine = false;
                break;
            }
            case TrailingComment trailingComment:
                this.Output.TrimTrailingWhitespace();
                this.Output.Append(' ').Append(trailingComment.Comment);
                this.CurrentWidth = indent.Length;
                this.NewLineNextStringValue = true;
                this.SkipNextNewLine = true;
                break;
            case ForceFlat forceFlat:
                this.Push(forceFlat.Contents, PrintMode.Flat, indent);
                break;
            case Align align:
                this.Push(align.Contents, mode, this.Indenter.AddAlign(indent, align.Width));
                break;
            default:
                throw new Exception("didn't handle " + doc);
        }
    }

    private void AppendComment(LeadingComment leadingComment, Indent indent)
    {
        // this may move around the opening /* for a multiline comment
        // but it doesn't properly line up comments of the following style if their
        // indentation changes
        // /*
        //  *
        //  */
        if (leadingComment.Type is CommentType.MultiLine)
        {
            this.Output.Append(indent.Value);
            this.Output.Append(leadingComment.Comment.TrimStart());
            return;
        }

        var stringReader = new StringReader(leadingComment.Comment);
        var line = stringReader.ReadLine();
        var firstLine = line;
        string? extraIndent = null;
        while (line != null)
        {
            this.Output.Append(indent.Value);
            if (extraIndent?.Length > 0)
            {
                this.Output.Append(extraIndent);
            }

            // this takes more work, fixes the issue above, but sometimes moves around the */ on a multiline comment
            // if (extraIndent != null && leadingComment.Type is CommentType.MultiLine)
            // {
            //     var startingSpace = 0;
            //     foreach (var character in line)
            //     {
            //         if (character == ' ')
            //         {
            //             startingSpace += 1;
            //         }
            //         else if (character == '\t')
            //         {
            //             startingSpace += 4;
            //         }
            //         else
            //         {
            //             break;
            //         }
            //     }
            //
            //     if (startingSpace > indent.Value.Length + extraIndent.Length)
            //     {
            //         this.Output.Append(
            //             ' ',
            //             startingSpace - (extraIndent.Length + indent.Value.Length)
            //         );
            //     }
            // }

            this.Output.Append(line.Trim());
            line = stringReader.ReadLine();
            if (line == null)
            {
                return;
            }

            this.Output.Append(this.EndOfLine);
            if (extraIndent != null)
            {
                continue;
            }

            // comparing the amount of whitespace ensures formatting like this is possible
            // /*
            //  *  keeps the * in line
            //  */
            var firstLineIndentLength =
                firstLine!.Replace("\t", "    ").Length
                - firstLine.TrimStart().Replace("\t", "    ").Length;
            var secondLineIndentLength =
                line.Replace("\t", "    ").Length - line.TrimStart().Replace("\t", "    ").Length;
            extraIndent = new string(
                ' ',
                Math.Max(secondLineIndentLength - firstLineIndentLength, 0)
            );
        }
    }

    private void ProcessString(StringDoc stringDoc, Indent indent)
    {
        if (string.IsNullOrEmpty(stringDoc.Value))
        {
            return;
        }

        // this ensures we don't print extra spaces after a trailing comment
        // newLineNextStringValue & skipNextNewLine are set to true when we print a trailing comment
        // when they are set we new line the next string we find. If we new line and then print a " " we end up with an extra space
        if (this.NewLineNextStringValue && this.SkipNextNewLine && stringDoc.Value == " ")
        {
            return;
        }

        if (this.NewLineNextStringValue)
        {
            this.Output.TrimTrailingWhitespace();
            this.Output.Append(this.EndOfLine).Append(indent.Value);
            this.CurrentWidth = indent.Length;
            this.NewLineNextStringValue = false;
        }

        this.Output.Append(stringDoc.Value);
        this.CurrentWidth += stringDoc.Value.GetPrintedWidth();
    }

    private void ProcessLine(LineDoc line, PrintMode mode, Indent indent)
    {
        if (mode is PrintMode.Flat or PrintMode.ForceFlat)
        {
            if (line.Type == LineDoc.LineType.Soft)
            {
                return;
            }

            if (line.Type == LineDoc.LineType.Normal)
            {
                this.Output.Append(' ');
                this.CurrentWidth += 1;
                return;
            }

            // This line was forced into the output even if we were in flattened mode, so we need to tell the next
            // group that no matter what, it needs to remeasure because the previous measurement didn't accurately
            // capture the entire expression (this is necessary for nested groups)
            this.ShouldRemeasure = true;
        }

        if (line.Squash && this.Output.Length > 0 && this.Output.EndsWithNewLineAndWhitespace())
        {
            return;
        }

        if (line.IsLiteral)
        {
            if (this.Output.Length > 0)
            {
                this.Output.Append(this.EndOfLine);
                this.CurrentWidth = 0;
            }
        }
        else
        {
            if (!this.SkipNextNewLine || !this.NewLineNextStringValue)
            {
                this.Output.TrimTrailingWhitespace();
                this.Output.Append(this.EndOfLine).Append(indent.Value);
                this.CurrentWidth = indent.Length;
            }

            if (this.SkipNextNewLine)
            {
                this.SkipNextNewLine = false;
            }
        }
    }

    private void ProcessGroup(Group group, PrintMode mode, Indent indent)
    {
        if (mode is PrintMode.Flat or PrintMode.ForceFlat && !this.ShouldRemeasure)
        {
            this.Push(group.Contents, group.Break ? PrintMode.Break : PrintMode.Flat, indent);
        }
        else
        {
            this.ShouldRemeasure = false;
            var possibleCommand = new PrintCommand(indent, PrintMode.Flat, group.Contents);

            if (!group.Break && this.Fits(possibleCommand))
            {
                this.RemainingCommands.Push(possibleCommand);
            }
            else if (group is ConditionalGroup conditionalGroup)
            {
                if (group.Break)
                {
                    this.Push(conditionalGroup.Options.Last(), PrintMode.Break, indent);
                }
                else
                {
                    var foundSomethingThatFits = false;
                    foreach (var option in conditionalGroup.Options.Skip(1))
                    {
                        possibleCommand = new PrintCommand(indent, mode, option);
                        if (!this.Fits(possibleCommand))
                        {
                            continue;
                        }

                        this.RemainingCommands.Push(possibleCommand);
                        foundSomethingThatFits = true;
                        break;
                    }

                    if (!foundSomethingThatFits)
                    {
                        this.RemainingCommands.Push(possibleCommand);
                    }
                }
            }
            else
            {
                this.Push(group.Contents, PrintMode.Break, indent);
            }
        }

        if (group.GroupId != null)
        {
            this.GroupModeMap[group.GroupId] = this.RemainingCommands.Peek().Mode;
        }
    }

    private bool Fits(PrintCommand possibleCommand)
    {
        return DocFitter.Fits(
            possibleCommand,
            this.RemainingCommands,
            this.PrinterOptions.Width - this.CurrentWidth,
            this.GroupModeMap,
            this.Indenter
        );
    }

    private void Push(Doc doc, PrintMode printMode, Indent indent)
    {
        this.RemainingCommands.Push(new PrintCommand(indent, printMode, doc));
    }
}

internal record PrintCommand(Indent Indent, PrintMode Mode, Doc Doc);

internal enum PrintMode
{
    Flat,
    Break,
    ForceFlat
}
