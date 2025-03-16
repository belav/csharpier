﻿using System.IO;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark]
    public void Default_CodeFormatter_Tests()
    {
        CSharpFormatter
            .FormatAsync(this.largeTestCode, new PrinterOptions())
            .GetAwaiter()
            .GetResult();
    }

    [Benchmark]
    public void Default_CodeFormatter_Complex()
    {
        CSharpFormatter
            .FormatAsync(this.largeComplexCode, new PrinterOptions())
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
        Path.Combine(RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/PathAxesTests.cs")
    );

    private readonly string largeComplexCode = File.ReadAllText(
        Path.Combine(RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Syntax.txt")
    );

    private readonly string code = File.ReadAllText(
        Path.Combine(RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Code.cs")
    );

    // benchmarks creates a new build inside of the bin and doesn't seem to respect CopyToOutputDirectory
    // finding the .git folder seems like the best way of dealing with the path to the code files
    private static string RepoRoot { get; } = GetRepoRoot();

    private static string GetRepoRoot()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (
            currentDirectory != null
            && !Directory.Exists(Path.Combine(currentDirectory.FullName, ".git"))
        )
        {
            currentDirectory = currentDirectory.Parent;
        }

        if (currentDirectory is null)
        {
            throw new Exception("Could not find .git directory");
        }

        return currentDirectory.FullName;
    }
}

public static class Program
{
    public static void Main()
    {
        _ = BenchmarkRunner.Run<Benchmarks>();
    }
}
