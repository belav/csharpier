using System.Xml;
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

    // TODO 1679
    // some edge cases to deal with
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        try
        {
            var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
            var elements = RawNodeReader.ReadAll(xml, lineEnding);
            var printingContext = new PrintingContext
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
                // TODO 1679 spit out version of our thing?
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
}
