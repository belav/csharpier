using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TupleExpression
    {
        public static Doc Print(TupleExpressionSyntax node) =>
            Doc.Group(
                ArgumentListLike.Print(node.OpenParenToken, node.Arguments, node.CloseParenToken)
            );
    }
}
