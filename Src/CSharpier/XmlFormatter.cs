namespace CSharpier;

using System.Text;
using System.Xml;

internal static class XmlFormatter
{
    internal static async Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        // TODO xml width?
        // TODO xml test other things
        var stringBuilder = new StringWriter();

        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = printerOptions.UseTabs ? "\t" : new string(' ', printerOptions.TabWidth),
            NewLineChars = PrinterOptions.GetLineEnding(code, printerOptions),
            OmitXmlDeclaration = true
        };

        var xmlTextWriter = XmlWriter.Create(stringBuilder, settings);
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(code);
        xmlDocument.Save(xmlTextWriter);

        return new CodeFormatterResult
        {
            Code = stringBuilder + PrinterOptions.GetLineEnding(code, printerOptions)
        };
    }

    private class XmlFragmentWriter : XmlTextWriter
    {
        public XmlFragmentWriter(TextWriter textWriter)
            : base(textWriter) { }

        public override void WriteStartDocument()
        {
            // Do nothing (omit the declaration)
        }
    }
}
