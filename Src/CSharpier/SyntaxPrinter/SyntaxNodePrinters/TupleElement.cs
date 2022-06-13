namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleElement
{
    public static Doc Print(TupleElementSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Type, context),
            node.Identifier.RawSyntaxKind() != SyntaxKind.None
                ? Doc.Concat(" ", Token.Print(node.Identifier, context))
                : Doc.Null
        );
    }
}
