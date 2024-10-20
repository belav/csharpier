namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CheckedExpression
{
    public static Doc Print(CheckedExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.Keyword, context),
            Doc.Group(
                Token.Print(node.OpenParenToken, context),
                Doc.Indent(Doc.SoftLine, Node.Print(node.Expression, context)),
                Doc.SoftLine,
                Token.Print(node.CloseParenToken, context)
            )
        );
    }
}
