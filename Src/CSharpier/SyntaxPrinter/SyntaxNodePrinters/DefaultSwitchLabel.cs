namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DefaultSwitchLabel
{
    public static Doc Print(DefaultSwitchLabelSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.Keyword, context),
            Token.Print(node.ColonToken, context)
        );
    }
}
