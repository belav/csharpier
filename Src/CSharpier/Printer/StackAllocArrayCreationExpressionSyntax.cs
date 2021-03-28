using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintStackAllocArrayCreationExpressionSyntax(
            StackAllocArrayCreationExpressionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.StackAllocKeyword, " "),
                this.Print(node.Type),
                node.Initializer != null
                    ? Concat(
                        " ",
                        this.PrintInitializerExpressionSyntax(node.Initializer)
                    )
                    : string.Empty
            );
        }
    }
}
