using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Tests.Generators;

[Generator]
public class FormattingTestsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var additionalFiles = context
            .AdditionalTextsProvider.Where(file =>
                file.Path.EndsWith(".test")
                && !file.Path.EndsWith(".actual.test")
                && !file.Path.EndsWith(".expected.test")
            )
            .Collect();

        context.RegisterSourceOutput(additionalFiles, GenerateSource);
    }

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<AdditionalText> additionalFiles
    )
    {
        var generator = new FormattingTestsGenerator();
        var template = Template.Parse(generator.GetContent(generator.GetType().Name + ".sbntxt"));

        var extensions = additionalFiles
            .Select(o => new FileInfo(o.Path).Directory!.Name)
            .Distinct();

        foreach (var extension in extensions)
        {
            var renderedSource = template.Render(
                generator.GetModel(additionalFiles, extension),
                member => member.Name
            );

            var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

            context.AddSource("FormattingTests_" + extension, sourceText);
        }
    }

    protected object GetModel(ImmutableArray<AdditionalText> additionalFiles, string extension)
    {
        var tests = additionalFiles
            .Where(o => new FileInfo(o.Path).Directory!.Name == extension)
            .Select(o => new
            {
                Name = Path.GetFileNameWithoutExtension(o.Path),
                FileExtension = new FileInfo(o.Path).Directory!.Name,
                UseTabs = Path.GetFileNameWithoutExtension(o.Path).EndsWith("_Tabs"),
            });

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
