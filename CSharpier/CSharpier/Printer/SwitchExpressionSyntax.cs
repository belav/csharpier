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
                        HardLine
                        // Join(
                        //     Concat(String(","), HardLine),
                        //     // TODO this is only for the SwitchExpressionArmNode
                        //     path.map(switchExpressionArmPath => {
                        //         return Concat(
                        //             switchExpressionArmPath.call(print, String("pattern")),
                        //             String(" => "),
                        //             switchExpressionArmPath.call(print, String("expression"))
                        //         );
                        //     }, String("arms"))
                        // )
                    )
                ),
                HardLine,
                String("}")
            );
        }
    }
}