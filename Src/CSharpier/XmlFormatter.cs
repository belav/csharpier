namespace CSharpier;

internal static class XmlFormatter
{
    internal static async Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    )
    {
        return new CodeFormatterResult { Code = code };
    }
}
