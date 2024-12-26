namespace CSharpier.SyntaxPrinter;

internal static class TrailingComma
{
    public static Doc Print(
        SyntaxToken closingToken,
        PrintingContext context,
        bool skipIfBreak = false
    )
    {
        var printedToken = Token.Print(SyntaxFactory.Token(SyntaxKind.CommaToken), context);

        return closingToken.LeadingTrivia.Any(o => o.IsDirective) ? Doc.Null
            : skipIfBreak ? printedToken
            : Doc.IfBreak(printedToken, Doc.Null);
    }
}
