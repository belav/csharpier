using System.CommandLine;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using CSharpier.Cli;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class CliBenchmarks
{
    private RootCommand Command { get; }

    private string SolutionPath { get; }

    private string CsFiles { get; }

    public CliBenchmarks()
    {
        Command =
        [
            FormattingCommands.CreateFormatCommand(),
            FormattingCommands.CreateCheckCommand(),
            PipeCommand.Create(),
            ServerCommand.Create(),
        ];

        SolutionPath = GetCallerMethodRootFilePath();
        CsFiles = string.Join(
            " ",
            Directory
                .EnumerateFiles(SolutionPath, "*.cs", SearchOption.AllDirectories)
                .Where(path => !path.Contains(@"\obj\") && !path.Contains(@"\bin\"))
                .Select(x => $"\"{x}\"")
        );
    }

    private static string GetCallerMethodRootFilePath([CallerFilePath] string path = "")
    {
        const string target = @"\csharpier";
        var index = path.IndexOf(target, StringComparison.OrdinalIgnoreCase);
        return index != -1 ? path[..(index + target.Length)] : path;
    }

    [Benchmark]
    public async Task Format() =>
        await Command.InvokeAsync($@"format ""{SolutionPath}"" --no-cache");

    [Benchmark]
    public async Task FormatWithCache() => await Command.InvokeAsync($@"format ""{SolutionPath}""");

    [Benchmark]
    public async Task CheckFiles() => await Command.InvokeAsync($@"check {CsFiles}");
}
