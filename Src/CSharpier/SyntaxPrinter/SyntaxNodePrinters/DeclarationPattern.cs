namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DeclarationPattern
{
    public static Doc Print(DeclarationPatternSyntax node)
    {
        return Doc.Concat(Node.Print(node.Type), " ", Node.Print(node.Designation));
    }
}
