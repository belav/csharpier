namespace CSharpier.SyntaxPrinter;

// TODO review the other places with trailing comments
// https://github.com/belav/csharpier/pull/1284/files
internal static class TrailingComma
{
    public static Doc Print(SyntaxToken nextSyntaxToken, FormattingContext context)
    {
        return !nextSyntaxToken.LeadingTrivia.Any(o => o.IsDirective)
            ? Doc.IfBreak(
                Token.Print(SyntaxFactory.Token(SyntaxKind.CommaToken), context),
                Doc.Null
            )
            : Doc.Null;
    }
}
