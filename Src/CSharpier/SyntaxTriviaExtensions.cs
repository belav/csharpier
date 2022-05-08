namespace CSharpier;

public static class SyntaxTriviaExtensions
{
    public static bool IsComment(this SyntaxTrivia syntaxTrivia)
    {
        return syntaxTrivia.RawSyntaxKind().IsComment();
    }

    public static bool IsComment(this SyntaxKind syntaxKind)
    {
        return syntaxKind
            is SyntaxKind.SingleLineCommentTrivia
                or SyntaxKind.MultiLineCommentTrivia
                or SyntaxKind.SingleLineDocumentationCommentTrivia
                or SyntaxKind.MultiLineDocumentationCommentTrivia;
    }

    public static SyntaxKind RawSyntaxKind(this SyntaxTrivia trivia)
    {
        return (SyntaxKind)trivia.RawKind;
    }
}
