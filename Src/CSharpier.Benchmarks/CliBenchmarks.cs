#pragma warning disable CA1822

using System.CommandLine;
using BenchmarkDotNet.Attributes;
using CSharpier.Cli;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class CliBenchmarks
{
    private static readonly string CsFiles = string.Join(
        " ",
        Directory
            .EnumerateFiles(
                Path.Combine(Paths.RepoRoot, "Src", "CSharpier.Core"),
                "*.cs",
                SearchOption.AllDirectories
            )
            .Select(o => $"\"{o}\"")
    );

    [Benchmark]
    public async Task Format()
    {
        await FormattingCommands.CreateFormatCommand().InvokeAsync($"{Paths.RepoRoot} --no-cache");
    }

    [Benchmark]
    public async Task FormatWithCache()
    {
        await FormattingCommands.CreateFormatCommand().InvokeAsync(Paths.RepoRoot);
    }

    [Benchmark]
    public async Task CheckFiles()
    {
        await FormattingCommands.CreateCheckCommand().InvokeAsync($"check {CsFiles}");
    }
}
