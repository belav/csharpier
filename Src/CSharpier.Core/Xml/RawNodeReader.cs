using System.Text.RegularExpressions;
using System.Xml;

namespace CSharpier.Core.Xml;

internal
#if !NETSTANDARD2_0
partial
#endif
class RawNodeReader(string originalXml, string lineEnding, XmlReader xmlReader)
{
    private readonly IXmlLineInfo xmlLineInfo = (xmlReader as IXmlLineInfo)!;

    private readonly string[] lines = originalXml
        .Split(["\n"], StringSplitOptions.None)
        .Prepend("")
        .ToArray();

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

        var thing = new RawNodeReader(originalXml, lineEnding, xmlReader);
        return thing.ReadStuff();
    }

    public List<RawNode> ReadStuff()
    {
        var elements = new List<RawNode>();
        var elementStack = new Stack<RawNode>();

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
                var lineNumber = this.xmlLineInfo.LineNumber;

                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    DebugLogger.Log(
                        "Element "
                            + xmlReader.Name
                            + " Position: "
                            + this.xmlLineInfo.LineNumber
                            + ":"
                            + this.xmlLineInfo.LinePosition
                    );

                    var line = this.lines[lineNumber];
                    while (
                        this.xmlLineInfo.LinePosition > line.Length
                        || !line[(this.xmlLineInfo.LinePosition - 1)..]
                            .StartsWith(xmlReader.Name, StringComparison.InvariantCulture)
                    )
                    {
                        lineNumber--;
                        line = this.lines[lineNumber];
                    }
                }

                var element = new RawNode
                {
                    Name = xmlReader.Name,
                    NodeType = xmlReader.NodeType,
                    IsEmpty = xmlReader.IsEmptyElement,
                    Attributes = xmlReader.AttributeCount > 0 ? this.GetAttributes(lineNumber) : [],
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

    private RawAttribute[] GetAttributes(int lineNumber)
    {
        xmlReader.MoveToFirstAttribute();

        var result = new RawAttribute[xmlReader.AttributeCount];

        string GetRawAttribute(string attributeName)
        {
            var line = this.lines[lineNumber];

            while (
                this.xmlLineInfo.LinePosition > line.Length
                || !line[(this.xmlLineInfo.LinePosition - 1)..]
                    .StartsWith(attributeName, StringComparison.InvariantCulture)
            )
            {
                lineNumber++;
                line = this.lines[lineNumber];
            }

            var index = line.IndexOf(
                attributeName + "=",
                this.xmlLineInfo.LinePosition - 1,
                StringComparison.Ordinal
            );

            var firstQuote = line.IndexOfAny(['"', '\''], index);
            var quoteCharacter = line[firstQuote];
            firstQuote += 1;

            var endQuote = line.IndexOf(quoteCharacter, firstQuote);
            // attribute on a single line, return in
            if (endQuote > 0)
            {
                return line[firstQuote..endQuote];
            }

            lineNumber++;
            var result = line[firstQuote..];
            var nextLine = this.lines[lineNumber];
            while (endQuote < 0)
            {
                result += lineEnding + nextLine;
                lineNumber++;
                nextLine = this.lines[lineNumber];
                endQuote = nextLine.IndexOf('"');
            }

            result += lineEnding + nextLine[..endQuote];

            return result;
        }

        for (var x = 0; x < xmlReader.AttributeCount; x++)
        {
            result[x] = new RawAttribute
            {
                Name = xmlReader.Name,
                Value = GetRawAttribute(xmlReader.Name).Replace("\"", "&quot;"),
            };

            xmlReader.MoveToNextAttribute();
        }

        xmlReader.MoveToElement();

        return result;
    }
}
