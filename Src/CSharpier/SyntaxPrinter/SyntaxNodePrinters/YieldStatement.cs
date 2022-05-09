namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class YieldStatement
{
    public static Doc Print(YieldStatementSyntax node, FormattingContext context)
    {
        Doc expression =
            node.Expression != null
                ? Doc.Concat(" ", Node.Print(node.Expression, context))
                : string.Empty;
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.YieldKeyword, " ", context),
            Token.Print(node.ReturnOrBreakKeyword, context),
            expression,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
