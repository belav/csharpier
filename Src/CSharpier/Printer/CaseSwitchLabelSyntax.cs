using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCaseSwitchLabelSyntax(CaseSwitchLabelSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.Keyword,
                    afterTokenIfNoTrailing: " "
                ),
                Doc.Group(this.Print(node.Value)),
                Token.Print(node.ColonToken)
            );
        }
    }
}
