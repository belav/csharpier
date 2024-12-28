namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NameEquals
{
    public static Doc Print(NameEqualsSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Name, context),
            " ",
            Token.PrintWithSuffix(node.EqualsToken, " ", context)
        );
    }
}
