using System.Text;
using System.Xml;
using System.Xml.Linq;

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
            printedAttributes.Add(PrintAttribute(attribute, xmlNode.Attributes![index]));

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

    private static Doc PrintAttribute(XAttribute attribute, XmlAttribute xmlAttribute)
    {
        // XAttribute mostly gets us what we want, but it removes new lines
        // XmlAttribute gives us those new lines
        // this makes use of both values to get the final value we want to display
        string GetAttributeValue()
        {
            var xValue = attribute
                .Value.Replace("\"", "&quot;")
                .Replace("\r\n", "&#010;")
                .Replace("\n", "&#010;");
            var xmlValue = xmlAttribute.Value;

            if (xValue == xmlValue)
            {
                return xValue;
            }

            var valueBuilder = new StringBuilder();
            var xmlIndex = 0;
            var xIndex = 0;
            while (xIndex < xValue.Length)
            {
                var xChar = xValue[xIndex];
                var xmlChar = xmlValue[xmlIndex];

                if (xChar == ' ' && xmlChar == '\r')
                {
                    valueBuilder.Append(xmlChar);
                    xmlIndex++;
                    xmlChar = xmlValue[xmlIndex];
                }

                if (xChar == xmlChar || (xChar == ' ' && xmlChar == '\n'))
                {
                    valueBuilder.Append(xmlChar);
                }

                if (xChar == '&')
                {
                    do
                    {
                        valueBuilder.Append(xChar);
                        xIndex++;
                        xChar = xValue[xIndex];
                    } while (xChar != ';');
                    valueBuilder.Append(xChar);
                }

                xIndex++;
                xmlIndex++;
            }

            return valueBuilder.ToString();
        }

        var name = attribute.Name.LocalName;
        var prefix = attribute.Parent!.GetPrefixOfNamespace(attribute.Name.Namespace);
        if (!string.IsNullOrEmpty(prefix))
        {
            name = $"{prefix}:{name}";
        }

        return $"{name}=\"{GetAttributeValue()}\"";
    }
}
