namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NameEquals
{
    public static Doc Print(NameEqualsSyntax node)
    {
        return Doc.Concat(Node.Print(node.Name), " ", Token.PrintWithSuffix(node.EqualsToken, " "));
    }
}
