using System.Xml.Linq;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Node
{
    internal static Doc Print(XNode xNode, XmlPrintingContext context)
    {
        if (xNode is XDocument xDocument)
        {
            var result = new List<Doc>();

            if (xDocument.Declaration is not null)
            {
                result.Add(xDocument.Declaration.ToString(), Doc.HardLine);
            }

            foreach (var node in xDocument.Nodes())
            {
                result.Add(Print(node, context), Doc.HardLine);
            }

            result.Add(Doc.HardLine);

            return Doc.Concat(result);
        }

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

        if (xNode is XComment or XProcessingInstruction)
        {
            return xNode.ToString();
        }

        throw new Exception("Need to handle + " + xNode.GetType());
    }

    private static Doc GetEncodedTextValue(XText xText)
    {
        if (xText.Value is null)
        {
            return Doc.Null;
        }

        if (xText is XCData xcData)
        {
            return xcData.ToString();
        }

        return new XElement("EncodeText", xText.Value).LastNode!.ToString();
    }
}
