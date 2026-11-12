using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class BaseCtorInvokeMemberExpression
{
    internal static Doc Print(BaseCtorInvokeMemberExpressionAst node, PrintContext context)
    {
        if (node.Arguments is not { Count: > 0 })
        {
            return "base()";
        }

        return ArgumentList.Print("base(", node.Arguments, ")", context);
    }
}
