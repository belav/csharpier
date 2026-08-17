using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class InvokeMemberExpression
{
    internal static Doc Print(InvokeMemberExpressionAst node, PrintContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            "::",
            Node.Print(node.Member, context),
            "()"
        );
    }
}
