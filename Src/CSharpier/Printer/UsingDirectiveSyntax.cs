using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintSyntaxToken(
                    node.UsingKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.StaticKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias),
                this.Print(node.Name),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
