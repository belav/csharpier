using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintThrowStatementSyntax(ThrowStatementSyntax node)
        {
            var expression = node.Expression != null ? Concat(String(" "), this.Print(node.Expression)) : "";
            return Concat(node.ThrowKeyword.Text, expression, ";");
        }
    }
}
