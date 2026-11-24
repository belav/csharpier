using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class BaseCtorInvokeMemberExpression
{
    // Comments between base and the parentheses go inside the parentheses.
    internal static Doc Print(BaseCtorInvokeMemberExpressionAst node, PrintContext context)
    {
        return ArgumentList.Print(
            "base(",
            (IReadOnlyList<Ast>?)node.Arguments ?? [],
            ")",
            node.Expression.Extent.EndOffset,
            node.Extent.EndOffset - 1,
            context
        );
    }
}
