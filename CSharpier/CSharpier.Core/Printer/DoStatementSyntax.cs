using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintDoStatementSyntax(DoStatementSyntax node)
        {
            return Concat(
                node.DoKeyword.Text,
                this.Print(node.Statement),
                HardLine,
                node.WhileKeyword.Text,
                " (",
                this.Print(node.Condition),
                ");"
            );
        }
    }
}
