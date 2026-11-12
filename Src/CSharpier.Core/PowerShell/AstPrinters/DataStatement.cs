using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class DataStatement
{
    internal static Doc Print(DataStatementAst node, PrintContext context)
    {
        // The variable name and the -SupportedCommand parameter are written the way the source
        // has them.
        var docs = new List<Doc> { "data " };
        var header = context.GetText(
            node.Extent.StartOffset,
            node.CommandsAllowed is { Count: > 0 }
                ? node.CommandsAllowed[0].Extent.StartOffset
                : node.Body.Extent.StartOffset
        );
        foreach (
            var word in header.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Skip(1)
        )
        {
            docs.Add(word);
            docs.Add(" ");
        }

        if (node.CommandsAllowed is { Count: > 0 })
        {
            docs.Add(Doc.Join(", ", node.CommandsAllowed.Select(o => Node.Print(o, context))));
            docs.Add(" ");
        }

        docs.Add(StatementBlock.Print(node.Body, context));
        return Doc.Concat(docs);
    }
}
