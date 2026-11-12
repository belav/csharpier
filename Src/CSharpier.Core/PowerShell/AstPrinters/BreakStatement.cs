using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class BreakStatement
{
    internal static Doc Print(BreakStatementAst node, PrintContext context)
    {
        return node.Label is null ? "break" : Doc.Concat("break ", Node.Print(node.Label, context));
    }
}
