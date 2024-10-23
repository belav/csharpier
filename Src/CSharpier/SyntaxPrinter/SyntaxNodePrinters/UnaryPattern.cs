namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UnaryPattern
{
    public static Doc Print(UnaryPatternSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.OperatorToken, " ", context),
            Node.Print(node.Pattern, context)
        );
    }
}
