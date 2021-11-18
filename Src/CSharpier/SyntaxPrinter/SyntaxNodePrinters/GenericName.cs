namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GenericName
{
    public static Doc Print(GenericNameSyntax node)
    {
        return Doc.Group(Token.Print(node.Identifier), Node.Print(node.TypeArgumentList));
    }
}
