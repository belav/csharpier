using System.Xml;

namespace CSharpier.Core.Xml;

internal class RawAttributeReader(string originalXml, string endOfLine)
{
    private readonly string[] lines = originalXml.Split(["\r\n", "\n"], StringSplitOptions.None);

    public string? GetRawAttribute(IXmlLineInfo xmlLineInfo, string attributeName)
    {
        var lineNumber = xmlLineInfo.LineNumber - 1;
        var line = this.lines[lineNumber];

        var index = line.IndexOf(
            attributeName,
            xmlLineInfo.LinePosition - 1,
            StringComparison.Ordinal
        );
        if (index < 0)
        {
            return null;
        }

        var firstQuote = line.IndexOfAny(['"', '\''], index);
        var quoteCharacter = line[firstQuote];
        firstQuote += 1;

        var endQuote = line.IndexOf(quoteCharacter, firstQuote);
        // attribute on a single line, return in
        if (endQuote > 0)
        {
            return line[firstQuote..endQuote];
        }

        var nextLineNumber = lineNumber + 1;
        var result = line[firstQuote..];
        var nextLine = this.lines[nextLineNumber];
        while (endQuote < 0)
        {
            result += endOfLine + nextLine;
            nextLineNumber++;
            nextLine = this.lines[nextLineNumber];
            endQuote = nextLine.IndexOf('"');
        }

        result += endOfLine + nextLine[..endQuote];

        return result;
    }

    public RawAttribute[] GetAttributes(BetterXmlReader xmlReader)
    {
        xmlReader.MoveToFirstAttribute();

        var result = new RawAttribute[xmlReader.AttributeCount];

        for (var x = 0; x < xmlReader.AttributeCount; x++)
        {
            result[x] = new RawAttribute
            {
                Name = xmlReader.Name,
                Value = this.GetRawAttribute(xmlReader.XmlLineInfo, xmlReader.Name),
            };

            xmlReader.MoveToNextAttribute();
        }

        xmlReader.MoveToElement();

        return result;
    }
}

internal struct RawAttribute
{
    public string Name { get; set; }
    public string Value { get; set; }
}

internal struct RawElement
{
    public required string? Name { get; set; }
    public required XmlNodeType NodeType { get; set; }
    public required bool IsEmpty { get; set; }
    public required RawAttribute[] Attributes { get; set; }
}
