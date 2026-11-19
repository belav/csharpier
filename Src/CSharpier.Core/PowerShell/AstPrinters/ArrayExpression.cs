using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ArrayExpression
{
    public static Doc Print(ArrayExpressionAst node, PrintContext context)
    {
        return StatementBlock.PrintInParens("@(", node, node.SubExpression, context);
    }
}
