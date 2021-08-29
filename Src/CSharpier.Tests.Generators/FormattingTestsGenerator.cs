﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;

namespace CSharpier.Tests.Generators
{
    [Generator]
    public class FormattingTestsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var tests = context.AdditionalFiles.Where(
                    o =>
                        o.Path.EndsWith(".cst")
                        && !o.Path.EndsWith(".actual.cst")
                        && !o.Path.EndsWith(".expected.cst")
                )
                .Select(
                    o =>
                        new
                        {
                            Name = Path.GetFileNameWithoutExtension(o.Path),
                            UseTabs = Path.GetFileNameWithoutExtension(o.Path) == "Tabs"
                        }
                );

            var template = Template.Parse(GetContent("FormattingTestsGenerator.sbntxt"));
            var renderedSource = template.Render(new { Tests = tests }, member => member.Name);

            var sourceText = SourceText.From(renderedSource, Encoding.UTF8);

            context.AddSource("FormattingTests", sourceText);
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
