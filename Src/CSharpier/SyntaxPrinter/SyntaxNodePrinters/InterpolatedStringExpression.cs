namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringExpression
{
    public static Doc Print(InterpolatedStringExpressionSyntax node, FormattingContext context)
    {
        // if any of the expressions in the intrepolation contain a newline then don't force this flat
        // ideally we would format the expressions in some way, but determining how much to indent is a hard problem
        // and InterpolatedVerbatimStrings are rare and new lines in expressions in them are even more rare.
        if (
            node.StringStartToken.RawSyntaxKind() is SyntaxKind.InterpolatedVerbatimStringStartToken
            && node.Contents.Any(o => o is InterpolationSyntax && o.ToString().Contains('\n'))
        )
        {
            return node.ToString();
        }

        var docs = new List<Doc>
        {
            Token.PrintWithoutLeadingTrivia(node.StringStartToken, context)
        };

        docs.AddRange(node.Contents.Select(o => Node.Print(o, context)));
        docs.Add(Token.Print(node.StringEndToken, context));

        return Doc.Concat(
            // pull out the leading trivia so it doesn't get forced flat
            Token.PrintLeadingTrivia(node.StringStartToken, context),
            Doc.ForceFlat(docs)
        );
    }
}
