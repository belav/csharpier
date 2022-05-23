using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests;

[TestFixture]
public class ConditionalServiceTests
{
    [Test]
    public void GetSymbolSets_Should_Handle_Basic_If()
    {
        RunTest(
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
        RunTest(
            @"#if ONE && TWO
public class Tester { }
#endif
",
            new[] { "DEBUG" }
        );
    }
    
    [Test]
    public void GetSymbolSets_Should_Handle_Or()
    {
        RunTest(
            @"#if ONE || TWO
public class Tester { }
#endif
",
            new[] { "DEBUG" }
        );
    }

    [Test]
    public void GetSymbolSets_Should_Handle_Basic_Not_If()
    {
        RunTest(
            @"#if !DEBUG
public class Tester { }
#endif
",
            Array.Empty<string>()
        );
    }

    [Test]
    public void GetSymbolSets_Should_Handle_Basic_If_With_Else()
    {
        RunTest(
            @"#if DEBUG
public class Tester { }
#else
public class Tester2 { }
#endif
",
            new[] { "DEBUG" },
            Array.Empty<string>()
        );
    }

    [Test]
    public void GetSymbolSets_Should_Handle_Basic_If_With_ElseIf()
    {
        RunTest(
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

    private void RunTest(string code, params string[][] symbolSets)
    {
        var result = ConditionalService.GetSymbolSets(code);

        result.Should().HaveCount(symbolSets.Length);
        for (var x = 0; x < symbolSets.Length; x++)
        {
            result[x].Should().HaveCount(symbolSets[x].Length);
            foreach (var symbol in symbolSets[x])
            {
                result[x].Should().Contain(symbol);
            }
        }
    }
}
