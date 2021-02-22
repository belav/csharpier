using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCaseSwitchLabelSyntax(CaseSwitchLabelSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.Keyword, " "),
                this.Print(node.Value),
                this.PrintSyntaxToken(node.ColonToken));
        }
    }
}
