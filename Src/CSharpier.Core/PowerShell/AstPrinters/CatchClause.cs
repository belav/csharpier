using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class CatchClause
{
    // Prints the types and the body. The catch keyword is printed by the try statement.
    internal static Doc Print(CatchClauseAst node, PrintContext context)
    {
        if (node.CatchTypes.Count == 0)
        {
            return StatementBlock.Print(node.Body, context);
        }

        return Doc.Concat(
            Doc.Join(", ", node.CatchTypes.Select(o => Node.Print(o, context))),
            " ",
            StatementBlock.Print(node.Body, context, node.CatchTypes[^1])
        );
    }
}
