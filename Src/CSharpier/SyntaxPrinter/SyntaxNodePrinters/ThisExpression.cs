using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ThisExpression
    {
        public static Doc Print(ThisExpressionSyntax node)
        {
            return Token.Print(node.Token);
        }
    }
}
