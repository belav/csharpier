using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExpressionStatement
{
    public static Doc Print(ExpressionStatementSyntax node, PrintingContext context)
    {
        var semicolonLeadingTrivia = Token.PrintLeadingTrivia(node.SemicolonToken, context);
        if (
            node.Expression is InvocationExpressionSyntax
            && semicolonLeadingTrivia is Concat { Count: > 1 } concat
            && concat[^1] is HardLine
            && concat[0] is LeadingComment
        )
        {
            concat[^1] = Doc.Null;
            semicolonLeadingTrivia = Doc.Concat(Doc.Indent(semicolonLeadingTrivia), Doc.HardLine);
        }

        return Doc.Group(
            ExtraNewLines.Print(node),
            Node.Print(node.Expression, context),
            semicolonLeadingTrivia,
            Token.PrintWithoutLeadingTrivia(node.SemicolonToken, context)
        );
    }
}
