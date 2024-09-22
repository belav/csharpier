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
        // if (forceBreakChildren(node)) {
        //     return [
        //         breakParent,
        //
        //         ...path.map((childPath) => {
        //         const childNode = childPath.node;
        //         const prevBetweenLine = !childNode.prev
        //         ? ""
        //         : printBetweenLine(childNode.prev, childNode);
        //         return [
        //         !prevBetweenLine
        //         ? ""
        //         : [
        //         prevBetweenLine,
        //         forceNextEmptyLine(childNode.prev)
        //             ? hardline
        //             : "",
        //         ],
        //         printChild(childPath, options, print),
        //         ];
        //         }, "children"),
        //     ];
        // }

        var groupIds = new List<string>();
        foreach (var child in node.ChildNodes)
        {
            groupIds.Add(Guid.NewGuid().ToString());
        }

        var result = new List<Doc>();
        var x = 0;
        foreach (XmlNode childNode in node.ChildNodes)
        {
            // if (child is XmlText) {
            //     if (childNode.prev && isTextLikeNode(childNode.prev)) {
            //         const prevBetweenLine = printBetweenLine(
            //             childNode.prev,
            //             childNode,
            //         );
            //         if (prevBetweenLine) {
            //             if (forceNextEmptyLine(childNode.prev)) {
            //                 return [
            //                     hardline,
            //                     hardline,
            //                     printChild(childPath, options, print),
            //                 ];
            //             }
            //             return [
            //                 prevBetweenLine,
            //                 printChild(childPath, options, print),
            //             ];
            //         }
            //     }
            //     return printChild(childPath, options, print);
            // }

            var prevParts = new List<Doc>();
            var leadingParts = new List<Doc>();
            var trailingParts = new List<Doc>();
            var nextParts = new List<Doc>();

            var prevBetweenLine = childNode.PreviousSibling is not null
                ? PrintBetweenLine(childNode.PreviousSibling, childNode)
                : Doc.Null;

            var nextBetweenLine = childNode.NextSibling is not null
                ? PrintBetweenLine(childNode, childNode.NextSibling)
                : Doc.Null;

            if (prevBetweenLine is not NullDoc)
            {
                if (ForceNextEmptyLine(childNode.PreviousSibling))
                {
                    prevParts.Add(Doc.HardLine);
                    prevParts.Add(Doc.HardLine);
                }
                else if (prevBetweenLine is HardLine)
                {
                    prevParts.Add(Doc.HardLine);
                }
                else if (childNode.PreviousSibling is XmlText)
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
                if (ForceNextEmptyLine(childNode))
                {
                    if (childNode.NextSibling is XmlText)
                    {
                        nextParts.Add(Doc.HardLine);
                        nextParts.Add(Doc.HardLine);
                    }
                }
                else if (nextBetweenLine is HardLine)
                {
                    if (childNode.NextSibling is XmlText)
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
                        PrintChild(childNode, Node.Print),
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

    private static bool ForceNextEmptyLine(XmlNode? childNode)
    {
        return false;
        // return (
        //     isFrontMatter(node) ||
        //     (node.next &&
        //      node.sourceSpan.end &&
        //      node.sourceSpan.end.line + 1 < node.next.sourceSpan.start.line)
        // );
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
        return prevNode is XmlText && nextNode is XmlText
            ? Doc.HardLine
            // ? prevNode.isTrailingSpaceSensitive
            //     ? prevNode.hasTrailingSpaces
            //         ? preferHardlineAsLeadingSpaces(nextNode)
            //             ? hardline
            //             : line
            //         : ""
            //     : preferHardlineAsLeadingSpaces(nextNode)
            //       ? hardline
            //       : softline
            :
            // (needsToBorrowNextOpeningTagStartMarker(prevNode) &&
            //       (hasPrettierIgnore(nextNode) ||
            //           /**
            //            *     123<a
            //            *          ~
            //            *       ><b>
            //            */
            //           nextNode.firstChild ||
            //           /**
            //            *     123<!--
            //            *            ~
            //            *     -->
            //            */
            //           nextNode.isSelfClosing ||
            //           /**
            //            *     123<span
            //            *             ~
            //            *       attr
            //            */
            //           (nextNode.type === "element" &&
            //               nextNode.attrs.length > 0))) ||
            //   /**
            //    *     <img
            //    *       src="long"
            //    *                 ~
            //    *     />123
            //    */
            //   (prevNode.type === "element" &&
            //       prevNode.isSelfClosing &&
            //       needsToBorrowPrevClosingTagEndMarker(nextNode))
            // ? ""
            // :
            // !nextNode.isLeadingSpaceSensitive ||
            //   preferHardlineAsLeadingSpaces(nextNode) ||
            //   /**
            //    *       Want to write us a letter? Use our<a
            //    *         ><b><a>mailing address</a></b></a
            //    *                                          ~
            //    *       >.
            //    */
            //   (needsToBorrowPrevClosingTagEndMarker(nextNode) &&
            //       prevNode.lastChild &&
            //       needsToBorrowParentClosingTagStartMarker(
            //           prevNode.lastChild,
            //       ) &&
            //       prevNode.lastChild.lastChild &&
            //       needsToBorrowParentClosingTagStartMarker(
            //           prevNode.lastChild.lastChild,
            //       ))
            // ? hardline
            // : nextNode.hasLeadingSpaces
            //   ? line
            //   : softline;
            Doc.HardLine;
    }
}
