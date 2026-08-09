using BenchmarkDotNet.Attributes;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using Microsoft.CodeAnalysis;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class CSharpBenchmarks
{
    [Benchmark]
    public void Default_CodeFormatter_Tests()
    {
        CSharpFormatter
            .FormatAsync(
                this.largeTestCode,
                new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict)
            )
            .GetAwaiter()
            .GetResult();
    }

    [Benchmark]
    public void Default_CodeFormatter_Complex()
    {
        CSharpFormatter
            .FormatAsync(
                this.largeComplexCode,
                new PrinterOptions(Formatter.CSharp, XmlWhitespaceSensitivity.Strict)
            )
            .GetAwaiter()
            .GetResult();
    }

    [Benchmark]
    public void Default_SyntaxNodeComparer()
    {
        var syntaxNodeComparer = new SyntaxNodeComparer(
            this.code,
            this.code,
            false,
            false,
            false,
            SourceCodeKind.Regular,
            CancellationToken.None
        );
        syntaxNodeComparer.CompareSource();
    }

    [Benchmark]
    public void IsCodeBasicallyEqual_SyntaxNodeComparer()
    {
        DisabledTextComparer.IsCodeBasicallyEqual(this.code, this.code);
    }

    private readonly string largeTestCode = File.ReadAllText(
        Path.Combine(Paths.RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/PathAxesTests.cs")
    );

    private readonly string largeComplexCode = File.ReadAllText(
        Path.Combine(Paths.RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Syntax.txt")
    );

    private readonly string code = File.ReadAllText(
        Path.Combine(Paths.RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Code.cs")
    );
}
