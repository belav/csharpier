using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseExpression
{
    public static Doc Print(BaseExpressionSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.Token, context);
    }
}
