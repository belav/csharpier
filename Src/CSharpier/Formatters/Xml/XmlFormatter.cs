using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using CSharpier.SyntaxPrinter;
using Node = CSharpier.Formatters.Xml.XNodePrinters.Node;

namespace CSharpier.Formatters.Xml;

internal static class XmlFormatter
{
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        var xDocument = XDocument.Parse(xml);

        // TODO #819 Error ./efcore/eng/sdl-tsa-vars.config - Threw exception while formatting.
        // Data at the root level is invalid. Line 1, position 1.
        // not all configs are xml, should we try to detect that?

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
        var doc = Node.Print(xDocument, printingContext);
        var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

        return new CodeFormatterResult
        {
            Code = formattedXml,
            DocTree = printerOptions.IncludeDocTree ? DocSerializer.Serialize(doc) : string.Empty,
            AST = printerOptions.IncludeAST ? JsonSerializer.Serialize(xDocument) : string.Empty,
        };
    }
}
