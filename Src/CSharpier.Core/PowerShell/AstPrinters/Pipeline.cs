using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class Pipeline
{
    public static Doc Print(PipelineAst node, PrintContext context)
    {
        var result = new DocListBuilder();
        foreach (var childNode in node.PipelineElements)
        {
            result.Add(Node.Print(childNode, context));
        }

        return Doc.Concat(ref result);
    }
}
