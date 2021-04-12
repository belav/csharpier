using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAssignmentExpressionSyntax(
            AssignmentExpressionSyntax node
        ) {
            return Concat(
                this.Print(node.Left),
                " ",
                this.PrintSyntaxToken(node.OperatorToken),
                node.Right is QueryExpressionSyntax ? Docs.Null : " ",
                this.Print(node.Right)
            );
        }
    }
}
