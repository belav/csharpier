using System.IO;
using System.Threading.Tasks;
using System.Xml;
using CSharpier.Core.CSharp.SyntaxPrinter;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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
            var (rootNode, normalizedXml) = RawNodeReader.ParseXml(
                xml,
                lineEnding,
                printerOptions.XmlWhitespaceSensitivity
            );
            var printingContext = new PrintingContext
            {
                NormalizedXml = normalizedXml,
                Options = new PrintingContext.PrintingContextOptions
                {
                    LineEnding = lineEnding,
                    IndentSize = printerOptions.IndentSize,
                    UseTabs = printerOptions.UseTabs,
                    XmlWhitespaceSensitivity = printerOptions.XmlWhitespaceSensitivity,
                },
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
        catch (XmlException ex)
        {
            return new CodeFormatterResult
            {
                Code = xml,
                ErrorDiagnostics = [CreateDiagnosticFromXmlException(xml, ex)],
                WarningMessage = "Appeared to be invalid xml so was not formatted.",
            };
        }
    }

    private static Diagnostic CreateDiagnosticFromXmlException(string xml, XmlException ex)
    {
        var sourceText = SourceText.From(xml);

        // XmlException is 1-based; Roslyn is 0-based
        var lineIndex = Math.Max(0, ex.LineNumber - 1);
        if (lineIndex >= sourceText.Lines.Count)
        {
            lineIndex = sourceText.Lines.Count - 1;
        }

        var line = sourceText.Lines[lineIndex];

        var charIndexInLine = Math.Max(0, ex.LinePosition - 1);
        var position = Math.Min(line.Start + charIndexInLine, line.End);

        var span = new TextSpan(position, 0);

        var location = Location.Create(
            filePath: string.Empty,
            textSpan: span,
            lineSpan: sourceText.Lines.GetLinePositionSpan(span)
        );

        var descriptor = new DiagnosticDescriptor(
            id: "XML001",
            title: "XML parsing error",
            messageFormat: "{0}",
            category: "XML",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        return Diagnostic.Create(descriptor, location, ex.Message);
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
            DtdProcessing = DtdProcessing.Ignore,
        };

        using var xmlReader = XmlReader.Create(stringReader, settings);
        while (await xmlReader.ReadAsync())
        {
            // XmlReader will throw XmlException if invalid
        }
    }
}
