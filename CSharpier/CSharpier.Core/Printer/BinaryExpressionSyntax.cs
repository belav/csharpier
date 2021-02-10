using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBinaryExpressionSyntax(BinaryExpressionSyntax node)
        {
            return this.PrintLeftRightOperator(node, node.Left, node.OperatorToken, node.Right);
        }
    }
}
