using System.Text;
using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Attributes
{
    public static Doc Print(
        BetterXmlReader xmlReader,
        RawAttribute[] attributes,
        PrintingContext context
    )
    {
        if (attributes.Length == 0)
        {
            return xmlReader.IsEmptyElement ? " " : Doc.Null;
        }

        var printedAttributes = new List<Doc>();
        var index = 0;

        foreach (var attribute in attributes)
        {
            printedAttributes.Add(PrintAttribute(attribute));

            index++;
        }

        var doNotBreakAttributes = attributes.Length == 1 && !attributes[0].Value.Contains('\n');
        // TODO 1679     && (element.Nodes().Any(o => o is XElement) || element.IsEmpty);
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
                false
            // TODO C1679 xmlReader.Nodes().Any()
            // && Tag.NeedsToBorrowParentOpeningTagEndMarker(xmlReader.Nodes().First())
            ) || doNotBreakAttributes
        )
        {
            parts.Add(xmlReader.IsEmptyElement ? " " : "");
        }
        else
        {
            parts.Add(xmlReader.IsEmptyElement ? Doc.Line : Doc.SoftLine);
        }

        return Doc.Concat(parts);
    }

    private static Doc PrintAttribute(RawAttribute attribute)
    {
        return $"{attribute.Name}=\"{attribute.Value}\"";

        // XAttribute mostly gets us what we want, but it removes new lines (per spec)
        // XmlAttribute gives us those new lines
        // this makes use of both values to get the final value we want to display
        // we want the new lines because it is common to add them with long conditions in csproj files
        // string GetAttributeValue()
        // {
        //     // this gives us the attribute value with everything encoded after we remove the name + quotes
        //     var xValue = attribute.ToString();
        //     xValue = xValue[(xValue.IndexOf('=') + 2)..];
        //     xValue = xValue[..^1];
        //     var xmlValue = xmlAttribute.Value;
        //
        //     if (xValue == xmlValue)
        //     {
        //         return xValue;
        //     }
        //
        //     var valueBuilder = new StringBuilder();
        //     var xmlIndex = 0;
        //     var xIndex = 0;
        //     while (xIndex < xValue.Length)
        //     {
        //         var xChar = xValue[xIndex];
        //         var xmlChar = xmlValue[xmlIndex];
        //
        //         if (xChar == ' ' && xmlChar == '\r')
        //         {
        //             valueBuilder.Append(xmlChar);
        //             xmlIndex++;
        //             xmlChar = xmlValue[xmlIndex];
        //         }
        //
        //         if (xChar == '&')
        //         {
        //             do
        //             {
        //                 valueBuilder.Append(xChar);
        //                 xIndex++;
        //                 xChar = xValue[xIndex];
        //             } while (xChar != ';');
        //             valueBuilder.Append(xChar);
        //         }
        //
        //         if (xChar == xmlChar || (xChar == ' ' && xmlChar == '\n'))
        //         {
        //             valueBuilder.Append(xmlChar);
        //         }
        //
        //         xIndex++;
        //         xmlIndex++;
        //     }
        //
        //     return valueBuilder.ToString();
        // }
        //
        // var name = attribute.Name.LocalName;
        // var prefix = attribute.Parent!.GetPrefixOfNamespace(attribute.Name.Namespace);
        // if (!string.IsNullOrEmpty(prefix))
        // {
        //     name = $"{prefix}:{name}";
        // }
        //
        // return $"{name}=\"{GetAttributeValue()}\"";
    }
}
