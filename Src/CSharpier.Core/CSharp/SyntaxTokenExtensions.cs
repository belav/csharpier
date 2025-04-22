using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.Core.CSharp;

internal static class SyntaxTokenExtensions
{
    public static SyntaxKind RawSyntaxKind(this SyntaxToken token)
    {
        return (SyntaxKind)token.RawKind;
    }
}
