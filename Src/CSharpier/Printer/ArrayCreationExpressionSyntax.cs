using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArrayCreationExpressionSyntax(
            ArrayCreationExpressionSyntax node
        ) {
            return Doc.Group(
                this.PrintSyntaxToken(
                    node.NewKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Type),
                node.Initializer != null
                    ? Doc.Concat(Doc.Line, this.Print(node.Initializer))
                    : Doc.Null
            );
        }
    }
}
