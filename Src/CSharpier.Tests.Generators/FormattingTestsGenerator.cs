using Microsoft.CodeAnalysis;

namespace CSharpier.Tests.Generators;

using System.Text;
using Microsoft.CodeAnalysis.Text;
using Scriban;

// the magic command to get source generators to actually regenerate when they get stuck with old code
// dotnet build-server shutdown
[Generator]
public class FormattingTestsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var template = Template.Parse(this.GetContent(this.GetType().Name + ".sbntxt"));

        var extensions = this.GetExtensions(context).ToList();
        foreach (var extension in extensions)
        {
            var renderedSource = template.Render(
                this.GetModel(context, extension),
                member => member.Name
            );

            var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

            context.AddSource("FormattingTests_" + extension, sourceText);
        }
    }

    private IEnumerable<string> GetExtensions(GeneratorExecutionContext context)
    {
        return context.AdditionalFiles
            .Where(
                o =>
                    o.Path.EndsWith(".test")
                    && !o.Path.EndsWith(".actual.test")
                    && !o.Path.EndsWith(".expected.test")
            )
            .Select(o => new FileInfo(o.Path).Directory!.Name)
            .Distinct();
    }

    protected object GetModel(GeneratorExecutionContext context, string extension)
    {
        var tests = context.AdditionalFiles
            .Where(
                o =>
                    o.Path.EndsWith(".test")
                    && !o.Path.EndsWith(".actual.test")
                    && !o.Path.EndsWith(".expected.test")
                    && new FileInfo(o.Path).Directory!.Name == extension
            )
            .Select(
                o =>
                    new
                    {
                        Name = Path.GetFileNameWithoutExtension(o.Path),
                        FileExtension = new FileInfo(o.Path).Directory!.Name,
                        UseTabs = Path.GetFileNameWithoutExtension(o.Path).EndsWith("_Tabs")
                    }
            );

        return new { Tests = tests, FileExtension = extension };
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
}
