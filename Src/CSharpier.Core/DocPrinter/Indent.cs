namespace CSharpier.Core.DocPrinter;

internal class Indent
{
    public string Value = string.Empty;
    public int Length;
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
        indent.Increased ??= new Indent
        {
            Value = this.PrinterOptions.UseTabs
                ? indent.Value + "\t"
                : indent.Value.PadRight(indent.Value.Length + this.PrinterOptions.IndentSize),
            Length = indent.Length + this.PrinterOptions.IndentSize,
        };

        return indent.Increased;
    }
}
