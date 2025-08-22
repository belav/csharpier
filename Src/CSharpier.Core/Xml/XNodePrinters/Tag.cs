using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(
        BetterXmlReader xmlReader,
        RawAttribute[] attributes,
        PrintingContext context
    )
    {
        return Doc.Concat(
            PrintOpeningTagStart(xmlReader, context),
            Attributes.Print(xmlReader, attributes, context),
            xmlReader.IsEmptyElement ? Doc.Null : PrintOpeningTagEnd(xmlReader)
        );
    }

    private static Doc PrintOpeningTagStart(BetterXmlReader xmlReader, PrintingContext context)
    {
        return
        // TOOD need to know if previous node is text like
        // xmlReader.PreviousNode is not null
        // && NeedsToBorrowNextOpeningTagStartMarker(xmlReader.PreviousNode)
        false
            ? Doc.Null
            : Doc.Concat(
                PrintOpeningTagPrefix(xmlReader),
                PrintOpeningTagStartMarker(xmlReader, context)
            );
    }

    private static Doc PrintOpeningTagEnd(BetterXmlReader xmlReader)
    {
        return false // node.FirstNode is not null && NeedsToBorrowParentOpeningTagEndMarker(node.FirstNode)
            ? Doc.Null
            : ">";
    }

    public static Doc PrintOpeningTagPrefix(BetterXmlReader xmlReader)
    {
        return string.Empty;
        // NeedsToBorrowParentOpeningTagEndMarker(element)
        //     ? ">"
        // : NeedsToBorrowPrevClosingTagEndMarker(element)
        //     ? PrintClosingTagEndMarker((element.PreviousNode as XElement)!)
        //: "";
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

    public static Doc PrintClosingTag(BetterXmlReader xmlReader, PrintingContext context)
    {
        return Doc.Concat(
            xmlReader.IsEmptyElement ? Doc.Null : PrintClosingTagStart(xmlReader, context),
            PrintClosingTagEnd(xmlReader, context)
        );
    }

    public static Doc PrintClosingTagStart(BetterXmlReader xmlReader, PrintingContext context)
    {
        // var lastChild = xmlReader.Nodes().LastOrDefault();

        return false //lastChild is not null && NeedsToBorrowParentClosingTagStartMarker(lastChild)
            ? Doc.Null
            : PrintClosingTagStartMarker(xmlReader, context);
    }

    public static Doc PrintClosingTagStartMarker(BetterXmlReader xmlReader, PrintingContext context)
    {
        return $"</{GetPrefixedElementName(xmlReader, context)}";
    }

    public static Doc PrintClosingTagEnd(BetterXmlReader xmlReader, PrintingContext context)
    {
        return false
            // (
            //     xmlReader.NextNode is not null
            //         ? NeedsToBorrowPrevClosingTagEndMarker(xmlReader.NextNode)
            //         : false
            // )
            ? Doc.Null
            : Doc.Concat(
                PrintClosingTagEndMarker(xmlReader),
                PrintClosingTagSuffix(xmlReader, context)
            );
    }

    public static Doc PrintClosingTagEndMarker(BetterXmlReader xmlReader)
    {
        return xmlReader.IsEmptyElement ? "/>" : ">";
    }

    public static Doc PrintClosingTagSuffix(BetterXmlReader xmlReader, PrintingContext context)
    {
        return "";
        // return NeedsToBorrowParentClosingTagStartMarker(node)
        //         ? PrintClosingTagStartMarker(node.Parent!, context)
        //     : NeedsToBorrowNextOpeningTagStartMarker(node)
        //         ? PrintOpeningTagStartMarker(node.NextNode!, context)
        // TODO 1679 should this be string.empty?
        //     : Doc.Null;
    }

    private static Doc PrintOpeningTagStartMarker(
        BetterXmlReader xmlReader,
        PrintingContext context
    )
    {
        if (xmlReader.NodeType != XmlNodeType.Element)
        {
            return "<" + xmlReader.Name;
        }

        return $"<{GetPrefixedElementName(xmlReader, context)}";
    }

    private static string GetPrefixedElementName(BetterXmlReader xmlReader, PrintingContext context)
    {
        return xmlReader.Name;
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
        return node.NextNode is null
            && node.IsTextLike()
            && node.GetLastDescendant() is XText and not XCData;
        // && !node.hasTrailingSpaces
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
            && node is XText xText and not XCData
            && xText.ToString()[0] is not ('\r' or '\n')
        // && node.isLeadingSpaceSensitive && !node.hasLeadingSpaces
        ;
    }
}
