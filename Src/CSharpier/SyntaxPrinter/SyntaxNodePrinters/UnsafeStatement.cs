namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UnsafeStatement
{
    public static Doc Print(UnsafeStatementSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.UnsafeKeyword, context),
            Node.Print(node.Block, context)
        );
    }
}
