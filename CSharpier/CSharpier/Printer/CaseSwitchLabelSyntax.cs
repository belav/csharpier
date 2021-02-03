using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCaseSwitchLabelSyntax(CaseSwitchLabelSyntax node)
        {
            return Concat(String(node.Keyword.Text), String(" "), this.Print(node.Value), String(":"));
        }
    }
}
