using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class OrderByClause
    {
        public static Doc Print(OrderByClauseSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OrderByKeyword, " "),
                SeparatedSyntaxList.Print(
                    node.Orderings,
                    orderingNode =>
                        Doc.Concat(
                            Node.Print(orderingNode.Expression),
                            " ",
                            Token.Print(orderingNode.AscendingOrDescendingKeyword)
                        ),
                    Doc.Null
                )
            );
        }
    }
}
