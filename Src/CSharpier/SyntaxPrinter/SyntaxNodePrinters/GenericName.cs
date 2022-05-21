namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GenericName
{
    public static Doc Print(GenericNameSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.Identifier, context),
            Node.Print(node.TypeArgumentList, context)
        );
    }
}
