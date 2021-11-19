namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleElement
{
    public static Doc Print(TupleElementSyntax node)
    {
        return Doc.Concat(
            Node.Print(node.Type),
            node.Identifier.Kind() != SyntaxKind.None
              ? Doc.Concat(" ", Token.Print(node.Identifier))
              : Doc.Null
        );
    }
}
