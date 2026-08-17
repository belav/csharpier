using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class FunctionDefinition
{
    internal static Doc Print(FunctionDefinitionAst node, PrintContext context)
    {
        var keyword = node.IsFilter ? "filter " : "function ";
        var body = node.Body;

        if (
            body.BeginBlock is not null
            || body.ProcessBlock is not null
            || body.DynamicParamBlock is not null
            || body.EndBlock is null
            || !body.EndBlock.Unnamed
        )
        {
            return Doc.Concat(keyword, node.Name, " ", Verbatim.Print(body.Extent));
        }

        var inner = new List<Doc>();
        if (body.ParamBlock is not null)
        {
            inner.Add(Verbatim.Print(body.ParamBlock.Extent));
            inner.Add(Doc.HardLine);
        }

        if (body.EndBlock.Statements.Count > 0)
        {
            if (inner.Count > 0)
            {
                inner.Add(Doc.HardLine);
            }

            inner.Add(
                Statements.Print(
                    body.EndBlock.Statements,
                    context,
                    body.Extent.StartOffset,
                    body.Extent.EndOffset
                )
            );
        }

        if (inner.Count == 0)
        {
            return Doc.Concat(keyword, node.Name, " { }");
        }

        return Doc.Concat(
            keyword,
            node.Name,
            " {",
            Doc.Indent(Doc.HardLine, Doc.Concat(inner)),
            Doc.HardLine,
            "}"
        );
    }
}
