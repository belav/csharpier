using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAssignmentExpressionSyntax(AssignmentExpressionSyntax node)
        {
            return this.PrintLeftRightOperator(node.Left, node.OperatorToken, node.Right);
        }
    }
}
