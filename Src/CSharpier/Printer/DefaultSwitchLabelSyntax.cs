using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDefaultSwitchLabelSyntax(DefaultSwitchLabelSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.Keyword),
                this.PrintSyntaxToken(node.ColonToken));
        }
    }
}
