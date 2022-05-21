namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UsingDirective
{
    public static Doc Print(UsingDirectiveSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.GlobalKeyword, " ", context),
            Token.PrintWithSuffix(node.UsingKeyword, " ", context),
            Token.PrintWithSuffix(node.StaticKeyword, " ", context),
            node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias, context),
            Node.Print(node.Name, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
