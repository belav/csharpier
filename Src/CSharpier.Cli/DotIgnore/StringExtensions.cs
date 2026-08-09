using System.Buffers;

namespace CSharpier.Cli.DotIgnore;

internal static class StringExtensions
{
    private static readonly SearchValues<char> invalidCharacters = SearchValues.Create(',', '/');

    internal static ReadOnlySpan<char> NormalisePath(this ReadOnlySpan<char> path)
    {
        var index = path.IndexOfAny(invalidCharacters);
        return index < 0
            ? path.Trim()
            : path.ToString()
                .Replace(":", string.Empty)
                .Replace(Path.DirectorySeparatorChar, '/')
                .AsSpan()
                .Trim();
    }
}
