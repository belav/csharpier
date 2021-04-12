using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhenClauseSyntax(WhenClauseSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.WhenKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Condition)
            );
        }
    }
}
