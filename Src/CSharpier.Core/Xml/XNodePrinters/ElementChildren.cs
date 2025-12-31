using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(RawNode node, PrintingContext context)
    {
        var groupIds = new List<string>();
        foreach (var _ in node.Nodes)
        {
            groupIds.Add(context.GroupFor("children group"));
        }

        var result = new List<Doc>();
        var x = 0;
        foreach (var childNode in node.Nodes)
        {
            if (childNode.NodeType is XmlNodeType.Whitespace)
            {
                result.Add(Doc.HardLine);
                continue;
            }

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
                else if (childNode.PreviousNode?.NodeType is XmlNodeType.Text)
                {
                    leadingParts.Add(prevBetweenLine);
                }
                else
                {
                    if (groupIds.Count > 1)
                    {
                        leadingParts.Add(Doc.IfBreak(Doc.Null, Doc.SoftLine, groupIds[x - 1]));
                    }
                    else
                    {
                        leadingParts.Add(prevBetweenLine);
                    }
                }
            }

            if (nextBetweenLine is not NullDoc)
            {
                if (nextBetweenLine is HardLine)
                {
                    if (childNode.NextNode?.NodeType is XmlNodeType.Text)
                    {
                        nextParts.Add(Doc.HardLine);
                    }
                }
                else
                {
                    trailingParts.Add(nextBetweenLine);
                }
            }

            result.AddRange(prevParts);
            result.Add(
                Doc.Group(
                    Doc.Concat(leadingParts),
                    Doc.GroupWithId(
                        groupIds[x],
                        PrintChild(childNode, context),
                        Doc.Concat(trailingParts)
                    )
                )
            );
            result.AddRange(nextParts);
            x++;
        }

        return Doc.Concat(result);
    }

    public static Doc PrintChild(RawNode child, PrintingContext context)
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

    public static Doc PrintBetweenLine(RawNode prevNode, RawNode nextNode)
    {
        return
            (
                prevNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
                && nextNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
            )
            || (
                prevNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
                && nextNode.NodeType is XmlNodeType.Comment
            )
            || (
                prevNode.NodeType is XmlNodeType.Comment
                && nextNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
            )
            || (
                prevNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
                && nextNode.NodeType is XmlNodeType.Element
            )
            || (
                prevNode.NodeType is XmlNodeType.Element
                && nextNode.NodeType is XmlNodeType.Text or XmlNodeType.CDATA
            )
            ? Doc.Null
            : Doc.HardLine;
    }
}
