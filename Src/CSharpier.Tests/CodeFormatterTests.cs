using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace CSharpier.Tests;

// TODO xml move these around
[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class CodeFormatterTests
{
    [TestCase(EndOfLine.LF, "\n")]
    [TestCase(EndOfLine.CRLF, "\r\n")]
    public void GetLineEndings_Should_Return_Easy_Cases(EndOfLine endOfLine, string expected)
    {
        var code = "tester\n";
        var result = PrinterOptions.GetLineEnding(
            code,
            new PrinterOptions { EndOfLine = endOfLine }
        );

        result.Should().Be(expected);
    }

    [TestCase("tester\n", "\n")]
    [TestCase("tester\r\n", "\r\n")]
    [TestCase("\ntester\n", "\n")]
    [TestCase("tester", "\n")]
    public void GetLineEndings_With_Auto_Should_Detect(string code, string expected)
    {
        var result = PrinterOptions.GetLineEnding(code, new PrinterOptions());

        result.Should().Be(expected);
    }

    [Test]
    public void Format_Should_Use_Default_Width()
    {
        var code = "var someVariable = someValue;";
        var result = CodeFormatter.Format(code);

        result.Code.Should().Be("var someVariable = someValue;\n");
        result.CompilationErrors.Should().BeEmpty();
    }

    [Test]
    public void Format_Should_Return_Compilation_Errors()
    {
        var code = "var someVariable = someValue";
        var result = CodeFormatter.Format(code);

        result.Code.Should().Be(code);
        result.CompilationErrors.Should().ContainSingle();
    }

    [Test]
    public void Format_Should_Use_Width()
    {
        var code = "var someVariable = someValue;";
        var result = CodeFormatter.Format(code, new CodeFormatterOptions { Width = 10 });

        result.Code.Should().Be("var someVariable =\n    someValue;\n");
    }

    [Test]
    public void Format_Should_Use_IndentStyle()
    {
        var code = "void someMethod() { var someVariable = someValue; }";
        var result = CodeFormatter.Format(
            code,
            new CodeFormatterOptions { IndentStyle = IndentStyle.Tabs, IndentSize = 1 }
        );

        result.Code.Should().Be("void someMethod()\n{\n\tvar someVariable = someValue;\n}\n");
    }

    [Test]
    public void Format_Should_Use_IndentWidth()
    {
        var code = "void someMethod() { var someVariable = someValue; }";
        var result = CodeFormatter.Format(
            code,
            new CodeFormatterOptions { IndentStyle = IndentStyle.Spaces, IndentSize = 1 }
        );

        result.Code.Should().Be("void someMethod()\n{\n var someVariable = someValue;\n}\n");
    }

    [Test]
    public void Format_Should_Use_EndOfLine()
    {
        var code = "var someVariable = someValue;";
        var result = CodeFormatter.Format(
            code,
            new CodeFormatterOptions { EndOfLine = EndOfLine.CRLF }
        );

        result.Code.Should().Be("var someVariable = someValue;\r\n");
    }

    [TestCase("\n")]
    [TestCase("\r\n")]
    public void Format_Should_Get_Line_Endings_With_SyntaxTree(string lineEnding)
    {
        var code = $"public class ClassName {{{lineEnding}}}";
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var result = CodeFormatter.Format(syntaxTree);

        result.Code.Should().Be("public class ClassName { }" + lineEnding);
    }
}
