using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintMemberAccessExpressionSyntax(MemberAccessExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Expression),
                node.OperatorToken.Text,
                this.Print(node.Name)
            );
        }
    }
}
