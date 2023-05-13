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
public class Tester {{ }}
#endif
",
            new[] { symbol }
        );
    }

    [Test]
    public void GetSets_Should_Handle_And()
    {
        this.RunTest(
            @"#if ONE && TWO
public class Tester { }
#endif
",
            new[] { "ONE", "TWO" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Or()
    {
        this.RunTest(
            @"#if ONE || TWO
public class Tester { }
#endif
",
            new[] { "ONE" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_Not_If()
    {
        this.RunTest(
            @"#if !DEBUG
public class Tester { }
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_If_With_Else()
    {
        this.RunTest(
            @"#if DEBUG
public class Tester { }
#else
public class Tester2 { }
#endif
",
            new[] { "DEBUG" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Basic_If_With_ElseIf()
    {
        this.RunTest(
            @"#if ONE
public class Tester { }
#elif TWO
public class Tester2 { }
#endif
",
            new[] { "ONE" },
            new[] { "TWO" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Parens()
    {
        this.RunTest(
            @"#if (ONE || TWO) && THREE
public class Tester { }
#endif
",
            new[] { "ONE", "THREE" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Equals_True()
    {
        this.RunTest(
            @"#if EQUALS_TRUE == true
public class Tester { }
#endif
",
            new[] { "EQUALS_TRUE" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Equals_False()
    {
        this.RunTest(
            @"#if EQUALS_TRUE == false
public class Tester { }
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_True()
    {
        this.RunTest(
            @"#if NOT_EQUALS_TRUE != true
public class Tester { }
#endif
"
        );
    }

    [Test]
    public void GetSets_Should_Handle_NotEquals_False()
    {
        this.RunTest(
            @"#if NOT_EQUALS_FALSE != false
public class Tester { }
#endif
",
            new[] { "NOT_EQUALS_FALSE" }
        );
    }

    [Test]
    public void GetSets_Should_Handle_Else()
    {
        // TODO we need to keep track of symbols for the whole if/elseif/else block
        // TODO also need a test that uses an elseif and an else
        // TODO and also an else
        // TODO and we need to make sure we can get into the elseif and else if at all possible
        this.RunTest(
            @"#if !NOT_IF
public class Tester1 { }
#else
public class Tester2 { }
#endif
",
            new[] { "NOT_IF" }
        );
    }

    // TODO lots more tests

    private void RunTest(string code, params string[][] symbolSets)
    {
        var result = PreprocessorSymbols.GetSets(code);

        result.Should().BeEquivalentTo(symbolSets);
    }
}
