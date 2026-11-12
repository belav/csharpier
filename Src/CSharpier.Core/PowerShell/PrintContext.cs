using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell;

internal sealed class PrintContext
{
    private readonly IReadOnlyList<IScriptExtent> comments;
    private readonly bool[] printed;
    private readonly AttachedComments attachedComments;
    private readonly string source;

    internal PrintContext(ScriptBlockAst root, IReadOnlyList<IScriptExtent> comments, string source)
    {
        this.source = source;
        this.comments = comments;
        this.printed = new bool[comments.Count];
        this.attachedComments = CommentAttacher.Attach(root, comments);
    }

    internal string GetText(int startOffset, int endOffset)
    {
        return this.source[startOffset..endOffset];
    }

    internal bool HasCommentIn(IScriptExtent extent)
    {
        var index = this.FindFirstCommentAtOrAfter(extent.StartOffset);
        return index < this.comments.Count && this.comments[index].StartOffset < extent.EndOffset;
    }

    internal bool HasUnprintedCommentIn(IScriptExtent extent)
    {
        for (
            var index = this.FindFirstCommentAtOrAfter(extent.StartOffset);
            index < this.comments.Count && this.comments[index].StartOffset < extent.EndOffset;
            index++
        )
        {
            if (!this.printed[index])
            {
                return true;
            }
        }

        return false;
    }

    internal void MarkCommentsPrintedIn(IScriptExtent extent)
    {
        this.MarkCommentsPrintedIn(extent.StartOffset, extent.EndOffset);
    }

    internal void MarkCommentsPrintedIn(int startOffset, int endOffset)
    {
        for (
            var index = this.FindFirstCommentAtOrAfter(startOffset);
            index < this.comments.Count && this.comments[index].StartOffset < endOffset;
            index++
        )
        {
            this.printed[index] = true;
        }
    }

    internal IReadOnlyList<IScriptExtent> GetLeadingComments(Ast node)
    {
        return this.attachedComments.Leading.TryGetValue(node, out var list) ? list : [];
    }

    internal IReadOnlyList<IScriptExtent> GetTrailingComments(Ast node)
    {
        return this.attachedComments.Trailing.TryGetValue(node, out var list) ? list : [];
    }

    internal IReadOnlyList<IScriptExtent> GetDanglingComments(Ast node)
    {
        return this.attachedComments.Dangling.TryGetValue(node, out var list) ? list : [];
    }

    // The line a node starts on once its leading comments are counted as part of it.
    internal int GetStartLine(Ast node)
    {
        var leading = this.GetLeadingComments(node);
        return leading.Count > 0 ? leading[0].StartLineNumber : GetOwnStartLine(node);
    }

    // The attributes of a param block, such as [CmdletBinding()], sit in front of its extent.
    internal static int GetOwnStartOffset(Ast node)
    {
        var startOffset = node.Extent.StartOffset;
        if (node is ParamBlockAst paramBlock)
        {
            foreach (var attribute in paramBlock.Attributes)
            {
                startOffset = Math.Min(startOffset, attribute.Extent.StartOffset);
            }
        }

        return startOffset;
    }

    internal static int GetOwnStartLine(Ast node)
    {
        var startLine = node.Extent.StartLineNumber;
        if (node is ParamBlockAst paramBlock)
        {
            foreach (var attribute in paramBlock.Attributes)
            {
                startLine = Math.Min(startLine, attribute.Extent.StartLineNumber);
            }
        }

        return startLine;
    }

    internal IEnumerable<IScriptExtent> GetUnprintedComments(IReadOnlyList<IScriptExtent> list)
    {
        foreach (var comment in list)
        {
            if (!this.IsPrinted(comment))
            {
                yield return comment;
            }
        }
    }

    internal int GetEndLine(Ast node)
    {
        var trailing = this.GetTrailingComments(node);
        return trailing.Count > 0 ? trailing[^1].EndLineNumber : node.Extent.EndLineNumber;
    }

    // Node.Print calls this for every node. Comments are only printed here for nodes that sit on
    // their own lines, where a line comment cannot swallow the code that follows it. For any other
    // node the comments stay unprinted unless the parent's printer prints them itself with
    // PrintLeadingComments/PrintTrailingComments, and an unprinted comment makes the nearest
    // ancestor print verbatim.
    internal Doc WrapWithComments(Ast node, Doc doc)
    {
        if (!CanPrintCommentsAround(node))
        {
            return doc;
        }

        var leading = this.PrintLeadingComments(node);
        var trailing = this.PrintTrailingComments(node);
        if (leading == Doc.Null && trailing == Doc.Null)
        {
            return doc;
        }

        return Doc.Concat(leading, doc, trailing);
    }

