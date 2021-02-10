using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintStackAllocArrayCreationExpressionSyntax(StackAllocArrayCreationExpressionSyntax node)
        {
            return Concat(
                "stackalloc ",
                this.Print(node.Type),
                node.Initializer != null
                    ? Concat(" ", this.PrintInitializerExpressionSyntax(node.Initializer))
                    : ""
            );
        }
    }
}
