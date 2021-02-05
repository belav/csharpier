using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLabeledStatementSyntax(LabeledStatementSyntax node)
        {
            var parts = new Parts();
            this.PrintAttributeLists(node, node.AttributeLists, parts);
            parts.Push(node.Identifier.Text, String(":"));
            if (node.Statement is BlockSyntax blockSyntax) {
                parts.Add(this.PrintBlockSyntax(blockSyntax));
            } else {
                parts.Push(HardLine, this.Print(node.Statement));
            }
            return Concat(parts);
        }
    }
}