    private static bool CanPrintCommentsAround(Ast node)
    {
        return node is ParamBlockAst
            || (node is StatementAst && node.Parent is NamedBlockAst or StatementBlockAst);
    }

    // Prints the leading comments of a node, each followed by a line break when the source had one
    // and by a space otherwise.
    internal Doc PrintLeadingComments(Ast node)
    {
        var leading = this.GetLeadingComments(node);
        if (leading.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        for (var index = 0; index < leading.Count; index++)
        {
            var comment = leading[index];
            if (this.IsPrinted(comment))
            {
                continue;
            }

            var nextStartLine =
                index + 1 < leading.Count
                    ? leading[index + 1].StartLineNumber
                    : GetOwnStartLine(node);

            docs.Add(this.PrintComment(comment));
            if (comment.EndLineNumber == nextStartLine)
            {
                docs.Add(" ");
            }
            else
            {
                docs.Add(Doc.HardLine);
                if (nextStartLine - comment.EndLineNumber > 1)
                {
                    docs.Add(Doc.HardLine);
                }
            }
        }

        return docs.Count == 0 ? Doc.Null : Doc.Concat(docs);
    }

    // Prints the trailing comments of a node. A line comment forces a line break after it.
    internal Doc PrintTrailingComments(Ast node)
    {
        var trailing = this.GetTrailingComments(node);
        if (trailing.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        foreach (var comment in trailing)
        {
            if (this.IsPrinted(comment))
            {
                continue;
            }

            if (IsLineComment(comment))
            {
                this.MarkPrinted(comment);
                docs.Add(Doc.TrailingComment(comment.Text, CommentType.SingleLine));
                docs.Add(Doc.BreakParent);
            }
            else
            {
                docs.Add(" ");
                docs.Add(this.PrintComment(comment));
            }
        }

        return docs.Count == 0 ? Doc.Null : Doc.Concat(docs);
    }

    // Prints one comment and records that it was printed. A block comment that starts its line
    // moves as a whole: the lines after the first that share the first line's indentation are
    // printed at the new indentation, and any other line is kept exactly as written.
    internal Doc PrintComment(IScriptExtent comment)
    {
        if (IsLineComment(comment) || comment.StartLineNumber == comment.EndLineNumber)
        {
            return Verbatim.Print(comment, this);
        }

        var lineStart = comment.StartOffset;
        while (lineStart > 0 && this.source[lineStart - 1] is not ('\n' or '\r'))
        {
            lineStart--;
        }

        var prefix = this.source[lineStart..comment.StartOffset];
        if (prefix.Any(o => o is not (' ' or '\t')))
        {
            return Verbatim.Print(comment, this);
        }

        this.MarkPrinted(comment);

        var lines = comment.Text.Replace("\r\n", "\n").Split('\n');
        var docs = new List<Doc> { lines[0] };
        for (var index = 1; index < lines.Length; index++)
        {
            var line = lines[index];
            if (line.Length > prefix.Length && line.StartsWith(prefix, StringComparison.Ordinal))
            {
                docs.Add(Doc.HardLine);
                docs.Add(line[prefix.Length..]);
            }
            else
            {
                docs.Add(Doc.LiteralLine);
                docs.Add(line == prefix ? string.Empty : line);
            }
        }

        return Doc.Concat(docs);
    }

    // True when the leading comments of a node are block comments on the line the node starts on,
    // so they can stay in front of it without a line break.
    internal bool HasOnlyInlineLeadingComments(Ast node)
    {
        foreach (var comment in this.GetLeadingComments(node))
        {
            if (
                IsLineComment(comment)
                || comment.StartLineNumber != node.Extent.StartLineNumber
                || comment.EndLineNumber != node.Extent.StartLineNumber
            )
            {
                return false;
            }
        }

        return true;
    }

    internal static bool IsLineComment(IScriptExtent comment)
    {
        return !comment.Text.StartsWith("<#", StringComparison.Ordinal);
    }

    private bool IsPrinted(IScriptExtent comment)
    {
        var index = this.FindFirstCommentAtOrAfter(comment.StartOffset);
        return index < this.comments.Count && this.printed[index];
    }

    private void MarkPrinted(IScriptExtent comment)
    {
        var index = this.FindFirstCommentAtOrAfter(comment.StartOffset);
        if (index < this.comments.Count)
        {
            this.printed[index] = true;
        }
    }

    private int FindFirstCommentAtOrAfter(int offset)
    {
        var low = 0;
        var high = this.comments.Count;
        while (low < high)
        {
            var middle = (low + high) / 2;
            if (this.comments[middle].StartOffset < offset)
            {
                low = middle + 1;
            }
            else
            {
                high = middle;
            }
        }

        return low;
    }
}
