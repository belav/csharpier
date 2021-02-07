using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCasePatternSwitchLabelSyntax(CasePatternSwitchLabelSyntax node)
        {
            return Concat(
                "case",
                " ",
                this.Print(node.Pattern),
                " ",
                this.Print(node.WhenClause),
                ":"
            );
        }
    }
}
