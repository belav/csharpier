using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWithExpressionSyntax(WithExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Expression),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(
                    node.WithKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Initializer)
            );
        }
    }
}
