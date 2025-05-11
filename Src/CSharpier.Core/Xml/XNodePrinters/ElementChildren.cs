using System.Xml.Linq;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(XElement element, XmlPrintingContext context)
    {
        var groupIds = new List<string>();
        foreach (var _ in element.Nodes())
        {
            groupIds.Add(context.GroupFor("symbol"));
        }

        var result = new List<Doc>();
        var x = 0;
        foreach (var childNode in element.Nodes())
        {
            var prevParts = new List<Doc>();
            var leadingParts = new List<Doc>();
            var trailingParts = new List<Doc>();
            var nextParts = new List<Doc>();

            var prevBetweenLine = childNode.PreviousNode is not null
                ? PrintBetweenLine(childNode.PreviousNode, childNode)
                : Doc.Null;

            var nextBetweenLine = childNode.NextNode is not null
                ? PrintBetweenLine(childNode, childNode.NextNode)
                : Doc.Null;

            if (prevBetweenLine is not NullDoc)
            {
                if (prevBetweenLine is HardLine)
                {
                    prevParts.Add(Doc.HardLine);
                }
                else if (childNode.PreviousNode is XText)
                {
                    leadingParts.Add(prevBetweenLine);
                }
                else
                {
                    leadingParts.Add(Doc.IfBreak(Doc.Null, Doc.SoftLine, groupIds[x - 1]));
                }
            }

            if (nextBetweenLine is not NullDoc)
            {
                if (nextBetweenLine is HardLine)
                {
                    if (childNode.NextNode is XText)
                    {
                        nextParts.Add(Doc.HardLine);
                    }
                }
                else
                {
                    trailingParts.Add(nextBetweenLine);
                }
            }

            List<Doc> innerResult =
            [
                .. prevParts,
                Doc.Group(
                    Doc.Concat(leadingParts),
                    Doc.GroupWithId(
                        groupIds[x],
                        PrintChild(childNode, context),
                        Doc.Concat(trailingParts)
                    )
                ),
                .. nextParts,
            ];

            result.AddRange(innerResult);
            x++;
        }

        return Doc.Concat(result);
    }

    public static Doc PrintChild(XNode child, XmlPrintingContext context)
    {
        // should we try to support csharpier-ignore some day?
        // if (HasPrettierIgnore(child))
        // {
        //     int endLocation = GetEndLocation(child);
        //
        //     return new List<Doc>
        //     {
        //         PrintOpeningTagPrefix(child, options),
        //         ReplaceEndOfLine(TrimEnd(options.OriginalText.Substring(
        //             LocStart(child) + (child.Prev != null && NeedsToBorrowNextOpeningTagStartMarker(child.Prev)
        //                 ? PrintOpeningTagStartMarker(child).Length : 0),
        //             endLocation - (child.Next != null && NeedsToBorrowPrevClosingTagEndMarker(child.Next)
        //                 ? PrintClosingTagEndMarker(child, options).Length : 0)
        //         ))),
        //         PrintClosingTagSuffix(child, options)
        //     };
        // }

        return Node.Print(child, context);
    }

    public static Doc PrintBetweenLine(XNode prevNode, XNode nextNode)
    {
        return
            (prevNode is XText && nextNode is XComment)
            || (prevNode is XComment && nextNode is XText)
            ? Doc.Null
            : Doc.HardLine;
    }
}
