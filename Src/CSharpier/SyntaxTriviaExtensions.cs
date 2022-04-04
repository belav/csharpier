namespace CSharpier;

public static class SyntaxTriviaExtensions
{
    public static bool IsComment(this SyntaxTrivia syntaxTrivia)
    {
        return syntaxTrivia.IsKind(SyntaxKind.SingleLineCommentTrivia)
            || syntaxTrivia.IsKind(SyntaxKind.MultiLineCommentTrivia)
            || syntaxTrivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
            || syntaxTrivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia);
    }
}
