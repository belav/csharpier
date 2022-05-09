namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class QualifiedName
{
    public static Doc Print(QualifiedNameSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Left, context),
            Token.Print(node.DotToken, context),
            Node.Print(node.Right, context)
        );
    }
}
