using CSharpier.Core.CSharp.SyntaxPrinter;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.CSharp.SyntaxPrinter;

[TestFixture]
public class CSharpierIgnoreTests
{
    [Test]
    public void KeepLineBreaks()
    {
        var testCode = @"
1

2
".ReplaceLineEndings("\n");

        var result = PrintWithoutFormatting(testCode);

        result.Should().Be(testCode);
    }

    [Test]
    public void TrimTrailing()
    {
        var testCode = @"1    
2".ReplaceLineEndings("\n");

        var result = PrintWithoutFormatting(testCode);

        result
            .Should()
            .Be(
                @"1
2".ReplaceLineEndings("\n")
            );
    }

    [Test]
    public void FullProperty()
    {
        var testCode = @"
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
".ReplaceLineEndings("\n");

        var result = PrintWithoutFormatting(testCode);

        result.Should().Be(testCode);
    }

    private static string PrintWithoutFormatting(string code)
    {
        return CSharpierIgnore
            .PrintWithoutFormatting(
                code,
                new PrintingContext
                {
                    Options = new PrintingContext.PrintingContextOptions
                    {
                        LineEnding = Environment.NewLine,
                        IndentSize = 4,
                        UseTabs = false,
                    },

                    Information = new PrintingContext.CodeInformation(false, false),
                }
            )
            .ReplaceLineEndings("\n");
    }
}
