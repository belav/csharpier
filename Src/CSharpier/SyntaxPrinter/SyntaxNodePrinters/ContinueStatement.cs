namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ContinueStatement
{
    public static Doc Print(ContinueStatementSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.ContinueKeyword, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
