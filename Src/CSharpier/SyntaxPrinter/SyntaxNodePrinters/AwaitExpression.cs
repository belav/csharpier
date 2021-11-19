namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AwaitExpression
{
    public static Doc Print(AwaitExpressionSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.AwaitKeyword, " "),
            Node.Print(node.Expression)
        );
    }
}
