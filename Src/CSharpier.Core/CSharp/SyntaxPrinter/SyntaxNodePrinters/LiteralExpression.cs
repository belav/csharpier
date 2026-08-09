using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LiteralExpression
{
    public static Doc Print(LiteralExpressionSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
