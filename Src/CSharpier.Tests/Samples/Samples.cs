using System.Text;
using AwesomeAssertions;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using Microsoft.CodeAnalysis;

namespace CSharpier.Tests.Samples;

public class Samples
{
    [Test]
    public async Task Scratch()
    {
        await RunTest("Scratch");
    }

    public static async Task RunTest(string fileName)
    {
        var directory = DirectoryFinder.FindParent("CSharpier.Tests");

        var file = Path.Combine(directory.FullName, $"Samples/{fileName}.test");
        if (!File.Exists(file))
        {
            await File.WriteAllTextAsync(file, "");
        }

        var code = await File.ReadAllTextAsync(file);
        var result = await CSharpFormatter.FormatAsync(
            code,
            new PrinterOptions(Formatter.CSharp) { IncludeDocTree = true, IncludeAST = true }
        );

        var syntaxNodeComparer = new SyntaxNodeComparer(
            code,
            result.Code,
            false,
            false,
            false,
            SourceCodeKind.Regular,
            CancellationToken.None
        );

        var compareResult = syntaxNodeComparer.CompareSource();
        compareResult.Should().BeEmpty();

        await File.WriteAllTextAsync(
            file.Replace(".test", ".actual.test"),
            result.Code,
            Encoding.UTF8
        );
        await File.WriteAllTextAsync(
            file.Replace(".test", ".doctree.txt"),
            result.DocTree,
            Encoding.UTF8
        );
    }
}
