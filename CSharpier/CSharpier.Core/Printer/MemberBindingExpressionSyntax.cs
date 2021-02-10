using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintMemberBindingExpressionSyntax(MemberBindingExpressionSyntax node)
        {
            return Concat(node.OperatorToken.Text, this.Print(node.Name));
        }
    }
}
