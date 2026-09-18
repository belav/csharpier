using AwesomeAssertions;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Tests.Utilities;

public class DocListBuilderTests
{
    [Test]
    public void Every_Construction_Site_Should_Be_Disposed()
    {
        var coreDirectory = Path.Combine(
            DirectoryFinder.FindParent("Src").FullName,
            "CSharpier.Core"
        );

        var leakingSites = new List<string>();

        foreach (
            var file in Directory.EnumerateFiles(coreDirectory, "*.cs", SearchOption.AllDirectories)
        )
        {
            var relativePath = Path.GetRelativePath(coreDirectory, file);
            if (IsBuildOutput(relativePath))
            {
                continue;
            }

            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();

            foreach (
                var creation in root.DescendantNodes().OfType<ObjectCreationExpressionSyntax>()
            )
            {
                if (creation.Type.ToString() != nameof(DocListBuilder))
                {
                    continue;
                }

                var declaration = creation.FirstAncestorOrSelf<LocalDeclarationStatementSyntax>();
                if (declaration?.UsingKeyword.IsKind(SyntaxKind.UsingKeyword) != true)
                {
                    var line = creation.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    leakingSites.Add($"{relativePath}:{line}");
                }
            }
        }

        leakingSites.Should().BeEmpty();
    }

    [Test]
    public void Dispose_Should_Clear_The_Span()
    {
        var builder = new DocListBuilder(4);
        builder.Add(Doc.Null);
        builder.Dispose();

        var threw = false;
        try
        {
            _ = builder.AsSpan();
        }
        catch (ArgumentOutOfRangeException)
        {
            threw = true;
        }

        threw.Should().BeTrue();
    }

    private static bool IsBuildOutput(string relativePath)
    {
        return relativePath.StartsWith(
                "obj" + Path.DirectorySeparatorChar,
                StringComparison.Ordinal
            )
            || relativePath.StartsWith(
                "bin" + Path.DirectorySeparatorChar,
                StringComparison.Ordinal
            );
    }
}
