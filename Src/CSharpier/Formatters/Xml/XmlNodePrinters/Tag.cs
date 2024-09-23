using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(XmlElement node)
    {
        return Doc.Concat(
            PrintOpeningTagStart(node),
            PrintAttributes(node),
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
            ? ""
            : Doc.Concat(PrintClosingTagEndMarker(node), PrintClosingTagSuffix(node));
    }

    private static bool NeedsToBorrowLastChildClosingTagEndMarker(XmlNode node)
    {
        /*
         *     <p
         *       ><a></a
         *       ></p
         *       ^
         *     >
         */
        return !node.LastChild!.GetLastDescendant().IsTextLike()
        // && node.lastChild?.isTrailingSpaceSensitive
        // && !node.lastChild.hasTrailingSpaces
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
            : "";
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
        return node.NextSibling is not null && !node.NextSibling.IsTextLike() && node.IsTextLike()
        // && node.isTrailingSpaceSensitive
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
            node.NextSibling is null
            // && !node.hasTrailingSpaces
            // && node.isTrailingSpaceSensitive
            && node.GetLastDescendant().IsTextLike()
        );
    }

    public static Doc PrintClosingTagPrefix(XmlElement node)
    {
        return Doc.Null;
        // return needsToBorrowLastChildClosingTagEndMarker(node)
        //     ? printClosingTagEndMarker(node.lastChild, options)
        //     : "";
    }

    private static bool NeedsToBorrowParentOpeningTagEndMarker(XmlNode node)
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
        // && node.isLeadingSpaceSensitive && !node.hasLeadingSpaces
        ;
    }

    public static Doc PrintAttributes(XmlElement node)
    {
        if (node.Attributes.Count == 0)
        {
            return node.IsEmpty ? " " : Doc.Null;
        }

        // this is just shoved in here for now
        var result = new List<Doc>();
        foreach (XmlAttribute attribute in node.Attributes)
        {
            result.Add(Doc.Line, attribute.Name, "=\"", attribute.Value, "\"");
        }

        return Doc.Indent(result);
        // const ignoreAttributeData =
        //     node.prev?.type === "comment" &&
        //     getPrettierIgnoreAttributeCommentData(node.prev.value);
        //
        // const hasPrettierIgnoreAttribute =
        //     typeof ignoreAttributeData === "boolean"
        //         ? () => ignoreAttributeData
        //         : Array.isArray(ignoreAttributeData)
        //           ? (attribute) => ignoreAttributeData.includes(attribute.rawName)
        //           : () => false;
        //
        // const printedAttributes = path.map(
        //     ({ node: attribute }) =>
        //         hasPrettierIgnoreAttribute(attribute)
        //             ? replaceEndOfLine(
        //                   options.originalText.slice(
        //                       locStart(attribute),
        //                       locEnd(attribute),
        //                   ),
        //               )
        //             : print(),
        //     "attrs",
        // );
        //
        // const forceNotToBreakAttrContent =
        //     node.type === "element" &&
        //     node.fullName === "script" &&
        //     node.attrs.length === 1 &&
        //     node.attrs[0].fullName === "src" &&
        //     node.children.length === 0;
        //
        // const shouldPrintAttributePerLine =
        //     options.singleAttributePerLine &&
        //     node.attrs.length > 1 &&
        //     !isVueSfcBlock(node, options);
        // const attributeLine = shouldPrintAttributePerLine ? hardline : line;
        //
        // /** @type {Doc[]} */
        // const parts = [
        //     indent([
        //         forceNotToBreakAttrContent ? " " : line,
        //         join(attributeLine, printedAttributes),
        //     ]),
        // ];
        //
        // if (
        //     /**
        //      *     123<a
        //      *       attr
        //      *           ~
        //      *       >456
        //      */
        //     (node.firstChild &&
        //         needsToBorrowParentOpeningTagEndMarker(node.firstChild)) ||
        //     /**
        //      *     <span
        //      *       >123<meta
        //      *                ~
        //      *     /></span>
        //      */
        //     (node.isSelfClosing &&
        //         needsToBorrowLastChildClosingTagEndMarker(node.parent)) ||
        //     forceNotToBreakAttrContent
        // ) {
        //     parts.push(node.isSelfClosing ? " " : "");
        // } else {
        //     parts.push(
        //         options.bracketSameLine
        //             ? node.isSelfClosing
        //                 ? " "
        //                 : ""
        //             : node.isSelfClosing
        //               ? line
        //               : softline,
        //     );
        // }
        //
        // return parts;
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
