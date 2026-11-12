using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ThrowStatement
{
    internal static Doc Print(ThrowStatementAst node, PrintContext context)
    {
        return node.Pipeline is null
            ? "throw"
            : Doc.Concat("throw ", Node.Print(node.Pipeline, context));
    }
}
