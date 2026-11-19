using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class VariableExpression
{
    internal static Doc Print(VariableExpressionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
