using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCasePatternSwitchLabelSyntax(CasePatternSwitchLabelSyntax node)
        {
            return Concat(
                String(node.Keyword.Text),
                String(" "),
                this.Print(node.Pattern),
                String(" "),
                this.Print(node.WhenClause),
                String(":")
            );
        }
    }
}
