namespace CSharpier.Tests.Formatters.CSharp;

using CSharpier.Formatters.CSharp;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class PreprocessorSymbolsTests
{
    [TestCase("DEBUG")]
    [TestCase("NET48")]
    [TestCase("NET48_OR_GREATER")]
    public void GetSets_Should_Handle_Basic_If(string symbol)
    {
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
            @"#if !DEBUG
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_If_With_Else()
    {
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
            @"#if ONE == false
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_True()
    {
        this.RunTest(
            @"#if ONE != true
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_False()
    {
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
            @"#if ONE
#if TWO
// ONE,TWO
#endif
#endif
",
            // TODO because the second is a subset of the first, we don't actually need it
            // but it's more work to figure that out for now
            "ONE,TWO",
            "ONE"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Duplicate_If()
    {
        this.RunTest(
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
        this.RunTest(
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
        this.RunTest(
            @"#if true
//
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_And_With_Not()
    {
        this.RunTest(
            @"
#if ONE && !TWO
// ONE
#endif
",
            "ONE"
        );
    }

    private void RunTest(string code, params string[] symbolSets)
    {
        var result = PreprocessorSymbols.GetSets(code);

        var actualSymbolSets = symbolSets.Select(o => o.Split(',').ToList()).ToList();

        result.Should().BeEquivalentTo(actualSymbolSets);
    }
}
