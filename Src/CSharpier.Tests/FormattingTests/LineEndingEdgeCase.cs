namespace CSharpier.Tests.FormattingTests;

using System.Text;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
internal class LineEndingEdgeCase
{
    [TestCase("\n", EndOfLine.LF)]
    [TestCase("\r\n", EndOfLine.LF)]
    [TestCase("\n", EndOfLine.CRLF)]
    [TestCase("\r\n", EndOfLine.CRLF)]
    public async Task Preprocessor_Symbols_With_CSharpier_Ignore_Gets_Proper_Line_Endings(
        string lineEnding,
        EndOfLine endOfLine
    )
    {
        var stringBuilder = new StringBuilder();

        void Append(string value)
        {
            stringBuilder.Append(value);
            stringBuilder.Append(lineEnding);
        }

        Append("class Unformatted");
        Append("{");
        Append("    void MethodName()");
        Append("    {");
        Append("        // csharpier-ignore");
        Append("        CallMethod()");
        Append("            .Should()");
        Append("            .BeNull();");
        Append("    }");
        Append("}");

        var result = await CSharpFormatter.FormatAsync(
            stringBuilder.ToString(),
            new PrinterOptions
            {
                EndOfLine = endOfLine,
                PreprocessorSymbolSets = new List<string[]> { new[] { "" }, new[] { "DEBUG" } }
            },
            CancellationToken.None
        );

        result.Code
            .Should()
            .Be(
                stringBuilder
                    .ToString()
                    .ReplaceLineEndings(endOfLine == EndOfLine.LF ? "\n" : "\r\n")
            );
    }
}
