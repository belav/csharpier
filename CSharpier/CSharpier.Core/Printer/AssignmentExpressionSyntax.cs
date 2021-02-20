using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAssignmentExpressionSyntax(AssignmentExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Left),
                " ",
                this.PrintSyntaxToken(node.OperatorToken),
                " ",
                this.Print(node.Right));
        }
    }
}
