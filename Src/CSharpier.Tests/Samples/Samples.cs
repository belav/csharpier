using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Samples;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Samples
{
    [Test]
    public async Task Scratch()
    {
        await RunTest("Scratch");
    }

    [Test]
    public async Task AllInOne()
    {
        await RunTest("AllInOne");
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
            new PrinterOptions { IncludeDocTree = true, IncludeAST = true }
        );

        var syntaxNodeComparer = new SyntaxNodeComparer(
            code,
            result.Code,
            false,
            false,
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
