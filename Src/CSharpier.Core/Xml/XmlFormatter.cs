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

    // TODO 1679 ignore all invalid xml in the repo so that we can make sure we don't screw up any
    // TODO 1679 some edge cases to deal with - https://github.com/belav/csharpier-repos/pull/150/files
    // cdata on its own line.. okay or not?
    // https://github.com/belav/csharpier-repos/pull/150/files#diff-aaa8f7c465894fd87e55f47256d3b6bdfb011d6bb8f9c5ff8368a42705091048
    // some files being reformatted with spaces removed etc, why?
    // https://github.com/belav/csharpier-repos/pull/150/files#diff-d43be0b1199187297fc0edd2da7abd74782fcf67b4614feb1bac76528f657b1f
    // https://github.com/belav/csharpier-repos/pull/150/files#diff-c89f46bbc5617ee258659fc31bea43fb82be10302363a86d638ec0e251019e1a
    internal static CodeFormatterResult Format(string xml, PrinterOptions printerOptions)
    {
        try
        {
            var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
            // TODO 1679 should probably use XmlReader to validate this is good xml before sending it through this thing
            var rootNode = RawNodeReader.ParseXml(xml, lineEnding);
            var printingContext = new PrintingContext
            {
                Options = new PrintingContext.PrintingContextOptions
                {
                    LineEnding = lineEnding,
                    IndentSize = printerOptions.IndentSize,
                    UseTabs = printerOptions.UseTabs,
                },
            };
            var doc = Node.Print(rootNode, printingContext);
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
