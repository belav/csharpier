using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class ArgumentList
{
    internal static bool HasComments(IReadOnlyList<Ast> items, PrintContext context)
    {
        foreach (var item in items)
        {
            if (
                context.GetLeadingComments(item).Count > 0
                || context.GetTrailingComments(item).Count > 0
            )
            {
                return true;
            }
        }

        return false;
    }

    // Prints comma separated items between delimiters. The items go one per line when they do not
    // fit or when one of them has a comment next to it.
    internal static Doc Print(
        string open,
        IReadOnlyList<Ast> items,
        string close,
        PrintContext context
    )
    {
        if (!HasComments(items, context))
        {
            var contents = new List<Doc> { Doc.SoftLine };
            for (var index = 0; index < items.Count; index++)
            {
                if (index > 0)
                {
                    contents.Add(",");
                    contents.Add(Doc.Line);
                }

                contents.Add(Node.Print(items[index], context));
            }

            return Doc.Group(open, Doc.Indent(contents), Doc.SoftLine, close);
        }

        var lines = new LineList();
        for (var index = 0; index < items.Count; index++)
        {
            var item = items[index];
            lines.Add(
                Doc.Concat(
                    context.PrintLeadingComments(item),
                    Node.Print(item, context),
                    index < items.Count - 1 ? "," : Doc.Null,
                    context.PrintTrailingComments(item)
                ),
                context.GetStartLine(item),
                context.GetEndLine(item)
            );
        }

        return Doc.Concat(open, Doc.Indent(Doc.HardLine, lines.ToDoc()), Doc.HardLine, close);
    }
}
