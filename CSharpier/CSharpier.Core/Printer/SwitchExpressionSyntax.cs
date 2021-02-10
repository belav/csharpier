using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSwitchExpressionSyntax(SwitchExpressionSyntax node)
        {
            return Concat(
                this.Print(node.GoverningExpression),
                " switch",
                HardLine,
                "{",
                Indent(
                    Concat(
                        HardLine,
                        Join(
                            Concat(",", HardLine),
                            node.Arms.Select(o => Concat(this.Print(o.Pattern),
                                " => ",
                                this.Print(o.Expression)))
                        )
                    )
                ),
                HardLine,
                "}"
            );
        }
    }
}