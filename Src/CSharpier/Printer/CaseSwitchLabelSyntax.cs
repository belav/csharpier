using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCaseSwitchLabelSyntax(CaseSwitchLabelSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.Keyword,
                    afterTokenIfNoTrailing: " "
                ),
                Docs.Group(this.Print(node.Value)),
                SyntaxTokens.Print(node.ColonToken)
            );
        }
    }
}
