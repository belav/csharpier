namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringText
{
    public static Doc Print(InterpolatedStringTextSyntax node, PrintingContext context)
    {
        return Token.Print(node.TextToken, context);
    }
}
