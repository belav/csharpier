namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class PredefinedType
{
    public static Doc Print(PredefinedTypeSyntax node)
    {
        return Token.Print(node.Keyword);
    }
}
