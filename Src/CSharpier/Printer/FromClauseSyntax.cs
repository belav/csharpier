using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.FromKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                node.Type != null
                    ? Doc.Concat(this.Print(node.Type), " ")
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
