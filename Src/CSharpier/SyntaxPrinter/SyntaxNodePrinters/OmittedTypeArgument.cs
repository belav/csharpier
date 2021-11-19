namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedTypeArgument
{
    public static Doc Print(OmittedTypeArgumentSyntax node)
    {
        return Doc.Null;
    }
}
