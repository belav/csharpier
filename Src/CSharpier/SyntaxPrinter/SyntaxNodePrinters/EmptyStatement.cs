namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class EmptyStatement
{
    public static Doc Print(EmptyStatementSyntax node, PrintingContext context)
    {
        return Token.Print(node.SemicolonToken, context);
    }
}
