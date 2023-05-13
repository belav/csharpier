namespace CSharpier.Tests.Formatters.CSharp;

using CSharpier.Formatters.CSharp;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class ConditionalServiceTests
{
    [Test]
    public void GetSymbolSets_Should_Handle_Basic_If()
    {
        this.RunTest(
            @"#if DEBUG
public class Tester { }
#endif
",
            new[] { "DEBUG" }
        );
    }

    [Test]
    public void GetSymbolSets_Should_Handle_And()
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
    public void GetSymbolSets_Should_Handle_Or()
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
    public void GetSymbolSets_Should_Handle_Basic_Not_If()
    {
        this.RunTest(
            @"#if !DEBUG
public class Tester { }
#endif
"
        );
    }

    [Test]
    public void GetSymbolSets_Should_Handle_Basic_If_With_Else()
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
    public void GetSymbolSets_Should_Handle_Basic_If_With_ElseIf()
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
    public void GetSymbolSets_Should_Handle_Parens()
    {
        this.RunTest(
            @"#if (ONE || TWO) && THREE
public class Tester { }
#endif
",
            new[] { "ONE", "THREE" }
        );
    }

    // TODO lots more tests

    private void RunTest(string code, params string[][] symbolSets)
    {
        // TODO we should always include an empty symbol set
        var result = ConditionalService.GetSymbolSets(code);

        result.Should().BeEquivalentTo(symbolSets);
    }
}
