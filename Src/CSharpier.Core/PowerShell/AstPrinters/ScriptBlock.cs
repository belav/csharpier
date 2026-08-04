using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ScriptBlock
{
    internal static Doc Print(ScriptBlockAst node, PrintContext context)
    {
        if (
            node.BeginBlock is not null
            || node.ProcessBlock is not null
            || node.DynamicParamBlock is not null
            || node.EndBlock is null
            || !node.EndBlock.Unnamed
        )
        {
            return Verbatim.Print(node.Extent);
        }

        var parts = new List<Doc>();
        var startOffset = node.Extent.StartOffset;
        if (node.ParamBlock is not null)
        {
            parts.Add(Verbatim.Print(node.ParamBlock.Extent));
            startOffset = node.ParamBlock.Extent.EndOffset;
        }

        if (node.EndBlock.Statements.Count > 0)
        {
            if (parts.Count > 0)
            {
                parts.Add(Doc.HardLine);
            }

            parts.Add(
                Statements.Print(
                    node.EndBlock.Statements,
                    context,
                    startOffset,
                    node.Extent.EndOffset
                )
            );
        }

        return parts.Count == 0 ? Doc.Null : Doc.Concat(parts);
    }
}
