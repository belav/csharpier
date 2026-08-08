namespace CSharpier.Cli.DotIgnore;

internal static class StringExtensions
{
    internal static string NormalisePath(this string path)
    {
        return path.Replace(":", string.Empty).Replace(Path.DirectorySeparatorChar, '/').Trim();
    }
}
