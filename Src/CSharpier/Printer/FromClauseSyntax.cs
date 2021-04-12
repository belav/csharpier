using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.FromKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                node.Type != null
                    ? Docs.Concat(this.Print(node.Type), " ")
                    : Doc.Null,
                this.PrintSyntaxToken(
                    node.Identifier,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.InKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Expression)
            );
        }
    }
}
