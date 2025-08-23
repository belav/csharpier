using System.Xml;

namespace CSharpier.Core.Xml;

internal class RawAttributeReader(string originalXml, string endOfLine, XmlReader xmlReader)
{
    private readonly IXmlLineInfo xmlLineInfo = (xmlReader as IXmlLineInfo)!;

    private readonly string[] lines = originalXml.Split(["\n"], StringSplitOptions.None);

    public RawAttribute[] GetAttributes()
    {
        xmlReader.MoveToFirstAttribute();

        var result = new RawAttribute[xmlReader.AttributeCount];

        for (var x = 0; x < xmlReader.AttributeCount; x++)
        {
            result[x] = new RawAttribute
            {
                Name = xmlReader.Name,
                Value = this.GetRawAttribute(xmlReader.Name).Replace("\"", "&quot;"),
            };

            xmlReader.MoveToNextAttribute();
        }

        xmlReader.MoveToElement();

        return result;
    }

    private string GetRawAttribute(string attributeName)
    {
        // TODO 1679 a whole lot of exceptions formatting files like
        //  .\mono\mcs\class\corlib\Documentation\en\System\Activator.xml
        // why is it off by one? can we go backwards til we find it?
        var lineNumber = this.xmlLineInfo.LineNumber - 1;
        var line = this.lines[lineNumber];

        DebugLogger.Log("Reading attribute " + attributeName + " on line " + lineNumber);

        var index = line.IndexOf(
            attributeName,
            this.xmlLineInfo.LinePosition - 1,
            StringComparison.Ordinal
        );
        if (index < 0)
        {
            throw new Exception(
                "Could not find attribute "
                    + attributeName
                    + " on line "
                    + this.xmlLineInfo.LineNumber
            );
            return string.Empty;
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
}
