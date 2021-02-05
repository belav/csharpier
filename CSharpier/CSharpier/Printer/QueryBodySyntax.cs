using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryBodySyntax(QueryBodySyntax node)
        {
            var parts = new Parts();
            parts.Add(Join(Line, node.Clauses.Select(this.Print)));
            if (node.Clauses.Count > 0)
            {
                parts.Add(Line);
            }

            parts.Add(this.Print(node.SelectOrGroup));
            if (node.Continuation != null)
            {
                // TODO indent when there is a group by before the into?
                parts.Push(
                    String(" "),
                    this.PrintQueryContinuationSyntax(node.Continuation)
                );
            }

            return Concat(parts);
        }
    }
}