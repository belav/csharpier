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
                    o.Path.EndsWith(".cst")
                    && !o.Path.EndsWith(".actual.cst")
                    && !o.Path.EndsWith(".expected.cst")
            )
            .Select(
                o =>
                    new
                    {
                        Name = Path.GetFileNameWithoutExtension(o.Path),
                        UseTabs = Path.GetFileNameWithoutExtension(o.Path).EndsWith("_Tabs")
                    }
            );

        return new { Tests = tests };
    }
}
