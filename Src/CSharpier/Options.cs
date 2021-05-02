namespace CSharpier
{
    public class Options
    {
        public bool IncludeAST { get; init; }
        public bool IncludeDocTree { get; init; }
        public bool UseTabs { get; init; }
        public int TabWidth { get; init; } = 4;
        public int Width { get; init; } = 100;
        public string EndOfLine { get; init; } = "\n";
        public bool TrimInitialLines { get; init; } = true;

        public const int TestingWidth = 80;
    }
}
