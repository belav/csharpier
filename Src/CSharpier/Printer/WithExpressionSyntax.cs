using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWithExpressionSyntax(WithExpressionSyntax node)
        {
            return Docs.Concat(
                this.Print(node.Expression),
                " ",
                this.PrintSyntaxToken(
                    node.WithKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Initializer)
            );
        }
    }
}
