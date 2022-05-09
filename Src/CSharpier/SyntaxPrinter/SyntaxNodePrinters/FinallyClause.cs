namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FinallyClause
{
    public static Doc Print(FinallyClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(Token.Print(node.FinallyKeyword, context), Node.Print(node.Block, context));
    }
}
