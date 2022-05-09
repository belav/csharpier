namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExpressionStatement
{
    public static Doc Print(ExpressionStatementSyntax node, FormattingContext context)
    {
        return Doc.Group(
            ExtraNewLines.Print(node),
            Node.Print(node.Expression, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
