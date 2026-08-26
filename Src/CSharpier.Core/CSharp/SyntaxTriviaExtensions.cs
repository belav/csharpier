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

    // these iterate the struct enumerator instead of going through System.Linq, which would
    // box the list and allocate an enumerator for every call
    public static bool AnyComment(this in SyntaxTriviaList triviaList)
    {
        foreach (var trivia in triviaList)
        {
            if (trivia.IsComment())
            {
                return true;
            }
        }

        return false;
    }

    public static bool AnyDirective(this in SyntaxTriviaList triviaList)
    {
        foreach (var trivia in triviaList)
        {
            if (trivia.IsDirective)
            {
                return true;
            }
        }

        return false;
    }

    public static bool AnyCommentOrDirective(this in SyntaxTriviaList triviaList)
    {
        foreach (var trivia in triviaList)
        {
            if (trivia.IsComment() || trivia.IsDirective)
            {
                return true;
            }
        }

        return false;
    }
}
