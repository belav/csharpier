using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class OrderByClause
{
    public static Doc Print(OrderByClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.OrderByKeyword, context),
            SeparatedSyntaxList.Print(
                node.Orderings,
                static (orderingNode, context) =>
                    Doc.Concat(
                        " ",
                        Node.Print(orderingNode.Expression, context),
                        string.IsNullOrEmpty(orderingNode.AscendingOrDescendingKeyword.Text)
                            ? Doc.Null
                            : " ",
                        Token.Print(orderingNode.AscendingOrDescendingKeyword, context)
                    ),
                Doc.Null,
                context
            )
        );
    }
}
