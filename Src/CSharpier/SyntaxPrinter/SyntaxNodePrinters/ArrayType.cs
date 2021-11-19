namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayType
{
    public static Doc Print(ArrayTypeSyntax node)
    {
        return Doc.Concat(
            Node.Print(node.ElementType),
            Doc.Concat(node.RankSpecifiers.Select(Node.Print).ToArray())
        );
    }
}
