using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
internal class CodeFormatterTests
{
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
