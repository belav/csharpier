using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
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

    internal static Doc Print(XmlReader xmlReader, PrintingContext context)
    {
        while (xmlReader.NodeType == XmlNodeType.Whitespace)
        {
            xmlReader.Read();
        }

        if (xmlReader.NodeType == XmlNodeType.XmlDeclaration)
        {
            var version = xmlReader.GetAttribute("version");
            var encoding = xmlReader.GetAttribute("encoding");
            var standalone = xmlReader.GetAttribute("standalone");

            var declaration = $"<?xml version=\"{version}\"";
            if (!string.IsNullOrEmpty(encoding))
            {
                declaration += $" encoding=\"{encoding}\"";
            }

            if (!string.IsNullOrEmpty(standalone))
            {
                declaration += $" standalone=\"{standalone}\"";
            }

            declaration += "?>";

            return Doc.Concat(declaration, Doc.HardLine);
        }

        if (xmlReader.NodeType == XmlNodeType.DocumentType)
        {
            return $"<!DOCTYPE {xmlReader.Name}>".Replace("[]", string.Empty);
        }

        if (xmlReader.NodeType == XmlNodeType.Element)
        {
            return Element.Print(xmlReader, context);
        }

        if (xmlReader.NodeType == XmlNodeType.None)
        {
            return Doc.Null;
        }

        if (xmlReader.NodeType == XmlNodeType.Text)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(xmlReader),
                GetEncodedTextValue(xmlReader),
                Tag.PrintClosingTagSuffix(xmlReader, context),
            ];

            if (doc.All(o => o is StringDoc))
            {
                var result = string.Join(string.Empty, doc.Select(o => ((StringDoc)o).Value));
                return result;
            }

            return Doc.Concat(doc);
        }
        //
        //         if (xNode is XComment or XProcessingInstruction)
        //         {
        //             return NewlineRegex
        // #if !NETSTANDARD2_0
        //                 ()
        // #endif
        //             .Replace(xNode.ToString(), context.Options.LineEnding);
        //         }

        throw new Exception("Need to handle + " + xmlReader.NodeType);
    }

    private static Doc GetEncodedTextValue(XmlReader xmlReader)
    {
        if (!xmlReader.HasValue)
        {
            return Doc.Null;
        }

        // TODO 1679
        // if (xText is XCData xcData)
        // {
        //     return xcData.ToString();
        // }

        var textValue = xmlReader.Value;
        // TODO 1679
        // if (xText.Parent?.FirstNode == xText)
        // {
        //     if (textValue[0] is '\r')
        //     {
        //         textValue = textValue[1..];
        //     }
        //
        //     if (textValue[0] is '\n')
        //     {
        //         textValue = textValue[1..];
        //     }
        // }

        return new XElement("EncodeText", textValue).LastNode!.ToString();
    }
}
