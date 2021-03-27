using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLabeledStatementSyntax(LabeledStatementSyntax node)
        {
            var parts = new Parts(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintSyntaxToken(node.Identifier),
                this.PrintSyntaxToken(node.ColonToken)
            );
            if (node.Statement is BlockSyntax blockSyntax)
            {
                parts.Push(this.PrintBlockSyntax(blockSyntax));
            }
            else
            {
                parts.Push(HardLine, this.Print(node.Statement));
            }
            return Concat(parts);
        }
    }
}
