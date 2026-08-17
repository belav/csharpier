using System.Collections.ObjectModel;
using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class Statements
{
    internal static Doc Print(
        ReadOnlyCollection<StatementAst> statements,
        PrintContext context,
        int startOffset,
        int endOffset
    )
    {
        var items = new List<(int Offset, int StartLine, int EndLine, Doc Doc)>();

        foreach (var statement in statements)
        {
            items.Add(
                (
                    statement.Extent.StartOffset,
                    statement.Extent.StartLineNumber,
                    statement.Extent.EndLineNumber,
                    Node.Print(statement, context)
                )
            );
        }

        // Comments are trivia, not attached to any Ast node. Emit the ones that live directly in
        // this block - between or around its statements - alongside the statements. A comment inside
        // a statement travels with that statement, which is emitted verbatim, so skip those here.
        foreach (var comment in context.CommentsBetween(startOffset, endOffset))
        {
            if (IsInsideAny(comment, statements))
            {
                continue;
            }

            items.Add(
                (
                    comment.StartOffset,
                    comment.StartLineNumber,
                    comment.EndLineNumber,
                    Verbatim.Print(comment)
                )
            );
        }

        items.Sort((first, second) => first.Offset.CompareTo(second.Offset));

        var docs = new List<Doc>();
        int? previousEndLine = null;
        foreach (var item in items)
        {
            if (previousEndLine is not null)
            {
                docs.Add(Doc.HardLine);
                if (item.StartLine - previousEndLine > 1)
                {
                    docs.Add(Doc.HardLine);
                }
            }

            docs.Add(item.Doc);
            previousEndLine = item.EndLine;
        }

        return Doc.Concat(docs);
    }

    private static bool IsInsideAny(
        IScriptExtent comment,
        ReadOnlyCollection<StatementAst> statements
    )
    {
        foreach (var statement in statements)
        {
            if (
                comment.StartOffset >= statement.Extent.StartOffset
                && comment.StartOffset < statement.Extent.EndOffset
            )
            {
                return true;
            }
        }

        return false;
    }
}
