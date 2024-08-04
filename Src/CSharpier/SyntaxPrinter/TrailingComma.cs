namespace CSharpier.SyntaxPrinter;

internal static class TrailingComma
{
    public static Doc Print(SyntaxToken closingToken, FormattingContext context)
    {
        return closingToken.LeadingTrivia.Any(o => o.IsDirective)
            ? Doc.Null
            : Doc.IfBreak(
                Token.Print(SyntaxFactory.Token(SyntaxKind.CommaToken), context),
                Doc.Null
            );
    }
}
