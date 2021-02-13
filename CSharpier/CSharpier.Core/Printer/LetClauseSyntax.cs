using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintLetClauseSyntax(LetClauseSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.LetKeyword, " "),
                this.PrintSyntaxToken(node.Identifier, " "),
                this.PrintSyntaxToken(node.EqualsToken, " "),
                this.Print(node.Expression));
        }
    }
}
