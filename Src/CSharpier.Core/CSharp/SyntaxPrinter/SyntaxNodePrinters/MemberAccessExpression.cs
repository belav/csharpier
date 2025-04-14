using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberAccessExpression
{
    public static Doc Print(MemberAccessExpressionSyntax node, PrintingContext context)
    {
        return InvocationExpression.PrintMemberChain(node, context);
    }
}
