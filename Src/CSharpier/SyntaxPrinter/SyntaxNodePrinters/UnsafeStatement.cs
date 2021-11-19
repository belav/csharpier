namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class UnsafeStatement
{
    public static Doc Print(UnsafeStatementSyntax node)
    {
        return Doc.Concat(Token.Print(node.UnsafeKeyword), Node.Print(node.Block));
    }
}
