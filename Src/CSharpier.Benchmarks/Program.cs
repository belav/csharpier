using System.Xml;
using System.Xml.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using CSharpier.Core.Xml;
using Microsoft.CodeAnalysis;

namespace CSharpier.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark]
    public void XmlDocument_Parse()
    {
        var root = new XmlDocument();
        root.LoadXml(this.largeXmlCode);
    }

    [Benchmark]
    public void XDocument_Parse()
    {
        _ = XDocument.Parse(this.largeXmlCode);
    }

    [Benchmark]
    public void CustomParser_Parse()
    {
        _ = RawNodeReader.ReadAll(this.largeXmlCode, Environment.NewLine);
    }

    [Benchmark]
    public void XmlReader_Parse()
    {
        using var xmlReader = XmlReader.Create(
            new StringReader(this.largeXmlCode),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        while (xmlReader.Read())
        {
            //
        }
    }

    [Benchmark]
    public void Default_CodeFormatter_Tests()
    {
        CSharpFormatter
            .FormatAsync(this.largeTestCode, new PrinterOptions(Formatter.CSharp))
            .GetAwaiter()
            .GetResult();
    }

    [Benchmark]
    public void Default_CodeFormatter_Complex()
    {
        CSharpFormatter
            .FormatAsync(this.largeComplexCode, new PrinterOptions(Formatter.CSharp))
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

    private readonly string largeXmlCode = File.ReadAllText(
        Path.Combine(RepoRoot, "Src/CSharpier.BenchMarks/CodeSamples/Type.xml")
    );

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
