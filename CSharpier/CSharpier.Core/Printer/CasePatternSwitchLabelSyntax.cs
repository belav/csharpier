using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCasePatternSwitchLabelSyntax(CasePatternSwitchLabelSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintSyntaxToken(node.Keyword, " "),
                this.Print(node.Pattern));
            if (node.WhenClause != null)
            {
                parts.Push(" ", this.Print(node.WhenClause));
            }
            parts.Push(this.PrintSyntaxToken(node.ColonToken));
            return Concat(parts);
        }
    }
}
