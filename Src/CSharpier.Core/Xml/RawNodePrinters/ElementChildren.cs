using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.RawNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(RawNode node, PrintingContext context)
    {
        var groupIds = new List<string>();
        foreach (var _ in node.Nodes)
        {
            groupIds.Add(context.GroupFor("symbol"));
        }

        var result = new ValueListBuilder<Doc>(node.Nodes.Count * 5);
        var x = 0;
        foreach (var childNode in node.Nodes)
        {
            var prevParts = new ValueListBuilder<Doc>([null, null]);
            var leadingParts = new ValueListBuilder<Doc>([null, null]);
            var trailingParts = new ValueListBuilder<Doc>([null, null]);
            var nextParts = new ValueListBuilder<Doc>([null, null]);

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
                    prevParts.Append(Doc.HardLine);
                }
                else if (childNode.PreviousNode?.NodeType is XmlNodeType.Text)
                {
                    leadingParts.Append(prevBetweenLine);
                }
                else
                {
                    leadingParts.Append(Doc.IfBreak(Doc.Null, Doc.SoftLine, groupIds[x - 1]));
                }
            }

            if (nextBetweenLine is not NullDoc)
            {
                if (nextBetweenLine is HardLine)
                {
                    if (childNode.NextNode?.NodeType is XmlNodeType.Text)
                    {
                        nextParts.Append(Doc.HardLine);
                    }
                }
                else
                {
                    trailingParts.Append(nextBetweenLine);
                }
            }

            result.Append(prevParts.AsSpan());
            result.Append(
                Doc.Group(
                    Doc.Concat(ref leadingParts),
                    Doc.GroupWithId(
                        groupIds[x],
                        PrintChild(childNode, context),
                        Doc.Concat(ref trailingParts)
                    )
                )
            );
            result.Append(nextParts.AsSpan());
            x++;
        }

        return Doc.Concat(ref result);
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
            (prevNode.NodeType is XmlNodeType.Text && nextNode.NodeType is XmlNodeType.Comment)
            || (prevNode.NodeType is XmlNodeType.Comment && nextNode.NodeType is XmlNodeType.Text)
            || (prevNode.NodeType is XmlNodeType.Text && nextNode.NodeType is XmlNodeType.Element)
            || (prevNode.NodeType is XmlNodeType.Element && nextNode.NodeType is XmlNodeType.Text)
            ? Doc.Null
            : Doc.HardLine;
    }
}
