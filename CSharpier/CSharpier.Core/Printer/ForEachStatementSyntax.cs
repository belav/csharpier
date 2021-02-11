using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintForEachStatementSyntax(ForEachStatementSyntax node)
        {
            var parts = new Parts();
            if (node.AwaitKeyword.RawKind != 0) {
                parts.Push("await ");
            }
            parts.Push("foreach ", "(");
            parts.Push(this.Print(node.Type), " ", node.Identifier.Text, " in ", this.Print(node.Expression));
            parts.Push(")");
            parts.Push(this.Print(node.Statement));
            return Concat(parts);
        }
    }
}
