using CSharpier.Formatters.Xml;

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
            new PrinterOptions(Formatter.CSharp)
            {
                Width = options.Width,
                UseTabs = options.IndentStyle == IndentStyle.Tabs,
                IndentSize = options.IndentSize,
                EndOfLine = options.EndOfLine,
                IncludeGenerated = options.IncludeGenerated,
            },
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
            new PrinterOptions(Formatter.CSharp)
            {
                Width = options.Width,
                UseTabs = options.IndentStyle == IndentStyle.Tabs,
                IndentSize = options.IndentSize,
                EndOfLine = options.EndOfLine,
            },
            SourceCodeKind.Regular,
            cancellationToken
        );
    }

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
