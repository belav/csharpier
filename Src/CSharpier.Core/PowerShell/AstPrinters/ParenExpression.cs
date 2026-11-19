using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ParenExpression
{
    public static Doc Print(ParenExpressionAst node, PrintContext context)
    {
        var inlineComments = context.HasOnlyInlineLeadingComments(node.Pipeline);
        var leadingComments = context.PrintLeadingComments(node.Pipeline);
        var contents = Node.Print(node.Pipeline, context);
        var trailingComments = context.PrintTrailingComments(node.Pipeline);
        if (leadingComments != Doc.Null && inlineComments && trailingComments == Doc.Null)
        {
            contents = Doc.Concat(leadingComments, contents);
        }
        else if (leadingComments != Doc.Null || trailingComments != Doc.Null)
        {
            return Doc.Concat(
                "(",
                Doc.Indent(Doc.HardLine, leadingComments, contents, trailingComments),
                Doc.HardLine,
                ")"
            );
        }

        if (Layout.BreaksInsideParens(node.Pipeline))
        {
            return Doc.Group("(", Doc.Indent(Doc.SoftLine, contents), Doc.SoftLine, ")");
        }

        return Doc.Concat("(", contents, ")");
    }
}
