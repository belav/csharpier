using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintStackAllocArrayCreationExpressionSyntax(
            StackAllocArrayCreationExpressionSyntax node
        ) {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.StackAllocKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Type),
                node.Initializer != null
                    ? Doc.Concat(
                            " ",
                            this.PrintInitializerExpressionSyntax(
                                node.Initializer
                            )
                        )
                    : string.Empty
            );
        }
    }
}
