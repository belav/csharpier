using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LiteralExpression
    {
        public static Doc Print(LiteralExpressionSyntax node)
        {
            return Token.Print(node.Token);
        }
    }
}
