using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRangeExpressionSyntax(RangeExpressionSyntax node)
        {
            return Doc.Concat(
                node.LeftOperand != null
                    ? this.Print(node.LeftOperand)
                    : Doc.Null,
                Token.Print(node.OperatorToken),
                node.RightOperand != null
                    ? this.Print(node.RightOperand)
                    : Doc.Null
            );
        }
    }
}
