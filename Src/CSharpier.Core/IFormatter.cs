namespace CSharpier.Core;

internal interface IFormatter
{
    Task<CodeFormatterResult> FormatAsync(
        string code,
        PrinterOptions printerOptions,
        CancellationToken cancellationToken
    );
}
