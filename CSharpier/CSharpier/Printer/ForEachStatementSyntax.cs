using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintForEachStatementSyntax(ForEachStatementSyntax node)
        {
            var parts = new Parts();
            if (NotNullToken(node.AwaitKeyword)) {
                parts.Add(String("await "));
            }
            parts.Push(String("foreach "), String("("));
            parts.Push(this.Print(node.Type), String(" "), node.Identifier.Text, String(" in "), this.Print(node.Expression));
            parts.Add(String(")"));
            parts.Add(this.Print(node.Statement));
            return Concat(parts);
        }
    }
}
