namespace CSharpier.SyntaxPrinter;

internal static class TrailingComma
{
    public static Doc Print(SyntaxToken nextSyntaxToken, FormattingContext context)
    {
        if (!context.UsePrettierStyleTrailingCommas)
        {
            return Doc.Null;
        }

        return !nextSyntaxToken.LeadingTrivia.Any(o => o.IsDirective)
            ? Doc.IfBreak(
                Token.Print(SyntaxFactory.Token(SyntaxKind.CommaToken), context),
                Doc.Null
            )
            : Doc.Null;
    }
}
