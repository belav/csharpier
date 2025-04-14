namespace CSharpier.Core;

public class CodeFormatterOptions
{
    public int Width { get; init; } = 100;
    public IndentStyle IndentStyle { get; init; } = IndentStyle.Spaces;
    public int IndentSize { get; init; } = 4;
    public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
    public bool IncludeGenerated { get; init; }

    internal PrinterOptions ToPrinterOptions()
    {
        return new(Formatter.CSharp)
        {
            Width = this.Width,
            UseTabs = this.IndentStyle == IndentStyle.Tabs,
            IndentSize = this.IndentSize,
            EndOfLine = this.EndOfLine,
            IncludeGenerated = this.IncludeGenerated,
        };
    }
}

public enum IndentStyle
{
    Spaces,
    Tabs,
}
