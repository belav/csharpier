namespace CSharpier.Core.DocPrinter;

internal class Indent
{
    public string Value = string.Empty;
    public int Length;

    // caching the next indent here rather than in a dictionary keyed by Value avoids hashing a
    // whitespace string whose length grows with the nesting depth
    public Indent? Increased;
}

internal class Indenter(PrinterOptions printerOptions)
{
    protected readonly PrinterOptions PrinterOptions = printerOptions;

    public static Indent GenerateRoot()
    {
        return new();
    }

    public Indent IncreaseIndent(Indent indent)
    {
        return indent.Increased ??= this.PrinterOptions.UseTabs
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
    }
}
