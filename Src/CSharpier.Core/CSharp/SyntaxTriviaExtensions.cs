using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Core.CSharp;

internal static class SyntaxTriviaExtensions
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

    public static int IndexOfNextEndOfLine(this SyntaxTriviaList leadingTrivia, int startingIndex)
    {
        for (var index = startingIndex + 1; index < leadingTrivia.Count; index++)
        {
            var rawSyntaxKind = leadingTrivia[index].RawSyntaxKind();
            if (rawSyntaxKind is SyntaxKind.WhitespaceTrivia)
            {
                continue;
            }

            if (rawSyntaxKind is SyntaxKind.EndOfLineTrivia)
            {
                return index;
            }

            return -1;
        }

        return -1;
    }
}
