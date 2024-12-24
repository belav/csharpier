using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(XElement element, XmlPrintingContext context)
    {
        return Doc.Concat(
            PrintOpeningTagStart(element),
            Attributes.Print(element, context),
            element.IsEmpty ? Doc.Null : PrintOpeningTagEnd(element)
        );
    }

    private static Doc PrintOpeningTagStart(XElement element)
    {
        return
            element.PreviousNode is not null
            && NeedsToBorrowNextOpeningTagStartMarker(element.PreviousNode)
            ? Doc.Null
            : Doc.Concat(PrintOpeningTagPrefix(element), PrintOpeningTagStartMarker(element));
    }

    private static Doc PrintOpeningTagEnd(XElement node)
    {
        return node.FirstNode is not null && NeedsToBorrowParentOpeningTagEndMarker(node.FirstNode)
            ? Doc.Null
            : PrintOpeningTagEndMarker(node);
    }

    public static Doc PrintOpeningTagPrefix(XNode element)
    {
        return NeedsToBorrowParentOpeningTagEndMarker(element)
                ? PrintOpeningTagEndMarker(element.Parent!)
            : NeedsToBorrowPrevClosingTagEndMarker(element)
                ? PrintClosingTagEndMarker((element.PreviousNode as XElement)!)
            : "";
    }

    private static bool NeedsToBorrowPrevClosingTagEndMarker(XNode node)
    {
        /*
         *     <p></p
         *     >123
         *     ^
         *
         *     <p></p
         *     ><a
         *     ^
         */
        return node.PreviousNode is not null
            // && node.prev.type !== "docType"
            //    && node.type !== "angularControlFlowBlock"
            && node.PreviousNode is not (XText or XComment)
            // && node.isLeadingSpaceSensitive
            && !true
        // && !node.hasLeadingSpaces
        ;
    }

    public static Doc PrintClosingTag(XElement node)
    {
        return Doc.Concat(
            node.IsEmpty ? Doc.Null : PrintClosingTagStart(node),
            PrintClosingTagEnd(node)
        );
    }

    public static Doc PrintClosingTagStart(XElement element)
    {
        var lastChild = element.Nodes().LastOrDefault();

        return lastChild is not null && NeedsToBorrowParentClosingTagStartMarker(lastChild)
            ? Doc.Null
            : Doc.Concat(PrintClosingTagPrefix(element), PrintClosingTagStartMarker(element));
    }

    public static Doc PrintClosingTagStartMarker(XElement element)
    {
        return $"</{GetPrefixedElementName(element)}";
    }

    public static Doc PrintClosingTagEnd(XElement node)
    {
        return (
            node.NextNode is not null
                ? NeedsToBorrowPrevClosingTagEndMarker(node.NextNode)
                : NeedsToBorrowLastChildClosingTagEndMarker(node.Parent!)
        )
            ? Doc.Null
            : Doc.Concat(PrintClosingTagEndMarker(node), PrintClosingTagSuffix(node));
    }

    public static bool NeedsToBorrowLastChildClosingTagEndMarker(XElement node)
    {
        /*
         *     <p
         *       ><a></a
         *       ></p
         *       ^
         *     >
         */
        return false
        // we don't want to take into account whitespace
        // && !node.Nodes().Last().GetLastDescendant().IsTextLike()
        // && !node.lastChild.hasTrailingSpaces
        // && node.lastChild?.isTrailingSpaceSensitive
        // && !isPreLikeNode(node)
        ;
    }

    public static Doc PrintClosingTagEndMarker(XElement node)
    {
        return node.IsEmpty ? "/>" : ">";
    }

    public static Doc PrintClosingTagSuffix(XNode node)
    {
        return NeedsToBorrowParentClosingTagStartMarker(node)
                ? PrintClosingTagStartMarker(node.Parent!)
            : NeedsToBorrowNextOpeningTagStartMarker(node)
                ? PrintOpeningTagStartMarker(node.NextNode!)
            : Doc.Null;
    }

    private static Doc PrintOpeningTagStartMarker(XNode node)
    {
        if (node is not XElement element)
        {
            return "<" + node;
        }

        return $"<{GetPrefixedElementName(element)}";
    }

    private static string GetPrefixedElementName(XElement element)
    {
        var prefix = element.GetPrefixOfNamespace(element.Name.Namespace);
        if (string.IsNullOrEmpty(prefix))
        {
            return element.Name.LocalName;
        }

        return $"{prefix}:{element.Name.LocalName}";
    }

    private static bool NeedsToBorrowNextOpeningTagStartMarker(XNode node)
    {
        /*
         *     123<p
         *        ^^
         *     >
         */
        return node.NextNode is not null
            && !node.NextNode.IsTextLike()
            // && node.IsTextLike()
            && node is XText and not XCData
        // && node.isTrailingSpaceSensitive
        // prettier does something with removing end of line nodes and setting this value, I don't know
        // that we have that funcionality
        // && !node.hasTrailingSpaces
        ;
    }

    private static bool NeedsToBorrowParentClosingTagStartMarker(XNode node)
    {
        /*
         *     <p>
         *       123</p
         *          ^^^
         *     >
         *
         *         123</b
         *       ></a
         *        ^^^
         *     >
         */
        return (
            node.NextNode is null
            && node.IsTextLike()
            && node.GetLastDescendant() is XText and not XCData
        // && !node.hasTrailingSpaces
        );
    }

    public static Doc PrintClosingTagPrefix(XElement node)
    {
        return Doc.Null;
        // return needsToBorrowLastChildClosingTagEndMarker(node)
        //     ? printClosingTagEndMarker(node.lastChild, options)
        //     : "";
    }

    public static bool NeedsToBorrowParentOpeningTagEndMarker(XNode node)
    {
        /*
         *     <p
         *       >123
         *       ^
         *
         *     <p
         *       ><a
         *       ^
         */
        return node.PreviousNode is null
            // I think isLeadingSpaceSensitive is true for text/comment
            // && node.IsTextLike()
            && node is XText and not XCData
        // && node.isLeadingSpaceSensitive && !node.hasLeadingSpaces
        ;
    }

    private static string PrintOpeningTagEndMarker(XNode node)
    {
        return ">";
        // assert(!node.isSelfClosing);
        // switch (node.type) {
        //     case "ieConditionalComment":
        //         return "]>";
        //     case "element":
        //         if (node.condition) {
        //             return "><!--<![endif]-->";
        //         }
        //     // fall through
        //     default:
        //         return ">";
        // }
    }
}
