using System.Text;
using System.Text.Json;
using CSharpier.SyntaxPrinter;

namespace CSharpier;

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
}
