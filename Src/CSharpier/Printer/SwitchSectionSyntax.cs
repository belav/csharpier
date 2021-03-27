using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSwitchSectionSyntax(SwitchSectionSyntax node)
        {
            var parts = new Parts(
                Join(HardLine, node.Labels.Select(this.Print))
            );
            if (
                node.Statements.Count == 1
                && node.Statements[0] is BlockSyntax blockSyntax
            )
            {
                parts.Push(this.PrintBlockSyntax(blockSyntax));
            }
            else
            {
                parts.Push(
                    Indent(
                        Concat(
                            HardLine,
                            Join(
                                HardLine,
                                node.Statements.Select(this.Print).ToArray()
                            )
                        )
                    )
                );
            }
            return Concat(parts);
        }
    }
}
