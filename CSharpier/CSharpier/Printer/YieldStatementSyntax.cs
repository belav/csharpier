using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintYieldStatementSyntax(YieldStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(String(" "), this.Print(node.Expression)) : "";
            return Concat(
                node.YieldKeyword.Text,
                String(" "),
                node.ReturnOrBreakKeyword.Text,
                expression,
                String(";")
            );
        }
    }
}
