using System.Collections.Generic;

namespace CSharpier
{
    internal class PrinterOptions
    {
        public bool IncludeAST { get; init; }
        public bool IncludeDocTree { get; init; }
        public bool UseTabs { get; init; }
        public int TabWidth { get; init; } = 4;
        public int Width { get; init; } = 100;
        public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
        public bool TrimInitialLines { get; init; } = true;
        public List<string[]>? PreprocessorSymbolSets { get; init; }

        public const int WidthUsedByTests = 100;
    }

    internal enum EndOfLine
    {
        Auto,
        CRLF,
        LF
    }
}
