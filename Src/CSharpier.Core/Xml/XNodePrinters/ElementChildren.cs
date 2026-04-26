using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

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
        var printIgnored = false;
        foreach (var childNode in node.Nodes)
        {
            if (childNode.CSharpierIgnoreType is CSharpierIgnoreType.IgnoreEnd)
            {
                printIgnored = false;
                x++;
            }

            if (printIgnored)
            {
                result.Add(
                    context
                        .NormalizedXml[childNode.StartPosition..childNode.EndPosition]
                        .Replace("\n", context.Options.LineEnding)
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
                        Node.Print(childNode, context),
                        Doc.Concat(trailingParts)
                    )
                )
            );
            result.AddRange(nextParts);
            x++;

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
