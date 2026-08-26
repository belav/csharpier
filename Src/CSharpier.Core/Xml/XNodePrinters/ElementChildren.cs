using System.Xml;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(RawNode node, XmlPrintingContext context)
    {
        var result = new List<Doc>();
        var hasMultipleChildren = node.Nodes.Count > 1;
        string? previousGroupId = null;
        var printIgnored = false;
        foreach (var childNode in node.Nodes)
        {
            if (childNode.CSharpierIgnoreType is CSharpierIgnoreType.IgnoreEnd)
            {
                printIgnored = false;
            }

            if (printIgnored)
            {
                result.Add(
                    context
                        .NormalizedXml[childNode.StartPosition..childNode.EndPosition]
                        .Replace("\n", context.LineEnding)
                );
                continue;
            }

            if (childNode.NodeType is XmlNodeType.Whitespace)
            {
                if (childNode.NextNode is not { NodeType: XmlNodeType.Text })
                {
                    result.Add(Doc.HardLine);
                }

                continue;
            }

            Doc? prevPart = null;
            Doc? leadingPart = null;
            Doc? trailingPart = null;
            Doc? nextPart = null;

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
                    prevPart = Doc.HardLine;
                }
                else if (childNode.PreviousNode?.NodeType is XmlNodeType.Text)
                {
                    leadingPart = prevBetweenLine;
                }
                else
                {
                    leadingPart = hasMultipleChildren
                        ? Doc.IfBreak(Doc.Null, Doc.SoftLine, previousGroupId)
                        : prevBetweenLine;
                }
            }

            if (nextBetweenLine is not NullDoc)
            {
                if (nextBetweenLine is HardLine)
                {
                    if (childNode.NextNode?.NodeType is XmlNodeType.Text)
                    {
                        nextPart = Doc.HardLine;
                    }
                }
                else
                {
                    trailingPart = nextBetweenLine;
                }
            }

            if (prevPart is not null)
            {
                result.Add(prevPart);
            }

            previousGroupId = context.GroupFor("children group");
            result.Add(
                Doc.Group(
                    leadingPart ?? Doc.Null,
                    Doc.GroupWithId(
                        previousGroupId,
                        Node.Print(childNode, context),
                        trailingPart ?? Doc.Null
                    )
                )
            );

            if (nextPart is not null)
            {
                result.Add(nextPart);
            }

            if (childNode.CSharpierIgnoreType is CSharpierIgnoreType.IgnoreStart)
            {
                printIgnored = true;
            }
        }

        return Doc.Concat(result);
    }

    public static Doc PrintBetweenLine(RawNode prevNode, RawNode nextNode)
    {
        return
            (prevNode.NodeType is XmlNodeType.Whitespace && nextNode.NodeType is XmlNodeType.Text)
            || (
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
            || prevNode.CSharpierIgnoreType is CSharpierIgnoreType.Ignore
            ? Doc.Null
            : Doc.HardLine;
    }
}
