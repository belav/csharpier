using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class UsingExpression
{
    public static Doc Print(UsingExpressionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
