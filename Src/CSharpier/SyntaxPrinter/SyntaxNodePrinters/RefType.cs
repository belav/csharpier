namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefType
{
    public static Doc Print(RefTypeSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.RefKeyword, " ", context),
            Token.PrintWithSuffix(node.ReadOnlyKeyword, " ", context),
            Node.Print(node.Type, context)
        );
    }
}
