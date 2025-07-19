using System.Globalization;

namespace CSharpier.Core;

internal class PrinterOptions(Formatter formatter)
{
    public bool IncludeAST { get; init; }
    public bool IncludeDocTree { get; init; }
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = formatter == Formatter.XML ? 2 : 4;
    public int Width { get; set; } = 100;
    public EndOfLine EndOfLine { get; set; } = EndOfLine.Auto;
    public bool TrimInitialLines { get; init; } = true;
    public bool IncludeGenerated { get; set; }
    public Formatter Formatter { get; set; } = formatter;

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

    public static Formatter GetFormatter(string filePath)
    {
        var extension = Path.GetExtension(filePath)[1..].ToLower(CultureInfo.InvariantCulture);

        var formatter = extension switch
        {
            "cs" => Formatter.CSharp,
            "csx" => Formatter.CSharpScript,
            "config" or "csproj" or "props" or "slnx" or "targets" or "xaml" or "xml" =>
                Formatter.XML,
            _ => Formatter.Unknown,
        };
        return formatter;
    }
}

internal enum Formatter
{
    Unknown,
    CSharp,
    CSharpScript,
    XML,
}
