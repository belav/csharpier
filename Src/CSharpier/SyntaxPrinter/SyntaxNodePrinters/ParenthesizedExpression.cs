namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParenthesizedExpression
{
    public static Doc Print(ParenthesizedExpressionSyntax node)
    {
        return Doc.Group(
            Token.Print(node.OpenParenToken),
            Doc.Indent(Doc.SoftLine, Node.Print(node.Expression)),
            Doc.SoftLine,
            Token.Print(node.CloseParenToken)
        );
    }
}
