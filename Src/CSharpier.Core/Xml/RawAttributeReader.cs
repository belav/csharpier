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

        var firstQuote = line.IndexOf('"', index) + 1;

        var endQuote = line.IndexOf('"', firstQuote);
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
}
