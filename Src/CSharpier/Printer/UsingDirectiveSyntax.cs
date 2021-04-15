using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(
                    node.UsingKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.StaticKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                node.Alias == null
                    ? Doc.Null
                    : this.PrintNameEqualsSyntax(node.Alias),
                this.Print(node.Name),
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
