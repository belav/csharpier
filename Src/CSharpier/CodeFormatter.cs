namespace CSharpier;

using CSharpier.Formatters.Xml;

public static class CodeFormatter
{
    public static CodeFormatterResult Format(string code, CodeFormatterOptions? options = null)
    {
        return FormatAsync(code, options).Result;
    }

    public static Task<CodeFormatterResult> FormatAsync(
        string code,
        CodeFormatterOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        options ??= new();

        return CSharpFormatter.FormatAsync(
            code,
            new PrinterOptions { Width = options.Width },
            cancellationToken
        );
    }

    public static CodeFormatterResult Format(
        SyntaxTree syntaxTree,
        CodeFormatterOptions? options = null
    )
    {
        return FormatAsync(syntaxTree, options).Result;
    }

    public static Task<CodeFormatterResult> FormatAsync(
        SyntaxTree syntaxTree,
        CodeFormatterOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        options ??= new();

        return CSharpFormatter.FormatAsync(
            syntaxTree,
            new PrinterOptions { Width = options.Width },
            cancellationToken
        );
    }

    internal static async Task<CodeFormatterResult> FormatAsync(
        string fileContents,
        string fileExtension,
        PrinterOptions options,
        CancellationToken cancellationToken
    )
    {
        var loweredExtension = fileExtension.ToLower();

        if (loweredExtension is ".cs")
        {
            return await CSharpFormatter.FormatAsync(fileContents, options, cancellationToken);
        }

        // TODO XML maybe make this opt in until it is better tested?
        // TODO XML maybe don't include xml by default?
        if (loweredExtension is ".csproj" or ".props" or ".targets" or ".xml")
        {
            return XmlFormatter.Format(fileContents, options);
        }

        return new CodeFormatterResult { FailureMessage = "Is an unsupported file type." };
    }
}
