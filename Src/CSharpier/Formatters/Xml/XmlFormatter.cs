using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;
using Node = CSharpier.Formatters.Xml.XNodePrinters.Node;

namespace CSharpier.Formatters.Xml;

internal static class XmlFormatter
{
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        // with xmlDocument we can't get the proper encoded values in an attribute
        // with xDocument we can't retain any newlines in attributes
        // so let's just use them both
        XDocument xDocument;
        try
        {
            xDocument = XDocument.Parse(xml);
        }
        catch (XmlException)
        {
            return new CodeFormatterResult
            {
                Code = xml,
                WarningMessage = "Appeared to be invalid xml so was not formatted.",
            };
        }
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        var mapping = new Dictionary<XNode, XmlNode>();
        CreateMapping(xDocument, xmlDocument, mapping);

        var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
        var printingContext = new XmlPrintingContext
        {
            Mapping = mapping,
            Options = new PrintingContext.PrintingContextOptions
            {
                LineEnding = lineEnding,
                IndentSize = printerOptions.IndentSize,
                UseTabs = printerOptions.UseTabs,
            },
        };
        var doc = Node.Print(xDocument, printingContext);
        var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formattedXml,
            DocTree = printerOptions.IncludeDocTree ? DocSerializer.Serialize(doc) : string.Empty,
            AST = printerOptions.IncludeAST
                ? JsonSerializer.Serialize(xDocument, XmlFormatterJsonSerializerOptions)
                : string.Empty,
        };
    }

    private static readonly JsonSerializerOptions XmlFormatterJsonSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
    };

    private static void CreateMapping(
        XNode? xNode,
        XmlNode? xmlNode,
        Dictionary<XNode, XmlNode> mapping
    )
    {
        if (xNode == null || xmlNode == null)
        {
            return;
        }

        mapping[xNode] = xmlNode;

        if (xNode is not XContainer xContainer)
        {
            return;
        }

        var index = 0;
        if (xmlNode.ChildNodes[0] is XmlDeclaration)
        {
            index++;
        }
        foreach (var xChild in xContainer.Nodes())
        {
            if (index > xmlNode.ChildNodes.Count)
            {
                break;
            }

            CreateMapping(xChild, xmlNode.ChildNodes[index], mapping);

            index++;
        }
    }
}

internal class XmlPrintingContext : PrintingContext
{
    public required Dictionary<XNode, XmlNode> Mapping { get; set; }
}
