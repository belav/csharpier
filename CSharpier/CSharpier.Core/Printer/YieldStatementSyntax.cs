using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintYieldStatementSyntax(YieldStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(" ", this.Print(node.Expression)) : "";
            return Concat(
                node.YieldKeyword.Text,
                " ",
                node.ReturnOrBreakKeyword.Text,
                expression,
                ";"
            );
        }
    }
}
