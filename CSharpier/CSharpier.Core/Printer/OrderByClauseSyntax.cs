using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintOrderByClauseSyntax(OrderByClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.OrderByKeyword, " "),
                this.PrintSeparatedSyntaxList(node.Orderings, orderingNode => Concat(
                            this.Print(orderingNode.Expression),
                            orderingNode.AscendingOrDescendingKeyword.RawKind != 0
                                ? " " + orderingNode.AscendingOrDescendingKeyword.Text
                                : ""
                        ), null)
                );
        }
    }
}
