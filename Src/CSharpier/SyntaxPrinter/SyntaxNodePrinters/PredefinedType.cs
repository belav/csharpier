namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PredefinedType
{
    public static Doc Print(PredefinedTypeSyntax node, PrintingContext context)
    {
        return Token.Print(node.Keyword, context);
    }
}
