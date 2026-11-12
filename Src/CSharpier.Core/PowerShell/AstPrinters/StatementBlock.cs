using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class StatementBlock
{
    internal static Doc Print(StatementBlockAst node, PrintContext context)
    {
        return Print(node, context, null);
    }

    // Comments between the header of a statement and the opening brace move into the block.
    internal static Doc Print(StatementBlockAst node, PrintContext context, Ast? header)
    {
        var firstComments = new List<IScriptExtent>();
        if (header is not null)
        {
            firstComments.AddRange(
                context.GetUnprintedComments(context.GetTrailingComments(header))
            );
        }

        firstComments.AddRange(context.GetUnprintedComments(context.GetLeadingComments(node)));

        var statements = Statements.Print(
            node.Statements,
            node.Traps,
            context,
            node,
            firstComments
        );
        if (statements == Doc.Null)
        {
            return "{ }";
        }

        return Doc.Concat("{", Doc.Indent(Doc.HardLine, statements), Doc.HardLine, "}");
    }

    // A block that was on one line with a single statement stays on one line when it fits.
    internal static Doc PrintCompact(StatementBlockAst node, PrintContext context, Ast? header)
    {
        if (
            Layout.IsMultiLine(node)
            || (header is not null && context.GetTrailingComments(header).Count > 0)
            || node.Statements.Count != 1
            || node.Traps is { Count: > 0 }
            || context.HasCommentIn(node.Extent)
            || context.GetUnprintedComments(context.GetLeadingComments(node)).Any()
        )
        {
            return Print(node, context, header);
        }

        return Doc.Group(
            "{",
            Doc.Indent(Doc.Line, Node.Print(node.Statements[0], context)),
            Doc.Line,
            "}"
        );
    }

    // Prints the keyword that follows the closing brace of a block, such as else or catch. Comments
    // between the brace and the keyword stay on their own lines in front of the keyword.
    internal static Doc PrintKeywordAfter(
        Ast previousBlock,
        Ast next,
        string keyword,
        PrintContext context
    )
    {
        var docs = new List<Doc> { context.PrintTrailingComments(previousBlock) };
        foreach (var comment in context.GetUnprintedComments(context.GetLeadingComments(next)))
        {
            docs.Add(Doc.HardLine);
            docs.Add(context.PrintComment(comment));
        }

        if (docs.Count == 1 && docs[0] == Doc.Null)
        {
            return " " + keyword;
        }

        docs.Add(Doc.HardLine);
        docs.Add(keyword);
        return Doc.Concat(docs);
    }

    internal static Doc PrintInParens(
        string open,
        Ast owner,
        StatementBlockAst node,
        PrintContext context
    )
    {
        if (node.Traps is { Count: > 0 })
        {
            return Verbatim.Print(owner.Extent, context);
        }

        var forceBreak =
            node.Statements.Count > 1
            || context.GetDanglingComments(node).Count > 0
            || (
                node.Statements.Count == 1
                && context.GetStartLine(node.Statements[0]) != owner.Extent.StartLineNumber
            );

        var statements = Statements.Print(node.Statements, null, context, node);
        if (statements == Doc.Null)
        {
            return open + ")";
        }

        return forceBreak
            ? Doc.Concat(open, Doc.Indent(Doc.HardLine, statements), Doc.HardLine, ")")
            : Doc.Group(open, Doc.Indent(Doc.SoftLine, statements), Doc.SoftLine, ")");
    }
}
