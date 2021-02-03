using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            return Concat(
                String(node.WhileKeyword.Text),
                String(" ("),
                this.Print(node.Condition),
                String(")"),
                this.Print(node.Statement)
            );
        }
    }
}
