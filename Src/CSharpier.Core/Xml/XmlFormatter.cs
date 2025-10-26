using System.IO;
using System.Threading.Tasks;
using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using Node = CSharpier.Core.Xml.XNodePrinters.Node;

namespace CSharpier.Core.Xml;

public static class XmlFormatter
{
    public static CodeFormatterResult Format(string xml, CodeFormatterOptions? options = null)
    {
        return FormatAsync(xml, (options ?? new()).ToPrinterOptions())
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    internal static async Task<CodeFormatterResult> FormatAsync(
        string xml,
        PrinterOptions printerOptions
    )
    {
        try
        {
            var validationTask = ValidateXmlAsync(xml);

            var lineEnding = PrinterOptions.GetLineEnding(xml, printerOptions);
            var rootNode = RawNodeReader.ParseXml(xml, lineEnding);
            var printingContext = new PrintingContext
            {
                Options = new PrintingContext.PrintingContextOptions
                {
                    LineEnding = lineEnding,
                    IndentSize = printerOptions.IndentSize,
                    UseTabs = printerOptions.UseTabs,
                },
                Information = new PrintingContext.CodeInformation(false, false),
            };
            var doc = Node.Print(rootNode, printingContext);
            var formattedXml = DocPrinter.DocPrinter.Print(doc, printerOptions, lineEnding);

            await validationTask;

            return new CodeFormatterResult
            {
                Code = formattedXml,
                DocTree = printerOptions.IncludeDocTree
                    ? DocSerializer.Serialize(doc)
                    : string.Empty,
                AST = RawNodeSyntaxWriter.Write(rootNode),
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

    // the RawNodeReader is very basic and doesn't care if xml is valid or not and could mangle invalid xml if we allow it to format the invalid xml
    private static async Task ValidateXmlAsync(string xml)
    {
        using var stringReader = new StringReader(xml);
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.None,
            CheckCharacters = true,
            Async = true,
        };

        using var xmlReader = XmlReader.Create(stringReader, settings);
        while (await xmlReader.ReadAsync())
        {
            // XmlReader will throw XmlException if invalid
        }
    }
}
