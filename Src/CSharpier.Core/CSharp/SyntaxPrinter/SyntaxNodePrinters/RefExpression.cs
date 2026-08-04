using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefExpression
{
    public static Doc Print(RefExpressionSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.RefKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
