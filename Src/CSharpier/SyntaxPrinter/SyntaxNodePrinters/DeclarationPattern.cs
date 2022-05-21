namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DeclarationPattern
{
    public static Doc Print(DeclarationPatternSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Type, context),
            " ",
            Node.Print(node.Designation, context)
        );
    }
}
