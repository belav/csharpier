using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintMemberBindingExpressionSyntax(
            MemberBindingExpressionSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.OperatorToken),
                this.Print(node.Name)
            );
        }
    }
}
