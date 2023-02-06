namespace CSharpier;

internal static class SyntaxTokenExtensions
{
    public static SyntaxKind RawSyntaxKind(this SyntaxToken token)
    {
        return (SyntaxKind)token.RawKind;
    }
}
