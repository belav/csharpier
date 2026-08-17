using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ExitStatement
{
    public static Doc Print(ExitStatementAst node, PrintContext context)
    {
        return Doc.Concat("exit ", Node.Print(node.Pipeline, context));
    }
}
