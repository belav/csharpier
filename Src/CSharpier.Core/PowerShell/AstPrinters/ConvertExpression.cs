using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ConvertExpression
{
    public static Doc Print(ConvertExpressionAst node, PrintContext context)
    {
        return Doc.Concat(Node.Print(node.Type, context), Node.Print(node.Child, context));
    }
}
