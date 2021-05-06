using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    [TestFixture]
    public class CodeFormatterTests
    {
        [TestCase(EndOfLine.LF, "\n")]
        [TestCase(EndOfLine.CRLF, "\r\n")]
        public void GetLineEndings_Should_Return_Easy_Cases(EndOfLine endOfLine, string expected)
        {
            var code = "tester\n";
            var result = CodeFormatter.GetLineEnding(
                code,
                new PrinterOptions() { EndOfLine = endOfLine }
            );

            result.Should().Be(expected);
        }

        [TestCase("tester\n", "\n")]
        [TestCase("tester\r\n", "\r\n")]
        [TestCase("\ntester\n", "\n")]
        [TestCase("tester", "\n")]
        public void GetLineEndings_With_Auto_Should_Detect(string code, string expected)
        {
            var result = CodeFormatter.GetLineEnding(code, new PrinterOptions());

            result.Should().Be(expected);
        }
    }
}
