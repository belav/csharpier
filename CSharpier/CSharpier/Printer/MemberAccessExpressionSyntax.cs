using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintMemberAccessExpressionSyntax(MemberAccessExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Expression),
                String(node.OperatorToken.Text),
                this.Print(node.Name)
            );
        }
    }
}
