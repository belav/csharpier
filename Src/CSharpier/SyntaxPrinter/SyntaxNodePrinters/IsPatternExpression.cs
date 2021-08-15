using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class IsPatternExpression
    {
        public static Doc Print(IsPatternExpressionSyntax node)
        {
            var useSpace =
                node.Parent is IfStatementSyntax or ParenthesizedExpressionSyntax
                && node.Pattern
                    is not (ParenthesizedPatternSyntax
                        or UnaryPatternSyntax
                        or RecursivePatternSyntax { PropertyPatternClause: { Subpatterns: { Count: 0 } } });

            return Doc.Group(
                Node.Print(node.Expression),
                Doc.IndentIf(
                    node.Parent is not (IfStatementSyntax or ParenthesizedExpressionSyntax),
                    Doc.Concat(
                        useSpace ? " " : Doc.Line,
                        Token.Print(node.IsKeyword),
                        node.Pattern is RecursivePatternSyntax { Type: null } ? Doc.Null : " ",
                        Node.Print(node.Pattern)
                    )
                )
            );
        }
    }
}
