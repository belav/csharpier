using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConditionalAccessExpression
{
    public static Doc Print(ConditionalAccessExpressionSyntax node)
    {
        return InvocationExpression.PrintMemberChain(node);
    }
}
