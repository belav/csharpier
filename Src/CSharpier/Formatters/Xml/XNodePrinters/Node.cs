using System.Xml.Linq;
using CSharpier.SyntaxPrinter;

namespace CSharpier.Formatters.Xml.XNodePrinters;

internal static class Node
{
    internal static Doc Print(XNode xNode, XmlPrintingContext context)
    {
        if (xNode is XDocument xDocument)
        {
            var result = new List<Doc>();
            foreach (var node in xDocument.Nodes())
            {
                result.Add(Print(node, context), Doc.HardLine);
            }

            result.Add(Doc.HardLine);

            return Doc.Concat(result);
        }

        // if (xNode is XDeclaration xmlDeclaration)
        // {
        //     return xmlDeclaration.OuterXml;
        // }

        if (xNode is XDocumentType xDocumentType)
        {
            return xDocumentType.ToString().Replace("[]", string.Empty);
        }

        if (xNode is XElement xElement)
        {
            return Element.Print(xElement, context);
        }

        if (xNode is XText xText)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(xText),
                GetEncodedTextValue(xText),
                Tag.PrintClosingTagSuffix(xText),
            ];

            if (doc.All(o => o is StringDoc))
            {
                var result = string.Join(string.Empty, doc.Select(o => ((StringDoc)o).Value));
                return result;
            }

            return Doc.Concat(doc);
        }

        if (xNode is XComment)
        {
            return xNode.ToString();
        }

        // if (xNode is XCDataSection)
        // {
        //     return xNode.OuterXml;
        // }

        throw new Exception("Need to handle + " + xNode);
    }

    // TODO #819 don't need this?
    private static Doc GetEncodedTextValue(XText xmlText)
    {
        if (xmlText.Value is null)
        {
            return Doc.Null;
        }

        return new XElement("EncodeText", xmlText.Value).LastNode!.ToString();
    }
}
