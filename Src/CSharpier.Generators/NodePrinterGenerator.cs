namespace CSharpier.Generators;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

// the magic command to get source generators to actually regenerate when they get stuck with old code
// dotnet build-server shutdown
[Generator]
public class NodePrinterGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var template = Template.Parse(this.GetContent(this.GetType().Name + ".sbntxt"));
        var renderedSource = template.Render(this.GetModel(context), member => member.Name);

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

    private object GetModel(GeneratorExecutionContext context)
    {
        var nodeTypes = context.Compilation.SyntaxTrees
            .Where(o => o.FilePath.Contains("SyntaxNodePrinters"))
            .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
            .Select(
                fileName =>
                    new
                    {
                        PrinterName = fileName,
                        SyntaxNodeName = fileName + "Syntax",
                        VariableName = char.ToLower(fileName[0]) + fileName[1..]
                    }
            )
            .OrderBy(o => o.SyntaxNodeName)
            .ToArray();

        return new { NodeTypes = nodeTypes };
    }
}
