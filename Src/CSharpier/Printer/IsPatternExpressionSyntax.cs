using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIsPatternExpressionSyntax(
            IsPatternExpressionSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Expression),
                " ",
                this.PrintSyntaxToken(
                    node.IsKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Doc.Indent(this.Print(node.Pattern))
            );
        }
    }
}
