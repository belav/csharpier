#pragma warning disable RSEXPERIMENTAL006

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Generators;

[Generator]
public class NodePrinterGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxTrees = context.CompilationProvider.Select(
            (compilation, _) =>
                compilation
                    .SyntaxTrees.Where(o => o.FilePath.Contains("SyntaxNodePrinters"))
                    .ToImmutableArray()
        );

        context.RegisterSourceOutput(syntaxTrees, GenerateSource);
    }

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<SyntaxTree> syntaxTrees
    )
    {
        if (syntaxTrees.Length == 0)
        {
            return;
        }

        var generator = new NodePrinterGenerator();
        var template = Template.Parse(generator.GetContent(generator.GetType().Name + ".sbntxt"));
        var renderedSource = template.Render(
            generator.GetModel(syntaxTrees),
            member => member.Name
        );

        var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

        context.AddSource("Node", sourceText);
    }

    public string GetContent(string relativePath)
    {
        var assembly = this.GetType().Assembly;
        var baseName = assembly.GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        var name = baseName + "." + resourceName;
        using var stream = assembly.GetManifestResourceStream(name);

        if (stream == null)
        {
            var list = assembly.GetManifestResourceNames();

            throw new Exception(
                $"No embedded resource found with the name {name}. Resources found are "
                    + string.Join(", ", list)
            );
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private object GetModel(ImmutableArray<SyntaxTree> syntaxTrees)
    {
        var nodeTypes = syntaxTrees
            .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
            .Select(fileName => new
            {
                PrinterName = fileName,
                SyntaxNodeName = $"{fileName}Syntax",
                VariableName = char.ToLower(fileName[0]) + fileName[1..],
            })
            .OrderBy(o => o.SyntaxNodeName)
            .ToArray();

        return new { NodeTypes = nodeTypes };
    }
}
