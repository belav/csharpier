namespace CSharpier.DocPrinter;

internal class Indent
{
    public string Value = string.Empty;
    public int Length;
}

internal class Indenter(PrinterOptions printerOptions)
{
    protected readonly PrinterOptions PrinterOptions = printerOptions;
    protected readonly Dictionary<string, Indent> IncreaseIndentCache = new();

    public static Indent GenerateRoot()
    {
        return new();
    }

    public Indent IncreaseIndent(Indent indent)
    {
        if (IncreaseIndentCache.TryGetValue(indent.Value, out var increasedIndent))
        {
            return increasedIndent;
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

        IncreaseIndentCache[indent.Value] = nextIndent;
        return nextIndent;
    }
}
