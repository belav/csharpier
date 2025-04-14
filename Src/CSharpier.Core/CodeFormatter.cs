using CSharpier.Core.CSharp;
using CSharpier.Core.Xml;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core;

internal static class CodeFormatter
{
    internal static async Task<CodeFormatterResult> FormatAsync(
        string fileContents,
        PrinterOptions options,
        CancellationToken cancellationToken
    )
    {
        return options.Formatter switch
        {
            Formatter.CSharp => await CSharpFormatter.FormatAsync(
                fileContents,
                options,
                SourceCodeKind.Regular,
                cancellationToken
            ),
            Formatter.CSharpScript => await CSharpFormatter.FormatAsync(
                fileContents,
                options,
                SourceCodeKind.Script,
                cancellationToken
            ),
            Formatter.XML => XmlFormatter.Format(fileContents, options),
            _ => new CodeFormatterResult { FailureMessage = "Is an unsupported file type." },
        };
    }
}
