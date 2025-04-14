using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConditionalAccessExpression
{
    public static Doc Print(ConditionalAccessExpressionSyntax node, PrintingContext context)
    {
        return InvocationExpression.PrintMemberChain(node, context);
    }
}
