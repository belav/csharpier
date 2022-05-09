namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class EmptyStatement
{
    public static Doc Print(EmptyStatementSyntax node, FormattingContext context)
    {
        return Token.Print(node.SemicolonToken, context);
    }
}
