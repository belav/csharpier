using System.Diagnostics.CodeAnalysis;
using System.Xml;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(XmlElement node, PrintingContext context)
    {
        return Doc.Concat(
            PrintOpeningTagStart(node),
            Attributes.Print(node, context),
            node.IsEmpty ? Doc.Null : PrintOpeningTagEnd(node)
        );
    }

    private static Doc PrintOpeningTagStart(XmlElement node)
    {
        return
            node.PreviousSibling is not null
            && NeedsToBorrowNextOpeningTagStartMarker(node.PreviousSibling)
            ? Doc.Null
            : Doc.Concat(PrintOpeningTagPrefix(node), PrintOpeningTagStartMarker(node));
    }

    private static Doc PrintOpeningTagEnd(XmlElement node)
    {
        return
            node.FirstChild is not null && NeedsToBorrowParentOpeningTagEndMarker(node.FirstChild)
            ? Doc.Null
            : PrintOpeningTagEndMarker(node);
    }

    public static Doc PrintOpeningTagPrefix(XmlNode node)
    {
        return NeedsToBorrowParentOpeningTagEndMarker(node)
                ? PrintOpeningTagEndMarker(node.ParentNode!)
            : NeedsToBorrowPrevClosingTagEndMarker(node)
                ? PrintClosingTagEndMarker((node.PreviousSibling as XmlElement)!)
            : "";
    }

    private static bool NeedsToBorrowPrevClosingTagEndMarker(XmlNode node)
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
        return node.PreviousSibling is not null
            // && node.prev.type !== "docType"
            //    && node.type !== "angularControlFlowBlock"
            && node.PreviousSibling is not (XmlText or XmlComment)
            // && node.isLeadingSpaceSensitive
            && !true
        // && !node.hasLeadingSpaces
        ;
    }

    public static Doc PrintClosingTag(XmlElement node)
    {
        return Doc.Concat(
            node.IsEmpty ? Doc.Null : PrintClosingTagStart(node),
            PrintClosingTagEnd(node)
        );
    }

    public static Doc PrintClosingTagStart(XmlElement node)
    {
        return
            node.LastChild is not null && NeedsToBorrowParentClosingTagStartMarker(node.LastChild)
            ? Doc.Null
            : Doc.Concat(PrintClosingTagPrefix(node), PrintClosingTagStartMarker(node));
    }

    public static Doc PrintClosingTagStartMarker(XmlNode node)
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
        return "</" + node.Name;
    }

    public static Doc PrintClosingTagEnd(XmlElement node)
    {
        return (
            node.NextSibling is not null
                ? NeedsToBorrowPrevClosingTagEndMarker(node.NextSibling)
                : NeedsToBorrowLastChildClosingTagEndMarker(node.ParentNode!)
        )
            ? Doc.Null
            : Doc.Concat(PrintClosingTagEndMarker(node), PrintClosingTagSuffix(node));
    }

    public static bool NeedsToBorrowLastChildClosingTagEndMarker(XmlNode node)
    {
        /*
         *     <p
         *       ><a></a
         *       ></p
         *       ^
         *     >
         */
        return !node.LastChild!.GetLastDescendant().IsTextLike()
            // we don't want to take into account whitespace
            && !true
        // && !node.lastChild.hasTrailingSpaces
        // && node.lastChild?.isTrailingSpaceSensitive
        // && !isPreLikeNode(node)
        ;
    }

    public static Doc PrintClosingTagEndMarker(XmlElement node)
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

    public static Doc PrintClosingTagSuffix(XmlNode node)
    {
        return NeedsToBorrowParentClosingTagStartMarker(node)
                ? PrintClosingTagStartMarker(node.ParentNode!)
            : NeedsToBorrowNextOpeningTagStartMarker(node)
                ? PrintOpeningTagStartMarker(node.NextSibling!)
            : Doc.Null;
    }

    private static Doc PrintOpeningTagStartMarker(XmlNode node)
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
        return $"<{node.Name}";
    }

    private static bool NeedsToBorrowNextOpeningTagStartMarker(XmlNode node)
    {
        /*
         *     123<p
         *        ^^
         *     >
         */
        return node.NextSibling is not null
            && !node.NextSibling.IsTextLike()
            // && node.IsTextLike()
            && node is XmlText
        // && node.isTrailingSpaceSensitive
        // prettier does something with removing end of line nodes and setting this value, I don't know
        // that we have that funcionality
        // && !node.hasTrailingSpaces
        ;
    }

    private static bool NeedsToBorrowParentClosingTagStartMarker(XmlNode node)
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
            node.NextSibling is null && node.IsTextLike() && node.GetLastDescendant() is XmlText
        // && !node.hasTrailingSpaces
        );
    }

    public static Doc PrintClosingTagPrefix(XmlElement node)
    {
        return Doc.Null;
        // return needsToBorrowLastChildClosingTagEndMarker(node)
        //     ? printClosingTagEndMarker(node.lastChild, options)
        //     : "";
    }

    public static bool NeedsToBorrowParentOpeningTagEndMarker(XmlNode node)
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
        return node.PreviousSibling is null
            // I think isLeadingSpaceSensitive is true for text/comment
            // && node.IsTextLike()
            && node is XmlText
        // && node.isLeadingSpaceSensitive && !node.hasLeadingSpaces
        ;
    }

    private static string PrintOpeningTagEndMarker(XmlNode node)
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
