using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIfStatementSyntax(IfStatementSyntax node)
        {
            var parts = new Parts(node.IfKeyword.Text, " ", "(", this.Print(node.Condition), ")");
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Push(statement);
            }
            else
            {
                parts.Push(Indent(Concat(HardLine, statement)));
            }

            if (NotNull(node.Else))
            {
                parts.Push(HardLine, this.Print(node.Else));
            }

            return Concat(parts);
        }
    }
}