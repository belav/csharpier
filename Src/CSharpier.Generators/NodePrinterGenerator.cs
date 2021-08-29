using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Generators
{
    [Generator]
    public class NodePrinterGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var nodeTypes = context.Compilation.SyntaxTrees.Where(
                    o => o.FilePath.Contains("SyntaxNodePrinters")
                )
                .Select(o => Path.GetFileNameWithoutExtension(o.FilePath))
                .Select(
                    fileName =>
                        new
                        {
                            PrinterName = fileName,
                            SyntaxNodeName = fileName + "Syntax",
                            VariableName = fileName[0].ToString().ToLower() + fileName[1..]
                        }
                )
                .OrderBy(o => o.SyntaxNodeName)
                .ToArray();

            var template = Template.Parse(GetContent("NodePrinterGenerator.sbntxt"));
            var renderedSource = template.Render(
                new { NodeTypes = nodeTypes },
                member => member.Name
            );

            var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

            context.AddSource("Node", sourceText);
        }

        public static string GetContent(string relativePath)
        {
            var baseName = Assembly.GetExecutingAssembly().GetName().Name;
            var resourceName = relativePath.TrimStart('.')
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            var name = baseName + "." + resourceName;
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            if (stream == null)
            {
                var list = Assembly.GetExecutingAssembly().GetManifestResourceNames();

                throw new Exception(
                    $"No embedded resource found with the name {name}. Resources found are "
                        + string.Join(", ", list)
                );
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
