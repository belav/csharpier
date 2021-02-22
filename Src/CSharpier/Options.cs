namespace CSharpier
{
    public class Options
    {
        public bool IncludeAST { get; set; }
        public bool IncludeDocTree { get; set; }
        public bool UseTabs { get; set; }
        public int TabWidth { get; set; } = 4;
        public int Width { get; set; } = 80;
    }
}
