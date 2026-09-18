using System.Text;
using AwesomeAssertions;
using CSharpier.Core.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Tests.CSharp;

public class CompareFullSpanTests
{
    [Test]
    public void Should_Be_Equal_For_Identical_Text_From_Distinct_Strings()
    {
        var original = "class ClassName { }";
        var formatted = new StringBuilder(original).ToString();

        ReferenceEquals(original, formatted).Should().BeFalse();

        CompareFullSpan(original, 0, formatted, 0).Should().BeTrue();
    }

    [Test]
    public void Should_Be_Equal_For_Identical_Text_At_A_Shifted_Offset()
    {
        var original = "class Other { }class ClassName { }";
        var formatted = "class ClassName { }";

        CompareFullSpan(original, 1, formatted, 0).Should().BeTrue();
    }

    [Test]
    public void Should_Not_Be_Equal_For_Differing_Text()
    {
        var original = "class ClassName { }";
        var formatted = "class OtherName { }";

        CompareFullSpan(original, 0, formatted, 0).Should().BeFalse();
    }

    private static bool CompareFullSpan(
        string original,
        int originalIndex,
        string formatted,
        int formattedIndex
    )
    {
        var comparer = new SyntaxNodeComparer(
            original,
            formatted,
            false,
            false,
            false,
            SourceCodeKind.Regular,
            CancellationToken.None
        );

        return comparer.CompareFullSpan(
            ClassAt(original, originalIndex),
            ClassAt(formatted, formattedIndex)
        );
    }

    private static ClassDeclarationSyntax ClassAt(string code, int index)
    {
        return CSharpSyntaxTree
            .ParseText(code)
            .GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .ElementAt(index);
    }
}
