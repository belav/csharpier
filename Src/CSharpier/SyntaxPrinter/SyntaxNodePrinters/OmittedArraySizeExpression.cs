namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class OmittedArraySizeExpression
{
    public static Doc Print(OmittedArraySizeExpressionSyntax node, FormattingContext context)
    {
        return Doc.Null;
    }
}
