namespace CSharpier
{
    public class PrinterOptions
    {
        public bool IncludeAST { get; init; }
        public bool IncludeDocTree { get; init; }
        public bool UseTabs { get; init; }
        public int TabWidth { get; init; } = 4;
        public int Width { get; init; } = 100;
        public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
        public bool TrimInitialLines { get; init; } = true;

        public const int WidthUsedByTests = 100;
    }

    public enum EndOfLine
    {
        Auto,
        CRLF,
        LF
    }
}
