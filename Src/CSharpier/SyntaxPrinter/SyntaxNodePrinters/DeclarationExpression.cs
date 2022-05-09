namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DeclarationExpression
{
    public static Doc Print(DeclarationExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(Node.Print(node.Type, context), " ", Node.Print(node.Designation, context));
    }
}
