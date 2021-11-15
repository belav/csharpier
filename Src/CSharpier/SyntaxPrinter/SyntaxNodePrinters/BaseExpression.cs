using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseExpression
{
    public static Doc Print(BaseExpressionSyntax node)
    {
        return Token.Print(node.Token);
    }
}
