using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class CommandExpression
{
    public static Doc Print(CommandExpressionAst node, PrintContext context)
    {
        return Node.Print(node.Expression, context);
    }
}
