using NUnit.Framework;

namespace CSharpier.Tests;

using CSharpier.SyntaxPrinter;
using FluentAssertions;

[TestFixture]
public class CSharpierIgnoreTests
{
    [Test]
    public void KeepLineBreaks()
    {
        var testCode =
            @"
1

2
";

        var result = PrintWithoutFormatting(testCode);

        result.Should().Be(testCode);
    }

    [Test]
    public void TrimTrailing()
    {
        var testCode =
            @"1    
2";

        var result = PrintWithoutFormatting(testCode);

        result
            .Should()
            .Be(
                @"1
2"
            );
    }

    [Test]
    public void FullProperty()
    {
        var testCode =
            @"
// csharpier-ignore
public string Example
{
  get
     {
       if (_example is not null)
         return _example;

       var number = Random.Shared.Next();

       return _example = number.ToString();
     }
}
";

        var result = PrintWithoutFormatting(testCode);

        result.Should().Be(testCode);
    }

    private string PrintWithoutFormatting(string code)
    {
        return CSharpierIgnore.PrintWithoutFormatting(
            code,
            new FormattingContext { LineEnding = Environment.NewLine }
        );
    }
}
