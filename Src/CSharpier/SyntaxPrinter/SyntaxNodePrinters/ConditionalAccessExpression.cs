using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConditionalAccessExpression
    {
        public static Doc Print(ConditionalAccessExpressionSyntax node)
        {
            return InvocationExpression.Print(node);
        }
    }
}
