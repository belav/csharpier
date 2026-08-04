using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThisExpression
{
    public static Doc Print(ThisExpressionSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
