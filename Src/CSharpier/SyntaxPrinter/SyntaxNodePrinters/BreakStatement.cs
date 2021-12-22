namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BreakStatement
{
    public static Doc Print(BreakStatementSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.BreakKeyword),
            Token.Print(node.SemicolonToken)
        );
    }
}
