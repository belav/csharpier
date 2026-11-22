using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;
using CSharpier.Core.PowerShell.AstPrinters;

namespace CSharpier.Core.PowerShell;

internal static class ArgumentList
{
    // True when one of the items has a comment next to it or there is a comment between the
    // delimiters before the first item or after the last one. innerStart and innerEnd are the
    // offsets where the text between the delimiters starts and ends.
    internal static bool HasComments(
        IReadOnlyList<Ast> items,
        int innerStart,
        int innerEnd,
        PrintContext context
    )
    {
        var (first, last) = GetCommentsOutsideItems(items, innerStart, innerEnd, context);
        if (first.Count > 0 || last.Count > 0)
        {
            return true;
        }

        foreach (var item in items)
        {
            if (
                context.GetUnprintedComments(context.GetLeadingComments(item)).Any()
                || context.GetUnprintedComments(context.GetTrailingComments(item)).Any()
            )
            {
                return true;
            }
        }

        return false;
    }

    // The comments in front of the first item and after the last one that are not attached to
    // them, such as a comment after the opening delimiter on its line, which is attached to the
    // node in front of the delimiter.
    private static (List<IScriptExtent> First, List<IScriptExtent> Last) GetCommentsOutsideItems(
        IReadOnlyList<Ast> items,
        int innerStart,
        int innerEnd,
        PrintContext context
    )
    {
        if (items.Count == 0)
        {
            return (context.GetUnprintedCommentsIn(innerStart, innerEnd), []);
        }

        var leading = context.GetLeadingComments(items[0]);
        var first = context.GetUnprintedCommentsIn(innerStart, items[0].Extent.StartOffset);
        first.RemoveAll(leading.Contains);

        var trailing = context.GetTrailingComments(items[^1]);
        var last = context.GetUnprintedCommentsIn(items[^1].Extent.EndOffset, innerEnd);
        last.RemoveAll(trailing.Contains);

        return (first, last);
    }

    // Prints comma separated items between delimiters. The items go one per line when they do not
    // fit or when there is a comment between the delimiters outside the items. Comments in front
    // of the first item go on their own lines.
    internal static Doc Print(
        string open,
        IReadOnlyList<Ast> items,
        string close,
        int innerStart,
        int innerEnd,
        PrintContext context
    )
    {
        if (!HasComments(items, innerStart, innerEnd, context))
        {
            if (items.Count == 0)
            {
                return open + close;
            }

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

        var (first, last) = GetCommentsOutsideItems(items, innerStart, innerEnd, context);
        var lines = new LineList();
        lines.AddCommentsWithoutBlankLines(first, context);
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

        lines.AddComments(last, context);

        return Doc.Concat(open, Doc.Indent(Doc.HardLine, lines.ToDoc()), Doc.HardLine, close);
    }
}
