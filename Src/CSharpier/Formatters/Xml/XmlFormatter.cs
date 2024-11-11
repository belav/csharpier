using System.Text.Json;
using System.Xml;
using CSharpier.SyntaxPrinter;
using Node = CSharpier.Formatters.Xml.XmlNodePrinters.Node;

namespace CSharpier.Formatters.Xml;

internal static class XmlFormatter
{
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);

        /* TODO #819 all of these things

still getting this
warning: in the working copy of 'runtime/src/tests/Common/mergedrunner.targets', LF will be replaced by CRLF the next time Git touches it

        */

        var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
        var printingContext = new PrintingContext
        {
            Options = new PrintingContext.PrintingContextOptions
            {
                LineEnding = lineEnding,
                IndentSize = printerOptions.IndentSize,
                UseTabs = printerOptions.UseTabs,
            },
        };
        var doc = Node.Print(xmlDocument, printingContext);
        var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formattedXml,
            DocTree = printerOptions.IncludeDocTree ? DocSerializer.Serialize(doc) : string.Empty,
            AST = printerOptions.IncludeAST ? JsonSerializer.Serialize(xmlDocument) : string.Empty,
        };
    }
}
