using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchExpressionSyntax(SwitchExpressionSyntax node)
        {
            return Concat(
                this.Print(node.GoverningExpression),
                String(" switch"),
                HardLine,
                String("{"),
                Indent(
                    Concat(
                        HardLine,
                        Join(
                            Concat(String(","), HardLine),
                            node.Arms.Select(o => Concat(this.Print(o.Pattern),
                                " => ",
                                this.Print(o.Expression)))
                        )
                    )
                ),
                HardLine,
                String("}")
            );
        }
    }
}