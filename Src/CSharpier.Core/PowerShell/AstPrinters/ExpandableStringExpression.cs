using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ExpandableStringExpression
{
    internal static Doc Print(ExpandableStringExpressionAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
