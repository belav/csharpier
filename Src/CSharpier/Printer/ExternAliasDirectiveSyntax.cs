using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(
            ExternAliasDirectiveSyntax node
        ) {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.ExternKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.AliasKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                Token.Print(node.Identifier),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
