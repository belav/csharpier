namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ContinueStatement
{
    public static Doc Print(ContinueStatementSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.ContinueKeyword),
            Token.Print(node.SemicolonToken)
        );
    }
}
