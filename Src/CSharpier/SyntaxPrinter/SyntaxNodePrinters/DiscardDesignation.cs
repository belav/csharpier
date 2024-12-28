namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DiscardDesignation
{
    public static Doc Print(DiscardDesignationSyntax node, PrintingContext context)
    {
        return Token.Print(node.UnderscoreToken, context);
    }
}
