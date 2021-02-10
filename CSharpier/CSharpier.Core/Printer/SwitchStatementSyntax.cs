using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSwitchStatementSyntax(SwitchStatementSyntax node)
        {
            var sections =
                node.Sections.Count == 0
                    ? " { }"
                    : Concat(
                          HardLine,
                          "{",
                          Indent(Concat(HardLine, Join(HardLine, node.Sections.Select(this.Print)))),
                          HardLine,
                          "}"
                      );
            return Concat(node.SwitchKeyword.Text, " (", this.Print(node.Expression), ")", sections);
        }
    }
}
