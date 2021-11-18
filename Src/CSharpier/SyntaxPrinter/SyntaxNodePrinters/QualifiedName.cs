namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QualifiedName
{
    public static Doc Print(QualifiedNameSyntax node)
    {
        return Doc.Concat(
            Node.Print(node.Left),
            Token.Print(node.DotToken),
            Node.Print(node.Right)
        );
    }
}
