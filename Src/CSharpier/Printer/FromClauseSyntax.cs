using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.FromKeyword, " "),
                node.Type != null ? Concat(this.Print(node.Type), " ") : null,
                this.PrintSyntaxToken(node.Identifier, " "),
                this.PrintSyntaxToken(node.InKeyword, " "),
                this.Print(node.Expression));
        }
    }
}
