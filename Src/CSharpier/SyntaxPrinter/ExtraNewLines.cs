namespace CSharpier.SyntaxPrinter;

internal static class ExtraNewLines
{
    public static Doc Print(CSharpSyntaxNode node)
    {
        return node.Parent is GlobalStatementSyntax ? Doc.Null : Print(node.GetLeadingTrivia());
    }

    public static Doc Print(SyntaxTriviaList syntaxTriviaList)
    {
        foreach (var leadingTrivia in syntaxTriviaList)
        {
            if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
            {
                // ensures we only print a single new line
                return Doc.HardLine;
            }
            if (leadingTrivia.Kind() != SyntaxKind.WhitespaceTrivia)
            {
                return Doc.Null;
            }
        }

        return Doc.Null;
    }
}
