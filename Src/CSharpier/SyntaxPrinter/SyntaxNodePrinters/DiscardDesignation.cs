namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardDesignation
{
    public static Doc Print(DiscardDesignationSyntax node)
    {
        return Token.Print(node.UnderscoreToken);
    }
}
