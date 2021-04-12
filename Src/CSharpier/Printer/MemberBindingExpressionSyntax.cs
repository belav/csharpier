using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintMemberBindingExpressionSyntax(
            MemberBindingExpressionSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.OperatorToken),
                this.Print(node.Name)
            );
        }
    }
}
