namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExternAliasDirective
{
    public static Doc Print(ExternAliasDirectiveSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.ExternKeyword, " ", context),
            Token.PrintWithSuffix(node.AliasKeyword, " ", context),
            Token.Print(node.Identifier, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
