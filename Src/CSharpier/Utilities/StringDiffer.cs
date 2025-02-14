using System.Globalization;
using System.Text;

namespace CSharpier.Utilities;

internal static class StringDiffer
{
    /// <summary>
    /// Given two strings that are different, this will print the first different line it encounters
    /// along with a single line before and after that different line.
    /// </summary>
    public static string PrintFirstDifference(string expected, string actual)
    {
        if (expected == actual)
        {
            return string.Empty;
        }

        if (!EndsWithExactlyOneNewline(actual))
        {
            return "The file did not end with a single newline.";
        }

        using var expectedReader = new StringReader(expected);
        using var actualReader = new StringReader(actual);

        var expectedLine = expectedReader.ReadLine();
        var actualLine = actualReader.ReadLine();
        var line = 1;
        string? previousExpectedLine = null;
        string? previousActualLine = null;
        var stringBuilder = new StringBuilder();
        while (expectedLine != null || actualLine != null)
        {
            if (expectedLine == actualLine)
            {
                line++;
                previousExpectedLine = expectedLine;
                previousActualLine = actualLine;
                expectedLine = expectedReader.ReadLine();
                actualLine = actualReader.ReadLine();
                continue;
            }

            stringBuilder.AppendLine(
                CultureInfo.InvariantCulture,
                $"----------------------------- Expected: Around Line {line} -----------------------------"
            );
            if (previousExpectedLine != null)
            {
                stringBuilder.AppendLine(previousExpectedLine);
            }

            stringBuilder.AppendLine(expectedLine);
            var nextExpectedLine = expectedReader.ReadLine();
            if (nextExpectedLine != null)
            {
                stringBuilder.AppendLine(nextExpectedLine);
            }

            stringBuilder.AppendLine(
                CultureInfo.InvariantCulture,
                $"----------------------------- Actual: Around Line {line} -----------------------------"
            );
            if (previousActualLine != null)
            {
                stringBuilder.AppendLine(previousActualLine);
            }

            if (string.IsNullOrWhiteSpace(actualLine))
            {
                stringBuilder.AppendLine(MakeWhiteSpaceVisible(actualLine));
            }
            else
            {
                if (actualLine[^1] is ' ' or '\t')
                {
                    var lastNonWhitespace = FindIndexOfNonWhitespace(actualLine);
                    stringBuilder.AppendLine(
                        actualLine[..lastNonWhitespace]
                            + MakeWhiteSpaceVisible(actualLine[lastNonWhitespace..])
                    );
                }
                else
                {
                    stringBuilder.AppendLine(actualLine);
                }
            }

            var nextActualLine = actualReader.ReadLine();
            if (nextActualLine != null)
            {
                stringBuilder.AppendLine(nextActualLine);
            }

            return stringBuilder.ToString();
        }

        return "The file contained different line endings than formatting it would result in.";
    }

    private static bool EndsWithExactlyOneNewline(string value)
    {
        return (
                !value.EndsWith("\r\n", StringComparison.Ordinal)
                && value.EndsWith('\n')
                && !value.EndsWith("\n\n", StringComparison.Ordinal)
            )
            || (
                value.EndsWith("\r\n", StringComparison.Ordinal)
                && !value.EndsWith("\r\n\r\n", StringComparison.Ordinal)
            );
    }

    private static int FindIndexOfNonWhitespace(string input)
    {
        for (var x = input.Length - 1; x >= 0; x--)
        {
            if (input[x] is not (' ' or '\t'))
            {
                return x;
            }
        }

        return -1;
    }

    private static string? MakeWhiteSpaceVisible(string? value)
    {
        return value?.Replace(' ', '·').Replace('\t', '→');
    }
}
