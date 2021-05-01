using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ThisExpression
    {
        public static Doc Print(ThisExpressionSyntax node)
        {
            return Token.Print(node.Token);
        }
    }
}
