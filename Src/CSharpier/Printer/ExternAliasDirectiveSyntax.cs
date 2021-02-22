using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(
            ExternAliasDirectiveSyntax node)
        {
            return Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.ExternKeyword, " "),
                this.PrintSyntaxToken(node.AliasKeyword, " "),
                this.PrintSyntaxToken(node.Identifier),
                this.PrintSyntaxToken(node.SemicolonToken));
        }
    }
}
