namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BinaryPattern
{
    public static Doc Print(BinaryPatternSyntax node, FormattingContext context)
    {
        return Doc.IndentIf(
            node.Parent is SubpatternSyntax or IsPatternExpressionSyntax,
            Doc.Concat(
                Node.Print(node.Left, context),
                Doc.Line,
                Token.PrintWithSuffix(node.OperatorToken, " ", context),
                Node.Print(node.Right, context)
            )
        );
    }
}
