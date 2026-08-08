namespace CSharpier.Cli;

internal static class GeneratedCodeUtilities
{
    public static bool IsGeneratedCodeFile(ReadOnlySpan<char> filePath)
    {
        var fileName = Path.GetFileName(filePath);
        if (fileName.StartsWithIgnoreCase("TemporaryGeneratedFile_"))
        {
            return true;
        }

        var extension = Path.GetExtension(fileName);
        if (extension.IsEmpty)
        {
            return false;
        }

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        return fileNameWithoutExtension.EndsWithIgnoreCase(".designer")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".generated")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".g")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".g.i");
    }

    private static bool StartsWithIgnoreCase(
        this ReadOnlySpan<char> value,
        ReadOnlySpan<char> otherValue
    )
    {
        return value.StartsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }

    private static bool EndsWithIgnoreCase(
        this ReadOnlySpan<char> value,
        ReadOnlySpan<char> otherValue
    )
    {
        return value.EndsWith(otherValue, StringComparison.OrdinalIgnoreCase);
    }
}
