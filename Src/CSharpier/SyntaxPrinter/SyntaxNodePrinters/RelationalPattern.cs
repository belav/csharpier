namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class RelationalPattern
{
    public static Doc Print(RelationalPatternSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.OperatorToken, " "),
            Node.Print(node.Expression)
        );
    }
}
