using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace Generators;

// this should be in a shared project but the build hates that sometimes and it isn't worth figuring out right now
// see https://github.com/dotnet/roslyn/discussions/47517
public abstract class TemplatedGenerator : ISourceGenerator
{
    protected abstract string SourceName { get; }

    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var template = Template.Parse(this.GetContent(this.GetType().Name + ".sbntxt"));
        var renderedSource = template.Render(this.GetModel(context), member => member.Name);

        var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

        context.AddSource(this.SourceName, sourceText);
    }

    protected abstract object GetModel(GeneratorExecutionContext context);

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
