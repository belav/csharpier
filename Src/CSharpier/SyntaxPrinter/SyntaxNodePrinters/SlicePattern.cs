namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SlicePattern
{
    public static Doc Print(SlicePatternSyntax node, FormattingContext context)
    {
        return Token.Print(node.DotDotToken, context);
    }
}
