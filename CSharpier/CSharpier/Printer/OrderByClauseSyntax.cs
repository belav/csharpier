using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintOrderByClauseSyntax(OrderByClauseSyntax node)
        {
            return Concat(
                String("orderby "),
                Join(
                    String(", "),
                    node.Orderings.Select(orderingNode => Concat(
                            this.Print(orderingNode.Expression),
                            orderingNode.AscendingOrDescendingKeyword.RawKind != 0
                                ? " " + orderingNode.AscendingOrDescendingKeyword.Text
                                : ""
                        )
                )
            ));
        }
    }
}
