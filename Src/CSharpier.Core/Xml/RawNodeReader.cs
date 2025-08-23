using System.Text.RegularExpressions;
using System.Xml;

namespace CSharpier.Core.Xml;

internal static
#if !NETSTANDARD2_0
partial
#endif
class RawNodeReader
{
#if NETSTANDARD2_0
    private static readonly Regex NewlineRegex = new(@"\r\n|\n|\r", RegexOptions.Compiled);
#else
    [GeneratedRegex(@"\r\n|\n|\r", RegexOptions.Compiled)]
    private static partial Regex NewlineRegex();
#endif

    public static List<RawNode> ReadAllNodes(string originalXml, string lineEnding)
    {
        // xml reader can get confused about line numbers if we don't normalize them
        originalXml = NewlineRegex
#if !NETSTANDARD2_0
            ()
#endif
        .Replace(originalXml, "\n");

        using var xmlReader = XmlReader.Create(
            new StringReader(originalXml),
            new XmlReaderSettings { IgnoreWhitespace = false }
        );

        var elements = new List<RawNode>();
        var elementStack = new Stack<RawNode>();
        var attributeReader = new RawAttributeReader(originalXml, lineEnding, xmlReader);

        while (xmlReader.Read())
        {
            if (xmlReader.NodeType is XmlNodeType.Whitespace or XmlNodeType.SignificantWhitespace)
            {
                continue;
            }
            if (xmlReader.NodeType == XmlNodeType.EndElement && elementStack.Count > 0)
            {
                var theElement = elementStack.Pop();
                for (var x = 0; x < theElement.Nodes.Count; x++)
                {
                    if (x > 0)
                    {
                        theElement.Nodes[x - 1].NextNode = theElement.Nodes[x];
                        theElement.Nodes[x].PreviousNode = theElement.Nodes[x - 1];
                    }

                    if (x == theElement.Nodes.Count - 2)
                    {
                        theElement.Nodes[x].NextNode = theElement.Nodes[x + 1];
                        theElement.Nodes[x + 1].PreviousNode = theElement.Nodes[x];
                    }
                }
            }
            else
            {
                var element = new RawNode
                {
                    Name = xmlReader.Name,
                    NodeType = xmlReader.NodeType,
                    IsEmpty = xmlReader.IsEmptyElement,
                    Attributes =
                        xmlReader.AttributeCount > 0 ? attributeReader.GetAttributes() : [],
                    Value = GetValue(xmlReader, lineEnding),
                };

                if (elementStack.Count > 0)
                {
                    element.Parent = elementStack.Peek();
                    elementStack.Peek().Nodes.Add(element);
                }
                else
                {
                    elements.Add(element);
                }

                if (xmlReader.NodeType == XmlNodeType.Element && !element.IsEmpty)
                {
                    elementStack.Push(element);
                }
            }
        }

        return elements;
    }

    private static string GetValue(XmlReader xmlReader, string lineEnding)
    {
        var normalizedTextValue = NewlineRegex
#if !NETSTANDARD2_0
            ()
#endif
        .Replace(xmlReader.Value, lineEnding);

        return xmlReader.NodeType switch
        {
            XmlNodeType.Text => normalizedTextValue,
            XmlNodeType.Comment => $"<!--{normalizedTextValue}-->",
            XmlNodeType.CDATA => $"<![CDATA[{normalizedTextValue}]]>",
            XmlNodeType.ProcessingInstruction => $"<?{xmlReader.Name} {normalizedTextValue}?>",
            _ => string.Empty,
        };
    }
}
