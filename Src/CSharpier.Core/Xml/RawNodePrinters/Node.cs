using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.RawNodePrinters;

internal static class Node
{
    internal static Doc Print(RawNode node, PrintingContext context)
    {
        if (node.NodeType == XmlNodeType.XmlDeclaration)
        {
            var version = node.Attributes.FirstOrDefault(o => o.Name == "version")?.Value;
            var encoding = node.Attributes.FirstOrDefault(o => o.Name == "encoding")?.Value;
            var standalone = node.Attributes.FirstOrDefault(o => o.Name == "standalone")?.Value;

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

        if (node.NodeType == XmlNodeType.DocumentType)
        {
            return $"<!DOCTYPE {node.Name}>".Replace("[]", string.Empty);
        }

        if (node.NodeType == XmlNodeType.Element)
        {
            return Element.Print(node, context);
        }

        if (node.NodeType == XmlNodeType.None)
        {
            return Doc.Null;
        }

        if (node.NodeType is XmlNodeType.Text)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(node),
                GetEncodedTextValue(node),
                Tag.PrintClosingTagSuffix(node, context),
            ];

            if (doc.All(o => o is StringDoc))
            {
                var result = string.Join(string.Empty, doc.Select(o => ((StringDoc)o).Value));
                return result;
            }

            return Doc.Concat(doc);
        }

        if (
            node.NodeType
            is XmlNodeType.Comment
                or XmlNodeType.ProcessingInstruction
                or XmlNodeType.CDATA
        )
        {
            if (node.Parent is null)
            {
                return Doc.Concat(node.Value!, Doc.HardLine);
            }

            return node.Value!;
        }

        throw new Exception("Need to handle + " + node.NodeType);
    }

    private static Doc GetEncodedTextValue(RawNode rawNode)
    {
        var textValue = rawNode.Value;

        if (string.IsNullOrEmpty(textValue))
        {
            return Doc.Null;
        }

        if (rawNode.NodeType is XmlNodeType.CDATA)
        {
            return textValue;
        }

        if (rawNode.Parent?.Nodes.First() == rawNode)
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
