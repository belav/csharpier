using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BaseExpression
    {
        public static Doc Print(BaseExpressionSyntax node)
        {
            return Token.Print(node.Token);
        }
    }
}
