namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PrimaryConstructorBaseType
{
    public static Doc Print(PrimaryConstructorBaseTypeSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Type, context),
            ArgumentList.Print(node.ArgumentList, context)
        );
    }
}
