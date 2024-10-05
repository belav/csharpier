namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardPattern
{
    public static Doc Print(DiscardPatternSyntax node, PrintingContext context)
    {
        return Token.Print(node.UnderscoreToken, context);
    }
}
