using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintRangeExpressionSyntax(RangeExpressionSyntax node)
        {
            return Docs.Concat(
                node.LeftOperand != null
                    ? this.Print(node.LeftOperand)
                    : Doc.Null,
                SyntaxTokens.Print(node.OperatorToken),
                node.RightOperand != null
                    ? this.Print(node.RightOperand)
                    : Doc.Null
            );
        }
    }
}
