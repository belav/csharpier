namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedTypeArgument
{
    public static Doc Print(OmittedTypeArgumentSyntax node, FormattingContext context)
    {
        return Doc.Null;
    }
}
