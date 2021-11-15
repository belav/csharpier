using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LiteralExpression
{
    public static Doc Print(LiteralExpressionSyntax node)
    {
        return Token.Print(node.Token);
    }
}
