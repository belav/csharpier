using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Attributes
{
    public static Doc Print(XElement element, PrintingContext context)
    {
        if (!element.Attributes().Any())
        {
            return element.IsEmpty ? " " : Doc.Null;
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
        foreach (var attribute in element.Attributes())
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

        var doNotBreakAttributes =
            element.Attributes().Count() == 1
            && !element.Attributes().First().Value.Contains('\n')
            && (element.Nodes().Any(o => o is XElement) || element.IsEmpty);
        var attributeLine = Doc.Line;

        var parts = new List<Doc>
        {
            Doc.Indent(
                doNotBreakAttributes ? " " : Doc.Line,
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
                element.Nodes().Any()
                && Tag.NeedsToBorrowParentOpeningTagEndMarker(element.Nodes().First())
            )
            /*
             *     <span
             *       >123<meta
             *                ~
             *     /></span>
             */
            || (element.IsEmpty && Tag.NeedsToBorrowLastChildClosingTagEndMarker(element.Parent!))
            || doNotBreakAttributes
        )
        {
            parts.Add(element.IsEmpty ? " " : "");
        }
        else
        {
            parts.Add(element.IsEmpty ? Doc.Line : Doc.SoftLine);
        }

        return Doc.Concat(parts);
    }

    private static Doc Print(XAttribute attribute, PrintingContext context)
    {
        var value = new XElement("EncodeText", attribute.Value).LastNode!.ToString();

        // TODO #819 may need to convert everything to use XDocument to get this working properly with preserving encoded values, ugh
        return Doc.Concat(attribute.Name.ToString(), "=", "\"", value, "\"");
    }
}
