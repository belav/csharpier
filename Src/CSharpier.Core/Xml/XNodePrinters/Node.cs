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

    internal static Doc Print(RawElement element, XmlPrintingContext context)
    {
        if (element.NodeType == XmlNodeType.XmlDeclaration)
        {
            var version = "TODO";
            // element.GetAttribute("version");
            // var encoding = element.GetAttribute("encoding");
            // var standalone = element.GetAttribute("standalone");

            var declaration = $"<?xml version=\"{version}\"";
            // if (!string.IsNullOrEmpty(encoding))
            // {
            //     declaration += $" encoding=\"{encoding}\"";
            // }
            //
            // if (!string.IsNullOrEmpty(standalone))
            // {
            //     declaration += $" standalone=\"{standalone}\"";
            // }

            declaration += "?>";

            return Doc.Concat(declaration, Doc.HardLine);
        }

        if (element.NodeType == XmlNodeType.DocumentType)
        {
            return $"<!DOCTYPE {element.Name}>".Replace("[]", string.Empty);
        }

        if (element.NodeType == XmlNodeType.Element)
        {
            return Element.Print(element, context);
        }

        if (element.NodeType == XmlNodeType.None)
        {
            return Doc.Null;
        }

        if (element.NodeType == XmlNodeType.Text)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(element),
                GetEncodedTextValue(element),
                Tag.PrintClosingTagSuffix(element, context),
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

        throw new Exception("Need to handle + " + element.NodeType);
    }

    private static Doc GetEncodedTextValue(RawElement rawElement)
    {
        // TODO 1679 don't allow empty?
        if (string.IsNullOrEmpty(rawElement.Value))
        {
            return Doc.Null;
        }

        if (rawElement.NodeType is XmlNodeType.CDATA)
        {
            return rawElement.Value;
        }

        var textValue = rawElement.Value;
        if (rawElement.Parent?.Nodes.First() == rawElement)
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
