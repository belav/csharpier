using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests.Samples;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Samples
{
    [Test]
    public void Scratch()
    {
        RunTest("Scratch");
    }

    [Test]
    public void AllInOne()
    {
        RunTest("AllInOne");
    }

    public static void RunTest(string fileName)
    {
        var directory = DirectoryFinder.FindParent("CSharpier.Tests");

        var file = Path.Combine(directory.FullName, $"Samples/{fileName}.cst");
        if (!File.Exists(file))
        {
            File.WriteAllText(file, "");
        }

        var code = File.ReadAllText(file);
        var result = CodeFormatter.Format(
            code,
            new PrinterOptions { IncludeDocTree = true, IncludeAST = true, }
        );

        var syntaxNodeComparer = new SyntaxNodeComparer(code, result.Code, CancellationToken.None);

        var compareResult = syntaxNodeComparer.CompareSource();
        compareResult.Should().BeEmpty();

        File.WriteAllText(file.Replace(".cst", ".actual.cst"), result.Code, Encoding.UTF8);
        File.WriteAllText(file.Replace(".cst", ".doctree.txt"), result.DocTree, Encoding.UTF8);
    }
}
