using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class Condition
{
    // Prints the parenthesized condition of if, while, do, and switch.
    internal static Doc Print(PipelineBaseAst condition, PrintContext context)
    {
        var inlineComments = context.HasOnlyInlineLeadingComments(condition);
        var leadingComments = context.PrintLeadingComments(condition);
        var contents = Node.Print(condition, context);
        if (leadingComments != Doc.Null && inlineComments)
        {
            contents = Doc.Concat(leadingComments, contents);
        }
        else if (leadingComments != Doc.Null)
        {
            return Doc.Concat(
                "(",
                Doc.Indent(Doc.HardLine, leadingComments, contents),
                Doc.HardLine,
                ")"
            );
        }

        return Layout.BreaksInsideParens(condition)
            ? Doc.Group("(", Doc.Indent(Doc.SoftLine, contents), Doc.SoftLine, ")")
            : Doc.Concat("(", contents, ")");
    }
}
