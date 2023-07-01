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
            // TODO when should this actually print?
            printExtraLines ? ExtraNewLines.Print(node) : Doc.Null,
            // TODO should only skip on the first one that exists
            Token.PrintWithSuffix(node.GlobalKeyword, " ", context, skipLeadingTrivia: true),
            Token.PrintWithSuffix(node.UsingKeyword, " ", context, skipLeadingTrivia: true),
            Token.PrintWithSuffix(node.StaticKeyword, " ", context, skipLeadingTrivia: true),
            node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias, context),
            Node.Print(node.NamespaceOrType, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
