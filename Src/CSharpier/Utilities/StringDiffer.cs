using System.IO;

namespace CSharpier.Utilities
{
    public static class StringDiffer
    {
        public static void PrintDifference(string expected, string actual, IConsole console)
        {
            if (expected == actual)
            {
                return;
            }

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

                console.WriteLine(
                    $"----------------------------- Expected: Around Line {line} -----------------------------"
                );
                if (previousExpectedLine != null)
                {
                    console.WriteLine(MakeWhiteSpaceVisible(previousExpectedLine));
                }
                console.WriteLine(MakeWhiteSpaceVisible(expectedLine));
                var nextExpectedLine = expectedReader.ReadLine();
                if (nextExpectedLine != null)
                {
                    console.WriteLine(MakeWhiteSpaceVisible(nextExpectedLine));
                }

                console.WriteLine(
                    $"----------------------------- Actual: Around Line {line} -----------------------------"
                );
                if (previousActualLine != null)
                {
                    console.WriteLine(MakeWhiteSpaceVisible(previousActualLine));
                }
                console.WriteLine(MakeWhiteSpaceVisible(actualLine));
                var nextActualLine = actualReader.ReadLine();
                if (nextActualLine != null)
                {
                    console.WriteLine(MakeWhiteSpaceVisible(nextActualLine));
                }

                return;
            }

            console.WriteLine(
                "The file contained different line endings than formatting it would result in."
            );
        }

        private static string? MakeWhiteSpaceVisible(string? value)
        {
            return value?.Replace(' ', (char)183).Replace('\t', '→');
        }
    }
}
