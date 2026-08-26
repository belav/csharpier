using System.Text.RegularExpressions;
using System.Xml;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;

namespace CSharpier.Core.Xml.XNodePrinters;

internal static partial class Node
{
#if NET6_0_OR_GREATER
    [GeneratedRegex("\\s+")]
    private static partial Regex WhitespaceRegexGenerator();

    private static readonly Regex WhitespaceRegex = WhitespaceRegexGenerator();
#else
    private static readonly Regex WhitespaceRegex = new("\\s+");
#endif

    internal static Doc Print(RawNode node, XmlPrintingContext context)
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
            var prefix = Tag.PrintOpeningTagPrefix(node, context);
            var textValue = GetTextValue(node, context);
            var suffix = Tag.PrintClosingTagSuffix(node, context);

            if (
                prefix is StringDoc prefixString
                && textValue is StringDoc textValueString
                && suffix is StringDoc suffixString
            )
            {
                return string.Concat(prefixString.Value, textValueString.Value, suffixString.Value);
            }

            return Doc.Concat(prefix, textValue, suffix);
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

    private static Doc GetTextValue(RawNode rawNode, XmlPrintingContext context)
    {
        var textValue = rawNode.Value;

        if (rawNode.XmlWhitespaceSensitivity is XmlWhitespaceSensitivity.Ignore)
        {
            if (rawNode.PreviousNode is null)
            {
                textValue = textValue.TrimStart();
            }

            if (rawNode.NextNode is null)
            {
                textValue = textValue.TrimEnd();
            }

            if (rawNode.Parent?.Nodes.Count == 1)
            {
                if (textValue.Length > 2)
                {
                    var innerValue = textValue[1..^1];
                    textValue =
                        textValue[0] + WhitespaceRegex.Replace(innerValue, " ") + textValue[^1];
                }
            }
        }

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

            if (textValue.Length > 0 && textValue[0] is '\n')
            {
                textValue = textValue[1..];
            }
        }

        return textValue;
    }
}
