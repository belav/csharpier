using System.Text.RegularExpressions;
using System.Xml.Linq;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static
#if !NETSTANDARD2_0
partial
#endif
class Node
{
#if NETSTANDARD2_0
    private static readonly Regex NewlineRegex = new(@"\r\n|\n|\r", RegexOptions.Compiled);
#else
    [GeneratedRegex(@"\r\n|\n|\r", RegexOptions.Compiled)]
    private static partial Regex NewlineRegex();
#endif

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
                Tag.PrintClosingTagSuffix(xText, context),
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
            return NewlineRegex
#if !NETSTANDARD2_0
                ()
#endif
            .Replace(xNode.ToString(), context.Options.LineEnding);
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

        var textValue = xText.Value;
        if (xText.Parent?.FirstNode == xText)
        {
            if (textValue[0] is '\r')
            {
                textValue = textValue[1..];
            }

            if (textValue[0] is '\n')
            {
                textValue = textValue[1..];
            }
        }

        return new XElement("EncodeText", textValue).LastNode!.ToString();
    }
}
