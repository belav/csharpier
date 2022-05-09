namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardPattern
{
    public static Doc Print(DiscardPatternSyntax node, FormattingContext context)
    {
        return Token.Print(node.UnderscoreToken, context);
    }
}
