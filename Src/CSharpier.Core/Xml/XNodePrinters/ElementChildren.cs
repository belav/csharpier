using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(BetterXmlReader xmlReader, XmlPrintingContext context)
    {
        var groupIds = new List<string>();

        // TODO can we calculate this better?
        var result = new ValueListBuilder<Doc>(20);
        var x = 0;
        var previousNodeType = XmlNodeType.None;
        while (true)
        {
            if (xmlReader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
            if (xmlReader.NodeType == XmlNodeType.Whitespace)
            {
                xmlReader.Read();
                continue;
            }

            var currentNodeType = xmlReader.NodeType;
            var currentContent = PrintChild(xmlReader, context);
            var nextNodeType = xmlReader.NodeType;

            groupIds.Add(context.GroupFor("symbol"));
            var prevParts = new ValueListBuilder<Doc>([null, null]);
            var leadingParts = new ValueListBuilder<Doc>([null, null]);
            var trailingParts = new ValueListBuilder<Doc>([null, null]);
            var nextParts = new ValueListBuilder<Doc>([null, null]);

            var prevBetweenLine =
                previousNodeType != XmlNodeType.None
                    ? PrintBetweenLine(previousNodeType, currentNodeType)
                    : Doc.Null;

            var nextBetweenLine =
                nextNodeType != XmlNodeType.None
                    ? PrintBetweenLine(currentNodeType, nextNodeType)
                    : Doc.Null;

            if (prevBetweenLine is not NullDoc)
            {
                if (prevBetweenLine is HardLine)
                {
                    prevParts.Append(Doc.HardLine);
                }
                else if (previousNodeType == XmlNodeType.Text)
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
                    if (nextNodeType == XmlNodeType.Text)
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
                    Doc.GroupWithId(groupIds[x], currentContent, Doc.Concat(ref trailingParts))
                )
            );
            result.Append(nextParts.AsSpan());

            previousNodeType = currentNodeType;
            x++;
        }

        return Doc.Concat(ref result);
    }

    public static Doc PrintChild(BetterXmlReader xmlReader, XmlPrintingContext context)
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

        var result = Node.Print(xmlReader, context);
        xmlReader.Read();
        return result;
    }

    public static Doc PrintBetweenLine(XmlNodeType prevNode, XmlNodeType nextNode)
    {
        return
            (prevNode == XmlNodeType.Text && nextNode == XmlNodeType.Comment)
            || (prevNode == XmlNodeType.Comment && nextNode == XmlNodeType.Text)
            || (prevNode == XmlNodeType.Text && nextNode == XmlNodeType.EndElement)
            || (prevNode == XmlNodeType.EndElement && nextNode == XmlNodeType.Text)
            ? Doc.Null
            : Doc.HardLine;
    }
}
