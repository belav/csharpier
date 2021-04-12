using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintOrderByClauseSyntax(OrderByClauseSyntax node)
        {
            return Docs.Concat(
                this.PrintSyntaxToken(
                    node.OrderByKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSeparatedSyntaxList(
                    node.Orderings,
                    orderingNode =>
                        Docs.Concat(
                            this.Print(orderingNode.Expression),
                            this.PrintSyntaxToken(
                                orderingNode.AscendingOrDescendingKeyword,
                                null,
                                " "
                            )
                        ),
                    Doc.Null
                )
            );
        }
    }
}
