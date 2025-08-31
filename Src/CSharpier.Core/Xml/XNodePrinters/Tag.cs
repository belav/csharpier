using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Tag
{
    public static Doc PrintOpeningTag(RawNode rawNode, PrintingContext context)
    {
        return Doc.Concat(
            PrintOpeningTagStart(rawNode, context),
            Attributes.Print(rawNode, context),
            rawNode.IsEmpty ? Doc.Null : PrintOpeningTagEnd(rawNode)
        );
    }

    private static Doc PrintOpeningTagStart(RawNode rawNode, PrintingContext context)
    {
        return
            rawNode.PreviousNode is not null
            && NeedsToBorrowNextOpeningTagStartMarker(rawNode.PreviousNode)
            ? Doc.Null
            : Doc.Concat(
                PrintOpeningTagPrefix(rawNode),
                PrintOpeningTagStartMarker(rawNode, context)
            );
    }

    private static Doc PrintOpeningTagEnd(RawNode rawNode)
    {
        return
            rawNode.Nodes.FirstOrDefault() is { } firstNode
            && NeedsToBorrowParentOpeningTagEndMarker(firstNode)
            ? Doc.Null
            : ">";
    }

    public static Doc PrintOpeningTagPrefix(RawNode rawNode)
    {
        return NeedsToBorrowParentOpeningTagEndMarker(rawNode) ? ">" : "";
    }

    public static Doc PrintClosingTag(RawNode rawNode, PrintingContext context)
    {
        return Doc.Concat(
            rawNode.IsEmpty ? Doc.Null : PrintClosingTagStart(rawNode, context),
            PrintClosingTagEnd(rawNode, context)
        );
    }

    public static Doc PrintClosingTagStart(RawNode rawNode, PrintingContext context)
    {
        var lastChild = rawNode.Nodes.LastOrDefault();

        return lastChild is not null && NeedsToBorrowParentClosingTagStartMarker(lastChild)
            ? Doc.Null
            : PrintClosingTagStartMarker(rawNode, context);
    }

    public static Doc PrintClosingTagStartMarker(RawNode rawNode, PrintingContext context)
    {
        return $"</{rawNode.Name}";
    }

    public static Doc PrintClosingTagEnd(RawNode rawNode, PrintingContext context)
    {
        return Doc.Concat(
            PrintClosingTagEndMarker(rawNode),
            PrintClosingTagSuffix(rawNode, context)
        );
    }

    public static Doc PrintClosingTagEndMarker(RawNode rawNode)
    {
        return rawNode.IsEmpty ? "/>" : ">";
    }

    public static Doc PrintClosingTagSuffix(RawNode rawNode, PrintingContext context)
    {
        return NeedsToBorrowParentClosingTagStartMarker(rawNode)
                ? PrintClosingTagStartMarker(rawNode.Parent!, context)
            : NeedsToBorrowNextOpeningTagStartMarker(rawNode)
                ? PrintOpeningTagStartMarker(rawNode.NextNode!, context)
            : Doc.Null;
    }

    private static Doc PrintOpeningTagStartMarker(RawNode rawNode, PrintingContext context)
    {
        if (rawNode.NodeType != XmlNodeType.Element)
        {
            return "<" + rawNode.Name;
        }

        return $"<{rawNode.Name}";
    }

    private static bool NeedsToBorrowNextOpeningTagStartMarker(RawNode rawNode)
    {
        /*
         *     123<p
         *        ^^
         *     >
         */
        return rawNode.NextNode is not null
            && !rawNode.NextNode.IsTextLike()
            && rawNode.IsTextLike()
            && rawNode.NodeType is XmlNodeType.Text and not XmlNodeType.CDATA
        // && node.isTrailingSpaceSensitive
        // prettier does something with removing end of line nodes and setting this value, I don't know
        // that we have that functionality
        // && !node.hasTrailingSpaces
        ;
    }

    private static bool NeedsToBorrowParentClosingTagStartMarker(RawNode rawNode)
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
        return rawNode.NextNode is null
            && rawNode.IsTextLike()
            && rawNode.GetLastDescendant().NodeType is XmlNodeType.Text;
    }

    public static bool NeedsToBorrowParentOpeningTagEndMarker(RawNode rawNode)
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
        return rawNode.PreviousNode is null
            && rawNode.NodeType is XmlNodeType.Text
            && rawNode.Value![0] is not ('\r' or '\n');
    }
}
