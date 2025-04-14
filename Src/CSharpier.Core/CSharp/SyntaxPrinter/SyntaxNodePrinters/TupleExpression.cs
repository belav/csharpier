using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleExpression
{
    public static Doc Print(TupleExpressionSyntax node, PrintingContext context) =>
        Doc.Group(
            ArgumentListLike.Print(
                node.OpenParenToken,
                node.Arguments,
                node.CloseParenToken,
                context
            )
        );
}
