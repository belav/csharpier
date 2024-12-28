namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CheckedStatement
{
    public static Doc Print(CheckedStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(Token.Print(node.Keyword, context), Block.Print(node.Block, context));
    }
}
