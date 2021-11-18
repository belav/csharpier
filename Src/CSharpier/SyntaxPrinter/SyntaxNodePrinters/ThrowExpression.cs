using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ThrowExpression
{
    public static Doc Print(ThrowExpressionSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.ThrowKeyword, " "),
            Node.Print(node.Expression)
        );
    }
}
