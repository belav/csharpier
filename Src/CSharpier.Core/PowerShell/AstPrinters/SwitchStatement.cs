using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class SwitchStatement
{
    internal static Doc Print(SwitchStatementAst node, PrintContext context)
    {
        var docs = new List<Doc> { Statements.PrintLabel(node), "switch " };

        // The flags are written the way the source has them, in the same order.
        var header = context.GetTokens(node.Extent.StartOffset, node.Condition.Extent.StartOffset);
        var isParenthesized = header is [.., { Kind: TokenKind.LParen }];
        foreach (var token in header)
        {
            if (token.Kind == TokenKind.Parameter)
            {
                docs.Add(token.Text);
                docs.Add(" ");
            }
        }

        docs.Add(
            isParenthesized
                ? Condition.Print(node.Condition, context)
                : Node.Print(node.Condition, context)
        );
        docs.Add(" {");

        var clauses = new List<(Ast First, StatementBlockAst Body, Doc Pattern)>();
        foreach (var (pattern, body) in node.Clauses)
        {
            clauses.Add((pattern, body, Node.Print(pattern, context)));
        }

        if (node.Default is not null)
        {
            clauses.Add((node.Default, node.Default, "default"));
        }

        clauses.Sort(
            (first, second) =>
                first.Body.Extent.StartOffset.CompareTo(second.Body.Extent.StartOffset)
        );

        var lines = new LineList();
        foreach (var (first, body, pattern) in clauses)
        {
            lines.Add(
                Doc.Concat(
                    context.PrintLeadingComments(first),
                    pattern,
                    " ",
                    StatementBlock.PrintCompact(body, context, first == body ? null : first),
                    context.PrintTrailingComments(body)
                ),
                context.GetStartLine(first),
                context.GetEndLine(body)
            );
        }

        lines.AddComments(context.GetDanglingComments(node), context);

        if (lines.IsEmpty)
        {
            docs.Add(" }");
        }
        else
        {
            docs.Add(Doc.Indent(Doc.HardLine, lines.ToDoc()));
            docs.Add(Doc.HardLine);
            docs.Add("}");
        }

        return Doc.Concat(docs);
    }
}
