namespace CSharpier;

internal class XmlFormatter : IFormatter
{
    public Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
