namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GotoStatement
{
    public static Doc Print(GotoStatementSyntax node, FormattingContext context)
    {
        var expression =
            node.Expression != null ? Doc.Concat(" ", Node.Print(node.Expression, context)) : string.Empty;
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.GotoKeyword, context),
            node.CaseOrDefaultKeyword.RawSyntaxKind() != SyntaxKind.None ? " " : Doc.Null,
            Token.Print(node.CaseOrDefaultKeyword, context),
            expression,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
