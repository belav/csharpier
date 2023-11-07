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
            new PrinterOptions { EndOfLine = endOfLine },
            CancellationToken.None
        );

        result
            .Code
            .Should()
            .Be(unformattedCode.ReplaceLineEndings(endOfLine == EndOfLine.LF ? "\n" : "\r\n"));
    }
}
