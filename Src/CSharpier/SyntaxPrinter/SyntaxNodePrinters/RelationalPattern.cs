namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RelationalPattern
{
    public static Doc Print(RelationalPatternSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.OperatorToken, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
