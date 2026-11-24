using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class IndexExpression
{
    public static Doc Print(IndexExpressionAst node, PrintContext context)
    {
        var target = Node.Print(node.Target, context);
        var open = node.NullConditional ? "?[" : "[";

        // A comment after [ on its line is a trailing comment of the target.
        var innerStart = node.Target.Extent.EndOffset;
        var innerEnd = node.Extent.EndOffset - 1;
        if (ArgumentList.HasComments([node.Index], innerStart, innerEnd, context))
        {
            return Doc.Concat(
                target,
                ArgumentList.Print(open, [node.Index], "]", innerStart, innerEnd, context)
            );
        }

        return Doc.Concat(target, open, Node.Print(node.Index, context), "]");
    }
}
