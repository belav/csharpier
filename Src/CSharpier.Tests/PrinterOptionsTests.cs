using AwesomeAssertions;
using CSharpier.Core;

namespace CSharpier.Tests;

internal sealed class PrinterOptionsTests
{
    [Test]
    [Arguments(EndOfLine.LF, "\n")]
    [Arguments(EndOfLine.CRLF, "\r\n")]
    public void GetLineEndings_Should_Return_Easy_Cases(EndOfLine endOfLine, string expected)
    {
        var code = "tester\n";
        var result = PrinterOptions.GetLineEnding(
            code,
            new PrinterOptions(Formatter.CSharp) { EndOfLine = endOfLine }
        );

        result.Should().Be(expected);
    }

    [Test]
    [Arguments("tester\n", "\n")]
    [Arguments("tester\r\n", "\r\n")]
    [Arguments("\ntester\n", "\n")]
    [Arguments("tester", "\n")]
    public void GetLineEndings_With_Auto_Should_Detect(string code, string expected)
    {
        var result = PrinterOptions.GetLineEnding(code, new PrinterOptions(Formatter.CSharp));

        result.Should().Be(expected);
    }
}
