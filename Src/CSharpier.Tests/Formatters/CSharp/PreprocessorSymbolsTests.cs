using CSharpier.Formatters.CSharp;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Formatters.CSharp;

[TestFixture]
public class PreprocessorSymbolsTests
{
    [TestCase("DEBUG")]
    [TestCase("NET48")]
    [TestCase("NET48_OR_GREATER")]
    public void GetSets_Should_Handle_Basic_If(string symbol)
    {
        RunTest(
            $@"#if {symbol}
// {symbol}
#endif
",
            symbol
        );
    }

    [Test]
    public void GetSets_Should_Handle_And()
    {
        RunTest(
            @"#if ONE && TWO
// ONE,TWO
#endif
",
            "ONE,TWO"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Or()
    {
        RunTest(
            @"#if ONE || TWO
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_Not_If()
    {
        RunTest(
            @"#if !DEBUG
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_If_With_Else()
    {
        RunTest(
            @"#if DEBUG
// DEBUG
#else
//
#endif
",
            "DEBUG"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_If_With_ElseIf()
    {
        RunTest(
            @"#if ONE
// ONE
#elif TWO
// TWO
#endif
",
            "ONE",
            "TWO"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Parens()
    {
        RunTest(
            @"#if (ONE || TWO) && THREE
// ONE,THREE
#endif
",
            "ONE,THREE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Equals_True()
    {
        RunTest(
            @"#if ONE == true
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Equals_False()
    {
        RunTest(
            @"#if ONE == false
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_True()
    {
        RunTest(
            @"#if ONE != true
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_False()
    {
        RunTest(
            @"#if ONE != false
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Else()
    {
        RunTest(
            @"#if !ONE
//
#else
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Else_If()
    {
        RunTest(
            @"#if ONE
// ONE
#elif TWO
// TWO
#endif
",
            "ONE",
            "TWO"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Else_If_And_Else()
    {
        RunTest(
            @"#if ONE
// ONE
#elif !TWO
// !TWO
#else
// TWO
#endif
",
            "ONE",
            "TWO"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Nested_If()
    {
        RunTest(
            @"#if ONE
#if TWO
// ONE,TWO
#endif
#endif
",
            "ONE,TWO",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Duplicate_If()
    {
        RunTest(
            @"#if ONE
// ONE
#endif
#if ONE
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Comments()
    {
        RunTest(
            @"#if ONE // comment
// ONE
#endif
",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_true()
    {
        RunTest(
            @"#if true
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_And_With_Not()
    {
        RunTest(
            @"
#if ONE && !TWO
// ONE
#endif
",
            "ONE"
        );
    }

    private static void RunTest(string code, params string[] symbolSets)
    {
        var result = PreprocessorSymbols.GetSets(code);

        var actualSymbolSets = symbolSets.Select(o => o.Split(',').ToList()).ToList();

        result.Should().BeEquivalentTo(actualSymbolSets);
    }
}
