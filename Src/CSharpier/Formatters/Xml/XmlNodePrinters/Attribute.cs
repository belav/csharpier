using System.Xml;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Attributes
{
    public static Doc Print(XmlElement node, PrintingContext context)
    {
        if (node.Attributes.Count == 0)
        {
            return node.IsEmpty ? " " : Doc.Null;
        }

        // const ignoreAttributeData =
        //     node.prev?.type === "comment" &&
        //     getPrettierIgnoreAttributeCommentData(node.prev.value);

        // const hasPrettierIgnoreAttribute =
        //     typeof ignoreAttributeData === "boolean"
        //         ? () => ignoreAttributeData
        //         : Array.isArray(ignoreAttributeData)
        //           ? (attribute) => ignoreAttributeData.includes(attribute.rawName)
        //           : () => false;

        var printedAttributes = new List<Doc>();
        foreach (XmlAttribute attribute in node.Attributes)
        {
            printedAttributes.Add(
                // hasPrettierIgnoreAttribute(attribute)
                // ? replaceEndOfLine(
                //     options.originalText.slice(
                //         locStart(attribute),
                //         locEnd(attribute),
                //     ),
                // ) :
                Print(attribute, context)
            );
        }

        var forceNotToBreakAttrContent =
            node.Attributes.Count == 1 && node.ChildNodes.Cast<XmlNode>().Any(o => o is XmlElement);
        // node.type === "element" &&
        // node.fullName === "script" &&
        // node.attrs.length === 1 &&
        // node.attrs[0].fullName === "src" &&
        // node.children.length === 0;

        // TODO xml probably attribute per line if there are more than x attributes
        // const shouldPrintAttributePerLine =
        //     options.singleAttributePerLine &&
        //     node.attrs.length > 1 &&
        //     !isVueSfcBlock(node, options);
        // const attributeLine = shouldPrintAttributePerLine ? hardline : line;
        var attributeLine = Doc.Line;

        var parts = new List<Doc>
        {
            Doc.Indent(
                forceNotToBreakAttrContent ? " " : Doc.Line,
                Doc.Join(attributeLine, printedAttributes)
            ),
        };

        if (
            /*
             *     123<a
             *       attr
             *           ~
             *       >456
             */
            (
                node.ChildNodes.Count > 0
                && Tag.NeedsToBorrowParentOpeningTagEndMarker(node.ChildNodes[0]!)
            )
            /*
             *     <span
             *       >123<meta
             *                ~
             *     /></span>
             */
            || (node.IsEmpty && Tag.NeedsToBorrowLastChildClosingTagEndMarker(node.ParentNode!))
            || forceNotToBreakAttrContent
        )
        {
            parts.Add(node.IsEmpty ? " " : "");
        }
        else
        {
            parts.Add(node.IsEmpty ? Doc.Line : Doc.SoftLine);
        }

        return Doc.Concat(parts);
    }

    private static Doc Print(XmlAttribute attribute, PrintingContext context)
    {
        const string quote = "\"";

        var value = attribute.Value.Replace("\"", "&quot;");

        return Doc.Concat(attribute.Name, "=", quote, value, quote);
    }
}
