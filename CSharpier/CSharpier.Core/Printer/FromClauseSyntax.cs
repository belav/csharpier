using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFromClauseSyntax(FromClauseSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.FromKeyword, " "),
                this.PrintSyntaxToken(node.Identifier, " "),
                this.PrintSyntaxToken(node.InKeyword, " "),
                this.Print(node.Expression));
        }
    }
}
