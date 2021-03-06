using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RangeExpression
    {
        public static Doc Print(RangeExpressionSyntax node)
        {
            return Doc.Concat(
                node.LeftOperand != null ? Node.Print(node.LeftOperand) : Doc.Null,
                Token.Print(node.OperatorToken),
                node.RightOperand != null ? Node.Print(node.RightOperand) : Doc.Null
            );
        }
    }
}
