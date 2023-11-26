namespace CSharpier;

internal static class GeneratedCodeUtilities
{
    public static bool IsGeneratedCodeFile(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        if (fileName.StartsWithIgnoreCase("TemporaryGeneratedFile_"))
        {
            return true;
        }

        var extension = Path.GetExtension(fileName);
        if (extension.IsBlank())
        {
            return false;
        }
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        return fileNameWithoutExtension.EndsWithIgnoreCase(".designer")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".generated")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".g")
            || fileNameWithoutExtension.EndsWithIgnoreCase(".g.i");
    }

    private static readonly string[] AutoGeneratedStrings = new[]
    {
        "<autogenerated",
        "<auto-generated"
    };

    public static bool BeginsWithAutoGeneratedComment(SyntaxNode root)
    {
        if (!root.HasLeadingTrivia)
        {
            return false;
        }

        foreach (var trivia in root.GetLeadingTrivia())
        {
            if (!IsComment(trivia))
            {
                continue;
            }

            var text = trivia.ToString();

            foreach (var autoGenerated in AutoGeneratedStrings)
            {
                if (text.Contains(autoGenerated))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsComment(SyntaxTrivia trivia)
    {
        return trivia.RawSyntaxKind()
            is SyntaxKind.SingleLineCommentTrivia
                or SyntaxKind.MultiLineCommentTrivia;
    }
}
