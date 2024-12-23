using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Attributes
{
    public static Doc Print(XElement element, XmlPrintingContext context)
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
        var index = 0;
        var xmlNode = context.Mapping[element];
        foreach (var attribute in element.Attributes())
        {
            printedAttributes.Add(Print((attribute, xmlNode.Attributes![index]), context));

            index++;
        }

        var doNotBreakAttributes =
            element.Attributes().Count() == 1
            && !context.Mapping[element].Attributes![0].Value.Contains('\n')
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

    private static Doc Print((XAttribute, XmlAttribute) attributes, XmlPrintingContext context)
    {
        var (attribute, xmlAttribute) = attributes;

        var value = new XElement("EncodeText", attribute.Value).LastNode!.ToString();

        // TODO #819 we need to somehow combine the value from XAttribute and XmlAttribute, ugh
        // probably also need to take into account \n vs \r\n
        return Doc.Concat(attribute.Name.ToString(), "=", "\"", xmlAttribute.Value, "\"");
    }
}
