#pragma warning disable RSEXPERIMENTAL006

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Generators;

[Generator]
public class PowerShellNodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var printers = context.CompilationProvider.Select(
            (compilation, _) =>
                compilation
                    .SyntaxTrees.Where(o => o.FilePath.Contains("AstPrinters"))
                    .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
                    .Select(o => new Printer(o, GetInheritanceDepth(compilation, o)))
                    .ToImmutableArray()
        );

        context.RegisterSourceOutput(printers, GenerateSource);
    }

    private readonly record struct Printer(string Name, int InheritanceDepth);

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<Printer> printers
    )
    {
        if (printers.Length == 0)
        {
            return;
        }

        var generator = new PowerShellNodeGenerator();
        var template = Template.Parse(generator.GetContent(generator.GetType().Name + ".sbntxt"));
        var renderedSource = template.Render(generator.GetModel(printers), member => member.Name);

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

    // The switch in Node.Print matches the first arm that fits, so a printer for a derived Ast type
    // has to come before the printer for its base type.
    private static int GetInheritanceDepth(Compilation compilation, string printerName)
    {
        var depth = 0;
        var type = compilation.GetTypeByMetadataName(
            $"System.Management.Automation.Language.{printerName}Ast"
        );
        while (type != null)
        {
            depth++;
            type = type.BaseType;
        }

        return depth;
    }

    private object GetModel(ImmutableArray<Printer> printers)
    {
        var nodeTypes = printers
            .Select(o => new
            {
                PrinterName = o.Name,
                SyntaxNodeName = $"{o.Name}Ast",
                VariableName = char.ToLower(o.Name[0]) + o.Name[1..],
                o.InheritanceDepth,
            })
            .OrderByDescending(o => o.InheritanceDepth)
            .ThenBy(o => o.SyntaxNodeName)
            .ToArray();

        return new { NodeTypes = nodeTypes };
    }
}
