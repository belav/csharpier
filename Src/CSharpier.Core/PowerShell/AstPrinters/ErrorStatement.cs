using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ErrorStatement
{
    internal static Doc Print(ErrorStatementAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
