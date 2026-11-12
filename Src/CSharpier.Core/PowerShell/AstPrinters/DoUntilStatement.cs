using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class DoUntilStatement
{
    internal static Doc Print(DoUntilStatementAst node, PrintContext context)
    {
        return DoWhileStatement.Print(node, "until", context);
    }
}
