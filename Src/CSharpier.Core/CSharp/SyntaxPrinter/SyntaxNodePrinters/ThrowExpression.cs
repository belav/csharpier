using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThrowExpression
{
    public static Doc Print(ThrowExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.ThrowKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
