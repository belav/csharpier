using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCaseSwitchLabelSyntax(CaseSwitchLabelSyntax node)
        {
            return Concat(node.Keyword.Text, " ", this.Print(node.Value), ":");
        }
    }
}
