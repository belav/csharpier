namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FinallyClause
{
    public static Doc Print(FinallyClauseSyntax node)
    {
        return Doc.Concat(Token.Print(node.FinallyKeyword), Node.Print(node.Block));
    }
}
