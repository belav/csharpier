using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class WhileStatement
{
    internal static Doc Print(WhileStatementAst node, PrintContext context)
    {
        return Doc.Concat(
            "while (",
            Verbatim.Print(node.Condition.Extent),
            ") ",
            StatementBlock.Print(node.Body, context)
        );
    }
}
