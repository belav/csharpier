using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(XElement node, PrintingContext context)
    {
        return Doc.Concat(
            PrintOpeningTagStart(node),
            Attributes.Print(node, context),
            node.IsEmpty ? Doc.Null : PrintOpeningTagEnd(node)
        );
    }

    private static Doc PrintOpeningTagStart(XElement node)
    {
        return
            node.PreviousNode is not null
            && NeedsToBorrowNextOpeningTagStartMarker(node.PreviousNode)
            ? Doc.Null
            : Doc.Concat(PrintOpeningTagPrefix(node), PrintOpeningTagStartMarker(node));
    }

    private static Doc PrintOpeningTagEnd(XElement node)
    {
        var firstChild = node.Nodes().FirstOrDefault();
        return firstChild is not null && NeedsToBorrowParentOpeningTagEndMarker(firstChild)
            ? Doc.Null
            : PrintOpeningTagEndMarker(node);
    }

    public static Doc PrintOpeningTagPrefix(XNode node)
    {
        return NeedsToBorrowParentOpeningTagEndMarker(node) ? PrintOpeningTagEndMarker(node.Parent!)
            : NeedsToBorrowPrevClosingTagEndMarker(node)
                ? PrintClosingTagEndMarker((node.PreviousNode as XElement)!)
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

    public static Doc PrintClosingTagStart(XElement node)
    {
        var lastChild = node.Nodes().LastOrDefault();

        return lastChild is not null && NeedsToBorrowParentClosingTagStartMarker(lastChild)
            ? Doc.Null
            : Doc.Concat(PrintClosingTagPrefix(node), PrintClosingTagStartMarker(node));
    }

    public static Doc PrintClosingTagStartMarker(XNode node)
    {
        // if (shouldNotPrintClosingTag(node)) {
        //     return "";
        // }
        // switch (node.type) {
        //     case "ieConditionalComment":
        //         return "<!";
        //     case "element":
        //         if (node.hasHtmComponentClosingTag) {
        //             return "<//";
        //         }
        //     // fall through
        //     default:
        return "</" + node;
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
        return !node.Nodes().Last().GetLastDescendant().IsTextLike()
            // we don't want to take into account whitespace
            && !true
        // && !node.lastChild.hasTrailingSpaces
        // && node.lastChild?.isTrailingSpaceSensitive
        // && !isPreLikeNode(node)
        ;
    }

    public static Doc PrintClosingTagEndMarker(XElement node)
    {
        // if (shouldNotPrintClosingTag(node, options)) {
        //     return "";
        // }
        // switch (node.type) {
        //     case "ieConditionalComment":
        //     case "ieConditionalEndComment":
        //         return "[endif]-->";
        //     case "ieConditionalStartComment":
        //         return "]><!-->";
        //     case "interpolation":
        //         return "}}";
        //     case "angularIcuExpression":
        //         return "}";
        //     case "element":
        //         if (node.isSelfClosing) {
        //             return "/>";
        //         }
        //     // fall through
        //     default:

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
        // switch (node.type) {
        // case "ieConditionalComment":
        // case "ieConditionalStartComment":
        //     return `<!--[if ${node.condition}`;
        // case "ieConditionalEndComment":
        //     return "<!--<!";
        // case "interpolation":
        //     return "{{";
        // case "docType":
        //     return node.value === "html" ? "<!doctype" : "<!DOCTYPE";
        // case "angularIcuExpression":
        //     return "{";
        // case "element":
        //     if (node.condition) {
        //         return `<!--[if ${node.condition}]><!--><${node.rawName}`;
        //     }
        // // fall through
        // default:
        return $"<{(node is XElement element ? element.Name : node.ToString())}";
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
            && node is XText
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
            node.NextNode is null && node.IsTextLike() && node.GetLastDescendant() is XText
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
            && node is XText
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
