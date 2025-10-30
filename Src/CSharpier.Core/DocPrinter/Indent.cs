namespace CSharpier.Core.DocPrinter;

internal class Indent
{
    public string Value = string.Empty;
    public int Length;
}

internal class Indenter(PrinterOptions printerOptions)
{
    protected readonly PrinterOptions PrinterOptions = printerOptions;
    protected readonly Dictionary<string, Indent> IncreaseIndentCache = [];
    protected readonly List<Indent> Cache = [];

    public static Indent GenerateRoot()
    {
        return new();
    }

    public Indent IncreaseIndent(Indent indent)
    {
        // if (IncreaseIndentCache.TryGetValue(indent.Value, out var increasedIndent))
        // {
        //     return increasedIndent;
        // }
        if (this.PrinterOptions.UseTabs && Cache.Count > indent.Length )
        {
            return Cache[indent.Length];
        }
        else if (Cache.Count > indent.Length / this.PrinterOptions.IndentSize)
        {
            return Cache[indent.Length/ this.PrinterOptions.IndentSize];
            
        }

        var nextIndent = this.PrinterOptions.UseTabs
            ? new Indent
            {
                Value = indent.Value + "\t",
                Length = indent.Length + this.PrinterOptions.IndentSize,
            }
            : new Indent
            {
                Value = indent.Value.PadRight(indent.Value.Length + this.PrinterOptions.IndentSize),
                Length = indent.Length + this.PrinterOptions.IndentSize,
            };

        // IncreaseIndentCache[indent.Value] = nextIndent;
        Cache.Add(nextIndent);
        return nextIndent;
    }
}
