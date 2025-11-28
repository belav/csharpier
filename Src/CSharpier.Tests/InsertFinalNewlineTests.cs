using CSharpier.Core;
using CSharpier.Core.CSharp;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal sealed class InsertFinalNewlineTests
{
    [Test]
    public async Task InsertFinalNewlineTests_Should_Add_Newlines_When_Not_Defined()
    {
        var code =
            @"class ClassName
{
    private string blah = @""hello, world"";
}";
        var printerOptions = new PrinterOptions(Formatter.CSharp) {};

        var result = await CSharpFormatter.FormatAsync(code, printerOptions);

        var expected = $"{code}\n";
        result.Code.Should().Be(expected);
    }

    [Test]
    public async Task InsertFinalNewlineTests_Should_Add_Newlines()
    {
        var code =
            @"class ClassName
{
    private string blah = @""hello, world"";
}";

        var printerOptions = new PrinterOptions(Formatter.CSharp) { InsertFinalNewline = true };

        var result = await CSharpFormatter.FormatAsync(code, printerOptions);

        var expected = $"{code}\n";
        result.Code.Should().Be(expected);
    }

    [Test]
    public async Task InsertFinalNewlineTests_Should_Remove_Newlines()
    {
        var code =
            @"class ClassName
{
    private string blah = @""hello, world"";
}
";

        var printerOptions = new PrinterOptions(Formatter.CSharp) { InsertFinalNewline = false };

        var result = await CSharpFormatter.FormatAsync(code, printerOptions);

        var expected = code.TrimEnd('\r', '\n');
        result.Code.Should().Be(expected);
    }
}
