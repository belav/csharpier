using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class MakeRefExpression
{
    public static Doc Print(MakeRefExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Expression, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
