namespace CSharpier;

internal class PrinterOptions
{
    public bool IncludeAST { get; init; }
    public bool IncludeDocTree { get; init; }
    public bool UseTabs { get; set; }
    public int TabWidth { get; set; } = 4;
    public int Width { get; set; } = 100;
    public EndOfLine EndOfLine { get; set; } = EndOfLine.Auto;
    public bool TrimInitialLines { get; init; } = true;
    public bool IncludeGenerated { get; set; } = false;

    public BraceNewLine NewLineBeforeOpenBrace { get; set; } = BraceNewLine.All;
    public bool NewLineBeforeElse { get; set; } = true;
    public bool NewLineBeforeCatch { get; set; } = true;
    public bool NewLineBeforeFinally { get; set; } = true;
    public bool? NewLineBeforeMembersInObjectInitializers { get; set; } = null;
    public bool? NewLineBeforeMembersInAnonymousTypes { get; set; } = null;
    public bool? NewLineBetweenQueryExpressionClauses { get; set; } = null;
    public bool UsePrettierStyleTrailingCommas { get; set; } = true;

    public const int WidthUsedByTests = 100;

    internal static string GetLineEnding(string code, PrinterOptions printerOptions)
    {
        if (printerOptions.EndOfLine != EndOfLine.Auto)
        {
            return printerOptions.EndOfLine == EndOfLine.CRLF ? "\r\n" : "\n";
        }

        var lineIndex = code.IndexOf('\n');
        if (lineIndex <= 0)
        {
            return "\n";
        }
        if (code[lineIndex - 1] == '\r')
        {
            return "\r\n";
        }

        return "\n";
    }
}
