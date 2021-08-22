using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier.Tests.Generators
{
    [Generator]
    public class FormattingTestsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var myFiles = context.AdditionalFiles.Where(
                o =>
                    o.Path.EndsWith(".cst")
                    && !o.Path.EndsWith(".actual.cst")
                    && !o.Path.EndsWith(".expected.cst")
            );

            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine(
                @"using NUnit.Framework;

namespace CSharpier.Tests.FormattingTests
{
    [TestFixture]
    public class TestFiles : BaseTest
    {"
            );

            foreach (var file in myFiles)
            {
                var name = Path.GetFileNameWithoutExtension(file.Path);

                sourceBuilder.AppendLine(
                    $@"        [Test]
        public void {name}()
        {{
            this.RunTest(""{name}""{(name == "Tabs" ? ", true" : string.Empty)});
        }}"
                );
            }

            sourceBuilder.AppendLine(
                @"    }
}"
            );

            var sourceText = SourceText.From(sourceBuilder.ToString(), Encoding.UTF8);

            context.AddSource("FormattingTests", sourceText);
        }
    }
}
