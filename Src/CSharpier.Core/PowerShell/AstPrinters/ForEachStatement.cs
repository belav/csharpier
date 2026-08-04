using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ForEachStatement
{
    internal static Doc Print(ForEachStatementAst node, PrintContext context)
    {
        return Doc.Concat(
            "foreach (",
            Verbatim.Print(node.Variable.Extent),
            " in ",
            Verbatim.Print(node.Condition.Extent),
            ") ",
            StatementBlock.Print(node.Body, context)
        );
    }
}
