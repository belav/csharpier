using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitStackAllocArrayCreationExpressionSyntax(
            ImplicitStackAllocArrayCreationExpressionSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.StackAllocKeyword),
                this.PrintSyntaxToken(node.OpenBracketToken),
                this.PrintSyntaxToken(
                    node.CloseBracketToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Initializer)
            );
        }
    }
}
