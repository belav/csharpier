namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedArraySizeExpression
{
    public static Doc Print(OmittedArraySizeExpressionSyntax node, PrintingContext context)
    {
        return Doc.Null;
    }
}
