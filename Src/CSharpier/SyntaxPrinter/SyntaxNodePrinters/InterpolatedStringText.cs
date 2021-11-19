namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringText
{
    public static Doc Print(InterpolatedStringTextSyntax node)
    {
        return Token.Print(node.TextToken);
    }
}
