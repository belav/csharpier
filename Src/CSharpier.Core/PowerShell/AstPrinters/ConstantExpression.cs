using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ConstantExpression
{
    internal static Doc Print(ConstantExpressionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
