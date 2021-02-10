using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintRangeExpressionSyntax(RangeExpressionSyntax node)
        {
            return Concat(
                node.LeftOperand != null ? this.Print(node.LeftOperand) : "",
                node.OperatorToken.Text,
                node.RightOperand != null ? this.Print(node.RightOperand) : ""
            );
        }
    }
}
