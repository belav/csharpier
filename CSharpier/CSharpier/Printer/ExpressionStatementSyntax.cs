using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExpressionStatementSyntax(ExpressionStatementSyntax node)
        {
            var parts = new Parts(this.Print(node.Expression), ";");
            // TODO printTrailingComments(node, parts, String("semicolonToken"));
            return Concat(parts);
        }
    }
}
