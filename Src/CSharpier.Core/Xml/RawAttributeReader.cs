using System.Xml;

namespace CSharpier.Core.Xml;

internal class RawAttributeReader(string originalXml)
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
        // TODO is it worth using a stringbuilder?
        var result = line[firstQuote..];
        // TODO use configured EOL
        var EOL = "\r\n";
        var nextLine = this.lines[nextLineNumber];
        while (endQuote < 0)
        {
            result += EOL + nextLine;
            nextLineNumber++;
            nextLine = this.lines[nextLineNumber];
            endQuote = nextLine.IndexOf('"');
        }

        result += EOL + nextLine[..endQuote];

        return result;
    }
}
