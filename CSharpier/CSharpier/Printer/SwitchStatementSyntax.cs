using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchStatementSyntax(SwitchStatementSyntax node)
        {
            var sections =
                node.Sections.Count == 0
                    ? String(" { }")
                    : Concat(
                          HardLine,
                          String("{"),
                          Indent(Concat(HardLine, Join(HardLine, node.Sections.Select(this.Print)))),
                          HardLine,
                          String("}")
                      );
            return Concat(node.SwitchKeyword.Text, String(" ("), this.Print(node.Expression), String(")"), sections);
        }
    }
}
