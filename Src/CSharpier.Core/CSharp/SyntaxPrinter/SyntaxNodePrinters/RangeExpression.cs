using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class RangeExpression
{
    public static Doc Print(RangeExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            node.LeftOperand != null ? Node.Print(node.LeftOperand, context) : Doc.Null,
            Token.Print(node.OperatorToken, context),
            node.RightOperand != null ? Node.Print(node.RightOperand, context) : Doc.Null
        );
    }
}
