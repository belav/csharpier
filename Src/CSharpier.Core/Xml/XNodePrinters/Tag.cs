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
            rawNode.IsEmpty ? Doc.Null : PrintOpeningTagEnd(rawNode, context)
        );
    }

    private static Doc PrintOpeningTagStart(RawNode rawNode, PrintingContext context)
    {
        return
            rawNode.PreviousNode is not null
            && NeedsToBorrowNextOpeningTagStartMarker(rawNode.PreviousNode)
            ? Doc.Null
            : Doc.Concat(
                PrintOpeningTagPrefix(rawNode, context),
                PrintOpeningTagStartMarker(rawNode, context)
            );
    }

    private static Doc PrintOpeningTagEnd(RawNode rawNode, PrintingContext context)
    {
        return
            rawNode.Nodes.FirstOrDefault() is { } firstNode
            && NeedsToBorrowParentOpeningTagEndMarker(firstNode, context)
            ? Doc.Null
            : ">";
    }

    public static Doc PrintOpeningTagPrefix(RawNode rawNode, PrintingContext context)
    {
        return NeedsToBorrowParentOpeningTagEndMarker(rawNode, context) ? ">" : "";
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

        return lastChild is not null && PrintParentClosingTagStartWithContent(lastChild, context)
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
        return PrintParentClosingTagStartWithContent(rawNode, context)
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

    private static bool PrintParentClosingTagStartWithContent(
        RawNode rawNode,
        PrintingContext context
    )
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
        // TODO #1790 we really want this last condition only if the indentation of the last line of the text value matches
        // the indentation of the start element. Bleh.
        /*
may have to handle one of these vs the second
<Root>
  <Element Attribute="TheSign">
    Life is demanding.
    </Element>
</Root>
<Root>
  <Element Attribute="TheSign">
    Life is demanding.
  </Element>
</Root>
there is also this case
<Root>
       <Element >
    Life is demanding.
         </Element>
</Root>
         */
        return context.Options.XmlWhitespaceSensitivity is XmlWhitespaceSensitivity.Strict
            && rawNode.NextNode is null
            && rawNode.IsTextLike()
            && rawNode.GetLastDescendant() is { NodeType: XmlNodeType.Text } textNode
            && (
                textNode.Value[^1] is not (' ' or '\r' or '\n')
                || !textNode.Value.Contains('\n')
                || rawNode.Parent!.Nodes.Any(o => !o.IsTextLike())
            );
    }

    public static bool NeedsToBorrowParentOpeningTagEndMarker(
        RawNode rawNode,
        PrintingContext context
    )
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
        return context.Options.XmlWhitespaceSensitivity is XmlWhitespaceSensitivity.Strict
            && rawNode.PreviousNode is null
            && rawNode.NodeType is XmlNodeType.Text
            && rawNode.Value![0] is not ('\r' or '\n');
    }
}
