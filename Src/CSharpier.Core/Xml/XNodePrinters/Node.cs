using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static class Node
{
    internal static Doc Print(RawNode node, PrintingContext context)
    {
        if (node.NodeType is XmlNodeType.Document)
        {
            var result = new DocListBuilder(node.Nodes.Count * 2 + 1);

            foreach (var childNode in node.Nodes)
            {
                result.Add(
                    Print(childNode, context),
                    childNode.NodeType is XmlNodeType.Whitespace ? Doc.Null : Doc.HardLine
                );
            }

            result.Add(Doc.HardLine);

            return Doc.Concat(ref result);
        }

        if (node.NodeType == XmlNodeType.DocumentType)
        {
            return node.Value;
        }

        if (node.NodeType == XmlNodeType.Element)
        {
            return Element.Print(node, context);
        }

        if (node.NodeType is XmlNodeType.Text)
        {
            List<Doc> doc =
            [
                Tag.PrintOpeningTagPrefix(node),
                GetTextValue(node),
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
                return Doc.Concat(node.Value, Doc.HardLine);
            }

            return node.Value;
        }

        if (node.NodeType is XmlNodeType.Whitespace)
        {
            return Doc.HardLine;
        }

        throw new Exception("Need to handle + " + node.NodeType);
    }

    private static Doc GetTextValue(RawNode rawNode)
    {
        var textValue = rawNode.Value;

        if (string.IsNullOrEmpty(textValue))
        {
            return Doc.Null;
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

        return textValue;
    }
}
