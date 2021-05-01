using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAssignmentExpressionSyntax(
            AssignmentExpressionSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Left),
                " ",
                Token.Print(node.OperatorToken),
                node.Right is QueryExpressionSyntax ? Doc.Null : " ",
                this.Print(node.Right)
            );
        }
    }
}
