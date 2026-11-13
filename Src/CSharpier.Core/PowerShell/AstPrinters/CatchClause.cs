using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class CatchClause
{
    internal static Doc Print(CatchClauseAst node, PrintContext context)
    {
        if (node.CatchTypes.Count == 0)
        {
            return Doc.Concat("catch ", StatementBlock.Print(node.Body, context));
        }

        return Doc.Concat(
            "catch ",
            Doc.Join(", ", node.CatchTypes.Select(o => Node.Print(o, context))),
            " ",
            StatementBlock.Print(node.Body, context, node.CatchTypes[^1])
        );
    }
}
