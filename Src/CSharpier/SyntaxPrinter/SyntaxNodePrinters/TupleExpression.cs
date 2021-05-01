using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TupleExpression
    {
        public static Doc Print(TupleExpressionSyntax node) =>
            Doc.Group(
                new Printer().PrintArgumentListLikeSyntax(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken
                )
            );
    }
}
