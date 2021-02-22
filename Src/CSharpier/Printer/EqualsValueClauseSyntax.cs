using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEqualsValueClauseSyntax(EqualsValueClauseSyntax node)
        {
            return Concat(
                " ",
                this.PrintSyntaxToken(node.EqualsToken, " "),
                this.Print(node.Value));
        }
    }
}
