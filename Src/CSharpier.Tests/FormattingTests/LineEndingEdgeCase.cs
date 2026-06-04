using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.CSharp;

namespace CSharpier.Tests.FormattingTests;

internal sealed class LineEndingEdgeCase
{
    [Test]
    [Arguments("\n", EndOfLine.LF)]
    [Arguments("\r\n", EndOfLine.LF)]
    [Arguments("\n", EndOfLine.CRLF)]
    [Arguments("\r\n", EndOfLine.CRLF)]
    public async Task Preprocessor_Symbols_With_CSharpier_Ignore_Gets_Proper_Line_Endings(
        string lineEnding,
        EndOfLine endOfLine
    )
    {
        var unformattedCode = @"class Unformatted
{
    void MethodName()
    {
        // csharpier-ignore
        CallMethod()
            .Should()
            .BeNull();
#if DEBUG
#endif
    }
}
".ReplaceLineEndings(lineEnding);

        var result = await CSharpFormatter.FormatAsync(
            unformattedCode,
            new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict)
            {
                EndOfLine = endOfLine,
            },
            CancellationToken.None
        );

        result
            .Code.Should()
            .Be(unformattedCode.ReplaceLineEndings(endOfLine == EndOfLine.LF ? "\n" : "\r\n"));
    }

    [Test]
    public async Task CSharpier_Ignore_Preserves_Preceding_Blank_Line()
    {
        var unformattedCode = """
            void X()
            {
                int i = 1;

                int x = 1;

                // csharpier-ignore
                int y=1;
            }
            """
            + "\n";

        var expectedCode = """
            void X()
            {
                int i = 1;

                int x = 1;

                // csharpier-ignore
                int y=1;
            }
            """
            + "\n";

        var result = await CSharpFormatter.FormatAsync(
            unformattedCode,
            new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict),
            CancellationToken.None
        );

        result.Code.Should().Be(expectedCode);
    }

    [Test]
    public async Task CSharpier_Ignore_Start_Preserves_Preceding_Blank_Line()
    {
        var unformattedCode = """
            void X()
            {
                int i = 1;

                int x = 1;

                // csharpier-ignore-start
                int y=1;
                // csharpier-ignore-end
            }
            """
            + "\n";

        var expectedCode = """
            void X()
            {
                int i = 1;

                int x = 1;

                // csharpier-ignore-start
                int y=1;
                // csharpier-ignore-end
            }
            """
            + "\n";

        var result = await CSharpFormatter.FormatAsync(
            unformattedCode,
            new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict),
            CancellationToken.None
        );

        result.Code.Should().Be(expectedCode);
    }
}
