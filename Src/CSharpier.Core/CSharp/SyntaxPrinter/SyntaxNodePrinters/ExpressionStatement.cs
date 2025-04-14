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
            && semicolonLeadingTrivia is Concat { Contents.Count: > 1 } concat
            && concat.Contents[^1] is HardLine
            && concat.Contents[0] is LeadingComment
        )
        {
            concat.Contents.RemoveAt(concat.Contents.Count - 1);
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
