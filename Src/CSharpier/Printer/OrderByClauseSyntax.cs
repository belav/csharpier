using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintOrderByClauseSyntax(OrderByClauseSyntax node)
        {
            return Doc.Concat(
                this.PrintSyntaxToken(
                    node.OrderByKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                SeparatedSyntaxList.Print(
                    node.Orderings,
                    orderingNode =>
                        Doc.Concat(
                            this.Print(orderingNode.Expression),
                            " ",
                            this.PrintSyntaxToken(
                                orderingNode.AscendingOrDescendingKeyword
                            )
                        ),
                    Doc.Null
                )
            );
        }
    }
}
