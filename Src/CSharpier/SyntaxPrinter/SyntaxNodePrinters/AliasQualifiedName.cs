namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AliasQualifiedName
{
    public static Doc Print(AliasQualifiedNameSyntax node)
    {
        return Doc.Concat(
            Node.Print(node.Alias),
            Token.Print(node.ColonColonToken),
            Node.Print(node.Name)
        );
    }
}
