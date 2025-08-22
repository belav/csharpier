using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(RawElement rawElement, PrintingContext context)
    {
        return Doc.Concat(
            PrintOpeningTagStart(rawElement, context),
            Attributes.Print(rawElement, context),
            rawElement.IsEmpty ? Doc.Null : PrintOpeningTagEnd(rawElement)
        );
    }

    private static Doc PrintOpeningTagStart(RawElement rawElement, PrintingContext context)
    {
        return
        // TOOD need to know if previous node is text like
        // rawElement.PreviousNode is not null
        // && NeedsToBorrowNextOpeningTagStartMarker(rawElement.PreviousNode)
        false
            ? Doc.Null
            : Doc.Concat(
                PrintOpeningTagPrefix(rawElement),
                PrintOpeningTagStartMarker(rawElement, context)
            );
    }

    private static Doc PrintOpeningTagEnd(RawElement rawElement)
    {
        return false // node.FirstNode is not null && NeedsToBorrowParentOpeningTagEndMarker(node.FirstNode)
            ? Doc.Null
            : ">";
    }

    public static Doc PrintOpeningTagPrefix(RawElement rawElement)
    {
        return string.Empty;
        // NeedsToBorrowParentOpeningTagEndMarker(element)
        //     ? ">"
        // : NeedsToBorrowPrevClosingTagEndMarker(element)
        //     ? PrintClosingTagEndMarker((element.PreviousNode as XElement)!)
        //: "";
    }

    // TODO 1679 kill this?
    private static bool NeedsToBorrowPrevClosingTagEndMarker(RawElement element)
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
        return element.PreviousNode is not null
            // && node.prev.type !== "docType"
            //    && node.type !== "angularControlFlowBlock"
            && element.PreviousNode.NodeType is not (XmlNodeType.Text or XmlNodeType.Comment)
            // && node.isLeadingSpaceSensitive
            && !true
        // && !node.hasLeadingSpaces
        ;
    }

    public static Doc PrintClosingTag(RawElement rawElement, PrintingContext context)
    {
        return Doc.Concat(
            rawElement.IsEmpty ? Doc.Null : PrintClosingTagStart(rawElement, context),
            PrintClosingTagEnd(rawElement, context)
        );
    }

    public static Doc PrintClosingTagStart(RawElement rawElement, PrintingContext context)
    {
        // var lastChild = rawElement.Nodes().LastOrDefault();

        return false //lastChild is not null && NeedsToBorrowParentClosingTagStartMarker(lastChild)
            ? Doc.Null
            : PrintClosingTagStartMarker(rawElement, context);
    }

    public static Doc PrintClosingTagStartMarker(RawElement rawElement, PrintingContext context)
    {
        return $"</{GetPrefixedElementName(rawElement, context)}";
    }

    public static Doc PrintClosingTagEnd(RawElement rawElement, PrintingContext context)
    {
        return (
            rawElement.NextNode is not null
                ? NeedsToBorrowPrevClosingTagEndMarker(rawElement.NextNode)
                : false
        )
            ? Doc.Null
            : Doc.Concat(
                PrintClosingTagEndMarker(rawElement),
                PrintClosingTagSuffix(rawElement, context)
            );
    }

    public static Doc PrintClosingTagEndMarker(RawElement rawElement)
    {
        return rawElement.IsEmpty ? "/>" : ">";
    }

    public static Doc PrintClosingTagSuffix(RawElement rawElement, PrintingContext context)
    {
        return NeedsToBorrowParentClosingTagStartMarker(rawElement)
                ? PrintClosingTagStartMarker(rawElement.Parent!, context)
            : NeedsToBorrowNextOpeningTagStartMarker(rawElement)
                ? PrintOpeningTagStartMarker(rawElement.NextNode!, context)
            // TODO 1679 should this be string.empty?
            : Doc.Null;
    }

    private static Doc PrintOpeningTagStartMarker(RawElement rawElement, PrintingContext context)
    {
        if (rawElement.NodeType != XmlNodeType.Element)
        {
            return "<" + rawElement.Name;
        }

        return $"<{GetPrefixedElementName(rawElement, context)}";
    }

    private static string GetPrefixedElementName(RawElement rawElement, PrintingContext context)
    {
        return rawElement.Name;
        // var prefix = element.GetPrefixOfNamespace(element.Name.Namespace);
        // if (
        //     string.IsNullOrEmpty(prefix)
        //     || !context.Mapping[element].Name.StartsWith(prefix, StringComparison.Ordinal)
        // )
        // {
        //     return element.Name.LocalName;
        // }
        //
        // return $"{prefix}:{element.Name.LocalName}";
    }

    private static bool NeedsToBorrowNextOpeningTagStartMarker(RawElement rawElement)
    {
        /*
         *     123<p
         *        ^^
         *     >
         */
        return rawElement.NextNode is not null
            && !rawElement.NextNode.IsTextLike()
            && rawElement.IsTextLike()
            && rawElement.NodeType is XmlNodeType.Text and not XmlNodeType.CDATA
        // && node.isTrailingSpaceSensitive
        // prettier does something with removing end of line nodes and setting this value, I don't know
        // that we have that functionality
        // && !node.hasTrailingSpaces
        ;
    }

    private static bool NeedsToBorrowParentClosingTagStartMarker(RawElement rawElement)
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
        return rawElement.NextNode is null && false;
        // TODO 1679
        // && rawElement.IsTextLike()
        // && rawElement.GetLastDescendant() is XText and not XCData;
    }

    public static bool NeedsToBorrowParentOpeningTagEndMarker(RawElement rawElement)
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
        return rawElement.PreviousNode is null
            && rawElement.NodeType is XmlNodeType.Text and not XmlNodeType.CDATA
            && rawElement.Value[0] is not ('\r' or '\n')
        // && node.isLeadingSpaceSensitive && !node.hasLeadingSpaces
        ;
    }
}
