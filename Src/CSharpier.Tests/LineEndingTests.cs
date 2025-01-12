using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class LineEndingTests
{
    [Test]
    public async Task LineEndings_Should_Not_Affect_Printed_Output_With_Verbatim_String()
    {
        // this is a verbatim string that is just the right size to format differently if it has \n vs \r\n in it
        var code =
            @"class ClassName
{
    private string blah = @""one11111111111111111111111111111111
two
three
four"";
}
";
        var codeWithLf = code.Replace("\r\n", "\n");
        var codeWithCrLf = codeWithLf.Replace("\n", "\r\n");

        var printerOptions = new PrinterOptions(Formatter.CSharp)
        {
            EndOfLine = EndOfLine.Auto,
            Width = 80,
        };
        var lfResult = await CSharpFormatter.FormatAsync(codeWithLf, printerOptions);
        var crLfResult = await CSharpFormatter.FormatAsync(codeWithCrLf, printerOptions);

        lfResult.Code.Should().Be(crLfResult.Code.Replace("\r\n", "\n"));
    }

    [Test]
    public async Task LineEndings_Should_Not_Affect_Printed_Output_With_Interpolated_String()
    {
        // this is a interpolated verbatim string that is just the right size to format differently if it has \n vs \r\n in it
        var code =
            @"class ClassName
{
    private string blah = @$""one1111111111111111111111111111111
two
three
four"";
}
";
        var codeWithLf = code.Replace("\r\n", "\n");
        var codeWithCrLf = codeWithLf.Replace("\n", "\r\n");

        var printerOptions = new PrinterOptions(Formatter.CSharp)
        {
            EndOfLine = EndOfLine.Auto,
            Width = 80,
        };
        var lfResult = await CSharpFormatter.FormatAsync(codeWithLf, printerOptions);
        var crLfResult = await CSharpFormatter.FormatAsync(codeWithCrLf, printerOptions);

        lfResult.Code.Should().Be(crLfResult.Code.Replace("\r\n", "\n"));
    }

    [TestCase("\r\n", EndOfLine.LF)]
    [TestCase("\n", EndOfLine.CRLF)]
    public async Task LineEndings_In_Verbatim_String_Should_Respect_Options(
        string newLine,
        EndOfLine endOfLine
    )
    {
        var code =
            @$"class ClassName
{{
    string value = @""one{newLine}two"";
}}
";
        var printerOptions = new PrinterOptions(Formatter.CSharp) { EndOfLine = endOfLine };
        var result = await CSharpFormatter.FormatAsync(code, printerOptions);
        result.Code.Should().NotContain($"one{newLine}two");
    }

    [TestCase("\\r\\n", EndOfLine.LF)]
    [TestCase("\\n", EndOfLine.CRLF)]
    public async Task Escaped_LineEndings_In_Verbatim_String_Should_Remain(
        string escapedNewLine,
        EndOfLine endOfLine
    )
    {
        var code =
            @$"class ClassName
{{
    string value = @""one{escapedNewLine}two"";
}}
";
        var printerOptions = new PrinterOptions(Formatter.CSharp) { EndOfLine = endOfLine };
        var result = await CSharpFormatter.FormatAsync(code, printerOptions);
        result.Code.Should().Contain(escapedNewLine);
    }
}
