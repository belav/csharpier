using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintCasePatternSwitchLabelSyntax(CasePatternSwitchLabelSyntax node)
        {
            var parts = new Parts();
            parts.Push(node.Keyword.Text, " ", this.Print(node.Pattern));
            if (node.WhenClause != null)
            {
                parts.Push(" ", this.Print(node.WhenClause));
            }
            parts.Push(":");
            return Concat(parts);
        }
    }
}
