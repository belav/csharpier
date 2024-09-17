using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class ElementChildren
{
    public static Doc Print(XmlElement node)
    {
        return PrintChildren(node, Node.Print);

        // if (node.ChildNodes.Count == 0)
        // {
        //     return Doc.Null;
        // }
        //
        // if (node.FirstChild is XmlText)
        // {
        //     return Node.Print(node.FirstChild);
        // }
        //
        // var result = new List<Doc>();
        //
        // foreach (XmlNode xmlNode in node.ChildNodes)
        // {
        //     result.Add(Doc.Line);
        //     result.Add(Node.Print(xmlNode));
        // }
        //
        // return Doc.Concat(Doc.BreakParent, Doc.Concat(result), Doc.Line);
    }

    public static Doc PrintChildren(XmlElement node, Func<XmlNode, Doc> print)
    {
        // this force breaks html, head, ul, ol, etc
        // if (ForceBreakChildren(node))
        // {
        //     var result = new List<Doc> { Doc.BreakParent };
        //
        //     foreach (XmlNode child in node.ChildNodes)
        //     {
        //         var prevBetweenLine =
        //             child.PreviousSibling == null
        //                 ? Doc.Null
        //                 : PrintBetweenLine(child.PreviousSibling, child);
        //         result.AddRange(
        //             prevBetweenLine == Doc.Null
        //                 ? new List<Doc>()
        //                 : new List<Doc>
        //                 {
        //                     prevBetweenLine,
        //                     ForceNextEmptyLine(child.Prev) ? Doc.HardLine : Doc.Null,
        //                 }
        //         );
        //         result.AddRange(PrintChild(child, print));
        //     }
        //
        //     return result;
        // }

        var groupIds = new List<string>(); // Assuming CSharpier has grouping feature
        foreach (var child in node.ChildNodes)
        {
            groupIds.Add(Guid.NewGuid().ToString());
        }

        var docs = new List<Doc>();
        foreach (XmlNode child in node.ChildNodes)
        {
            var prevBetweenLine =
                child.PreviousSibling == null
                    ? Doc.Null
                    : PrintBetweenLine(child.PreviousSibling, child);
            var nextBetweenLine =
                child.NextSibling == null ? Doc.Null : PrintBetweenLine(child, child.NextSibling);

            var parts = new List<Doc>();
            if (prevBetweenLine is not NullDoc)
            {
                parts.Add(prevBetweenLine == Doc.HardLine ? Doc.HardLine : Doc.SoftLine);
            }

            parts.Add(PrintChild(child, print));

            if (nextBetweenLine is not NullDoc)
            {
                parts.Add(nextBetweenLine == Doc.HardLine ? Doc.HardLine : Doc.SoftLine);
            }

            docs.Add(Doc.Group(parts));
        }

        return Doc.Concat(docs);
    }

    public static Doc PrintChild(XmlNode child, Func<XmlNode, Doc> print)
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

        return print(child);
    }

    // public static int GetEndLocation(XmlElement node)
    // {
    //     int endLocation = LocEnd(node);
    //
    //     // Element can be unclosed
    //     if (node.Type == "element" && node.Children != null && node.Children.Count > 0)
    //     {
    //         return Math.Max(endLocation, GetEndLocation(node.Children[^1])); // Using C# ^ for last element
    //     }
    //
    //     return endLocation;
    // }

    public static Doc PrintBetweenLine(XmlNode prevNode, XmlNode nextNode)
    {
        return Doc.Line;

        // if (prevNode is XmlText && nextNode is XmlText)
        // {
        //     if (prevNode.IsTrailingSpaceSensitive)
        //     {
        //         return prevNode.HasTrailingSpaces
        //             ? PreferHardlineAsLeadingSpaces(nextNode)
        //                 ? Doc.HardLine
        //                 : Line()
        //             : Doc.Empty;
        //     }
        //     return PreferHardlineAsLeadingSpaces(nextNode) ? Doc.HardLine : Doc.SoftLine;
        // }
        //
        // return ShouldBreakBetweenNodes(prevNode, nextNode) ? Doc.HardLine
        //     : nextNode.HasLeadingSpaces ? Line()
        //     : Doc.SoftLine;
    }
}
