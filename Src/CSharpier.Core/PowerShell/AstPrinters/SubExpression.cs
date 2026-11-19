using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class SubExpression
{
    public static Doc Print(SubExpressionAst node, PrintContext context)
    {
        return StatementBlock.PrintInParens("$(", node, node.SubExpression, context);
    }
}
