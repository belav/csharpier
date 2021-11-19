namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringExpression
{
    public static Doc Print(InterpolatedStringExpressionSyntax node)
    {
        var docs = new List<Doc> { Token.PrintWithoutLeadingTrivia(node.StringStartToken) };

        docs.AddRange(node.Contents.Select(Node.Print));
        docs.Add(Token.Print(node.StringEndToken));

        return Doc.Concat(
            // pull out the leading trivia so it doesn't get forced flat
            Token.PrintLeadingTrivia(node.StringStartToken),
            Doc.ForceFlat(docs)
        );
    }
}
