using System.IO;
using System.Text;

namespace CSharpier.Utilities
{
    public static class StringDiffer
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

            var stringBuilder = new StringBuilder();

            using var expectedReader = new StringReader(expected);
            using var actualReader = new StringReader(actual);

            var expectedLine = expectedReader.ReadLine();
            var actualLine = actualReader.ReadLine();
            var line = 1;
            string? previousExpectedLine = null;
            string? previousActualLine = null;
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
                    $"----------------------------- Expected: Around Line {line} -----------------------------"
                );
                if (previousExpectedLine != null)
                {
                    stringBuilder.AppendLine(MakeWhiteSpaceVisible(previousExpectedLine));
                }
                stringBuilder.AppendLine(MakeWhiteSpaceVisible(expectedLine));
                var nextExpectedLine = expectedReader.ReadLine();
                if (nextExpectedLine != null)
                {
                    stringBuilder.AppendLine(MakeWhiteSpaceVisible(nextExpectedLine));
                }

                stringBuilder.AppendLine(
                    $"----------------------------- Actual: Around Line {line} -----------------------------"
                );
                if (previousActualLine != null)
                {
                    stringBuilder.AppendLine(MakeWhiteSpaceVisible(previousActualLine));
                }
                stringBuilder.AppendLine(MakeWhiteSpaceVisible(actualLine));
                var nextActualLine = actualReader.ReadLine();
                if (nextActualLine != null)
                {
                    stringBuilder.AppendLine(MakeWhiteSpaceVisible(nextActualLine));
                }

                return stringBuilder.ToString();
            }

            stringBuilder.AppendLine(
                "The file contained different line endings than formatting it would result in."
            );

            return stringBuilder.ToString();
        }

        private static string? MakeWhiteSpaceVisible(string? value)
        {
            return value?.Replace(' ', '·').Replace('\t', '→');
        }
    }
}
