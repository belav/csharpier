using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThrowStatementSyntax(ThrowStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(String(" "), this.Print(node.Expression)) : "";
            // TODO this was used by both throws before, so this is maybe doesn't make sense
            return Concat(node.ThrowKeyword.Text, expression, node is ThrowStatementSyntax ? String(";") : "");
        }
    }
}
