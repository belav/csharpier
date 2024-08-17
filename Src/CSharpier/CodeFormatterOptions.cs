namespace CSharpier;

public class CodeFormatterOptions
{
    public int Width { get; init; } = 100;
    public IndentStyle IndentStyle { get; init; } = IndentStyle.Spaces;
    public int IndentSize { get; init; } = 4;
    public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
    public bool IncludeGenerated { get; init; }
}

public enum IndentStyle
{
    Spaces,
    Tabs,
}
