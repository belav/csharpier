namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedTypeArgument
{
    public static Doc Print(OmittedTypeArgumentSyntax node, PrintingContext context)
    {
        return Doc.Null;
    }
}
