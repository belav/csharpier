using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEqualsValueClauseSyntax(EqualsValueClauseSyntax node)
        {
            return Concat(
                // TODO GH-6 this should probably be line, but that breaks a ton of things
                " ",
                this.PrintSyntaxToken(node.EqualsToken, " "),
                this.Print(node.Value));
        }
    }
}
