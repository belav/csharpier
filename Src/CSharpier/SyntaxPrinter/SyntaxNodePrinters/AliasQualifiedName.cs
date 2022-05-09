namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AliasQualifiedName
{
    public static Doc Print(AliasQualifiedNameSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Alias, context),
            Token.Print(node.ColonColonToken, context),
            Node.Print(node.Name, context)
        );
    }
}
