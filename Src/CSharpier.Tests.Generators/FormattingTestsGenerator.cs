using System.IO;
using System.Linq;
using Generators;
using Microsoft.CodeAnalysis;

namespace CSharpier.Tests.Generators;

[Generator]
public class FormattingTestsGenerator : TemplatedGenerator
{
    protected override string SourceName => "FormattingTests";

    protected override object GetModel(GeneratorExecutionContext context)
    {
        var tests = context.AdditionalFiles
            .Where(
                o =>
                    o.Path.EndsWith(".test")
                    && !o.Path.EndsWith(".actual.test")
                    && !o.Path.EndsWith(".expected.test")
            )
            .Select(
                o =>
                    new
                    {
                        Name = Path.GetFileNameWithoutExtension(o.Path),
                        Type = new FileInfo(o.Path).Directory!.Name,
                        UseTabs = Path.GetFileNameWithoutExtension(o.Path).EndsWith("_Tabs")
                    }
            );

        return new { Tests = tests };
    }
}
