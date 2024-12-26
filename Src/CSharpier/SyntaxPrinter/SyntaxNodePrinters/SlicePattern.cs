namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SlicePattern
{
    public static Doc Print(SlicePatternSyntax node, PrintingContext context)
    {
        if (node.Pattern is null)
        {
            return Token.Print(node.DotDotToken, context);
        }

        return Doc.Concat(
            Token.Print(node.DotDotToken, context),
            " ",
            Node.Print(node.Pattern, context)
        );
    }
}
