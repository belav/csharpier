using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ContinueStatement
{
    internal static Doc Print(ContinueStatementAst node, PrintContext context)
    {
        return node.Label is null
            ? "continue"
            : Doc.Concat("continue ", Node.Print(node.Label, context));
    }
}
