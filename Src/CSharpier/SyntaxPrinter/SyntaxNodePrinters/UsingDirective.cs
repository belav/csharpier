namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UsingDirective
{
    public static Doc Print(
        UsingDirectiveSyntax node,
        FormattingContext context,
        bool printExtraLines = true
    )
    {
        return Doc.Concat(
            printExtraLines ? ExtraNewLines.Print(node) : Doc.Null,
            Token.PrintWithSuffix(node.GlobalKeyword, " ", context),
            Token.PrintWithSuffix(node.UsingKeyword, " ", context),
            Token.PrintWithSuffix(node.StaticKeyword, " ", context),
            node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias, context),
            Node.Print(node.Name, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
