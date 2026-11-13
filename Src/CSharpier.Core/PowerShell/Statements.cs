using System.Collections.ObjectModel;
using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

// Collects docs that each go on their own line. At most one blank line from the source is kept
// between two of them.
internal sealed class LineList
{
    private readonly List<Doc> docs = [];
    private int? previousEndLine;

    internal bool IsEmpty => this.docs.Count == 0;

    internal void Add(Doc doc, int startLine, int endLine, bool forceBlankLine = false)
    {
        if (this.previousEndLine is not null)
        {
            this.docs.Add(Doc.HardLine);
            if (forceBlankLine || startLine - this.previousEndLine > 1)
            {
                this.docs.Add(Doc.HardLine);
            }
        }

        this.docs.Add(doc);
        this.previousEndLine = endLine;
    }

    internal void AddComments(IEnumerable<IScriptExtent> comments, PrintContext context)
    {
        foreach (var comment in comments)
        {
            this.Add(context.PrintComment(comment), comment.StartLineNumber, comment.EndLineNumber);
        }
    }

    // For comments that moved here from another place, where the blank lines of the source say
    // nothing about the lines around them.
    internal void AddCommentsWithoutBlankLines(
        IEnumerable<IScriptExtent> comments,
        PrintContext context
    )
    {
        foreach (var comment in comments)
        {
            this.Add(context.PrintComment(comment), 0, int.MaxValue);
        }
    }

    // Adds a node that Node.Print does not wrap with its comments.
    internal void AddWithComments(
        Ast node,
        Doc doc,
        PrintContext context,
        bool forceBlankLine = false
    )
    {
        this.Add(
            Doc.Concat(
                context.PrintLeadingComments(node),
                doc,
                context.PrintTrailingComments(node)
            ),
            context.GetStartLine(node),
            context.GetEndLine(node),
            forceBlankLine
        );
    }

    internal Doc ToDoc()
    {
        return this.docs.Count == 0 ? Doc.Null : Doc.Concat(this.docs);
    }
}

internal static class Statements
{
    // Prints the statements and traps of a block one per line, followed by the dangling comments of
    // the node that owns the block. The comments in firstComments go before the first statement.
    internal static Doc Print(
        ReadOnlyCollection<StatementAst> statements,
        ReadOnlyCollection<TrapStatementAst>? traps,
        PrintContext context,
        Ast owner,
        IEnumerable<IScriptExtent>? firstComments = null
    )
    {
        var lines = new LineList();
        if (firstComments is not null)
        {
            lines.AddCommentsWithoutBlankLines(firstComments, context);
        }

        IEnumerable<StatementAst> items = statements;
        if (traps is { Count: > 0 })
        {
            items = statements.Concat(traps).OrderBy(o => o.Extent.StartOffset);
        }

        foreach (var statement in items)
        {
            if (IsBaseConstructorCall(statement))
            {
                continue;
            }

            lines.Add(
                Node.Print(statement, context),
                context.GetStartLine(statement),
                context.GetEndLine(statement)
            );
        }

        lines.AddComments(context.GetDanglingComments(owner), context);

        return lines.ToDoc();
    }

    internal static Doc PrintLabel(LabeledStatementAst node)
    {
        return node.Label is null ? Doc.Null : ":" + node.Label + " ";
    }

    // A constructor's call to its base constructor is the first statement of the body in the
    // tree, but it is written in front of the body.
    internal static bool IsBaseConstructorCall(StatementAst statement)
    {
        return statement is CommandExpressionAst { Expression: BaseCtorInvokeMemberExpressionAst };
    }
}
