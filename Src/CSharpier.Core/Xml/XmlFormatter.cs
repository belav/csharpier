using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using Node = CSharpier.Core.Xml.RawNodePrinters.Node;

namespace CSharpier.Core.Xml;

public static class XmlFormatter
{
    public static CodeFormatterResult Format(string xml, CodeFormatterOptions? options = null)
    {
        return Format(xml, (options ?? new()).ToPrinterOptions());
    }

    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        try
        {
            using var xmlReader = XmlReader.Create(
                new StringReader(xml),
                new XmlReaderSettings { IgnoreWhitespace = false }
            );

            var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
            var elements = RawNodeReader.ReadAllNodes(xml, lineEnding, xmlReader);
            var printingContext = new XmlPrintingContext
            {
                Options = new PrintingContext.PrintingContextOptions
                {
                    LineEnding = lineEnding,
                    IndentSize = printerOptions.IndentSize,
                    UseTabs = printerOptions.UseTabs,
                },
            };
            var results = new List<Doc>();
            foreach (var element in elements)
            {
                results.Add(Node.Print(element, printingContext));
            }

            var doc = Doc.Concat(results);
            var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

            return new CodeFormatterResult
            {
                Code = formattedXml,
                DocTree = printerOptions.IncludeDocTree
                    ? DocSerializer.Serialize(doc)
                    : string.Empty,
                // TODO 1679 use xdocument or something?
                AST = string.Empty,
            };
        }
        catch (XmlException)
        {
            return new CodeFormatterResult
            {
                Code = xml,
                WarningMessage = "Appeared to be invalid xml so was not formatted.",
            };
        }
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

// TODO 1679 kill this
internal class XmlPrintingContext : PrintingContext { }
