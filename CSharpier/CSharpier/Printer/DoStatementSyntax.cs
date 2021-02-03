using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDoStatementSyntax(DoStatementSyntax node)
        {
            return Concat(
                String(node.DoKeyword.Text),
                this.Print(node.Statement),
                HardLine,
                String(node.WhileKeyword.Text),
                String(" ("),
                this.Print(node.Condition),
                String(");")
            );
        }
    }
}
