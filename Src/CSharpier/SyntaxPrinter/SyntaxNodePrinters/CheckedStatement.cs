namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CheckedStatement
{
    public static Doc Print(CheckedStatementSyntax node, FormattingContext context)
    {
        return Doc.Concat(Token.Print(node.Keyword, context), Block.Print(node.Block, context));
    }
}
