using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ErrorExpression
{
    internal static Doc Print(ErrorExpressionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
