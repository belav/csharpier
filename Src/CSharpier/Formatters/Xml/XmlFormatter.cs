using System.Text.Json;
using System.Xml;
using CSharpier.Formatters.Xml.XmlNodePrinters;

namespace CSharpier.Formatters.Xml;

internal static class XmlFormatter
{
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

        var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
        var doc = Node.Print(xmlDocument);
        var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formattedXml,
            DocTree = printerOptions.IncludeDocTree ? DocSerializer.Serialize(doc) : string.Empty,
            AST = printerOptions.IncludeAST ? JsonSerializer.Serialize(xmlDocument) : string.Empty,
        };
    }
}
