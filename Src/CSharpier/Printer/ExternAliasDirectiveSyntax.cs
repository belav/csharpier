using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(
            ExternAliasDirectiveSyntax node
        ) {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.ExternKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.AliasKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SyntaxTokens.Print(node.Identifier),
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
