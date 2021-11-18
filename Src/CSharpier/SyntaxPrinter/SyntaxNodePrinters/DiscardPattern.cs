namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardPattern
{
    public static Doc Print(DiscardPatternSyntax node)
    {
        return Token.Print(node.UnderscoreToken);
    }
}
