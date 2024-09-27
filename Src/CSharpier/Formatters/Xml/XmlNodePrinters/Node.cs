using System.Xml;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XmlNodePrinters;

internal static class Node
{
    internal static Doc Print(XmlNode xmlNode, PrintingContext context)
    {
        if (xmlNode is XmlDocument xmlDocument)
        {
            var result = new List<Doc>();
            foreach (XmlNode node in xmlDocument.ChildNodes)
            {
                result.Add(Print(node, context), Doc.HardLine);
            }

            result.Add(Doc.HardLine);

            return Doc.Concat(result);
        }

        if (xmlNode is XmlDeclaration xmlDeclaration)
        {
            return xmlDeclaration.OuterXml;
        }

        if (xmlNode is XmlElement xmlElement)
        {
            return Element.Print(xmlElement, context);
        }

        if (xmlNode is XmlText xmlText)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(xmlText),
                .. Utils.GetTextValueParts(xmlText),
                Tag.PrintClosingTagSuffix(xmlText),
            ];

            if (doc.All(o => o is StringDoc))
            {
                var result = string.Join(string.Empty, doc.Select(o => ((StringDoc)o).Value));
                return result;
            }

            return Doc.Concat(doc);

            // var printed = CleanDoc(doc);
            //
            //
            // // if (Array.isArray(printed)) {
            // //     return fill(printed);
            // // }
            //
            // return printed;
        }

        if (xmlNode is XmlComment)
        {
            return xmlNode.OuterXml;
        }
        // TODO xml printer-html.js

        throw new Exception("Need to handle + " + xmlNode);
    }
}
