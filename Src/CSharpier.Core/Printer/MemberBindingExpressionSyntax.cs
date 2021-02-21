using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintMemberBindingExpressionSyntax(MemberBindingExpressionSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.OperatorToken), this.Print(node.Name));
        }
    }
}
