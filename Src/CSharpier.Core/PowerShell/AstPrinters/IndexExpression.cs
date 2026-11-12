using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class IndexExpression
{
    public static Doc Print(IndexExpressionAst node, PrintContext context)
    {
        return Doc.Concat(
            Node.Print(node.Target, context),
            node.NullConditional ? "?[" : "[",
            Node.Print(node.Index, context),
            "]"
        );
    }
}
