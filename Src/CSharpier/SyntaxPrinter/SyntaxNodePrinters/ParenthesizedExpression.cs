namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParenthesizedExpression
{
    public static Doc Print(ParenthesizedExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintLeadingTrivia(node.OpenParenToken, context),
            Doc.Group(
                Token.PrintWithoutLeadingTrivia(node.OpenParenToken, context),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression, context)),
                Doc.SoftLine,
                Token.Print(node.CloseParenToken, context)
            )
        );
    }
}
