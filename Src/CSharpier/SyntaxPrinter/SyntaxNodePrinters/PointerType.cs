namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PointerType
{
    public static Doc Print(PointerTypeSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.ElementType, context),
            Token.Print(node.AsteriskToken, context)
        );
    }
}
