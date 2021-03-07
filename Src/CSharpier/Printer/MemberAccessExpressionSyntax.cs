using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintMemberAccessExpressionSyntax(
            MemberAccessExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Expression),
                SoftLine,
                this.PrintSyntaxToken(node.OperatorToken),
                this.Print(node.Name));
        }
    }
}
