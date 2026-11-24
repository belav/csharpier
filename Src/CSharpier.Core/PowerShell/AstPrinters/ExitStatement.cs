using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ExitStatement
{
    internal static Doc Print(ExitStatementAst node, PrintContext context)
    {
        return node.Pipeline is null
            ? "exit"
            : Doc.Concat("exit ", Node.Print(node.Pipeline, context));
    }
}
