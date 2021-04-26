namespace CSharpier
{
    public class Options
    {
        public bool IncludeAST { get; init; }
        public bool IncludeDocTree { get; init; }
        public bool UseTabs { get; init; }
        public int TabWidth { get; init; } = 4;
        public int Width { get; init; } = 100;
        // TODO this should just be an enum?
        public string EndOfLine { get; set; } = "\n";
    }
}
