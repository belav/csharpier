using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintWhileStatementSyntax(WhileStatementSyntax node)
        {
            return Concat(
                node.WhileKeyword.Text,
                " (",
                this.Print(node.Condition),
                ")",
                this.Print(node.Statement)
            );
        }
    }
}
