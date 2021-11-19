namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UsingDirective
{
    public static Doc Print(UsingDirectiveSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.GlobalKeyword, " "),
            Token.PrintWithSuffix(node.UsingKeyword, " "),
            Token.PrintWithSuffix(node.StaticKeyword, " "),
            node.Alias == null ? Doc.Null : NameEquals.Print(node.Alias),
            Node.Print(node.Name),
            Token.Print(node.SemicolonToken)
        );
    }
}
