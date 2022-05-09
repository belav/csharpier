namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringExpression
{
    public static Doc Print(InterpolatedStringExpressionSyntax node, FormattingContext context)
    {
        var docs = new List<Doc> { Token.PrintWithoutLeadingTrivia(node.StringStartToken, context) };

        docs.AddRange(node.Contents.Select(o => Node.Print(o, context)));
        docs.Add(Token.Print(node.StringEndToken, context));

        return Doc.Concat(
            // pull out the leading trivia so it doesn't get forced flat
            Token.PrintLeadingTrivia(node.StringStartToken, context),
            Doc.ForceFlat(docs)
        );
    }
}
